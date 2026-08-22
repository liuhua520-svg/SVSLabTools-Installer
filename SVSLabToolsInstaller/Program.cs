using System;
using System.Windows.Forms;
using SVSLabToolsInstaller.Core;

namespace SVSLabToolsInstaller
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 捕获所有未处理异常并展示给用户，而不是让程序静默崩溃退出——
            // 安装器这类工具一旦崩溃、用户往往搞不清楚到底装到了哪一步，
            // 至少弹一个错误框、把异常信息显示出来，方便用户截图反馈。
            Application.ThreadException += (s, e) =>
            {
                MessageBox.Show(
                    Strings.Get("Msg.ThreadExceptionBody", e.Exception),
                    Strings.Get("Msg.ThreadExceptionTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            };
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                MessageBox.Show(
                    Strings.Get("Msg.UnhandledExceptionBody", e.ExceptionObject),
                    Strings.Get("Msg.UnhandledExceptionTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            };

            try
            {
                Application.Run(new MainForm());
            }
            finally
            {
                // 程序退出前清理 PayloadExtractor 释放到临时目录的文件
                // （micromamba.exe / MSI / EnvAssets）。放在 finally 里
                // 保证无论窗体正常关闭还是中途抛出未处理异常导致
                // Application.Run 提前返回，都会执行清理，不会在
                // %TEMP% 下留下这次运行释放出的 52MB 文件。
                // Cleanup() 内部本身吞掉了失败（比如文件被占用），
                // 这里不需要额外 try-catch。
                PayloadExtractor.Cleanup();
            }
        }
    }
}
