using System;
using System.Threading;
using System.Threading.Tasks;

namespace SVSLabToolsInstaller.Core
{
    /// <summary>
    /// 描述一个安装步骤：界面上显示的名字 + 真正执行的异步逻辑。
    ///
    /// 这是从 bat 版本里 run_setup.bat 依次 call 各个 steps\*.bat 迁移
    /// 过来的核心抽象——原来是"一个 .bat 文件对应一个步骤"，现在是
    /// "一个 InstallStep 对应一个步骤"，执行体从"启动外部 .bat 进程"
    /// 换成了"直接在 C# 里跑一段异步代码（内部会调用 MicromambaRunner
    /// 启动 micromamba.exe 子进程）"。
    /// </summary>
    public sealed class InstallStep
    {
        /// <summary>界面进度列表 / 日志里显示的步骤名称，例如 "Core: 创建主环境"。</summary>
        public string DisplayName { get; }

        /// <summary>
        /// 真正的执行逻辑。返回 true 表示成功，false 表示失败——按已确认
        /// 的失败处理策略（全部步骤失败只警告继续，最后统一汇总），调用方
        /// 不会因为某一步返回 false 就中止后续步骤。
        /// </summary>
        public Func<CancellationToken, Task<bool>> ExecuteAsync { get; }

        public InstallStep(string displayName, Func<CancellationToken, Task<bool>> executeAsync)
        {
            DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
            ExecuteAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
        }
    }

    /// <summary>单个步骤执行完毕后的结果，供 MainForm 汇总展示。</summary>
    public sealed class InstallStepResult
    {
        public string DisplayName { get; }
        public bool Success { get; }

        public InstallStepResult(string displayName, bool success)
        {
            DisplayName = displayName;
            Success = success;
        }
    }
}
