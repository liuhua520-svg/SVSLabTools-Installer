using System.Collections.Generic;

namespace SVSLabToolsInstaller.Core
{
    public static partial class Strings
    {
        // 繁体中文（採用台灣地區慣用詞彙，如「網路」「硬碟」「軟體」）。
        private static readonly Dictionary<string, string> _zhHant = new Dictionary<string, string>
        {
            ["Ui.WindowTitle"] = "SVS Lab Tools 安裝程式",

            ["Ui.GroupComponents"] = "選擇要安裝的功能",
            ["Ui.ChkCore"] = "Core（核心環境，主運行環境，必選）",
            ["Ui.GroupLang"] = "MFA 語言模型（依賴 Core）",
            ["Ui.LangChinese"] = "中文普通話",
            ["Ui.LangEnglish"] = "英語",
            ["Ui.LangJapanese"] = "日語",
            ["Ui.LangKorean"] = "韓語",
            ["Ui.LangCantonese"] = "粵語",
            ["Ui.ChkNemo"] = "NeMo-FA（獨立環境，強制對齊服務）",
            ["Ui.ChkWhisperX"] = "WhisperX（獨立環境，語音辨識服務）",
            ["Ui.ChkQwen3Tts"] = "Qwen3-TTS（獨立環境，語音合成服務）",

            ["Ui.GroupTorch"] = "PyTorch 硬體類型（影響所有環境的 torch 版本）",
            ["Ui.TorchCpu"] = "CPU（無獨立顯示卡、非 N 卡，或不確定，推薦）",
            ["Ui.TorchCuda118"] = "CUDA 11.8（舊款顯示卡，如 GTX 10 系列 Pascal 架構）",
            ["Ui.TorchCuda121"] = "CUDA 12.1（較新獨立顯示卡，如 RTX 30/40 系列）",

            ["Ui.InstallDirLabel"] = "安裝目錄：",
            ["Ui.BrowseButton"] = "瀏覽...",
            ["Ui.BrowseDialogDescription"] = "選擇安裝目錄",
            ["Ui.SizeHint"] =
                "預估磁碟空間占用（僅供參考，實際以下載為準）：\n" +
                "Core 約 3-5 GB（含 PyTorch）；每個語言模型約 100-500 MB；\n" +
                "NeMo-FA 約 3-4 GB；WhisperX 約 3-5 GB；Qwen3-TTS 約 4-6 GB。",

            ["Ui.StartInstall"] = "開始安裝",
            ["Ui.CancelInstall"] = "取消安裝",
            ["Ui.CurrentStepRunning"] = "正在執行: {0}",
            ["Ui.CurrentStepDone"] = "安裝完成",
            ["Ui.CurrentStepDoneWithFailures"] = "安裝完成（{0} 項失敗）",

            ["Step.Core"] = "Core：建立主環境（.mfa_env）並安裝依賴套件",
            ["Step.Lang"] = "MFA 語言模型: {0}",
            ["Step.Nemo"] = "NeMo-FA：建立獨立環境並安裝依賴套件",
            ["Step.WhisperX"] = "WhisperX：建立獨立環境並安裝依賴套件",
            ["Step.Qwen3Tts"] = "Qwen3-TTS：建立獨立環境並安裝依賴套件",
            ["Step.InstallMsi"] = "安裝程式檔案（backend/frontend）",

            ["Msg.MissingCoreBody"] =
                "已選擇 MFA 語言模型，但未勾選 Core（核心環境），且未偵測到已存在的\n" +
                ".mfa_env 主環境。語言模型下載依賴 Core 建立的主環境，缺少它會導致\n" +
                "語言模型下載步驟失敗。\n\n是否現在一併勾選 Core？",
            ["Msg.MissingCoreTitle"] = "缺少依賴的功能",

            ["Msg.NoSelectionBody"] = "請至少選擇一項要安裝的功能。",
            ["Msg.NoSelectionTitle"] = "未選擇任何功能",

            ["Msg.NoInstallDirBody"] = "請先選擇安裝目錄。",
            ["Msg.NoInstallDirTitle"] = "未選擇安裝目錄",

            ["Msg.MissingMicromambaBody"] = "找不到 micromamba.exe：\n{0}\n\n安裝套件可能不完整，請重新下載。",
            ["Msg.MissingMsiBody"] = "找不到 MSI 安裝套件：\n{0}\n\n安裝套件可能不完整，請重新下載。",
            ["Msg.MissingFileTitle"] = "缺少必要檔案",
            ["Msg.PayloadExtractFailedBody"] = "程式內建資源釋放失敗，安裝套件可能已損壞：\n\n{0}\n\n請重新下載完整版安裝程式。",
            ["Msg.ThreadExceptionBody"] = "發生未預期的錯誤：\n\n{0}",
            ["Msg.ThreadExceptionTitle"] = "SVS Lab Tools 安裝程式 - 錯誤",
            ["Msg.UnhandledExceptionBody"] = "發生未預期的嚴重錯誤：\n\n{0}",
            ["Msg.UnhandledExceptionTitle"] = "SVS Lab Tools 安裝程式 - 嚴重錯誤",

            ["Msg.CancelledBody"] = "安裝已取消。",
            ["Msg.CancelledTitle"] = "已取消",

            ["Msg.UnexpectedErrorBody"] = "安裝過程中發生未預期的錯誤：\n\n{0}",
            ["Msg.UnexpectedErrorTitle"] = "安裝失敗",

            ["Msg.AllSuccessBody"] = "所有已選取的功能均已安裝成功。",
            ["Msg.AllSuccessRebootSuffix"] = "\n\n程式檔案安裝需要重新啟動電腦才能完全生效，建議現在儲存好工作並重新啟動。",
            ["Msg.AllSuccessTitle"] = "安裝完成",

            ["Msg.PartialFailIntro"] = "安裝已完成，但以下項目未能成功安裝：",
            ["Msg.PartialFailOutro"] =
                "其餘已勾選且未在上方列出的功能均已安裝成功。\n" +
                "失敗的功能可以重新執行本安裝程式、只勾選失敗項目重試，\n" +
                "最常見的原因是網路波動，重試通常可以解決。",
            ["Msg.PartialFailTitle"] = "安裝完成（部分失敗）",

            ["Msg.ClosingWhileInstallingBody"] = "安裝正在進行中，確定要取消並結束嗎？",
            ["Msg.ClosingWhileInstallingTitle"] = "安裝進行中",

            ["Msg.ConfirmRecreateBody"] = "{0} 環境已存在：\n{1}\n\n是否刪除並重新建立？\n（選擇「否」將使用現有環境）",
            ["Msg.ConfirmRecreateTitle"] = "環境已存在",
            ["Ui.MfaMainEnvName"] = "MFA 主環境",

            ["Log.AllDone"] = "安裝全部完成",
            ["Log.DoneWithFailuresHeader"] = "安裝完成，但以下項目失敗：",
            ["Log.FailedItemLine"] = "  - {0}",
            ["Log.UserCancelled"] = "[!] 安裝已被使用者取消。",
            ["Log.StepException"] = "[ERROR] 步驟執行時擲回例外狀況: {0}",
            ["Log.UnexpectedError"] = "[ERROR] 安裝過程中發生未預期的錯誤: {0}",

            ["Log.UseExistingMfaEnv"] = "[OK] 使用現有 MFA 主環境，跳過建立",
            ["Log.CreatingEnv"] = "建立環境中... 請耐心等候（可能需要幾分鐘）...",
            ["Log.EnvCreateFailed"] = "[ERROR] 環境建立失敗",
            ["Log.InstallingPyAv"] = "[*] 安裝 PyAV 11.0.0 二進位依賴套件（conda-forge）...",
            ["Log.PyAvInstallFailed"] = "[ERROR] PyAV 11.0.0 安裝失敗，請檢查網路或上方錯誤訊息。",
            ["Log.CreatingKaldiEnv"] = "[*] 建立獨立的 kaldi 環境 (.kaldi_env)...",
            ["Log.KaldiInstallFailed"] = "[ERROR] kaldi 安裝失敗，請檢查網路後重試。",
            ["Log.KaldiEnvExists"] = "[OK] .kaldi_env 已存在，跳過建立",
            ["Log.InstallingKalpy"] = "[*] 在 .kaldi_env 中安裝 kalpy（MFA 的 Kaldi Python 綁定）...",
            ["Log.KalpyInstallFailed"] = "[ERROR] kalpy 安裝失敗，請檢查網路後重試。",
            ["Log.KalpyPydMissing1"] = "[ERROR] 找不到 _kalpy.cp310-win_amd64.pyd —— .kaldi_env 中 kalpy 編譯出的",
            ["Log.KalpyPydMissing2"] = "        擴充模組可能不是 Python 3.10 版本，與 .mfa_env 的 ABI 不相容。",
            ["Log.KalpyPydMissing3"] = "        請刪除後重試: {0}",
            ["Log.KalpyPydOk"] = "[OK] kalpy 的 _kalpy.cp310-win_amd64.pyd 版本與 .mfa_env 相符",
            ["Log.InstallingPynini"] = "[*] 在 .kaldi_env 中安裝 pynini（MFA 的 G2P/文字正規化依賴套件）...",
            ["Log.PyniniFailed1"] = "[!] pynini 安裝失敗（非致命，不中斷安裝）。部分語言的文字正規化/G2P",
            ["Log.PyniniFailed2"] = "    功能可能會出現 ModuleNotFoundError: No module named 'pynini'。",
            ["Log.PyniniOk"] = "[OK] pynini 已安裝到獨立環境",
            ["Log.CoreDepsFailed"] = "[ERROR] 依賴套件安裝失敗，請檢查上方錯誤訊息。",
            ["Log.CoreDepsOk"] = "[OK] 所有 Python 依賴套件已安裝",
            ["Log.CoreDone"] = "[OK] Core 功能安裝完成",

            ["Log.DeployingPatch"] = "[*] 部署 speechbrain Windows 路徑分隔符號修補...",
            ["Log.PatchSitePackagesNotFound"] = "[!] 未能定位 .mfa_env 的 site-packages 目錄，跳過 speechbrain 修補部署",
            ["Log.PatchPathNotExist"] = "[!] 取得的路徑不存在，跳過 speechbrain 修補部署: {0}",
            ["Log.PatchWrongLeaf"] = "[!] 取得的路徑末層目錄名稱不是 site-packages，跳過部署以避免裝錯位置: {0}",
            ["Log.PatchSourceMissing"] = "[!] 找不到 {0}，跳過修補部署",
            ["Log.PatchDeployed"] = "[OK] speechbrain 修補已部署: {0}",
            ["Log.PatchFailedNonFatal"] = "[!] speechbrain 修補部署失敗（非致命，跳過）: {0}",

            ["Log.MfaEnvMissing1"] = "[ERROR] .mfa_env 主環境不存在，無法下載語言模型。",
            ["Log.MfaEnvMissing2"] = "        請確認 Core 功能已成功安裝（語言模型依賴主環境中的 mfa_utils.py）。",
            ["Log.MfaUtilsMissing1"] = "[ERROR] 找不到 {0}，無法下載語言模型。",
            ["Log.MfaUtilsMissing2"] = "        安裝套件可能不完整，請重新下載完整版安裝程式。",
            ["Log.DownloadingLangModel"] = "[*] 下載 {0} - {1} 模型...",
            ["Log.LangModelDownloadFailed"] = "[!] {0} 模型下載失敗，請檢查網路後稍後再試。",
            ["Log.LangModelDownloadOk"] = "[OK] {0} 模型已下載",

            ["Log.UseExistingIsolatedEnv"] = "[OK] 使用現有 {0} 環境，跳過建立",
            ["Log.CreatingIsolatedEnv"] = "建立 {0} 獨立環境中... 請耐心等候（可能需要幾分鐘）...",
            ["Log.IsolatedEnvCreateFailed"] = "[ERROR] {0} 環境建立失敗",
            ["Log.IsolatedEnvReady"] = "[OK] {0} 環境已準備完成",
            ["Log.IsolatedDepsFailed"] = "[ERROR] {0} 依賴套件安裝失敗，可稍後重試。",
            ["Log.NemoInstallStep1"] = "[*] 第一步：安裝 flask / nemo_toolkit / soundfile 等基礎依賴（預設 PyPI 來源）...",
            ["Log.NemoInstallStep1Failed"] = "[ERROR] 基礎依賴安裝失敗，請檢查上方錯誤訊息（可稍後重試）。",
            ["Log.NemoInstallStep2"] = "[*] 第二步：安裝 torch / torchaudio（PyTorch 官方來源）...",
            ["Log.NemoInstallStep2Failed"] = "[ERROR] torch/torchaudio 安裝失敗，請檢查網路後重試（基礎依賴已安裝成功，不需要重新執行第一步）。",
            ["Log.IsolatedDepsOk"] = "[OK] {0} 依賴套件已安裝",

            ["Log.AppDirEmpty"] = "appDir 不能為空",
            ["Log.MicromambaNotFound"] = "找不到 micromamba.exe，安裝套件可能不完整。",
            ["Log.EmbeddedResourceMissing"] = "[ERROR] 程式內嵌資源缺失: {0}（安裝套件可能已損壞，請重新下載）",
            ["Log.MissingRequirementsFile"] = "[ERROR] 找不到依賴套件檔案: {0}",
            ["Log.UpgradingPip"] = "[*] 升級 pip/setuptools/wheel...",
            ["Log.InstallingRequirements"] = "[*] 根據 {0} 安裝依賴套件（請耐心等候，過程會即時顯示）...",
            ["Log.RunningCommand"] = "[cmd] micromamba {0}",
            ["Log.MicromambaStartFailed"] = "[ERROR] 啟動 micromamba.exe 失敗: {0}",

            ["Log.MsiNotFound1"] = "[ERROR] 找不到 MSI 安裝套件: {0}",
            ["Log.MsiNotFound2"] = "        安裝套件可能不完整，請重新下載完整版安裝程式。",
            ["Log.MsiInstalling"] = "[*] 正在安裝程式檔案（backend/frontend）...",
            ["Log.MsiInstallOk"] = "[OK] 程式檔案安裝完成",
            ["Log.MsiInstallOkNeedsReboot"] = "[OK] 程式檔案安裝完成（需要重新啟動電腦才能完全生效）",
            ["Log.MsiStartFailed"] = "[ERROR] 啟動 msiexec 失敗: {0}",
            ["Log.MsiInstallFailed"] = "[ERROR] MSI 安裝失敗，結束碼: {0}",
            ["Log.MsiInstallFailedLogRef"] = "        詳細日誌: {0}",
            ["Log.MsiLogTailHeader"] = "        ---- msiexec 日誌尾端 ----",
            ["Log.MsiLogTailFooter"] = "        ---------------------------",
        };
    }
}
