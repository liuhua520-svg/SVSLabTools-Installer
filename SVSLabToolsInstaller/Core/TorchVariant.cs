using System;

namespace SVSLabToolsInstaller.Core
{
    /// <summary>
    /// 用户在界面上选择的 PyTorch 硬件变体（CPU / CUDA 11.8 / CUDA 12.1）。
    ///
    /// 【为什么不做自动检测】已与用户确认：完全交给用户在界面上手动选择，
    /// 不用 nvidia-smi / 注册表做自动探测。自动检测在多显卡、驱动版本与
    /// CUDA 运行时版本不完全对应等情况下容易给出误导性的"推荐"，与其做
    /// 一个不总是准确的自动判断，不如清楚地把三个选项摆出来，附上简短
    /// 的选型说明文字（见 MainForm 上单选框旁的提示标签），让用户自己
    /// 决定——这也是原 requirements-*.txt 里注释一直采用的方式（写清楚
    /// "RTX 30/40 系用 cu121"，"GTX 10 系 Pascal 架构用 cu118"，交给
    /// 用户按自己的硬件对号入座）。
    /// </summary>
    public enum TorchVariant
    {
        Cpu,
        Cuda118,
        Cuda121
    }

    /// <summary>
    /// 每份 requirements 文件对"如何声明 torch 相关行"的规则并不完全一致
    /// ——这不是历史遗留的不一致，而是各自环境的真实约束不同（详见下面
    /// 每个字段的注释），必须原样保留这些差异，不能为了图省事而统一成
    /// 同一套逻辑，否则会重新踩中 requirements-*.txt 文件头部注释里
    /// 记录过的那些坑（比如 --index-url 会把默认 PyPI 源整个替换掉，
    /// 导致同文件里其它包全部找不到）。
    /// </summary>
    public sealed class TorchRequirementRule
    {
        /// <summary>torch 的基础版本号（不含 +cpu/+cu118/+cu121 后缀）。</summary>
        public string TorchBaseVersion { get; }

        /// <summary>是否需要一并声明 torchvision（目前只有 qwen3tts 需要）。</summary>
        public bool IncludeTorchVision { get; }

        /// <summary>torchvision 的基础版本号（IncludeTorchVision=false 时忽略）。</summary>
        public string TorchVisionBaseVersion { get; }

        /// <summary>
        /// 索引 URL 的处理策略：
        ///   ExtraIndexTopOfFile - 在文件最开头插入一行
        ///     "--extra-index-url https://download.pytorch.org/whl/&lt;tag&gt;"
        ///     （Core / qwen3 / qwen3tts 三份文件用这个策略：--extra-index-url
        ///     是"追加"而非"替换"默认源，PyPI 上的其它包正常从 PyPI 装，
        ///     torch 系列在 PyPI 找不到时才去 PyTorch 官方索引找）。
        ///   ReplaceIndexInlineForCudaOnly - CPU 场景完全不写任何
        ///     index-url；只有选了 CUDA 时才在 torch/torchaudio 那两行
        ///     后面追加一行 "--index-url https://download.pytorch.org/whl/&lt;tag&gt;"
        ///     （nemo 场景专用：nemo_toolkit 依赖链更复杂，用
        ///     --extra-index-url 在这里曾经实测会引发解析问题，CPU 场景
        ///     必须完全不写这一行——历史 requirements-nemo.txt 文件里
        ///     用大写警告注释特别强调过这一点，"这里千万不要再写任何
        ///     --index-url 了"）。
        /// </summary>
        public IndexUrlStrategy IndexStrategy { get; }

        public TorchRequirementRule(
            string torchBaseVersion,
            bool includeTorchVision,
            string torchVisionBaseVersion,
            IndexUrlStrategy indexStrategy)
        {
            TorchBaseVersion = torchBaseVersion;
            IncludeTorchVision = includeTorchVision;
            TorchVisionBaseVersion = torchVisionBaseVersion;
            IndexStrategy = indexStrategy;
        }
    }

    public enum IndexUrlStrategy
    {
        ExtraIndexTopOfFile,
        ReplaceIndexInlineForCudaOnly
    }

    public static class TorchVariantExtensions
    {
        /// <summary>
        /// 返回该硬件变体对应的 wheel 后缀标签，用于拼 "torch==X.Y.Z+&lt;tag&gt;"
        /// 以及 PyTorch 索引 URL 里的路径片段（.../whl/&lt;tag&gt;）。两处用的
        /// 是同一个字符串，PyTorch 官方索引的 URL 路径命名和 wheel 文件名
        /// 后缀恰好一致（cpu / cu118 / cu121），这不是巧合，是 PyTorch 官方
        /// 索引本身的组织方式。
        /// </summary>
        public static string WheelTag(this TorchVariant variant)
        {
            switch (variant)
            {
                case TorchVariant.Cpu: return "cpu";
                case TorchVariant.Cuda118: return "cu118";
                case TorchVariant.Cuda121: return "cu121";
                default:
                    throw new ArgumentOutOfRangeException(nameof(variant), variant, null);
            }
        }

        /// <summary>
        /// 界面单选框旁边展示的简短说明文字，帮助用户对号入座。
        /// 【当前未被调用】MainForm.Designer.cs 里三个 radioTorchXxx.Text
        /// 目前是直接走 Strings.Get("Ui.TorchXxx") 赋值，没有引用这个方法。
        /// 保留并同样接入字符串表是为了避免以后有人接上调用点时又变回
        /// 硬编码中文——两处文案内容也不完全一致（这里更简短），如果
        /// 将来真要复用，建议先确认是要与 Designer.cs 的文案合并成一份，
        /// 而不是维护两份长得很像但不同步的翻译。
        /// </summary>
        public static string DisplayLabel(this TorchVariant variant)
        {
            switch (variant)
            {
                case TorchVariant.Cpu:
                    return Strings.Get("Ui.TorchCpu");
                case TorchVariant.Cuda118:
                    return Strings.Get("Ui.TorchCuda118");
                case TorchVariant.Cuda121:
                    return Strings.Get("Ui.TorchCuda121");
                default:
                    throw new ArgumentOutOfRangeException(nameof(variant), variant, null);
            }
        }
    }
}
