using System.IO;
using System.Text;

namespace SVSLabToolsInstaller.Core
{
    /// <summary>
    /// 把"固定依赖列表"（不含 torch，原样迁移自历史 requirements-*.txt 的
    /// 内容）和"按用户选择的 TorchVariant 动态生成的 torch 相关行"拼接成
    /// 一份最终的 requirements 文件，写到临时目录供 pip install -r 使用。
    ///
    /// 【为什么不能继续用打包进安装包的静态 .txt 文件】原方案（bat 版本）
    /// 里 torch 版本是固定写死在 requirements*.txt 里的，因为那时候没有
    /// "运行时选择硬件"这个需求。现在用户要在界面上选 CPU/CUDA 11.8/
    /// CUDA 12.1，这个选择只有在安装器运行时才知道，所以 torch 那几行
    /// 不能再是静态文件的一部分，必须在生成最终 requirements 文件时按
    /// 选择动态拼接。
    /// </summary>
    public static class RequirementsBuilder
    {
        // ────────────────────────────────────────────────────────────
        // 固定依赖列表：原样迁移自历史 requirements.txt（Core 环境），
        // 删除了原文件顶部的 --extra-index-url 行和 torch/torchaudio
        // 两行（这些改为按 TorchVariant 动态生成，见 BuildCoreRequirements）。
        // 其余内容（包括每一处 pgvector/spacy-pkuseg 等的详细修复说明
        // 注释）原样保留，避免遗漏任何一个已知坑的解释。
        //
        // 【2026-08 架构调整，务必保持与 backend/requirements.txt 同步】
        // WhisperX 和 Qwen3-ASR 在主环境（.mfa_env）里的位置互换了：
        //   - WhisperX 迁出主环境，现在跑在独立的 whisperx_server.py
        //     进程里（.whisperx_env，见下方 WhisperXFixedBody /
        //     BuildWhisperXRequirements）。原因：whisperx==3.2.0 精确
        //     锁定 transformers==4.39.3（经由 faster-whisper==1.0.0 +
        //     ctranslate2==4.4.0 + tokenizers<0.16 一路锁下来），直接
        //     和下面 qwen-asr 精确锁定的 transformers==4.57.6 冲突——
        //     两个版本区间历史上从未有过交集，装进同一个环境
        //     `pip install` 必然 ResolutionImpossible，无解。
        //   - Qwen3-ASR / Qwen3-ForcedAligner（qwen-asr 包）反过来迁入
        //     主环境，在 app.py 进程内本地加载，不再需要独立的
        //     qwen3_server.py / .qwen3_env（该服务已下线，对应的
        //     BuildQwen3Requirements 仍保留但不再被调用，见其注释）。
        // ────────────────────────────────────────────────────────────
        private const string CoreFixedBody = @"
# SVS Lab Tools / SVS Lab Aligner - fixed main environment
# Target: Python 3.10 / Windows x64
# IMPORTANT: WhisperX and NeMo are intentionally NOT installed here.
# They run as separate HTTP server environments to avoid dependency conflicts.
# Qwen3-ASR / Qwen3-ForcedAligner (qwen-asr package) ARE installed here and run
# in-process (see the ""Qwen3-ASR / Qwen3-ForcedAligner"" section below for why).

# Core
numpy==1.26.4
setuptools==81.0.0

# Web/API
flask==2.3.3
flask-cors==4.0.0
requests==2.34.2
tqdm==4.70.0

# Audio / DSP
soundfile==0.12.1
librosa==0.11.0
resampy==0.4.3
pyworld==0.3.5
torchcrepe==0.0.24

# MFA / text
montreal-forced-aligner==3.3.9
# 【修复】MFA 3.3.9 的 db.py 里硬编码 `from pgvector.sqlalchemy import Vector`，
# 但它自己的 wheel 元数据（Requires-Dist）并没有声明依赖 pgvector，属于
# 上游打包遗漏，pip 装 montreal-forced-aligner 时不会连带装上，必须在这里
# 显式列出，否则一 import montreal_forced_aligner 就会
# ModuleNotFoundError: No module named 'pgvector'。
pgvector==0.3.6
textgrid==1.5
pypinyin>=0.53.0
pycantonese>=5.0.0
sudachipy>=0.6.8
sudachidict-core>=20240409
jamo>=0.4.1
nltk>=3.10.2
num2words>=0.5.14
g2p_en>=2.1.0

# 【修复】MFA 的 tokenization/spacy.py 在实际跑对齐前会按语言检查专用分词器
# 是否已安装，缺哪个就直接报 ImportError 中断当前任务（不是安装失败，是运行
# 时才报）。本项目支持的语言里，中文 (cmn -> Language.chinese) 和韩语 (kor)
# 用到的包这里之前没有声明，日语 (jpn) 靠上面已有的 sudachipy 侥幸覆盖；
# 粤语 (yue -> Language.yue) 这个检查函数里没有对应分支，不受影响。
spacy-pkuseg==1.0.1
dragonmapper==0.3.0
hanziconv==0.3.2
python-mecab-ko==1.3.7

# ====================== WhisperX ======================
# NOTE: whisperx is intentionally NOT installed here (removed — this is the
# other half of the 2026-08 architecture swap described above the class).
# WhisperX now runs exclusively inside the separate whisperx_server.py
# process (.whisperx_env, see WhisperXFixedBody / BuildWhisperXRequirements)
# and is called over HTTP on 127.0.0.1:5854 — alt_aligners.py's
# WhisperXAligner class is a thin HTTP client (POST /transcribe,
# POST /align), matching the pattern Qwen3ASRAligner/Qwen3ForcedAligner use
# below. This env never needs whisperx/faster-whisper/ctranslate2 installed.

# ====================== Qwen3-ASR / Qwen3-ForcedAligner ======================
# 【2026-08 架构调整】此前 qwen-asr 因为版本冲突（见上面 WhisperX 一节）被
# 排除在这个环境之外，只能运行在独立的 qwen3_server.py 进程
# （.qwen3_env）里，通过 HTTP 调用。现在 WhisperX 已经迁出这个环境，
# qwen-asr 的 transformers==4.57.6 精确锁定不再与任何其它包冲突，可以
# 直接安装进这个主环境，Qwen3-ASR 和 Qwen3-ForcedAligner 都改为在
# app.py 进程内本地加载（qwen3_server.py 已下线）。
qwen-asr==0.0.6
# qwen-asr 自身在 PyPI 上精确锁定了 transformers==4.57.6 /
# accelerate==1.12.0（见其 requires_dist），下面两行的宽松下限不会与
# 之冲突，pip 解析时最终会以 qwen-asr 的精确锁定版本为准；这里保留
# 宽松下限只是为了在 qwen-asr 未来放宽自己的锁定范围时仍有一个基本
# 版本保障，不需要跟着精确改动。
transformers==4.57.6
accelerate==1.12.0

# Traditional -> Simplified Chinese conversion
opencc-python-reimplemented>=0.1.7

# Edge TTS / Windows SAPI helper
edge-tts>=7.0.0
pywin32>=306; sys_platform == ""win32""

# Project file formats
ruamel.yaml==0.19.1
mido==1.3.3

# NOTE:
# kalpy-kaldi is intentionally NOT installed by pip here.
# The upstream package is a Kaldi binding and recommends conda-forge Kaldi/Kalpy.
# Install it separately in the MFA environment if your setup script requires it.
";

