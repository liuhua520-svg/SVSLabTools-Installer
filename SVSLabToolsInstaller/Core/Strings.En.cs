using System.Collections.Generic;

namespace SVSLabToolsInstaller.Core
{
    public static partial class Strings
    {
        // English — the fallback language for any system locale that
        // doesn't precisely match zh-Hans/zh-Hant/ja/ko.
        private static readonly Dictionary<string, string> _en = new Dictionary<string, string>
        {
            ["Ui.WindowTitle"] = "SVS Lab Tools Installer",

            ["Ui.GroupComponents"] = "Select components to install",
            ["Ui.ChkCore"] = "Core (main runtime environment, recommended)",
            ["Ui.GroupLang"] = "MFA language models (requires Core)",
            ["Ui.LangChinese"] = "Mandarin Chinese",
            ["Ui.LangEnglish"] = "English",
            ["Ui.LangJapanese"] = "Japanese",
            ["Ui.LangKorean"] = "Korean",
            ["Ui.LangCantonese"] = "Cantonese",
            ["Ui.ChkNemo"] = "NeMo-FA (isolated environment, forced alignment service)",
            ["Ui.ChkQwen3Asr"] = "Qwen3-ASR (isolated environment, speech recognition service)",
            ["Ui.ChkQwen3Tts"] = "Qwen3-TTS (isolated environment, speech synthesis service)",

            ["Ui.GroupTorch"] = "PyTorch hardware type (affects the torch build used by every environment)",
            ["Ui.TorchCpu"] = "CPU (no dedicated GPU, non-NVIDIA, or unsure — recommended)",
            ["Ui.TorchCuda118"] = "CUDA 11.8 (older GPUs, e.g. GTX 10-series Pascal)",
            ["Ui.TorchCuda121"] = "CUDA 12.1 (newer dedicated GPUs, e.g. RTX 30/40-series)",

            ["Ui.InstallDirLabel"] = "Install location:",
            ["Ui.BrowseButton"] = "Browse...",
            ["Ui.BrowseDialogDescription"] = "Choose an install location",
            ["Ui.SizeHint"] =
                "Estimated disk usage (approximate — actual size depends on downloads):\n" +
                "Core ~3-5 GB (incl. PyTorch); each language model ~100-500 MB;\n" +
                "NeMo-FA ~3-4 GB; Qwen3-ASR ~4-6 GB; Qwen3-TTS ~4-6 GB.",

            ["Ui.StartInstall"] = "Install",
            ["Ui.CancelInstall"] = "Cancel Installation",
            ["Ui.CurrentStepRunning"] = "Running: {0}",
            ["Ui.CurrentStepDone"] = "Installation complete",
            ["Ui.CurrentStepDoneWithFailures"] = "Installation complete ({0} failed)",

            ["Step.Core"] = "Core: create main environment (.mfa_env) and install dependencies",
            ["Step.Lang"] = "MFA language model: {0}",
            ["Step.Nemo"] = "NeMo-FA: create isolated environment and install dependencies",
            ["Step.Qwen3Asr"] = "Qwen3-ASR: create isolated environment and install dependencies",
            ["Step.Qwen3Tts"] = "Qwen3-TTS: create isolated environment and install dependencies",
            ["Step.InstallMsi"] = "Install program files (backend/frontend)",

            ["Msg.MissingCoreBody"] =
                "You selected an MFA language model but did not check Core (the main\n" +
                "environment), and no existing .mfa_env environment was detected.\n" +
                "Downloading language models requires the main environment created by\n" +
                "Core — without it, the download step will fail.\n\n" +
                "Check Core as well now?",
            ["Msg.MissingCoreTitle"] = "Missing required component",

            ["Msg.NoSelectionBody"] = "Please select at least one component to install.",
            ["Msg.NoSelectionTitle"] = "Nothing selected",

            ["Msg.NoInstallDirBody"] = "Please choose an install location first.",
            ["Msg.NoInstallDirTitle"] = "No install location selected",

            ["Msg.MissingMicromambaBody"] = "micromamba.exe was not found:\n{0}\n\nThe installer package may be incomplete. Please download it again.",
            ["Msg.MissingMsiBody"] = "The MSI package was not found:\n{0}\n\nThe installer package may be incomplete. Please download it again.",
            ["Msg.MissingFileTitle"] = "Required file missing",
            ["Msg.PayloadExtractFailedBody"] = "Failed to extract the built-in installer resources. The package may be corrupted:\n\n{0}\n\nPlease download the full installer again.",
            ["Msg.ThreadExceptionBody"] = "An unexpected error occurred:\n\n{0}",
            ["Msg.ThreadExceptionTitle"] = "SVS Lab Tools Installer - Error",
            ["Msg.UnhandledExceptionBody"] = "An unexpected fatal error occurred:\n\n{0}",
            ["Msg.UnhandledExceptionTitle"] = "SVS Lab Tools Installer - Fatal Error",

            ["Msg.CancelledBody"] = "Installation was cancelled.",
            ["Msg.CancelledTitle"] = "Cancelled",

            ["Msg.UnexpectedErrorBody"] = "An unexpected error occurred during installation:\n\n{0}",
            ["Msg.UnexpectedErrorTitle"] = "Installation failed",

            ["Msg.AllSuccessBody"] = "All selected components were installed successfully.",
            ["Msg.AllSuccessRebootSuffix"] = "\n\nInstalling the program files requires a restart to take full effect. It's recommended to save your work and restart now.",
            ["Msg.AllSuccessTitle"] = "Installation complete",

            ["Msg.PartialFailIntro"] = "Installation finished, but the following items could not be installed:",
            ["Msg.PartialFailOutro"] =
                "All other selected components (not listed above) were installed successfully.\n" +
                "You can re-run this installer and select only the failed items to retry.\n" +
                "This is most commonly caused by network issues, and a retry usually resolves it.",
            ["Msg.PartialFailTitle"] = "Installation complete (with failures)",

            ["Msg.ClosingWhileInstallingBody"] = "Installation is in progress. Are you sure you want to cancel and exit?",
            ["Msg.ClosingWhileInstallingTitle"] = "Installation in progress",

            ["Msg.ConfirmRecreateBody"] = "The {0} environment already exists:\n{1}\n\nDelete and recreate it?\n(Choose \"No\" to keep the existing environment)",
            ["Msg.ConfirmRecreateTitle"] = "Environment already exists",
            ["Ui.MfaMainEnvName"] = "MFA main environment",

            ["Log.AllDone"] = "Installation complete",
            ["Log.DoneWithFailuresHeader"] = "Installation complete, but the following items failed:",
            ["Log.FailedItemLine"] = "  - {0}",
            ["Log.UserCancelled"] = "[!] Installation was cancelled by the user.",
            ["Log.StepException"] = "[ERROR] Step threw an exception: {0}",
            ["Log.UnexpectedError"] = "[ERROR] An unexpected error occurred during installation: {0}",

            ["Log.UseExistingMfaEnv"] = "[OK] Using existing MFA main environment, skipping creation",
            ["Log.CreatingEnv"] = "Creating environment... please be patient (this may take a few minutes)...",
            ["Log.EnvCreateFailed"] = "[ERROR] Environment creation failed",
            ["Log.InstallingPyAv"] = "[*] Installing PyAV 11.0.0 binary dependency (conda-forge)...",
            ["Log.PyAvInstallFailed"] = "[ERROR] Failed to install PyAV 11.0.0. Please check your network connection or the error above.",
            ["Log.CreatingKaldiEnv"] = "[*] Creating isolated kaldi environment (.kaldi_env)...",
            ["Log.KaldiInstallFailed"] = "[ERROR] Failed to install kaldi. Please check your network connection and try again.",
            ["Log.KaldiEnvExists"] = "[OK] .kaldi_env already exists, skipping creation",
            ["Log.InstallingKalpy"] = "[*] Installing kalpy (MFA's Kaldi Python bindings) into .kaldi_env...",
            ["Log.KalpyInstallFailed"] = "[ERROR] Failed to install kalpy. Please check your network connection and try again.",
            ["Log.KalpyPydMissing1"] = "[ERROR] _kalpy.cp310-win_amd64.pyd was not found — the compiled kalpy",
            ["Log.KalpyPydMissing2"] = "        extension in .kaldi_env may not be Python 3.10, causing an ABI mismatch with .mfa_env.",
            ["Log.KalpyPydMissing3"] = "        Please delete and retry: {0}",
            ["Log.KalpyPydOk"] = "[OK] kalpy's _kalpy.cp310-win_amd64.pyd version matches .mfa_env",
            ["Log.InstallingPynini"] = "[*] Installing pynini (MFA's G2P/text normalization dependency) into .kaldi_env...",
            ["Log.PyniniFailed1"] = "[!] Failed to install pynini (non-fatal, installation continues). Text normalization/G2P",
            ["Log.PyniniFailed2"] = "    for some languages may report ModuleNotFoundError: No module named 'pynini'.",
            ["Log.PyniniOk"] = "[OK] pynini installed into the isolated environment",
            ["Log.CoreDepsFailed"] = "[ERROR] Failed to install dependencies. Please check the error output above.",
            ["Log.CoreDepsOk"] = "[OK] All Python dependencies installed",
            ["Log.CoreDone"] = "[OK] Core installation complete",

            ["Log.DeployingPatch"] = "[*] Deploying speechbrain Windows path separator patch...",
            ["Log.PatchSitePackagesNotFound"] = "[!] Could not locate the site-packages directory of .mfa_env, skipping speechbrain patch",
            ["Log.PatchPathNotExist"] = "[!] The resolved path does not exist, skipping speechbrain patch: {0}",
            ["Log.PatchWrongLeaf"] = "[!] The resolved path's last folder is not named site-packages, skipping to avoid installing to the wrong location: {0}",
            ["Log.PatchSourceMissing"] = "[!] {0} not found, skipping patch deployment",
            ["Log.PatchDeployed"] = "[OK] speechbrain patch deployed: {0}",
            ["Log.PatchFailedNonFatal"] = "[!] Failed to deploy the speechbrain patch (non-fatal, skipped): {0}",

            ["Log.MfaEnvMissing1"] = "[ERROR] The .mfa_env main environment does not exist, cannot download language models.",
            ["Log.MfaEnvMissing2"] = "        Please make sure Core was installed successfully (language models depend on mfa_utils.py in the main environment).",
            ["Log.MfaUtilsMissing1"] = "[ERROR] {0} not found, cannot download language models.",
            ["Log.MfaUtilsMissing2"] = "        The installer package may be incomplete. Please download the full installer again.",
            ["Log.DownloadingLangModel"] = "[*] Downloading {0} - {1} model...",
            ["Log.LangModelDownloadFailed"] = "[!] Failed to download the {0} model. Please check your network connection and try again later.",
            ["Log.LangModelDownloadOk"] = "[OK] {0} model downloaded",

            ["Log.UseExistingIsolatedEnv"] = "[OK] Using existing {0} environment, skipping creation",
            ["Log.CreatingIsolatedEnv"] = "Creating isolated {0} environment... please be patient (this may take a few minutes)...",
            ["Log.IsolatedEnvCreateFailed"] = "[ERROR] Failed to create the {0} environment",
            ["Log.IsolatedEnvReady"] = "[OK] {0} environment is ready",
            ["Log.IsolatedDepsFailed"] = "[ERROR] Failed to install {0} dependencies. You can try again later.",
            ["Log.NemoInstallStep1"] = "[*] Step 1: installing base dependencies — flask / nemo_toolkit / soundfile, etc. (default PyPI index)...",
            ["Log.NemoInstallStep1Failed"] = "[ERROR] Failed to install base dependencies. Please check the error output above (you can try again later).",
            ["Log.NemoInstallStep2"] = "[*] Step 2: installing torch / torchaudio (official PyTorch index)...",
            ["Log.NemoInstallStep2Failed"] = "[ERROR] Failed to install torch/torchaudio. Please check your network connection and try again (the base dependencies from step 1 are already installed, no need to redo that step).",
            ["Log.IsolatedDepsOk"] = "[OK] {0} dependencies installed",

            ["Log.AppDirEmpty"] = "appDir must not be empty",
            ["Log.MicromambaNotFound"] = "micromamba.exe was not found. The installer package may be incomplete.",
            ["Log.EmbeddedResourceMissing"] = "[ERROR] Missing embedded resource: {0} (the installer package may be corrupted — please download it again)",
            ["Log.MissingRequirementsFile"] = "[ERROR] Requirements file not found: {0}",
            ["Log.UpgradingPip"] = "[*] Upgrading pip/setuptools/wheel...",
            ["Log.InstallingRequirements"] = "[*] Installing dependencies from {0} (please be patient, progress is shown live)...",
            ["Log.RunningCommand"] = "[cmd] micromamba {0}",
            ["Log.MicromambaStartFailed"] = "[ERROR] Failed to start micromamba.exe: {0}",

            ["Log.MsiNotFound1"] = "[ERROR] MSI package not found: {0}",
            ["Log.MsiNotFound2"] = "        The installer package may be incomplete. Please download the full installer again.",
            ["Log.MsiInstalling"] = "[*] Installing program files (backend/frontend)...",
            ["Log.MsiInstallOk"] = "[OK] Program files installed",
            ["Log.MsiInstallOkNeedsReboot"] = "[OK] Program files installed (a restart is required to take full effect)",
            ["Log.MsiStartFailed"] = "[ERROR] Failed to start msiexec: {0}",
            ["Log.MsiInstallFailed"] = "[ERROR] MSI installation failed, exit code: {0}",
            ["Log.MsiInstallFailedLogRef"] = "        Full log: {0}",
            ["Log.MsiLogTailHeader"] = "        ---- msiexec log tail ----",
            ["Log.MsiLogTailFooter"] = "        ---------------------------",
        };
    }
}
