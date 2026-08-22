using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SVSLabToolsInstaller.Core
{
    /// <summary>
    /// 对 micromamba.exe 的进程调用封装，从原 _common.bat 的 :init /
    /// :pip_install 子程序迁移而来，思路完全一致，只是调度语言从 batch
    /// 换成了 C#：
    ///   - MAMBA_ROOT_PREFIX 的取值规则（固定放在 APPDIR\_setup\.mamba_root
    ///     下，跟随安装目录，卸载时随 APPDIR 一起清理）与 _common.bat
    ///     :init 完全一致。
    ///   - pip install 前先 --upgrade pip setuptools wheel，与
    ///     _common.bat :pip_install 完全一致。
    ///   - micromamba run 默认就会把子进程 stdout/stderr 实时流式输出
    ///     （不像 conda run 需要显式加 --no-capture-output），这里通过
    ///     RedirectStandardOutput/Error + OutputDataReceived 事件把这些
    ///     输出实时转发给调用方，用于界面日志面板显示。
    /// </summary>
    public sealed class MicromambaRunner
    {
        private readonly string _micromambaExePath;
        private readonly string _mambaRootPrefix;

        /// <param name="appDir">安装目录（末尾可带可不带反斜杠，内部会自动规整）。</param>
        /// <param name="micromambaExePath">micromamba.exe 的完整路径（随安装包 Payload 一起分发）。</param>
        public MicromambaRunner(string appDir, string micromambaExePath)
        {
            if (string.IsNullOrEmpty(appDir))
                throw new ArgumentException(Strings.Get("Log.AppDirEmpty"), nameof(appDir));
            if (!File.Exists(micromambaExePath))
                throw new FileNotFoundException(Strings.Get("Log.MicromambaNotFound"), micromambaExePath);

            _micromambaExePath = micromambaExePath;

            string normalizedAppDir = appDir.TrimEnd('\\', '/');
            _mambaRootPrefix = Path.Combine(normalizedAppDir, "_setup", ".mamba_root");
            Directory.CreateDirectory(_mambaRootPrefix);
        }

        /// <summary>
        /// 供调用方订阅的日志行事件：每当子进程产出一行 stdout/stderr，
        /// 都会通过这个事件推送出去。事件在子进程的后台读取线程上触发，
        /// 不是 UI 线程——订阅方（MainForm）负责用 Invoke/BeginInvoke 切回
        /// UI 线程再更新控件，这里不做任何线程封送，保持这个类本身与
        /// UI 框架无关、可独立测试。
        /// </summary>
        public event Action<string> LineReceived;

        /// <summary>
        /// 创建一个新环境：micromamba create -y -p &lt;envPath&gt; -c conda-forge &lt;packages...&gt;
        /// </summary>
        public async Task<bool> CreateEnvAsync(string envPath, string channel, string packagesSpec, CancellationToken ct)
        {
            string args = $"create -y -p \"{envPath}\" -c {channel} {packagesSpec}";
            return await RunAsync(args, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// 向已存在的环境安装额外的 conda-forge 包：
        /// micromamba install -y -p &lt;envPath&gt; -c conda-forge &lt;packagesSpec&gt;
        /// </summary>
        public async Task<bool> InstallCondaPackageAsync(string envPath, string channel, string packagesSpec, CancellationToken ct)
        {
            string args = $"install -y -p \"{envPath}\" -c {channel} {packagesSpec}";
            return await RunAsync(args, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// 在指定环境里跑一条任意命令：micromamba run -p &lt;envPath&gt; &lt;command&gt;
        /// </summary>
        public async Task<bool> RunInEnvAsync(string envPath, string command, CancellationToken ct)
        {
            string args = $"run -p \"{envPath}\" {command}";
            return await RunAsync(args, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// 统一的 pip install 包装，等价于原 _common.bat 的 :pip_install：
        /// 先升级 pip/setuptools/wheel，再按 requirements 文件安装依赖。
        /// 升级步骤失败不算整体失败（与原 bat 行为一致，原脚本对升级那行
        /// 没有做 errorlevel 判断）；requirements 安装失败才返回 false。
        /// </summary>
        public async Task<bool> PipInstallAsync(string envPath, string requirementsFilePath, CancellationToken ct)
        {
            if (!File.Exists(requirementsFilePath))
            {
                RaiseLine(Strings.Get("Log.MissingRequirementsFile", requirementsFilePath));
                return false;
            }

            RaiseLine(Strings.Get("Log.UpgradingPip"));
            await RunInEnvAsync(envPath, "python -m pip install --upgrade pip setuptools wheel", ct)
                .ConfigureAwait(false);

            RaiseLine(Strings.Get("Log.InstallingRequirements", requirementsFilePath));
            bool ok = await RunInEnvAsync(envPath, $"python -m pip install -r \"{requirementsFilePath}\"", ct)
                .ConfigureAwait(false);
            return ok;
        }

        /// <summary>
        /// 真正启动 micromamba.exe 子进程、异步等待退出、实时转发输出行。
        /// 用 TaskCompletionSource 把基于事件的 Process.Exited 桥接成
        /// awaitable Task，这是 .NET Framework（没有内置
        /// Process.WaitForExitAsync，那是 .NET 5+ 才加入的 API）下驱动
        /// 异步等待外部进程退出的标准写法。
        /// </summary>
        private Task<bool> RunAsync(string arguments, CancellationToken ct)
        {
            var tcs = new TaskCompletionSource<bool>();

            var psi = new ProcessStartInfo
            {
                FileName = _micromambaExePath,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8,
            };
            // MAMBA_ROOT_PREFIX 是 micromamba 自己的"环境注册表根目录"
            // （pkgs 缓存、.condarc 等），和 -p/--prefix 指定的"某一个具体
            // 环境装在哪"是两回事——即使每个环境都用 -p 指到固定路径，
            // MAMBA_ROOT_PREFIX 仍然必须指向一个有效目录，否则 micromamba
            // 会直接报错退出。与原 _common.bat :init 的处理完全一致。
            psi.EnvironmentVariables["MAMBA_ROOT_PREFIX"] = _mambaRootPrefix;

            var process = new Process { StartInfo = psi, EnableRaisingEvents = true };

            process.OutputDataReceived += (s, e) =>
            {
                if (e.Data != null) RaiseLine(e.Data);
            };
            process.ErrorDataReceived += (s, e) =>
            {
                if (e.Data != null) RaiseLine(e.Data);
            };
            process.Exited += (s, e) =>
            {
                bool success = process.ExitCode == 0;
                process.Dispose();
                tcs.TrySetResult(success);
            };

            CancellationTokenRegistration ctReg = default(CancellationTokenRegistration);
            if (ct.CanBeCanceled)
            {
                ctReg = ct.Register(() =>
                {
                    try
                    {
                        if (!process.HasExited) process.Kill();
                    }
                    catch (InvalidOperationException)
                    {
                        // 进程已经退出，Kill 时进程句柄可能已失效，忽略即可。
                    }
                    tcs.TrySetCanceled(ct);
                });
            }

            try
            {
                RaiseLine(Strings.Get("Log.RunningCommand", arguments));
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
            }
            catch (Exception ex)
            {
                RaiseLine(Strings.Get("Log.MicromambaStartFailed", ex.Message));
                ctReg.Dispose();
                return Task.FromResult(false);
            }

            return tcs.Task.ContinueWith(t =>
            {
                ctReg.Dispose();
                return t.Status == TaskStatus.RanToCompletion && t.Result;
            }, TaskScheduler.Default);
        }

        private void RaiseLine(string line)
        {
            LineReceived?.Invoke(line);
        }
    }
}
