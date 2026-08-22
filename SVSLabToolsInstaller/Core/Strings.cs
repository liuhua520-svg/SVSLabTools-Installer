using System.Collections.Generic;
using System.Globalization;

namespace SVSLabToolsInstaller.Core
{
    /// <summary>
    /// 支持的界面语言。精确匹配简体中文/繁体中文/英语/日语/韩语，
    /// 其余任何系统语言一律回退到英语（不是简体中文——这是产品
    /// 决策：面向国际用户时，无法识别的语言环境更适合给英语兜底，
    /// 而不是默认假定用户能看懂中文）。
    /// </summary>
    public enum AppLanguage
    {
        ZhHans,  // 简体中文
        ZhHant,  // 繁体中文
        En,      // 英语（回退默认）
        Ja,      // 日语
        Ko,      // 韩语
    }

    /// <summary>
    /// 轻量级界面字符串资源表。按 <see cref="AppLanguage"/> 查表返回
    /// 对应语言的文本，覆盖静态控件文本、MessageBox 内容、以及安装
    /// 过程中的日志提示行（EnvironmentPlanner/MsiLauncher/
    /// MicromambaRunner 里我们自己代码写的字符串）。
    ///
    /// 【范围边界，务必保持】只翻译本项目自己代码里硬编码的字符串。
    /// micromamba.exe / pip / conda 等外部进程通过
    /// OutputDataReceived/ErrorDataReceived 转发过来的原始输出
    /// （下载进度条、依赖解析日志等）不受本类控制，原样透传，
    /// 不做任何改写或翻译——那是第三方工具自己的输出，我们既没有
    /// 权限也没有可靠方式去改写它。
    ///
    /// 具体各语言字典分散在 Strings.ZhHans.cs / Strings.ZhHant.cs /
    /// Strings.En.cs / Strings.Ja.cs / Strings.Ko.cs 几个 partial 文件里，
    /// 按语言拆分方便单独校对、单独更新，不用五种语言挤在一个几百行
    /// 的大文件里互相干扰。
    /// </summary>
    public static partial class Strings
    {
        /// <summary>
        /// 当前生效的界面语言。默认在类初始化时根据系统 UI 语言探测一次；
        /// MainForm 不需要手动设置这个值，除非未来要支持"设置里手动切换
        /// 语言"这种功能（目前没有这个需求，按当前产品决策是纯自动检测）。
        /// </summary>
        public static AppLanguage Current { get; set; } = DetectSystemLanguage();

        /// <summary>
        /// 根据 CultureInfo.CurrentUICulture 探测应该用哪种界面语言。
        /// 精确匹配简体中文（zh-Hans / zh-CN 等）、繁体中文（zh-Hant /
        /// zh-TW / zh-HK / zh-MO）、日语（ja）、韩语（ko）；其余（含无法
        /// 识别的中文变体、任何其它语言）一律回退到英语。
        /// </summary>
        public static AppLanguage DetectSystemLanguage()
        {
            CultureInfo culture = CultureInfo.CurrentUICulture;

            // .NET Framework 4.7.2 下 CultureInfo 没有内建"是否繁体/简体"
            // 的直接布尔属性，用 culture.Name 前缀 + 已知繁简变体代码
            // 判断，覆盖常见的系统语言设置组合（简体中文-中国/新加坡；
            // 繁体中文-台湾/香港/澳门）。
            string name = culture.Name; // 例如 "zh-CN"、"zh-Hant-TW"、"ja-JP"

            if (name.StartsWith("zh"))
            {
                if (name.IndexOf("Hant", System.StringComparison.OrdinalIgnoreCase) >= 0
                    || name.EndsWith("-TW") || name.EndsWith("-HK") || name.EndsWith("-MO"))
                {
                    return AppLanguage.ZhHant;
                }
                // zh-Hans / zh-CN / zh-SG / 裸 "zh" 等，都视为简体中文。
                return AppLanguage.ZhHans;
            }
            if (name.StartsWith("ja")) return AppLanguage.Ja;
            if (name.StartsWith("ko")) return AppLanguage.Ko;

            // 精确匹配不上的任何其它语言（含英语本身），一律回退英语。
            return AppLanguage.En;
        }

        /// <summary>
        /// 按当前语言取字符串。key 不存在于当前语言字典时，依次回退
        /// 英语字典、简体中文字典；两者都没有则原样返回 key 本身（
        /// 方便一眼看出翻译表漏填了哪个 key，而不是静默显示空白）。
        /// </summary>
        public static string Get(string key)
        {
            Dictionary<string, string> table = TableFor(Current);
            string value;
            if (table != null && table.TryGetValue(key, out value)) return value;

            if (Current != AppLanguage.En && _en.TryGetValue(key, out value)) return value;
            if (Current != AppLanguage.ZhHans && _zhHans.TryGetValue(key, out value)) return value;

            return key;
        }

        /// <summary>
        /// 带格式化参数的取值，等价于 string.Format(Get(key), args)。
        /// 日志/消息框里大量字符串带动态内容（路径、语言名、错误信息等），
        /// 这个方法避免每处调用都手写 string.Format。
        /// </summary>
        public static string Get(string key, params object[] args)
        {
            return string.Format(Get(key), args);
        }

        private static Dictionary<string, string> TableFor(AppLanguage lang)
        {
            switch (lang)
            {
                case AppLanguage.ZhHans: return _zhHans;
                case AppLanguage.ZhHant: return _zhHant;
                case AppLanguage.En: return _en;
                case AppLanguage.Ja: return _ja;
                case AppLanguage.Ko: return _ko;
                default: return _en;
            }
        }
    }
}