        private const string NemoFixedBody = @"
# nemo_server.py 独立服务依赖
#
# 说明：
#   nemo_toolkit[asr] 对 packaging / fsspec / omegaconf / hydra-core /
#   lightning 等核心依赖有严格的版本上限，装进任何已有环境都容易把这些
#   公共依赖""降级""，与其它工具产生冲突。因此单独建一个干净环境，只为
#   nemo_server.py 服务，主环境只通过 HTTP 调用 127.0.0.1:5002。

# ====================== 服务框架 ======================
flask==2.3.3

# ====================== NeMo Forced Aligner ======================
nemo_toolkit[asr]==2.7.3

# ====================== 音频处理 ======================
soundfile==0.12.1

# ====================== 网络与其他基础库 ======================
requests
tqdm
";

        /// <summary>
        /// 【当前未被调用，服务已下线】qwen3_server.py 独立服务
        /// （.qwen3_env）已于 2026-08 架构调整中下线——Qwen3-ASR /
        /// Qwen3-ForcedAligner（qwen-asr 包）现在直接内联安装进主环境
        /// .mfa_env（见 CoreFixedBody），不再需要一个独立进程。原来这个
        /// 常量对应的安装步骤（BuildQwen3Requirements）已被界面上的
        /// WhisperX 选项取代，见 EnvironmentPlanner.BuildPlan /
        /// RunWhisperXEnvAsync。保留这个常量和下面的方法只是为了留存
        /// 历史实现，不在当前安装路径里被引用。
        /// </summary>
        private const string Qwen3FixedBody = @"
# qwen3_server.py 独立服务依赖（Qwen3-ASR-1.7B / Qwen3-ForcedAligner-0.6B）
# 主环境只通过 HTTP 调用 127.0.0.1:5001，不需要装 qwen-asr / transformers。

# ====================== 服务框架 ======================
flask==2.3.3

# ====================== Qwen3-ASR / ForcedAligner ======================
qwen-asr==0.0.6
transformers==4.57.6
accelerate==1.12.0

# ====================== 基础数值库 ======================
numpy==1.26.4

# ====================== 音频处理 ======================
soundfile==0.12.1

# ====================== 网络与其他基础库 ======================
requests
tqdm
";

