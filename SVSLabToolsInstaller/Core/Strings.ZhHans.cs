using System.Collections.Generic;

namespace SVSLabToolsInstaller.Core
{
    public static partial class Strings
    {
        // 简体中文——项目原始语言，也是翻译校对的基准版本。
        // key 命名规则：<来源模块前缀>.<用途简述>
        //   Ui.       = MainForm.Designer.cs 控件文本
        //   Msg.       = MessageBox 标题/正文
        //   Log.       = 安装过程日志行（EnvironmentPlanner / MicromambaRunner / MsiLauncher）
        // 带 {0}{1}... 占位符的 value，对应调用处用 Strings.Get(key, arg0, arg1...)。
        private static readonly Dictionary<string, string> _zhHans = new Dictionary<string, string>
        {
            // ── Ui: 窗口标题 ──────────────────────────────────────────
            ["Ui.WindowTitle"] = "SVS Lab Tools 安装器",

            // ── Ui: 功能选择区 ────────────────────────────────────────
            ["Ui.GroupComponents"] = "选择要安装的功能",
            ["Ui.ChkCore"] = "Core（核心环境，主运行环境，必选）",
            ["Ui.GroupLang"] = "MFA 语言模型（依赖 Core）",
            ["Ui.LangChinese"] = "中文普通话",
            ["Ui.LangEnglish"] = "英语",
            ["Ui.LangJapanese"] = "日语",
            ["Ui.LangKorean"] = "韩语",
            ["Ui.LangCantonese"] = "粤语",
            ["Ui.ChkNemo"] = "NeMo-FA（独立环境，强制对齐服务）",
            ["Ui.ChkWhisperX"] = "WhisperX（独立环境，语音识别服务）",
            ["Ui.ChkQwen3Tts"] = "Qwen3-TTS（独立环境，语音合成服务）",

            // ── Ui: PyTorch 硬件类型区 ────────────────────────────────
            ["Ui.GroupTorch"] = "PyTorch 硬件类型（影响所有环境的 torch 版本）",
            ["Ui.TorchCpu"] = "CPU（无独立显卡、非N卡，或不确定，推荐）",
            ["Ui.TorchCuda118"] = "CUDA 11.8（旧款显卡，如 GTX 10 系 Pascal 架构）",
            ["Ui.TorchCuda121"] = "CUDA 12.1（较新独立显卡，如 RTX 30/40 系）",

            // ── Ui: 安装目录区 ────────────────────────────────────────
            ["Ui.InstallDirLabel"] = "安装目录：",
            ["Ui.BrowseButton"] = "浏览...",
            ["Ui.BrowseDialogDescription"] = "选择安装目录",
            ["Ui.SizeHint"] =
                "预计磁盘占用（仅供参考，实际以下载为准）：\n" +
                "Core 约 3-5 GB（含 PyTorch）；每个语言模型约 100-500 MB；\n" +
                "NeMo-FA 约 3-4 GB；WhisperX 约 3-5 GB；Qwen3-TTS 约 4-6 GB。",

            // ── Ui: 按钮/进度 ─────────────────────────────────────────
            ["Ui.StartInstall"] = "开始安装",
            ["Ui.CancelInstall"] = "取消安装",
            ["Ui.CurrentStepRunning"] = "正在执行: {0}",
            ["Ui.CurrentStepDone"] = "安装完成",
            ["Ui.CurrentStepDoneWithFailures"] = "安装完成（{0} 项失败）",

            // ── Ui: 安装计划里的步骤名（原 step.DisplayName，现在走字符串表） ──
            ["Step.Core"] = "Core: 创建主环境（.mfa_env）与安装依赖",
            ["Step.Lang"] = "MFA 语言模型: {0}",
            ["Step.Nemo"] = "NeMo-FA: 创建独立环境与安装依赖",
            ["Step.WhisperX"] = "WhisperX: 创建独立环境与安装依赖",
            ["Step.Qwen3Tts"] = "Qwen3-TTS: 创建独立环境与安装依赖",
            ["Step.InstallMsi"] = "安装程序文件（backend/frontend）",

            // ── Msg: 缺少依赖的功能（勾了语言模型但没勾 Core） ───────────
            ["Msg.MissingCoreBody"] =
                "已选择 MFA 语言模型，但未勾选 Core（核心环境），且未检测到已存在的\n" +
                ".mfa_env 主环境。语言模型下载依赖 Core 创建的主环境，缺少它会导致\n" +
                "语言模型下载步骤失败。\n\n是否现在一并勾选 Core？",
            ["Msg.MissingCoreTitle"] = "缺少依赖的功能",

            // ── Msg: 未选择任何功能 ──────────────────────────────────
            ["Msg.NoSelectionBody"] = "请至少选择一项要安装的功能。",
            ["Msg.NoSelectionTitle"] = "未选择任何功能",

            // ── Msg: 未选择安装目录 ──────────────────────────────────
            ["Msg.NoInstallDirBody"] = "请先选择安装目录。",
            ["Msg.NoInstallDirTitle"] = "未选择安装目录",

            // ── Msg: 缺少必要文件（micromamba.exe / MSI） ────────────
            ["Msg.MissingMicromambaBody"] = "未找到 micromamba.exe：\n{0}\n\n安装包可能不完整，请重新下载。",
            ["Msg.MissingMsiBody"] = "未找到 MSI 安装包：\n{0}\n\n安装包可能不完整，请重新下载。",
            ["Msg.MissingFileTitle"] = "缺少必要文件",
            ["Msg.PayloadExtractFailedBody"] = "程序内置资源释放失败，安装包可能已损坏：\n\n{0}\n\n请重新下载完整版安装程序。",
            ["Msg.ThreadExceptionBody"] = "发生未预期的错误：\n\n{0}",
            ["Msg.ThreadExceptionTitle"] = "SVS Lab Tools 安装器 - 错误",
            ["Msg.UnhandledExceptionBody"] = "发生未预期的严重错误：\n\n{0}",
            ["Msg.UnhandledExceptionTitle"] = "SVS Lab Tools 安装器 - 严重错误",

            // ── Msg: 安装取消 ─────────────────────────────────────────
            ["Msg.CancelledBody"] = "安装已取消。",
            ["Msg.CancelledTitle"] = "已取消",

            // ── Msg: 未预期的错误 ─────────────────────────────────────
            ["Msg.UnexpectedErrorBody"] = "安装过程中发生未预期的错误：\n\n{0}",
            ["Msg.UnexpectedErrorTitle"] = "安装失败",

            // ── Msg: 安装完成（全部成功） ─────────────────────────────
            ["Msg.AllSuccessBody"] = "全部选中的功能均已安装成功。",
            ["Msg.AllSuccessRebootSuffix"] = "\n\n程序文件安装需要重启电脑才能完全生效，建议现在保存好工作并重启。",
            ["Msg.AllSuccessTitle"] = "安装完成",

            // ── Msg: 安装完成（部分失败） ─────────────────────────────
            ["Msg.PartialFailIntro"] = "安装已完成，但以下项目未能成功安装：",
            ["Msg.PartialFailOutro"] =
                "其余已勾选且未在上方列出的功能均已安装成功。\n" +
                "失败的功能可以重新运行本安装程序、只勾选失败项重试，\n" +
                "最常见的原因是网络波动，重试通常可以解决。",
            ["Msg.PartialFailTitle"] = "安装完成（部分失败）",

            // ── Msg: 关闭窗口时安装仍在进行 ───────────────────────────
            ["Msg.ClosingWhileInstallingBody"] = "安装正在进行中，确定要取消并退出吗？",
            ["Msg.ClosingWhileInstallingTitle"] = "安装进行中",

            // ── Msg: 环境已存在，是否重建（ConfirmRecreate） ──────────
            ["Msg.ConfirmRecreateBody"] = "{0} 环境已存在：\n{1}\n\n是否删除并重新创建？\n（选择\"否\"将使用现有环境）",
            ["Msg.ConfirmRecreateTitle"] = "环境已存在",
            ["Ui.MfaMainEnvName"] = "MFA 主环境",

            // ── Log: 汇总区 ───────────────────────────────────────────
            ["Log.AllDone"] = "安装全部完成",
            ["Log.DoneWithFailuresHeader"] = "安装完成，但以下项目失败：",
            ["Log.FailedItemLine"] = "  - {0}",
            ["Log.UserCancelled"] = "[!] 安装已被用户取消。",
            ["Log.StepException"] = "[ERROR] 步骤执行时抛出异常: {0}",
            ["Log.UnexpectedError"] = "[ERROR] 安装过程中发生未预期的错误: {0}",

            // ── Log: EnvironmentPlanner — Core 环境 ──────────────────
            ["Log.UseExistingMfaEnv"] = "[OK] 使用现有 MFA 主环境，跳过创建",
            ["Log.CreatingEnv"] = "创建环境中... 请耐心等待（可能需要几分钟）...",
            ["Log.EnvCreateFailed"] = "[ERROR] 环境创建失败",
            ["Log.InstallingPyAv"] = "[*] 安装 PyAV 11.0.0 二进制依赖（conda-forge）...",
            ["Log.PyAvInstallFailed"] = "[ERROR] PyAV 11.0.0 安装失败，请检查网络或上方错误。",
            ["Log.CreatingKaldiEnv"] = "[*] 创建独立的 kaldi 环境 (.kaldi_env)...",
            ["Log.KaldiInstallFailed"] = "[ERROR] kaldi 安装失败，请检查网络后重试。",
            ["Log.KaldiEnvExists"] = "[OK] .kaldi_env 已存在，跳过创建",
            ["Log.InstallingKalpy"] = "[*] 在 .kaldi_env 中安装 kalpy（MFA 的 Kaldi Python 绑定）...",
            ["Log.KalpyInstallFailed"] = "[ERROR] kalpy 安装失败，请检查网络后重试。",
            ["Log.KalpyPydMissing1"] = "[ERROR] 未找到 _kalpy.cp310-win_amd64.pyd —— .kaldi_env 里 kalpy 编译出的",
            ["Log.KalpyPydMissing2"] = "        扩展模块可能不是 Python 3.10 版本，和 .mfa_env 的 ABI 不兼容。",
            ["Log.KalpyPydMissing3"] = "        请删除后重试: {0}",
            ["Log.KalpyPydOk"] = "[OK] kalpy 的 _kalpy.cp310-win_amd64.pyd 版本与 .mfa_env 匹配",
            ["Log.InstallingPynini"] = "[*] 在 .kaldi_env 中安装 pynini（MFA 的 G2P/文本规整依赖）...",
            ["Log.PyniniFailed1"] = "[!] pynini 安装失败（非致命，不中断安装）。部分语言的文本规整/G2P",
            ["Log.PyniniFailed2"] = "    功能可能会报 ModuleNotFoundError: No module named 'pynini'。",
            ["Log.PyniniOk"] = "[OK] pynini 已安装到独立环境",
            ["Log.CoreDepsFailed"] = "[ERROR] 依赖安装失败，请检查上方报错信息。",
            ["Log.CoreDepsOk"] = "[OK] 所有 Python 依赖已安装",
            ["Log.CoreDone"] = "[OK] Core 功能安装完成",

            // ── Log: EnvironmentPlanner — speechbrain 补丁 ───────────
            ["Log.DeployingPatch"] = "[*] 部署 speechbrain Windows 路径分隔符补丁...",
            ["Log.PatchSitePackagesNotFound"] = "[!] 未能定位 .mfa_env 的 site-packages 目录，跳过 speechbrain 补丁部署",
            ["Log.PatchPathNotExist"] = "[!] 获取到的路径不存在，跳过 speechbrain 补丁部署: {0}",
            ["Log.PatchWrongLeaf"] = "[!] 获取到的路径末级目录名不是 site-packages，跳过部署避免装错位置: {0}",
            ["Log.PatchSourceMissing"] = "[!] 未找到 {0}，跳过补丁部署",
            ["Log.PatchDeployed"] = "[OK] speechbrain 补丁已部署: {0}",
            ["Log.PatchFailedNonFatal"] = "[!] speechbrain 补丁部署失败（非致命，跳过）: {0}",

            // ── Log: EnvironmentPlanner — 语言模型下载 ───────────────
            ["Log.MfaEnvMissing1"] = "[ERROR] .mfa_env 主环境不存在，无法下载语言模型。",
            ["Log.MfaEnvMissing2"] = "        请确认 Core 功能已成功安装（语言模型依赖主环境里的 mfa_utils.py）。",
            ["Log.MfaUtilsMissing1"] = "[ERROR] 未找到 {0}，无法下载语言模型。",
            ["Log.MfaUtilsMissing2"] = "        安装包可能不完整，请重新下载完整版安装程序。",
            ["Log.DownloadingLangModel"] = "[*] 下载 {0} - {1} 模型...",
            ["Log.LangModelDownloadFailed"] = "[!] {0} 模型下载失败，请检查网络后可稍后重试。",
            ["Log.LangModelDownloadOk"] = "[OK] {0} 模型已下载",

            // ── Log: EnvironmentPlanner — 独立环境（NeMo/WhisperX/Qwen3-TTS） ──
            ["Log.UseExistingIsolatedEnv"] = "[OK] 使用现有 {0} 环境，跳过创建",
            ["Log.CreatingIsolatedEnv"] = "创建 {0} 独立环境中... 请耐心等待（可能需要几分钟）...",
            ["Log.IsolatedEnvCreateFailed"] = "[ERROR] {0} 环境创建失败",
            ["Log.IsolatedEnvReady"] = "[OK] {0} 环境已准备",
            ["Log.IsolatedDepsFailed"] = "[ERROR] {0} 依赖安装失败，可稍后重试。",
            ["Log.NemoInstallStep1"] = "[*] 第一步：安装 flask / nemo_toolkit / soundfile 等基础依赖（默认 PyPI 源）...",
            ["Log.NemoInstallStep1Failed"] = "[ERROR] 基础依赖安装失败，请检查上方报错信息（可稍后重试）。",
            ["Log.NemoInstallStep2"] = "[*] 第二步：安装 torch / torchaudio（PyTorch 官方源）...",
            ["Log.NemoInstallStep2Failed"] = "[ERROR] torch/torchaudio 安装失败，请检查网络后重试（基础依赖已安装成功，无需重新执行第一步）。",
            ["Log.IsolatedDepsOk"] = "[OK] {0} 依赖已安装",

            // ── Log: MicromambaRunner ────────────────────────────────
            ["Log.AppDirEmpty"] = "appDir 不能为空",
            ["Log.MicromambaNotFound"] = "未找到 micromamba.exe，安装包可能不完整。",
            ["Log.EmbeddedResourceMissing"] = "[ERROR] 程序内嵌资源缺失: {0}（安装包可能已损坏，请重新下载）",
            ["Log.MissingRequirementsFile"] = "[ERROR] 找不到依赖文件: {0}",
            ["Log.UpgradingPip"] = "[*] 升级 pip/setuptools/wheel...",
            ["Log.InstallingRequirements"] = "[*] 根据 {0} 安装依赖（请耐心等待，过程会实时显示）...",
            ["Log.RunningCommand"] = "[cmd] micromamba {0}",
            ["Log.MicromambaStartFailed"] = "[ERROR] 启动 micromamba.exe 失败: {0}",

            // ── Log: MsiLauncher ──────────────────────────────────────
            ["Log.MsiNotFound1"] = "[ERROR] 未找到 MSI 安装包: {0}",
            ["Log.MsiNotFound2"] = "        安装包可能不完整，请重新下载完整版安装程序。",
            ["Log.MsiInstalling"] = "[*] 正在安装程序文件（backend/frontend）...",
            ["Log.MsiInstallOk"] = "[OK] 程序文件安装完成",
            ["Log.MsiInstallOkNeedsReboot"] = "[OK] 程序文件安装完成（需要重启电脑才能完全生效）",
            ["Log.MsiStartFailed"] = "[ERROR] 启动 msiexec 失败: {0}",
            ["Log.MsiInstallFailed"] = "[ERROR] MSI 安装失败，退出码: {0}",
            ["Log.MsiInstallFailedLogRef"] = "        详细日志: {0}",
            ["Log.MsiLogTailHeader"] = "        ---- msiexec 日志尾部 ----",
            ["Log.MsiLogTailFooter"] = "        ---------------------------",
        };
    }
}
