using System.Collections.Generic;

namespace SVSLabToolsInstaller.Core
{
    public static partial class Strings
    {
        // 日本語
        private static readonly Dictionary<string, string> _ja = new Dictionary<string, string>
        {
            ["Ui.WindowTitle"] = "SVS Lab Tools インストーラー",

            ["Ui.GroupComponents"] = "インストールする機能を選択",
            ["Ui.ChkCore"] = "Core（コア環境、メイン実行環境、選択を必要）",
            ["Ui.GroupLang"] = "MFA 言語モデル（Core が必要）",
            ["Ui.LangChinese"] = "中国語（普通話）",
            ["Ui.LangEnglish"] = "英語",
            ["Ui.LangJapanese"] = "日本語",
            ["Ui.LangKorean"] = "韓国語",
            ["Ui.LangCantonese"] = "広東語",
            ["Ui.ChkNemo"] = "NeMo-FA（独立環境、強制アライメントサービス）",
            ["Ui.ChkWhisperX"] = "WhisperX（独立環境、音声認識サービス）",
            ["Ui.ChkQwen3Tts"] = "Qwen3-TTS（独立環境、音声合成サービス）",

            ["Ui.GroupTorch"] = "PyTorch のハードウェアタイプ（すべての環境の torch バージョンに影響）",
            ["Ui.TorchCpu"] = "CPU（専用GPUなし、非NVIDIA、または不明な場合はこちらを推奨）",
            ["Ui.TorchCuda118"] = "CUDA 11.8（旧世代GPU、GTX 10シリーズ Pascal アーキテクチャなど）",
            ["Ui.TorchCuda121"] = "CUDA 12.1（比較的新しい専用GPU、RTX 30/40シリーズなど）",

            ["Ui.InstallDirLabel"] = "インストール先：",
            ["Ui.BrowseButton"] = "参照...",
            ["Ui.BrowseDialogDescription"] = "インストール先フォルダーを選択",
            ["Ui.SizeHint"] =
                "推定ディスク使用量（目安です。実際の使用量はダウンロード内容により変動します）：\n" +
                "Core は約 3～5 GB（PyTorch 含む）、言語モデルは1つあたり約 100～500 MB、\n" +
                "NeMo-FA は約 3～4 GB、WhisperX は約 3～5 GB、Qwen3-TTS は約 4～6 GB。",

            ["Ui.StartInstall"] = "インストール開始",
            ["Ui.CancelInstall"] = "インストールを中止",
            ["Ui.CurrentStepRunning"] = "実行中: {0}",
            ["Ui.CurrentStepDone"] = "インストール完了",
            ["Ui.CurrentStepDoneWithFailures"] = "インストール完了（{0} 件失敗）",

            ["Step.Core"] = "Core: メイン環境（.mfa_env）の作成と依存関係のインストール",
            ["Step.Lang"] = "MFA 言語モデル: {0}",
            ["Step.Nemo"] = "NeMo-FA: 独立環境の作成と依存関係のインストール",
            ["Step.WhisperX"] = "WhisperX: 独立環境の作成と依存関係のインストール",
            ["Step.Qwen3Tts"] = "Qwen3-TTS: 独立環境の作成と依存関係のインストール",
            ["Step.InstallMsi"] = "プログラムファイルのインストール（backend/frontend）",

            ["Msg.MissingCoreBody"] =
                "MFA 言語モデルが選択されていますが、Core（コア環境）が選択されておらず、\n" +
                "既存の .mfa_env メイン環境も検出されませんでした。言語モデルのダウンロードには\n" +
                "Core が作成するメイン環境が必要です。これがないとダウンロードに失敗します。\n\n" +
                "今すぐ Core も一緒に選択しますか？",
            ["Msg.MissingCoreTitle"] = "必要な機能が不足しています",

            ["Msg.NoSelectionBody"] = "インストールする機能を1つ以上選択してください。",
            ["Msg.NoSelectionTitle"] = "機能が選択されていません",

            ["Msg.NoInstallDirBody"] = "先にインストール先を選択してください。",
            ["Msg.NoInstallDirTitle"] = "インストール先が未選択です",

            ["Msg.MissingMicromambaBody"] = "micromamba.exe が見つかりません：\n{0}\n\nインストールパッケージが不完全な可能性があります。再度ダウンロードしてください。",
            ["Msg.MissingMsiBody"] = "MSI パッケージが見つかりません：\n{0}\n\nインストールパッケージが不完全な可能性があります。再度ダウンロードしてください。",
            ["Msg.MissingFileTitle"] = "必要なファイルが見つかりません",
            ["Msg.PayloadExtractFailedBody"] = "内蔵リソースの展開に失敗しました。インストールパッケージが破損している可能性があります：\n\n{0}\n\n完全版インストーラーを再度ダウンロードしてください。",
            ["Msg.ThreadExceptionBody"] = "予期しないエラーが発生しました：\n\n{0}",
            ["Msg.ThreadExceptionTitle"] = "SVS Lab Tools インストーラー - エラー",
            ["Msg.UnhandledExceptionBody"] = "予期しない重大なエラーが発生しました：\n\n{0}",
            ["Msg.UnhandledExceptionTitle"] = "SVS Lab Tools インストーラー - 重大なエラー",

            ["Msg.CancelledBody"] = "インストールはキャンセルされました。",
            ["Msg.CancelledTitle"] = "キャンセルしました",

            ["Msg.UnexpectedErrorBody"] = "インストール中に予期しないエラーが発生しました：\n\n{0}",
            ["Msg.UnexpectedErrorTitle"] = "インストールに失敗しました",

            ["Msg.AllSuccessBody"] = "選択したすべての機能のインストールが完了しました。",
            ["Msg.AllSuccessRebootSuffix"] = "\n\nプログラムファイルの変更を完全に反映するには再起動が必要です。作業を保存してから再起動することをお勧めします。",
            ["Msg.AllSuccessTitle"] = "インストール完了",

            ["Msg.PartialFailIntro"] = "インストールは完了しましたが、以下の項目はインストールできませんでした：",
            ["Msg.PartialFailOutro"] =
                "上記に記載のない、選択済みのその他の機能は正常にインストールされています。\n" +
                "失敗した機能については、本インストーラーを再実行し、失敗した項目のみを選択して\n" +
                "再試行できます。多くの場合、原因はネットワークの不安定さであり、再試行で解決します。",
            ["Msg.PartialFailTitle"] = "インストール完了（一部失敗）",

            ["Msg.ClosingWhileInstallingBody"] = "インストールが進行中です。中止して終了してもよろしいですか？",
            ["Msg.ClosingWhileInstallingTitle"] = "インストール中",

            ["Msg.ConfirmRecreateBody"] = "{0} 環境はすでに存在します：\n{1}\n\n削除して再作成しますか？\n（「いいえ」を選択すると既存の環境を使用します）",
            ["Msg.ConfirmRecreateTitle"] = "環境がすでに存在します",
            ["Ui.MfaMainEnvName"] = "MFA メイン環境",

            ["Log.AllDone"] = "すべてのインストールが完了しました",
            ["Log.DoneWithFailuresHeader"] = "インストールは完了しましたが、以下の項目が失敗しました：",
            ["Log.FailedItemLine"] = "  - {0}",
            ["Log.UserCancelled"] = "[!] インストールはユーザーによってキャンセルされました。",
            ["Log.StepException"] = "[ERROR] ステップの実行中に例外が発生しました: {0}",
            ["Log.UnexpectedError"] = "[ERROR] インストール中に予期しないエラーが発生しました: {0}",

            ["Log.UseExistingMfaEnv"] = "[OK] 既存の MFA メイン環境を使用します（作成をスキップ）",
            ["Log.CreatingEnv"] = "環境を作成しています... しばらくお待ちください（数分かかる場合があります）...",
            ["Log.EnvCreateFailed"] = "[ERROR] 環境の作成に失敗しました",
            ["Log.InstallingPyAv"] = "[*] PyAV 11.0.0 バイナリ依存関係をインストールしています（conda-forge）...",
            ["Log.PyAvInstallFailed"] = "[ERROR] PyAV 11.0.0 のインストールに失敗しました。ネットワーク接続または上記のエラーを確認してください。",
            ["Log.CreatingKaldiEnv"] = "[*] 独立した kaldi 環境 (.kaldi_env) を作成しています...",
            ["Log.KaldiInstallFailed"] = "[ERROR] kaldi のインストールに失敗しました。ネットワーク接続を確認して再試行してください。",
            ["Log.KaldiEnvExists"] = "[OK] .kaldi_env はすでに存在します（作成をスキップ）",
            ["Log.InstallingKalpy"] = "[*] .kaldi_env に kalpy（MFA の Kaldi Python バインディング）をインストールしています...",
            ["Log.KalpyInstallFailed"] = "[ERROR] kalpy のインストールに失敗しました。ネットワーク接続を確認して再試行してください。",
            ["Log.KalpyPydMissing1"] = "[ERROR] _kalpy.cp310-win_amd64.pyd が見つかりません —— .kaldi_env でビルドされた kalpy の",
            ["Log.KalpyPydMissing2"] = "        拡張モジュールが Python 3.10 版ではなく、.mfa_env と ABI が一致していない可能性があります。",
            ["Log.KalpyPydMissing3"] = "        削除してから再試行してください: {0}",
            ["Log.KalpyPydOk"] = "[OK] kalpy の _kalpy.cp310-win_amd64.pyd のバージョンは .mfa_env と一致しています",
            ["Log.InstallingPynini"] = "[*] .kaldi_env に pynini（MFA の G2P／テキスト正規化の依存関係）をインストールしています...",
            ["Log.PyniniFailed1"] = "[!] pynini のインストールに失敗しました（致命的ではないため続行します）。一部言語のテキスト正規化／G2P",
            ["Log.PyniniFailed2"] = "    機能で ModuleNotFoundError: No module named 'pynini' が発生する可能性があります。",
            ["Log.PyniniOk"] = "[OK] pynini を独立環境にインストールしました",
            ["Log.CoreDepsFailed"] = "[ERROR] 依存関係のインストールに失敗しました。上記のエラー内容を確認してください。",
            ["Log.CoreDepsOk"] = "[OK] すべての Python 依存関係をインストールしました",
            ["Log.CoreDone"] = "[OK] Core 機能のインストールが完了しました",

            ["Log.DeployingPatch"] = "[*] speechbrain の Windows パス区切り文字パッチを適用しています...",
            ["Log.PatchSitePackagesNotFound"] = "[!] .mfa_env の site-packages ディレクトリを特定できなかったため、speechbrain パッチの適用をスキップします",
            ["Log.PatchPathNotExist"] = "[!] 取得したパスが存在しないため、speechbrain パッチの適用をスキップします: {0}",
            ["Log.PatchWrongLeaf"] = "[!] 取得したパスの末尾フォルダー名が site-packages ではないため、誤った場所への適用を避けるためスキップします: {0}",
            ["Log.PatchSourceMissing"] = "[!] {0} が見つからないため、パッチの適用をスキップします",
            ["Log.PatchDeployed"] = "[OK] speechbrain パッチを適用しました: {0}",
            ["Log.PatchFailedNonFatal"] = "[!] speechbrain パッチの適用に失敗しました（致命的ではないためスキップ）: {0}",

            ["Log.MfaEnvMissing1"] = "[ERROR] .mfa_env メイン環境が存在しないため、言語モデルをダウンロードできません。",
            ["Log.MfaEnvMissing2"] = "        Core 機能が正常にインストールされているか確認してください（言語モデルはメイン環境内の mfa_utils.py に依存します）。",
            ["Log.MfaUtilsMissing1"] = "[ERROR] {0} が見つからないため、言語モデルをダウンロードできません。",
            ["Log.MfaUtilsMissing2"] = "        インストールパッケージが不完全な可能性があります。完全版インストーラーを再度ダウンロードしてください。",
            ["Log.DownloadingLangModel"] = "[*] {0} - {1} モデルをダウンロードしています...",
            ["Log.LangModelDownloadFailed"] = "[!] {0} モデルのダウンロードに失敗しました。ネットワーク接続を確認し、しばらくしてから再試行してください。",
            ["Log.LangModelDownloadOk"] = "[OK] {0} モデルをダウンロードしました",

            ["Log.UseExistingIsolatedEnv"] = "[OK] 既存の {0} 環境を使用します（作成をスキップ）",
            ["Log.CreatingIsolatedEnv"] = "{0} の独立環境を作成しています... しばらくお待ちください（数分かかる場合があります）...",
            ["Log.IsolatedEnvCreateFailed"] = "[ERROR] {0} 環境の作成に失敗しました",
            ["Log.IsolatedEnvReady"] = "[OK] {0} 環境の準備が完了しました",
            ["Log.IsolatedDepsFailed"] = "[ERROR] {0} の依存関係のインストールに失敗しました。しばらくしてから再試行してください。",
            ["Log.NemoInstallStep1"] = "[*] ステップ1: flask / nemo_toolkit / soundfile などの基本依存関係をインストールしています（デフォルトの PyPI インデックス）...",
            ["Log.NemoInstallStep1Failed"] = "[ERROR] 基本依存関係のインストールに失敗しました。上記のエラー内容を確認してください（後で再試行できます）。",
            ["Log.NemoInstallStep2"] = "[*] ステップ2: torch / torchaudio をインストールしています（PyTorch 公式インデックス）...",
            ["Log.NemoInstallStep2Failed"] = "[ERROR] torch/torchaudio のインストールに失敗しました。ネットワーク接続を確認して再試行してください（ステップ1の基本依存関係は既にインストール済みのため、やり直す必要はありません）。",
            ["Log.IsolatedDepsOk"] = "[OK] {0} の依存関係をインストールしました",

            ["Log.AppDirEmpty"] = "appDir を空にすることはできません",
            ["Log.MicromambaNotFound"] = "micromamba.exe が見つかりません。インストールパッケージが不完全な可能性があります。",
            ["Log.EmbeddedResourceMissing"] = "[ERROR] 埋め込みリソースが見つかりません: {0}（インストールパッケージが破損している可能性があります。再度ダウンロードしてください）",
            ["Log.MissingRequirementsFile"] = "[ERROR] 依存関係ファイルが見つかりません: {0}",
            ["Log.UpgradingPip"] = "[*] pip/setuptools/wheel をアップグレードしています...",
            ["Log.InstallingRequirements"] = "[*] {0} に基づいて依存関係をインストールしています（しばらくお待ちください。進行状況はリアルタイムで表示されます）...",
            ["Log.RunningCommand"] = "[cmd] micromamba {0}",
            ["Log.MicromambaStartFailed"] = "[ERROR] micromamba.exe の起動に失敗しました: {0}",

            ["Log.MsiNotFound1"] = "[ERROR] MSI パッケージが見つかりません: {0}",
            ["Log.MsiNotFound2"] = "        インストールパッケージが不完全な可能性があります。完全版インストーラーを再度ダウンロードしてください。",
            ["Log.MsiInstalling"] = "[*] プログラムファイルをインストールしています（backend/frontend）...",
            ["Log.MsiInstallOk"] = "[OK] プログラムファイルのインストールが完了しました",
            ["Log.MsiInstallOkNeedsReboot"] = "[OK] プログラムファイルのインストールが完了しました（完全に反映するには再起動が必要です）",
            ["Log.MsiStartFailed"] = "[ERROR] msiexec の起動に失敗しました: {0}",
            ["Log.MsiInstallFailed"] = "[ERROR] MSI のインストールに失敗しました。終了コード: {0}",
            ["Log.MsiInstallFailedLogRef"] = "        詳細ログ: {0}",
            ["Log.MsiLogTailHeader"] = "        ---- msiexec ログの末尾 ----",
            ["Log.MsiLogTailFooter"] = "        ---------------------------",
        };
    }
}