        // ────────────────────────────────────────────────────────────
        // whisperx_server.py 独立服务依赖（.whisperx_env），原样对齐
        // backend/requirements-whisperx.txt 里 --extra-index-url 那一行
        // 之后、torch 那几行之前的固定部分。torch/torchaudio 由
        // BuildWhisperXRequirements 按用户选择的硬件动态生成，不写在
        // 这个常量里，与其它三个环境的处理方式一致。
        //
        // 【为什么单独建环境，不能装进 .mfa_env】whisperx==3.2.0 经由
        // faster-whisper==1.0.0 + ctranslate2==4.4.0 精确锁定
        // transformers==4.39.3（因为 faster-whisper==1.0.0 要求
        // tokenizers<0.16），这与 .mfa_env 里 qwen-asr==0.0.6 精确锁定
        // 的 transformers==4.57.6 直接冲突——两个包历史上都从未放宽过
        // 这条锁定，装进同一个环境 pip 必然 ResolutionImpossible，无论
        // backtracking 到哪个版本都无法同时满足两者。详见
        // CoreFixedBody 顶部的架构调整说明。
        // ────────────────────────────────────────────────────────────
        private const string WhisperXFixedBody = @"
# whisperx_server.py 独立服务依赖（不要装进主 .mfa_env，也不要和
# .qwen3tts_env / .nemo_env 共用！）
#
# 说明：
#   这是 WhisperX 跑在独立进程里所需的全部依赖。主 Flask 应用（app.py
#   所在的 .mfa_env）只通过 HTTP 调用 127.0.0.1:5854，完全不需要装
#   whisperx / faster-whisper / ctranslate2 / 对应版本的 transformers
#   这一套。

# ====================== 服务框架 ======================
flask==2.3.3

# ====================== 打包基础设施 ======================
# ctranslate2==4.4.0 在导入时执行 `import pkg_resources`（来自
# setuptools，并非标准库）。较新的 conda-forge python=3.10 引导环境 /
# 某些 venv 默认不再自带 setuptools，若缺失会在首次真正调用
# whisperx.load_model()（即首次 /transcribe 请求，而非服务启动时）才
# 报 ModuleNotFoundError: No module named 'pkg_resources'，具有较强
# 迷惑性。显式锁定版本号，避免仅依赖""升级 pip/setuptools/wheel""这一步
# 隐式满足。
setuptools>=65

# ====================== WhisperX ======================
# WhisperX 3.2.0 is retained because this project intentionally keeps NumPy 1.26.x.
# WhisperX 3.2.0 requires faster-whisper==1.0.0, ctranslate2==4.4.0 and an unconstrained transformers.
# faster-whisper==1.0.0 requires tokenizers<0.16, so pin an older Transformers stack here.
# PyAV 11.0.0 is NOT installed by pip: the installer installs conda-forge av=11.0.0 first
# to avoid a Windows source build that requires FFmpeg import libraries.
whisperx==3.2.0
ctranslate2==4.4.0
transformers==4.39.3
tokenizers==0.15.2
huggingface-hub==0.36.2

# ====================== 基础数值库 ======================
numpy==1.26.4

# ====================== 音频处理 ======================
soundfile==0.12.1
librosa==0.11.0
resampy==0.4.3

# ====================== 网络与其他基础库 ======================
requests
tqdm
";

