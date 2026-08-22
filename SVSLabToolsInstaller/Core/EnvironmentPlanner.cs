using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SVSLabToolsInstaller.Core
{
    /// <summary>用户在界面上的全部勾选状态，供 EnvironmentPlanner 生成步骤列表。</summary>
    public sealed class InstallSelection
    {
        public bool Core { get; set; }
        public bool LangChinese { get; set; }
        public bool LangEnglish { get; set; }
        public bool LangJapanese { get; set; }
        public bool LangKorean { get; set; }
        public bool LangCantonese { get; set; }
        public bool Nemo { get; set; }
        public bool Qwen3Asr { get; set; }
        public bool Qwen3Tts { get; set; }
        public TorchVariant Torch { get; set; } = TorchVariant.Cpu;
    }

    /// <summary>
    /// 把 InstallSelection（用户勾选状态）翻译成一份有序的 InstallStep
    /// 列表，思路对应原 run_setup.bat 里那一长串
    /// "if /i SEL_XXX==1 call steps\xxx.bat" 的调度逻辑，只是这里直接
    /// 生成 C# 委托而不是拼命令行调用外部 .bat 文件。
    /// </summary>
    public sealed class EnvironmentPlanner
    {
        private readonly string _appDir;
        private readonly string _micromambaExePath;
        private readonly string _envAssetsDir;

        /// <param name="appDir">虚拟环境的安装根目录（.mfa_env 等建在这下面）。</param>
        /// <param name="micromambaExePath">micromamba.exe 路径。</param>
        /// <param name="envAssetsDir">
        /// 存放 mfa_env_sitecustomize.py / mfa_utils.py 的目录。
        /// 【重要】这不是 appDir\backend——backend 现在由 MSI 单独安装，
        /// 时序上可能在虚拟环境建立之前还不存在。这两个文件改为随本
        /// exe 一起打包在 Payload\EnvAssets\ 下，虚拟环境搭建步骤不再
        /// 依赖 MSI 是否已经跑完，两条安装流程彻底解耦。
        /// </param>
        public EnvironmentPlanner(string appDir, string micromambaExePath, string envAssetsDir)
        {
            _appDir = appDir.TrimEnd('\\', '/');
            _micromambaExePath = micromambaExePath;
            _envAssetsDir = envAssetsDir.TrimEnd('\\', '/');
        }

        public List<InstallStep> BuildPlan(InstallSelection sel)
        {
            var steps = new List<InstallStep>();

            if (sel.Core)
            {
                steps.Add(new InstallStep(Strings.Get("Step.Core"),
                    ct => RunCoreEnvAsync(sel.Torch, ct)));
            }

            // 【与原 run_setup.bat 一致的提醒】语言模型依赖 Core 建立的
            // .mfa_env——即使用户没勾选 Core，语言模型步骤仍然会尝试执行
            // （如果用户单独勾了某个语言但没勾 Core，大概率是想复用一个
            // 已经装好的既有环境），只是会在执行时发现 .mfa_env 不存在
            // 而失败，报错信息里会清楚指出原因，不在这里做"未勾选 Core
            // 就跳过语言步骤"这种额外的隐式判断。
            if (sel.LangChinese)
                steps.Add(new InstallStep(Strings.Get("Step.Lang", Strings.Get("Ui.LangChinese")), ct => DownloadLanguageModelAsync("cmn", Strings.Get("Ui.LangChinese"), ct)));
            if (sel.LangEnglish)
                steps.Add(new InstallStep(Strings.Get("Step.Lang", Strings.Get("Ui.LangEnglish")), ct => DownloadLanguageModelAsync("eng", Strings.Get("Ui.LangEnglish"), ct)));
            if (sel.LangJapanese)
                steps.Add(new InstallStep(Strings.Get("Step.Lang", Strings.Get("Ui.LangJapanese")), ct => DownloadLanguageModelAsync("jpn", Strings.Get("Ui.LangJapanese"), ct)));
            if (sel.LangKorean)
                steps.Add(new InstallStep(Strings.Get("Step.Lang", Strings.Get("Ui.LangKorean")), ct => DownloadLanguageModelAsync("kor", Strings.Get("Ui.LangKorean"), ct)));
            if (sel.LangCantonese)
                steps.Add(new InstallStep(Strings.Get("Step.Lang", Strings.Get("Ui.LangCantonese")), ct => DownloadLanguageModelAsync("yue", Strings.Get("Ui.LangCantonese"), ct)));

            if (sel.Nemo)
            {
                steps.Add(new InstallStep(Strings.Get("Step.Nemo"),
                    ct => RunNemoEnvAsync(sel.Torch, ct)));
            }

            if (sel.Qwen3Asr)
            {
                steps.Add(new InstallStep(Strings.Get("Step.Qwen3Asr"),
                    ct => RunIsolatedEnvAsync(
                        envDirName: ".qwen3_env",
                        displayName: "Qwen3-ASR",
                        pythonVersion: "3.10",
                        requirementsContent: RequirementsBuilder.BuildQwen3Requirements(sel.Torch),
                        requirementsLabel: "qwen3",
                        ct: ct)));
            }

            if (sel.Qwen3Tts)
            {
                steps.Add(new InstallStep(Strings.Get("Step.Qwen3Tts"),
                    ct => RunIsolatedEnvAsync(
                        envDirName: ".qwen3tts_env",
                        // 【关键差异，务必保留】Python 3.12，不是其它两个
                        // 独立环境用的 3.10——这是 Qwen3-TTS 官方要求，
                        // 用于支持 Qwen3-TTS-Tokenizer-12Hz 的模型结构。
                        displayName: "Qwen3-TTS",
                        pythonVersion: "3.12",
                        requirementsContent: RequirementsBuilder.BuildQwen3TtsRequirements(sel.Torch),
                        requirementsLabel: "qwen3tts",
                        ct: ct)));
            }

            return steps;
        }

        // ────────────────────────────────────────────────────────────
        // Core 环境：迁移自 01_core_env.bat
        // ────────────────────────────────────────────────────────────
        private async Task<bool> RunCoreEnvAsync(TorchVariant torch, CancellationToken ct)
        {
            var runner = new MicromambaRunner(_appDir, _micromambaExePath);
            runner.LineReceived += OnRunnerLine;

            string envPrefix = Path.Combine(_appDir, ".mfa_env");
            string kaldiEnvPrefix = Path.Combine(_appDir, ".kaldi_env");

            if (Directory.Exists(envPrefix) && !ConfirmRecreate(Strings.Get("Ui.MfaMainEnvName"), envPrefix))
            {
                OnRunnerLine(Strings.Get("Log.UseExistingMfaEnv"));
            }
            else
            {
                if (Directory.Exists(envPrefix)) Directory.Delete(envPrefix, recursive: true);
                OnRunnerLine(Strings.Get("Log.CreatingEnv"));
                if (!await runner.CreateEnvAsync(envPrefix, "conda-forge", "python=3.10 pip", ct).ConfigureAwait(false))
                {
                    OnRunnerLine(Strings.Get("Log.EnvCreateFailed"));
                    return false;
                }
            }

            OnRunnerLine(Strings.Get("Log.InstallingPyAv"));
            if (!await runner.InstallCondaPackageAsync(envPrefix, "conda-forge", "av=11.0.0", ct).ConfigureAwait(false))
            {
                OnRunnerLine(Strings.Get("Log.PyAvInstallFailed"));
                return false;
            }

            // .kaldi_env：kaldi + kalpy + pynini。python=3.10 必须与 .mfa_env
            // 一致，否则 kalpy 编译出的 _kalpy.pyd 会因 ABI 不匹配在
            // .mfa_env 里 import 失败（表现为 ModuleNotFoundError，但成因
            // 是 ABI 不匹配，不是真的没装）——与 01_core_env.bat 完全一致。
            if (!File.Exists(Path.Combine(kaldiEnvPrefix, "python.exe")))
            {
                OnRunnerLine(Strings.Get("Log.CreatingKaldiEnv"));
                if (!await runner.CreateEnvAsync(kaldiEnvPrefix, "conda-forge", "python=3.10 kaldi", ct).ConfigureAwait(false))
                {
                    OnRunnerLine(Strings.Get("Log.KaldiInstallFailed"));
                    return false;
                }
            }
            else
            {
                OnRunnerLine(Strings.Get("Log.KaldiEnvExists"));
            }

            OnRunnerLine(Strings.Get("Log.InstallingKalpy"));
            if (!await runner.InstallCondaPackageAsync(kaldiEnvPrefix, "conda-forge", "kalpy", ct).ConfigureAwait(false))
            {
                OnRunnerLine(Strings.Get("Log.KalpyInstallFailed"));
                return false;
            }

            // 【安全检查】校验 _kalpy 编译出的 .pyd 版本与 .mfa_env 的
            // Python 大版本一致，尽早暴露 ABI 不匹配问题（与
            // 01_core_env.bat 同款检查）。
            string kalpyPyd = Path.Combine(kaldiEnvPrefix, "Lib", "site-packages", "_kalpy.cp310-win_amd64.pyd");
            if (!File.Exists(kalpyPyd))
            {
                OnRunnerLine(Strings.Get("Log.KalpyPydMissing1"));
                OnRunnerLine(Strings.Get("Log.KalpyPydMissing2"));
                OnRunnerLine(Strings.Get("Log.KalpyPydMissing3", kaldiEnvPrefix));
                return false;
            }
            OnRunnerLine(Strings.Get("Log.KalpyPydOk"));

            // pynini 是可选依赖，失败不中断安装（与 01_core_env.bat 一致）。
            OnRunnerLine(Strings.Get("Log.InstallingPynini"));
            if (!await runner.InstallCondaPackageAsync(kaldiEnvPrefix, "conda-forge", "pynini", ct).ConfigureAwait(false))
            {
                OnRunnerLine(Strings.Get("Log.PyniniFailed1"));
                OnRunnerLine(Strings.Get("Log.PyniniFailed2"));
            }
            else
            {
                OnRunnerLine(Strings.Get("Log.PyniniOk"));
            }

            // pip 装动态生成的 requirements（含用户选择的 torch 变体）。
            string reqContent = RequirementsBuilder.BuildCoreRequirements(torch);
            string reqPath = RequirementsBuilder.WriteToTempFile(reqContent, "core");
            if (!await runner.PipInstallAsync(envPrefix, reqPath, ct).ConfigureAwait(false))
            {
                OnRunnerLine(Strings.Get("Log.CoreDepsFailed"));
                return false;
            }
            OnRunnerLine(Strings.Get("Log.CoreDepsOk"));

            DeploySpeechbrainPatch(envPrefix);

            OnRunnerLine(Strings.Get("Log.CoreDone"));
            return true;
        }

        // ────────────────────────────────────────────────────────────
        // speechbrain Windows 路径分隔符补丁，迁移自 01_core_env.bat
        // 用 sysconfig 而不是 site.getsitepackages()[0] 定位 site-packages
        // 的原因见 01_core_env.bat 里的详细注释：后者在某些 conda/Windows
        // 组合下会返回环境根目录本身而不是 site-packages 子目录。
        // C# 版本用 Python 单行脚本通过 micromamba run 获取这个路径，
        // 复用同一套探测逻辑，不用自己在 C# 里重新猜测规则。
        // ────────────────────────────────────────────────────────────
        private void DeploySpeechbrainPatch(string envPrefix)
        {
            OnRunnerLine(Strings.Get("Log.DeployingPatch"));
            try
            {
                string tempScript = Path.Combine(Path.GetTempPath(), $"mfa_site_packages_{System.Guid.NewGuid():N}.py");
                string tempOutput = Path.Combine(Path.GetTempPath(), $"mfa_site_packages_{System.Guid.NewGuid():N}.txt");
                File.WriteAllText(tempScript,
                    "import sysconfig\r\n" +
                    $"with open(r\"{tempOutput}\", \"w\", encoding=\"utf-8\") as _f:\r\n" +
                    "    _f.write(sysconfig.get_paths()[\"purelib\"])\r\n");

                var psi = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = Path.Combine(envPrefix, "python.exe"),
                    Arguments = $"\"{tempScript}\"",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };
                using (var p = System.Diagnostics.Process.Start(psi))
                {
                    p.WaitForExit();
                }
                File.Delete(tempScript);

                if (!File.Exists(tempOutput))
                {
                    OnRunnerLine(Strings.Get("Log.PatchSitePackagesNotFound"));
                    return;
                }
                string sitePackages = File.ReadAllText(tempOutput).Trim();
                File.Delete(tempOutput);

                if (string.IsNullOrEmpty(sitePackages) || !Directory.Exists(sitePackages))
                {
                    OnRunnerLine(Strings.Get("Log.PatchPathNotExist", sitePackages));
                    return;
                }
                string leaf = new DirectoryInfo(sitePackages).Name;
                if (!string.Equals(leaf, "site-packages", System.StringComparison.OrdinalIgnoreCase))
                {
                    OnRunnerLine(Strings.Get("Log.PatchWrongLeaf", sitePackages));
                    return;
                }

                string sitecustomizeSource = Path.Combine(_envAssetsDir, "mfa_env_sitecustomize.py");
                if (!File.Exists(sitecustomizeSource))
                {
                    OnRunnerLine(Strings.Get("Log.PatchSourceMissing", sitecustomizeSource));
                    return;
                }
                File.Copy(sitecustomizeSource, Path.Combine(sitePackages, "sitecustomize.py"), overwrite: true);
                OnRunnerLine(Strings.Get("Log.PatchDeployed", Path.Combine(sitePackages, "sitecustomize.py")));
            }
            catch (System.Exception ex)
            {
                OnRunnerLine(Strings.Get("Log.PatchFailedNonFatal", ex.Message));
            }
        }

        // ────────────────────────────────────────────────────────────
        // 语言模型下载，迁移自 02_lang_*.bat
        // ────────────────────────────────────────────────────────────
        private async Task<bool> DownloadLanguageModelAsync(string langCode, string langName, CancellationToken ct)
        {
            var runner = new MicromambaRunner(_appDir, _micromambaExePath);
            runner.LineReceived += OnRunnerLine;

            string envPrefix = Path.Combine(_appDir, ".mfa_env");
            if (!File.Exists(Path.Combine(envPrefix, "python.exe")))
            {
                OnRunnerLine(Strings.Get("Log.MfaEnvMissing1"));
                OnRunnerLine(Strings.Get("Log.MfaEnvMissing2"));
                return false;
            }

            string mfaUtilsPath = Path.Combine(_envAssetsDir, "mfa_utils.py");
            if (!File.Exists(mfaUtilsPath))
            {
                OnRunnerLine(Strings.Get("Log.MfaUtilsMissing1", mfaUtilsPath));
                OnRunnerLine(Strings.Get("Log.MfaUtilsMissing2"));
                return false;
            }

            OnRunnerLine(Strings.Get("Log.DownloadingLangModel", langCode, langName));
            string pyCode =
                "import sys; " +
                $"sys.path.insert(0, r'{_envAssetsDir}'); " +
                "from mfa_utils import MFAChecker; " +
                $"success, msg = MFAChecker.download_model('{langCode}'); " +
                "print(msg); " +
                "sys.exit(0 if success else 1)";
            bool ok = await runner.RunInEnvAsync(envPrefix, $"python -c \"{pyCode}\"", ct).ConfigureAwait(false);
            if (!ok)
            {
                OnRunnerLine(Strings.Get("Log.LangModelDownloadFailed", langName));
                return false;
            }
            OnRunnerLine(Strings.Get("Log.LangModelDownloadOk", langName));
            return true;
        }

        // ────────────────────────────────────────────────────────────
        // NeMo-FA 独立环境：与其余三个独立环境（Qwen3-ASR / Qwen3-TTS）
        // 共享"建环境"这部分逻辑，但依赖安装部分刻意不走
        // RunIsolatedEnvAsync 那套"生成 requirements 文件 + pip install -r"
        // 的流程，改成两条独立的 pip install 命令。
        //
        // 【为什么单独拎出来，不能沿用通用流程】历史 requirements-nemo.txt
        // 里必须同时出现 --extra-index-url（放在文件顶部，供 flask 等
        // 普通包正常从 PyPI 装）和 --index-url（跟在 torch/torchaudio
        // 行后面，仅 CUDA 场景才需要）。但 --index-url 出现在
        // requirements 文件中间时，pip 会把它当成对整份文件生效的全局
        // 替换 —— 一旦解析到这一行，前面 flask 等包也会改成去
        // PyTorch 官方索引（只有 torch 系列包）里找，导致 404 找不到，
        // 整个 `pip install -r` 直接失败退出，一个包都装不上。这个坑
        // 在实际使用中真实出现过（用户反馈 nemo_server.py 报
        // ModuleNotFoundError: No module named 'flask'，排查后发现
        // .nemo_env 里只有 conda 环境自带的 pip/setuptools/wheel，
        // requirements 文件从未真正装成功过一个包）。
        //
        // 解决方式：不再用单一 requirements 文件 + 一次性 `pip install -r`，
        // 改成两条独立命令，各自用各自的索引参数，互不干扰：
        //   第一步：flask + nemo_toolkit[asr] + soundfile + requests + tqdm
        //           （默认 PyPI 源，不带任何 --index-url/--extra-index-url）
        //   第二步：torch + torchaudio（仅 CUDA 场景才追加 --index-url；
        //           CPU 场景完全不传这个参数，直接从默认 PyPI 源装
        //           +cpu 后缀的 wheel，PyPI 上有这些包）
        // 两步分别判断成功/失败，即使第二步因为网络问题失败，第一步
        // 装好的 flask 等包也不会受影响，不会重现"整个环境是空的"
        // 这种情况。
        //
        // torch 版本号（2.6.0）、wheel 标签（cpu/cu118/cu121）沿用
        // TorchVariant.WheelTag()，与其它三个环境用的是同一份换算逻辑，
        // 不在这里另起一套硬编码——只是索引 URL 的拼接策略不同（这里是
        // "仅 CUDA 才加"，其它环境是"文件头部统一追加 extra-index-url"，
        // 这个差异是 nemo_toolkit 依赖链本身的要求，不是随意的实现选择，
        // 详见 TorchVariant.cs 里 IndexUrlStrategy 的注释）。
        // ────────────────────────────────────────────────────────────
        private async Task<bool> RunNemoEnvAsync(TorchVariant torch, CancellationToken ct)
        {
            const string displayName = "NeMo-FA";
            var runner = new MicromambaRunner(_appDir, _micromambaExePath);
            runner.LineReceived += OnRunnerLine;

            string envPrefix = Path.Combine(_appDir, ".nemo_env");

            if (Directory.Exists(envPrefix) && !ConfirmRecreate(displayName, envPrefix))
            {
                OnRunnerLine(Strings.Get("Log.UseExistingIsolatedEnv", displayName));
            }
            else
            {
                if (Directory.Exists(envPrefix)) Directory.Delete(envPrefix, recursive: true);
                OnRunnerLine(Strings.Get("Log.CreatingIsolatedEnv", displayName));
                if (!await runner.CreateEnvAsync(envPrefix, "conda-forge", "python=3.10 pip", ct).ConfigureAwait(false))
                {
                    OnRunnerLine(Strings.Get("Log.IsolatedEnvCreateFailed", displayName));
                    return false;
                }
            }
            OnRunnerLine(Strings.Get("Log.IsolatedEnvReady", displayName));

            OnRunnerLine(Strings.Get("Log.UpgradingPip"));
            await runner.RunInEnvAsync(envPrefix, "python -m pip install --upgrade pip setuptools wheel", ct)
                .ConfigureAwait(false);

            // 第一步：基础包，默认 PyPI 源，不带任何 index-url 参数。
            OnRunnerLine(Strings.Get("Log.NemoInstallStep1"));
            const string basePackages =
                "flask==2.3.3 \"nemo_toolkit[asr]==2.7.3\" soundfile==0.12.1 requests tqdm";
            if (!await runner.RunInEnvAsync(envPrefix, $"python -m pip install {basePackages}", ct)
                    .ConfigureAwait(false))
            {
                OnRunnerLine(Strings.Get("Log.NemoInstallStep1Failed"));
                return false;
            }

            // 第二步：torch/torchaudio，CPU 场景完全不传 --index-url
            // （直接从默认 PyPI 源装 +cpu 后缀 wheel），CUDA 场景才追加。
            // 这个"CPU 场景绝不写 index-url"的规则和历史
            // requirements-nemo.txt 注释里用大写强调过的一致，是
            // nemo_toolkit 依赖链验证过的真实约束，不是随意选择。
            OnRunnerLine(Strings.Get("Log.NemoInstallStep2"));
            string tag = torch.WheelTag();
            string torchPackages = $"torch==2.6.0+{tag} torchaudio==2.6.0+{tag}";
            string torchArgs = torch == TorchVariant.Cpu
                ? $"python -m pip install {torchPackages}"
                : $"python -m pip install {torchPackages} --index-url https://download.pytorch.org/whl/{tag}";
            if (!await runner.RunInEnvAsync(envPrefix, torchArgs, ct).ConfigureAwait(false))
            {
                OnRunnerLine(Strings.Get("Log.NemoInstallStep2Failed"));
                return false;
            }

            OnRunnerLine(Strings.Get("Log.IsolatedDepsOk", displayName));
            return true;
        }

        // ────────────────────────────────────────────────────────────
        // 独立环境（Qwen3-ASR / Qwen3-TTS），迁移自
        // 05_qwen3.bat / 06_qwen3tts.bat —— 两者结构完全一致，统一成
        // 一个参数化方法。NeMo 原本也走这个方法，现在改用上面专门的
        // RunNemoEnvAsync（见其注释说明原因），这个通用方法继续服务
        // 剩下两个仍然用"requirements 文件 + pip install -r"方式的环境。
        // ────────────────────────────────────────────────────────────
        private async Task<bool> RunIsolatedEnvAsync(
            string envDirName,
            string displayName,
            string pythonVersion,
            string requirementsContent,
            string requirementsLabel,
            CancellationToken ct)
        {
            var runner = new MicromambaRunner(_appDir, _micromambaExePath);
            runner.LineReceived += OnRunnerLine;

            string envPrefix = Path.Combine(_appDir, envDirName);

            if (Directory.Exists(envPrefix) && !ConfirmRecreate(displayName, envPrefix))
            {
                OnRunnerLine(Strings.Get("Log.UseExistingIsolatedEnv", displayName));
            }
            else
            {
                if (Directory.Exists(envPrefix)) Directory.Delete(envPrefix, recursive: true);
                OnRunnerLine(Strings.Get("Log.CreatingIsolatedEnv", displayName));
                if (!await runner.CreateEnvAsync(envPrefix, "conda-forge", $"python={pythonVersion} pip", ct).ConfigureAwait(false))
                {
                    OnRunnerLine(Strings.Get("Log.IsolatedEnvCreateFailed", displayName));
                    return false;
                }
            }
            OnRunnerLine(Strings.Get("Log.IsolatedEnvReady", displayName));

            string reqPath = RequirementsBuilder.WriteToTempFile(requirementsContent, requirementsLabel);
            if (!await runner.PipInstallAsync(envPrefix, reqPath, ct).ConfigureAwait(false))
            {
                OnRunnerLine(Strings.Get("Log.IsolatedDepsFailed", displayName));
                return false;
            }
            OnRunnerLine(Strings.Get("Log.IsolatedDepsOk", displayName));
            return true;
        }

        // ────────────────────────────────────────────────────────────
        // "环境已存在，是否删除重建" 的确认弹窗，迁移自
        // _common.bat 的 :confirm_recreate（原来是命令行 set /p 交互，
        // 这里换成原生 MessageBox，更符合 GUI 程序的交互习惯）。
        // 【注意】这个方法会阻塞调用它的线程直到用户点击弹窗按钮——
        // EnvironmentPlanner 的方法目前都是在后台 Task 线程上运行
        // （见 MainForm 里 Task.Run 的调用方式），MessageBox.Show 在
        // 非 UI 线程上调用本身是被 Windows 允许的（会创建一个独立的
        // 模态对话框，不需要绑定到某个特定的 Form），但为了保证父子
        // 窗口关系正确、弹窗置顶等行为符合预期，后续如果观察到窗口
        // 层级异常，可以改造成通过 SynchronizationContext 切回 UI
        // 线程再弹出。
        // ────────────────────────────────────────────────────────────
        private bool ConfirmRecreate(string displayName, string envPath)
        {
            var result = MessageBox.Show(
                Strings.Get("Msg.ConfirmRecreateBody", displayName, envPath),
                Strings.Get("Msg.ConfirmRecreateTitle"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }

        private void OnRunnerLine(string line)
        {
            LineReceived?.Invoke(line);
        }

        /// <summary>转发底层 MicromambaRunner 的日志行，供 MainForm 订阅显示。</summary>
        public event System.Action<string> LineReceived;
    }
}
