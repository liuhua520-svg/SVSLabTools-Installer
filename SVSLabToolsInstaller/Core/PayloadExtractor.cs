using System;
using System.IO;
using System.Reflection;

namespace SVSLabToolsInstaller.Core
{
    /// <summary>
    /// 把随程序集一起嵌入编译的 Payload 文件（micromamba.exe、
    /// "SVS Lab Tools.msi"、EnvAssets 下的两个 .py 文件）在程序启动时
    /// 释放到一个临时目录，返回各文件释放后的实际路径。
    ///
    /// 【为什么要这样做】原来的设计是 Payload\ 作为运行时输出目录下的
    /// 普通文件夹，随 exe 一起分发（.csproj 里 <None Include="Payload\**\*">
    /// 在编译时复制到输出目录）。这次改造把这些文件转为
    /// <EmbeddedResource>，编译进程序集内部，最终只产出单个 exe，不再
    /// 需要额外的 Payload 文件夹一起拷贝分发。
    ///
    /// 【释放策略：每次启动都重新释放，不做增量校验】已与用户确认。
    /// 更简单可靠：不需要考虑"上次释放到一半失败、文件残缺但被误判为
    /// 已存在完整文件"这类边界情况。52MB 体量下，每次启动多花的几秒
    /// 磁盘 I/O 时间可以接受。
    ///
    /// 【为什么临时目录用固定的、非随机的名字】如果每次启动都用
    /// Path.GetTempFileName() 式的随机目录名，旧的临时目录会越攒越多、
    /// 没有自然的复用/清理时机。固定名字（形如
    /// %TEMP%\SVSLabToolsInstaller_Payload）配合"启动时先删除旧目录
    /// 整个重建"，保证任意时刻磁盘上最多只有一份，不会无限堆积。
    /// </summary>
    public static class PayloadExtractor
    {
        private const string TempDirName = "SVSLabToolsInstaller_Payload";

        // 【关键】这四个字符串必须和 .csproj 里对应 EmbeddedResource 项的
        // LogicalName 完全一致（区分大小写）。不依赖 MSBuild 按文件路径
        // 自动推算清单资源名——那套推算规则会把文件夹名、空格、多个点
        // 做转义替换，具体转义结果没有可靠的公开算法可以在代码里复现
        // （连 MSBuild 官方 issue 里都承认"没有已知的工具方法可以转换"），
        // "SVS Lab Tools.msi" 这种带空格又带点的文件名尤其容易猜错。
        // 显式指定 LogicalName 元数据可以完全绕开这套推算——LogicalName
        // 一旦设置，编译器保证优先使用它，结果就是这里写什么字符串，
        // 编译出的资源就叫什么，不存在"猜错"的可能。
        private const string ResMicromamba = "Payload_micromamba_exe";
        private const string ResMsi = "Payload_SVSLabTools_msi";
        private const string ResSitecustomize = "Payload_EnvAssets_sitecustomize_py";
        private const string ResMfaUtils = "Payload_EnvAssets_mfa_utils_py";

        /// <summary>
        /// 释放结果：Payload 里各文件释放到临时目录后的实际路径。
        /// </summary>
        public sealed class ExtractedPaths
        {
            public string RootDir { get; }
            public string MicromambaExe { get; }
            public string MsiPath { get; }
            public string EnvAssetsDir { get; }

            public ExtractedPaths(string rootDir, string micromambaExe, string msiPath, string envAssetsDir)
            {
                RootDir = rootDir;
                MicromambaExe = micromambaExe;
                MsiPath = msiPath;
                EnvAssetsDir = envAssetsDir;
            }
        }

        /// <summary>
        /// 执行释放。任何一步失败都会向上抛出异常（不在这里吞掉），
        /// 由调用方（MainForm 构造函数）决定如何向用户展示——这类失败
        /// 意味着程序集本身可能损坏或被截断，属于需要立刻让用户知道、
        /// 不应该悄悄继续的情况。
        /// </summary>
        public static ExtractedPaths Extract()
        {
            string rootDir = Path.Combine(Path.GetTempPath(), TempDirName);

            // 每次启动都重新释放：先整个删掉旧目录再重建，不做增量判断。
            if (Directory.Exists(rootDir))
            {
                try
                {
                    Directory.Delete(rootDir, recursive: true);
                }
                catch (IOException)
                {
                    // 上一个实例可能还没完全退出、文件被占用一小会儿；
                    // 这种情况下退而求其次，直接在旧目录基础上覆盖写入
                    // （下面的 File.Create 本身就是覆盖模式），不阻断启动。
                }
            }
            Directory.CreateDirectory(rootDir);

            string setupDir = Path.Combine(rootDir, "_setup");
            Directory.CreateDirectory(setupDir);
            string micromambaExe = ExtractResource(ResMicromamba, Path.Combine(rootDir, "micromamba.exe"));

            string msiDir = Path.Combine(rootDir, "Msi");
            Directory.CreateDirectory(msiDir);
            string msiPath = ExtractResource(ResMsi, Path.Combine(msiDir, "SVS Lab Tools.msi"));

            string envAssetsDir = Path.Combine(rootDir, "EnvAssets");
            Directory.CreateDirectory(envAssetsDir);
            ExtractResource(ResSitecustomize, Path.Combine(envAssetsDir, "mfa_env_sitecustomize.py"));
            ExtractResource(ResMfaUtils, Path.Combine(envAssetsDir, "mfa_utils.py"));

            return new ExtractedPaths(rootDir, micromambaExe, msiPath, envAssetsDir);
        }

        /// <summary>
        /// 程序退出时清理释放出来的临时目录。在 Program.cs 的
        /// Application.Run(...) 之后调用；失败（比如某个文件还被子进程
        /// 占用）不影响程序正常退出，静默忽略——临时目录留在
        /// %TEMP% 下不会造成功能性问题，下次启动会被整个重新覆盖。
        /// </summary>
        public static void Cleanup()
        {
            string rootDir = Path.Combine(Path.GetTempPath(), TempDirName);
            try
            {
                if (Directory.Exists(rootDir))
                    Directory.Delete(rootDir, recursive: true);
            }
            catch
            {
                // 静默忽略，见上方注释。
            }
        }

        /// <summary>
        /// 从当前程序集按 <paramref name="resourceName"/>（必须与 .csproj 里
        /// 对应 EmbeddedResource 项的 LogicalName 完全一致）取出资源流，
        /// 写入 <paramref name="destPath"/>。
        /// </summary>
        private static string ExtractResource(string resourceName, string destPath)
        {
            Assembly asm = Assembly.GetExecutingAssembly();

            using (Stream resStream = asm.GetManifestResourceStream(resourceName))
            {
                if (resStream == null)
                {
                    throw new FileNotFoundException(
                        Strings.Get("Log.EmbeddedResourceMissing", resourceName), resourceName);
                }
                using (FileStream fileStream = File.Create(destPath))
                {
                    resStream.CopyTo(fileStream);
                }
            }

            return destPath;
        }
    }
}
