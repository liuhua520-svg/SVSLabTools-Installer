using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SVSLabToolsInstaller.Core
{
    /// <summary>
    /// 环境搭建全部完成后，负责静默调起打包好的 "SVS Lab Tools.msi"，
    /// 把用户在本程序里选定的安装目录通过 APPDIR 属性传给它，让 MSI
    /// 只负责它擅长的那部分——写 backend/frontend 程序文件、开始菜单
    /// 快捷方式、"添加或删除程序"里的卸载条目。
    ///
    /// 职责边界（务必保持）：
    ///   - 本程序（C#）：micromamba 虚拟环境（.mfa_env / .kaldi_env /
    ///     .nemo_env / .qwen3_env / .qwen3tts_env），不再复制
    ///     backend/frontend，避免和 MSI 文件表里已登记的 337 个文件
    ///     产生"两份来源不一致"的覆盖冲突。
    ///   - MSI：backend/frontend 静态文件 + 快捷方式 + 卸载信息；卸载时
    ///     通过自定义操作 AI_REMOVE_APPDIR_TREE 把整个 APPDIR（含本程序
    ///     建的虚拟环境）一并删除，见 .aip 工程里的对应改动。
    ///
    /// 【MSI 文件从哪来】不再由这个类自己解析路径。程序改为单文件 exe
    /// 后，"SVS Lab Tools.msi" 作为嵌入资源打包进程序集，由
    /// PayloadExtractor 在程序启动时统一释放到临时目录；这个类只管
    /// "拿到一个 MSI 文件路径后怎么静默装它"，不关心这个路径的来源，
    /// 调用方（MainForm）负责把 PayloadExtractor 释放出的路径传进来。
    /// </summary>
    public static class MsiLauncher
    {
        /// <summary>
        /// 静默安装 MSI，把 appDir 透传为 APPDIR 属性。
        /// 返回 (成功与否, msiexec 退出码, 日志文件路径)。
        /// 退出码 0 = 成功；3010 = 成功但需要重启才能完成——两者都视为
        /// 安装成功，只是 3010 需要提示用户。
        /// </summary>
        /// <param name="msiPath">
        /// 已释放到磁盘的 "SVS Lab Tools.msi" 实际路径（由
        /// PayloadExtractor.Extract() 得到），调用前应已确认文件存在。
        /// </param>
        public static async Task<MsiInstallResult> InstallSilentlyAsync(
            string msiPath,
            string appDir,
            Action<string> onLogLine,
            CancellationToken ct)
        {
            if (!File.Exists(msiPath))
            {
                onLogLine?.Invoke(Strings.Get("Log.MsiNotFound1", msiPath));
                onLogLine?.Invoke(Strings.Get("Log.MsiNotFound2"));
                return new MsiInstallResult(false, -1, null);
            }

            // 日志固定写到 APPDIR\_setup\ 下，和虚拟环境安装日志放在一起，
            // 方便用户一次性打包发给开发者排障。
            string setupDir = Path.Combine(appDir, "_setup");
            Directory.CreateDirectory(setupDir);
            string logPath = Path.Combine(setupDir, "msi_install.log");

            // APPDIR 必须以反斜杠结尾——MSI 属性里的目录型属性若不以
            // 反斜杠结尾，Windows Installer 在某些版本上会把最后一段
            // 当成文件名而不是目录名处理，导致路径拼接出错。
            string normalizedAppDir = appDir.TrimEnd('\\', '/') + "\\";

            string arguments =
                $"/i \"{msiPath}\" /qn /norestart " +
                $"APPDIR=\"{normalizedAppDir}\" " +
                $"/log \"{logPath}\"";

            onLogLine?.Invoke(Strings.Get("Log.MsiInstalling"));
            onLogLine?.Invoke($"    msiexec {arguments}");

            var psi = new ProcessStartInfo
            {
                FileName = "msiexec.exe",
                Arguments = arguments,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = false,
                RedirectStandardError = false,
            };

            int exitCode;
            try
            {
                using (var process = new Process { StartInfo = psi, EnableRaisingEvents = true })
                {
                    process.Start();

                    using (ct.Register(() =>
                    {
                        try { if (!process.HasExited) process.Kill(); }
                        catch { /* 进程可能已经在退出过程中，忽略 */ }
                    }))
                    {
                        await Task.Run(() => process.WaitForExit()).ConfigureAwait(false);
                    }

                    ct.ThrowIfCancellationRequested();
                    exitCode = process.ExitCode;
                }
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                onLogLine?.Invoke(Strings.Get("Log.MsiStartFailed", ex.Message));
                return new MsiInstallResult(false, -1, logPath);
            }

            if (exitCode == 0)
            {
                onLogLine?.Invoke(Strings.Get("Log.MsiInstallOk"));
                return new MsiInstallResult(true, exitCode, logPath);
            }
            if (exitCode == 3010)
            {
                onLogLine?.Invoke(Strings.Get("Log.MsiInstallOkNeedsReboot"));
                return new MsiInstallResult(true, exitCode, logPath);
            }

            onLogLine?.Invoke(Strings.Get("Log.MsiInstallFailed", exitCode));
            onLogLine?.Invoke(Strings.Get("Log.MsiInstallFailedLogRef", logPath));
            AppendTailOfLog(logPath, onLogLine);
            return new MsiInstallResult(false, exitCode, logPath);
        }

        // MSI 日志可能有几千行，安装失败时只把最后一小段内容回显到主
        // 界面日志框，方便用户第一时间看到失败原因，而不必手动去打开
        // 日志文件——完整内容仍然留在 logPath，供进一步排查。
        private static void AppendTailOfLog(string logPath, Action<string> onLogLine)
        {
            try
            {
                if (!File.Exists(logPath)) return;
                // MSI 日志常见编码是 UTF-16LE（带 BOM），Encoding.Default
                // 在非英文 Windows 上未必等于 UTF-16，这里显式尝试按
                // UTF-16LE 读取，读取失败再退回默认编码。
                string[] lines;
                try
                {
                    lines = File.ReadAllLines(logPath, Encoding.Unicode);
                }
                catch
                {
                    lines = File.ReadAllLines(logPath);
                }

                int start = Math.Max(0, lines.Length - 25);
                onLogLine?.Invoke(Strings.Get("Log.MsiLogTailHeader"));
                for (int i = start; i < lines.Length; i++)
                    onLogLine?.Invoke("        " + lines[i]);
                onLogLine?.Invoke(Strings.Get("Log.MsiLogTailFooter"));
            }
            catch
            {
                // 读日志失败不应该影响主流程的错误提示，静默忽略。
            }
        }
    }

    public sealed class MsiInstallResult
    {
        public bool Success { get; }
        public int ExitCode { get; }
        public string LogPath { get; }
        public bool NeedsReboot => ExitCode == 3010;

        public MsiInstallResult(bool success, int exitCode, string logPath)
        {
            Success = success;
            ExitCode = exitCode;
            LogPath = logPath;
        }
    }
}