        private const string Qwen3TtsFixedBody = @"
# qwen3tts_server.py 独立服务依赖（Qwen3-TTS：CustomVoice / VoiceDesign /
# Base-VoiceClone 三套 12Hz checkpoint）。官方推荐 Python 3.12（用于支持
# Qwen3-TTS-Tokenizer-12Hz 的模型结构），与 qwen3_server.py（Python 3.10）
# 不是同一个环境。主环境只通过 HTTP 调用 127.0.0.1:5003。

# ====================== 服务框架 ======================
flask==2.3.3

# ====================== Qwen3-TTS ======================
qwen-tts==0.1.1
transformers==4.57.3
accelerate==1.12.0

# ====================== 基础数值库 ======================
numpy==1.26.4

# ====================== 音频处理 ======================
soundfile>=0.12.1

# ====================== 网络与其他基础库 ======================
requests
tqdm
";

        /// <summary>
        /// 生成 Core 环境（主 .mfa_env）的最终 requirements 内容。
        /// 索引策略：ExtraIndexTopOfFile，torch 基础版本固定 2.3.1（与历史
        /// requirements.txt 一致），不含 torchvision。
        /// </summary>
        public static string BuildCoreRequirements(TorchVariant variant)
        {
            var rule = new TorchRequirementRule(
                torchBaseVersion: "2.3.1",
                includeTorchVision: false,
                torchVisionBaseVersion: null,
                indexStrategy: IndexUrlStrategy.ExtraIndexTopOfFile);
            return Compose(rule, variant, CoreFixedBody, includeTorchAudio: true);
        }

        /// <summary>
        /// 生成 NeMo 独立环境的最终 requirements 内容。
        /// 索引策略：ReplaceIndexInlineForCudaOnly，torch 基础版本 2.6.0
        /// （nemo_toolkit 2.7.3 要求 torch>=2.6.0，与 Core/qwen3 的 2.3.1
        /// 不同，必须保留这个差异，不能统一成同一个版本号）。
        ///
        /// 【当前未被调用】NeMo 环境的依赖安装已改为
        /// EnvironmentPlanner.RunNemoEnvAsync 里两条独立的直接
        /// pip install 命令，不再生成/使用 requirements 文件——原因见
        /// RunNemoEnvAsync 方法上的详细注释（--index-url 出现在
        /// requirements 文件中间会把默认 PyPI 源整体替换掉，导致 flask
        /// 等基础包一起装不上，这个坑在实际使用中真实出现过）。这个
        /// 方法和它引用的 NemoFixedBody 常量保留是为了留存历史实现和
        /// 里面的版本号依据，如果以后想恢复"单文件 requirements"方式
        /// 可以参考，但目前的安装路径完全不经过这里。
        /// </summary>
        public static string BuildNemoRequirements(TorchVariant variant)
        {
            var rule = new TorchRequirementRule(
                torchBaseVersion: "2.6.0",
                includeTorchVision: false,
                torchVisionBaseVersion: null,
                indexStrategy: IndexUrlStrategy.ReplaceIndexInlineForCudaOnly);
            return Compose(rule, variant, NemoFixedBody, includeTorchAudio: true);
        }

        /// <summary>
        /// 【当前未被调用，服务已下线】生成 Qwen3-ASR 独立环境
        /// （.qwen3_env）的 requirements 内容。qwen3_server.py 已下线，
        /// 见 Qwen3FixedBody 上的说明。保留这个方法只是为了留存历史
        /// 实现，当前安装路径不再引用它——界面上原来的"Qwen3-ASR"选项
        /// 已被"WhisperX"取代，见 BuildWhisperXRequirements。
        /// </summary>
        public static string BuildQwen3Requirements(TorchVariant variant)
        {
            var rule = new TorchRequirementRule(
                torchBaseVersion: "2.3.1",
                includeTorchVision: false,
                torchVisionBaseVersion: null,
                indexStrategy: IndexUrlStrategy.ExtraIndexTopOfFile);
            // 历史 requirements-qwen3.txt 只声明 torch，没有 torchaudio。
            return Compose(rule, variant, Qwen3FixedBody, includeTorchAudio: false);
        }

