using System.Collections.Generic;

namespace SVSLabToolsInstaller.Core
{
    public static partial class Strings
    {
        // 한국어
        private static readonly Dictionary<string, string> _ko = new Dictionary<string, string>
        {
            ["Ui.WindowTitle"] = "SVS Lab Tools 설치 프로그램",

            ["Ui.GroupComponents"] = "설치할 기능 선택",
            ["Ui.ChkCore"] = "Core(핵심 환경, 기본 실행 환경, 선택 필요)",
            ["Ui.GroupLang"] = "MFA 언어 모델(Core 필요)",
            ["Ui.LangChinese"] = "중국어(표준어)",
            ["Ui.LangEnglish"] = "영어",
            ["Ui.LangJapanese"] = "일본어",
            ["Ui.LangKorean"] = "한국어",
            ["Ui.LangCantonese"] = "광둥어",
            ["Ui.ChkNemo"] = "NeMo-FA(독립 환경, 강제 정렬 서비스)",
            ["Ui.ChkWhisperX"] = "WhisperX(독립 환경, 음성 인식 서비스)",
            ["Ui.ChkQwen3Tts"] = "Qwen3-TTS(독립 환경, 음성 합성 서비스)",

            ["Ui.GroupTorch"] = "PyTorch 하드웨어 유형(모든 환경의 torch 버전에 영향)",
            ["Ui.TorchCpu"] = "CPU(전용 GPU 없음, NVIDIA 아님, 또는 잘 모를 경우 — 권장)",
            ["Ui.TorchCuda118"] = "CUDA 11.8(구형 GPU, 예: GTX 10 시리즈 Pascal 아키텍처)",
            ["Ui.TorchCuda121"] = "CUDA 12.1(비교적 최신 전용 GPU, 예: RTX 30/40 시리즈)",

            ["Ui.InstallDirLabel"] = "설치 위치:",
            ["Ui.BrowseButton"] = "찾아보기...",
            ["Ui.BrowseDialogDescription"] = "설치 위치 선택",
            ["Ui.SizeHint"] =
                "예상 디스크 사용량(참고용이며 실제 크기는 다운로드 내용에 따라 달라짐):\n" +
                "Core 약 3-5GB(PyTorch 포함); 언어 모델당 약 100-500MB;\n" +
                "NeMo-FA 약 3-4GB; WhisperX 약 3-5GB; Qwen3-TTS 약 4-6GB.",

            ["Ui.StartInstall"] = "설치 시작",
            ["Ui.CancelInstall"] = "설치 취소",
            ["Ui.CurrentStepRunning"] = "실행 중: {0}",
            ["Ui.CurrentStepDone"] = "설치 완료",
            ["Ui.CurrentStepDoneWithFailures"] = "설치 완료({0}개 실패)",

            ["Step.Core"] = "Core: 기본 환경(.mfa_env) 생성 및 종속성 설치",
            ["Step.Lang"] = "MFA 언어 모델: {0}",
            ["Step.Nemo"] = "NeMo-FA: 독립 환경 생성 및 종속성 설치",
            ["Step.WhisperX"] = "WhisperX: 독립 환경 생성 및 종속성 설치",
            ["Step.Qwen3Tts"] = "Qwen3-TTS: 독립 환경 생성 및 종속성 설치",
            ["Step.InstallMsi"] = "프로그램 파일 설치(backend/frontend)",

            ["Msg.MissingCoreBody"] =
                "MFA 언어 모델을 선택했지만 Core(핵심 환경)를 선택하지 않았으며,\n" +
                "기존 .mfa_env 기본 환경도 감지되지 않았습니다. 언어 모델 다운로드는\n" +
                "Core가 생성하는 기본 환경이 필요하며, 이것이 없으면 언어 모델 다운로드\n" +
                "단계가 실패합니다.\n\n지금 Core도 함께 선택하시겠습니까？",
            ["Msg.MissingCoreTitle"] = "필요한 기능이 누락되었습니다",

            ["Msg.NoSelectionBody"] = "설치할 기능을 하나 이상 선택하세요.",
            ["Msg.NoSelectionTitle"] = "선택된 기능이 없습니다",

            ["Msg.NoInstallDirBody"] = "먼저 설치 위치를 선택하세요.",
            ["Msg.NoInstallDirTitle"] = "설치 위치가 선택되지 않았습니다",

            ["Msg.MissingMicromambaBody"] = "micromamba.exe를 찾을 수 없습니다:\n{0}\n\n설치 패키지가 손상되었을 수 있습니다. 다시 다운로드해 주세요.",
            ["Msg.MissingMsiBody"] = "MSI 패키지를 찾을 수 없습니다:\n{0}\n\n설치 패키지가 손상되었을 수 있습니다. 다시 다운로드해 주세요.",
            ["Msg.MissingFileTitle"] = "필수 파일 누락",
            ["Msg.PayloadExtractFailedBody"] = "내장 리소스 압축 해제에 실패했습니다. 설치 패키지가 손상되었을 수 있습니다:\n\n{0}\n\n전체 설치 프로그램을 다시 다운로드하세요.",
            ["Msg.ThreadExceptionBody"] = "예기치 않은 오류가 발생했습니다:\n\n{0}",
            ["Msg.ThreadExceptionTitle"] = "SVS Lab Tools 설치 프로그램 - 오류",
            ["Msg.UnhandledExceptionBody"] = "예기치 않은 심각한 오류가 발생했습니다:\n\n{0}",
            ["Msg.UnhandledExceptionTitle"] = "SVS Lab Tools 설치 프로그램 - 심각한 오류",

            ["Msg.CancelledBody"] = "설치가 취소되었습니다.",
            ["Msg.CancelledTitle"] = "취소됨",

            ["Msg.UnexpectedErrorBody"] = "설치 중 예기치 않은 오류가 발생했습니다:\n\n{0}",
            ["Msg.UnexpectedErrorTitle"] = "설치 실패",

            ["Msg.AllSuccessBody"] = "선택한 모든 기능이 성공적으로 설치되었습니다.",
            ["Msg.AllSuccessRebootSuffix"] = "\n\n프로그램 파일 설치를 완전히 적용하려면 재부팅이 필요합니다. 지금 작업 내용을 저장한 후 재부팅하는 것을 권장합니다.",
            ["Msg.AllSuccessTitle"] = "설치 완료",

            ["Msg.PartialFailIntro"] = "설치가 완료되었지만 다음 항목은 설치하지 못했습니다:",
            ["Msg.PartialFailOutro"] =
                "위에 나열되지 않은 나머지 선택 기능은 모두 정상적으로 설치되었습니다.\n" +
                "실패한 기능은 이 설치 프로그램을 다시 실행하여 실패한 항목만 선택해\n" +
                "재시도할 수 있습니다. 가장 흔한 원인은 네트워크 문제이며, 재시도로 대부분 해결됩니다.",
            ["Msg.PartialFailTitle"] = "설치 완료(일부 실패)",

            ["Msg.ClosingWhileInstallingBody"] = "설치가 진행 중입니다. 취소하고 종료하시겠습니까？",
            ["Msg.ClosingWhileInstallingTitle"] = "설치 진행 중",

            ["Msg.ConfirmRecreateBody"] = "{0} 환경이 이미 존재합니다:\n{1}\n\n삭제하고 다시 생성하시겠습니까？\n(\"아니요\"를 선택하면 기존 환경을 사용합니다)",
            ["Msg.ConfirmRecreateTitle"] = "환경이 이미 존재합니다",
            ["Ui.MfaMainEnvName"] = "MFA 기본 환경",

            ["Log.AllDone"] = "모든 설치가 완료되었습니다",
            ["Log.DoneWithFailuresHeader"] = "설치가 완료되었지만 다음 항목이 실패했습니다:",
            ["Log.FailedItemLine"] = "  - {0}",
            ["Log.UserCancelled"] = "[!] 사용자가 설치를 취소했습니다.",
            ["Log.StepException"] = "[ERROR] 단계 실행 중 예외가 발생했습니다: {0}",
            ["Log.UnexpectedError"] = "[ERROR] 설치 중 예기치 않은 오류가 발생했습니다: {0}",

            ["Log.UseExistingMfaEnv"] = "[OK] 기존 MFA 기본 환경을 사용합니다(생성 건너뜀)",
            ["Log.CreatingEnv"] = "환경을 생성하는 중입니다... 잠시만 기다려 주세요(몇 분 정도 걸릴 수 있습니다)...",
            ["Log.EnvCreateFailed"] = "[ERROR] 환경 생성에 실패했습니다",
            ["Log.InstallingPyAv"] = "[*] PyAV 11.0.0 바이너리 종속성을 설치하는 중입니다(conda-forge)...",
            ["Log.PyAvInstallFailed"] = "[ERROR] PyAV 11.0.0 설치에 실패했습니다. 네트워크 연결 또는 위의 오류를 확인하세요.",
            ["Log.CreatingKaldiEnv"] = "[*] 독립된 kaldi 환경(.kaldi_env)을 생성하는 중입니다...",
            ["Log.KaldiInstallFailed"] = "[ERROR] kaldi 설치에 실패했습니다. 네트워크 연결을 확인하고 다시 시도하세요.",
            ["Log.KaldiEnvExists"] = "[OK] .kaldi_env가 이미 존재합니다(생성 건너뜀)",
            ["Log.InstallingKalpy"] = "[*] .kaldi_env에 kalpy(MFA의 Kaldi Python 바인딩)를 설치하는 중입니다...",
            ["Log.KalpyInstallFailed"] = "[ERROR] kalpy 설치에 실패했습니다. 네트워크 연결을 확인하고 다시 시도하세요.",
            ["Log.KalpyPydMissing1"] = "[ERROR] _kalpy.cp310-win_amd64.pyd를 찾을 수 없습니다 —— .kaldi_env에서 컴파일된 kalpy",
            ["Log.KalpyPydMissing2"] = "        확장 모듈이 Python 3.10 버전이 아니어서 .mfa_env와 ABI가 호환되지 않을 수 있습니다.",
            ["Log.KalpyPydMissing3"] = "        삭제 후 다시 시도하세요: {0}",
            ["Log.KalpyPydOk"] = "[OK] kalpy의 _kalpy.cp310-win_amd64.pyd 버전이 .mfa_env와 일치합니다",
            ["Log.InstallingPynini"] = "[*] .kaldi_env에 pynini(MFA의 G2P/텍스트 정규화 종속성)를 설치하는 중입니다...",
            ["Log.PyniniFailed1"] = "[!] pynini 설치에 실패했습니다(치명적이지 않으므로 설치를 계속합니다). 일부 언어의 텍스트 정규화/G2P",
            ["Log.PyniniFailed2"] = "    기능에서 ModuleNotFoundError: No module named 'pynini' 오류가 발생할 수 있습니다.",
            ["Log.PyniniOk"] = "[OK] pynini가 독립 환경에 설치되었습니다",
            ["Log.CoreDepsFailed"] = "[ERROR] 종속성 설치에 실패했습니다. 위의 오류 내용을 확인하세요.",
            ["Log.CoreDepsOk"] = "[OK] 모든 Python 종속성이 설치되었습니다",
            ["Log.CoreDone"] = "[OK] Core 기능 설치가 완료되었습니다",

            ["Log.DeployingPatch"] = "[*] speechbrain Windows 경로 구분자 패치를 배포하는 중입니다...",
            ["Log.PatchSitePackagesNotFound"] = "[!] .mfa_env의 site-packages 디렉터리를 찾을 수 없어 speechbrain 패치 배포를 건너뜁니다",
            ["Log.PatchPathNotExist"] = "[!] 확인된 경로가 존재하지 않아 speechbrain 패치 배포를 건너뜁니다: {0}",
            ["Log.PatchWrongLeaf"] = "[!] 확인된 경로의 마지막 폴더 이름이 site-packages가 아니어서 잘못된 위치에 설치되는 것을 방지하기 위해 건너뜁니다: {0}",
            ["Log.PatchSourceMissing"] = "[!] {0}을(를) 찾을 수 없어 패치 배포를 건너뜁니다",
            ["Log.PatchDeployed"] = "[OK] speechbrain 패치가 배포되었습니다: {0}",
            ["Log.PatchFailedNonFatal"] = "[!] speechbrain 패치 배포에 실패했습니다(치명적이지 않으므로 건너뜀): {0}",

            ["Log.MfaEnvMissing1"] = "[ERROR] .mfa_env 기본 환경이 존재하지 않아 언어 모델을 다운로드할 수 없습니다.",
            ["Log.MfaEnvMissing2"] = "        Core 기능이 성공적으로 설치되었는지 확인하세요(언어 모델은 기본 환경의 mfa_utils.py에 의존합니다).",
            ["Log.MfaUtilsMissing1"] = "[ERROR] {0}을(를) 찾을 수 없어 언어 모델을 다운로드할 수 없습니다.",
            ["Log.MfaUtilsMissing2"] = "        설치 패키지가 손상되었을 수 있습니다. 전체 설치 프로그램을 다시 다운로드하세요.",
            ["Log.DownloadingLangModel"] = "[*] {0} - {1} 모델을 다운로드하는 중입니다...",
            ["Log.LangModelDownloadFailed"] = "[!] {0} 모델 다운로드에 실패했습니다. 네트워크 연결을 확인한 후 나중에 다시 시도하세요.",
            ["Log.LangModelDownloadOk"] = "[OK] {0} 모델이 다운로드되었습니다",

            ["Log.UseExistingIsolatedEnv"] = "[OK] 기존 {0} 환경을 사용합니다(생성 건너뜀)",
            ["Log.CreatingIsolatedEnv"] = "{0} 독립 환경을 생성하는 중입니다... 잠시만 기다려 주세요(몇 분 정도 걸릴 수 있습니다)...",
            ["Log.IsolatedEnvCreateFailed"] = "[ERROR] {0} 환경 생성에 실패했습니다",
            ["Log.IsolatedEnvReady"] = "[OK] {0} 환경이 준비되었습니다",
            ["Log.IsolatedDepsFailed"] = "[ERROR] {0} 종속성 설치에 실패했습니다. 나중에 다시 시도할 수 있습니다.",
            ["Log.NemoInstallStep1"] = "[*] 1단계: flask / nemo_toolkit / soundfile 등 기본 종속성을 설치하는 중입니다(기본 PyPI 인덱스)...",
            ["Log.NemoInstallStep1Failed"] = "[ERROR] 기본 종속성 설치에 실패했습니다. 위의 오류 내용을 확인하세요(나중에 다시 시도할 수 있습니다).",
            ["Log.NemoInstallStep2"] = "[*] 2단계: torch / torchaudio를 설치하는 중입니다(PyTorch 공식 인덱스)...",
            ["Log.NemoInstallStep2Failed"] = "[ERROR] torch/torchaudio 설치에 실패했습니다. 네트워크 연결을 확인한 후 다시 시도하세요(1단계의 기본 종속성은 이미 설치되어 있으므로 다시 실행할 필요가 없습니다).",
            ["Log.IsolatedDepsOk"] = "[OK] {0} 종속성이 설치되었습니다",

            ["Log.AppDirEmpty"] = "appDir는 비어 있을 수 없습니다",
            ["Log.MicromambaNotFound"] = "micromamba.exe를 찾을 수 없습니다. 설치 패키지가 손상되었을 수 있습니다.",
            ["Log.EmbeddedResourceMissing"] = "[ERROR] 내장 리소스가 없습니다: {0}(설치 패키지가 손상되었을 수 있습니다. 다시 다운로드해 주세요)",
            ["Log.MissingRequirementsFile"] = "[ERROR] 종속성 파일을 찾을 수 없습니다: {0}",
            ["Log.UpgradingPip"] = "[*] pip/setuptools/wheel을 업그레이드하는 중입니다...",
            ["Log.InstallingRequirements"] = "[*] {0}에 따라 종속성을 설치하는 중입니다(잠시만 기다려 주세요, 진행 상황이 실시간으로 표시됩니다)...",
            ["Log.RunningCommand"] = "[cmd] micromamba {0}",
            ["Log.MicromambaStartFailed"] = "[ERROR] micromamba.exe 시작에 실패했습니다: {0}",

            ["Log.MsiNotFound1"] = "[ERROR] MSI 패키지를 찾을 수 없습니다: {0}",
            ["Log.MsiNotFound2"] = "        설치 패키지가 손상되었을 수 있습니다. 전체 설치 프로그램을 다시 다운로드하세요.",
            ["Log.MsiInstalling"] = "[*] 프로그램 파일을 설치하는 중입니다(backend/frontend)...",
            ["Log.MsiInstallOk"] = "[OK] 프로그램 파일 설치가 완료되었습니다",
            ["Log.MsiInstallOkNeedsReboot"] = "[OK] 프로그램 파일 설치가 완료되었습니다(완전히 적용하려면 재부팅이 필요합니다)",
            ["Log.MsiStartFailed"] = "[ERROR] msiexec 시작에 실패했습니다: {0}",
            ["Log.MsiInstallFailed"] = "[ERROR] MSI 설치에 실패했습니다. 종료 코드: {0}",
            ["Log.MsiInstallFailedLogRef"] = "        전체 로그: {0}",
            ["Log.MsiLogTailHeader"] = "        ---- msiexec 로그 끝부분 ----",
            ["Log.MsiLogTailFooter"] = "        ---------------------------",
        };
    }
}