        /// <summary>
        /// 生成 WhisperX 独立环境（.whisperx_env）的最终 requirements
        /// 内容。索引策略：ExtraIndexTopOfFile（与 Core/qwen3tts 相同，
        /// 不是 NeMo 那种会踩坑的 ReplaceIndexInlineForCudaOnly——
        /// requirements-whisperx.txt 顶部用的就是 --extra-index-url，
        /// 可以直接走通用的"生成 requirements 文件 + pip install -r"
        /// 流程，不需要像 NeMo 那样拆成多条直接 pip 命令）。
        /// torch 基础版本 2.3.1，与 Core/qwen3 一致；含 torchaudio，
        /// 不含 torchvision。
        /// </summary>
        public static string BuildWhisperXRequirements(TorchVariant variant)
        {
            var rule = new TorchRequirementRule(
                torchBaseVersion: "2.3.1",
                includeTorchVision: false,
                torchVisionBaseVersion: null,
                indexStrategy: IndexUrlStrategy.ExtraIndexTopOfFile);
            return Compose(rule, variant, WhisperXFixedBody, includeTorchAudio: true);
        }

        /// <summary>
        /// 生成 Qwen3-TTS 独立环境的最终 requirements 内容。
        /// 唯一需要 torchvision 的环境（IncludeTorchVision=true）。
        /// </summary>
        public static string BuildQwen3TtsRequirements(TorchVariant variant)
        {
            var rule = new TorchRequirementRule(
                torchBaseVersion: "2.3.1",
                includeTorchVision: true,
                torchVisionBaseVersion: "0.18.1",
                indexStrategy: IndexUrlStrategy.ExtraIndexTopOfFile);
            return Compose(rule, variant, Qwen3TtsFixedBody, includeTorchAudio: true);
        }

        private static string Compose(
            TorchRequirementRule rule,
            TorchVariant variant,
            string fixedBody,
            bool includeTorchAudio)
        {
            var sb = new StringBuilder();
            string tag = variant.WheelTag();

            if (rule.IndexStrategy == IndexUrlStrategy.ExtraIndexTopOfFile)
            {
                // --extra-index-url 是"追加"而非"替换"默认 PyPI 源，必须放在
                // 文件最开头才对整份文件生效（pip 对 requirements 文件里的
                // --extra-index-url 是全局生效的，不是只作用于紧邻的几行）。
                sb.AppendLine($"--extra-index-url https://download.pytorch.org/whl/{tag}");
                sb.AppendLine();
            }

            sb.Append(fixedBody.TrimStart('\r', '\n'));
            sb.AppendLine();
            sb.AppendLine("# PyTorch (由安装器根据用户选择的硬件类型动态生成)");
            sb.AppendLine($"torch=={rule.TorchBaseVersion}+{tag}");

            if (rule.IncludeTorchVision)
            {
                sb.AppendLine($"torchvision=={rule.TorchVisionBaseVersion}+{tag}");
            }
            if (includeTorchAudio)
            {
                sb.AppendLine($"torchaudio=={rule.TorchBaseVersion}+{tag}");
            }

            if (rule.IndexStrategy == IndexUrlStrategy.ReplaceIndexInlineForCudaOnly
                && variant != TorchVariant.Cpu)
            {
                // 【与历史 requirements-nemo.txt 保持一致的关键规则】CPU 场景
                // 完全不写这一行；只有选了 CUDA 时才追加 --index-url（注意
                // 不是 --extra-index-url）。这条规则和上面 Core/qwen3/
                // qwen3tts 用的 ExtraIndexTopOfFile 策略是两套不同的逻辑，
                // 不要试图合并简化，历史注释里已经用大写警告强调过 CPU 场景
                // 下这里绝对不能再写 --index-url。
                sb.AppendLine($"--index-url https://download.pytorch.org/whl/{tag}");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 把生成的 requirements 内容写入临时文件，返回文件路径，供
        /// MicromambaRunner 的 pip install -r 使用。用 GUID 命名避免多个
        /// 环境并发/串行安装时互相覆盖同名临时文件。
        /// </summary>
        public static string WriteToTempFile(string content, string labelForFileName)
        {
            string dir = Path.Combine(Path.GetTempPath(), "SVSLabToolsInstaller");
            Directory.CreateDirectory(dir);
            string path = Path.Combine(dir, $"requirements-{labelForFileName}-{System.Guid.NewGuid():N}.txt");
            File.WriteAllText(path, content, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
            return path;
        }
    }
}
