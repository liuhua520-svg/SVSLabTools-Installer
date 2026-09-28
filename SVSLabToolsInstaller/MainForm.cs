using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SVSLabToolsInstaller.Core;

namespace SVSLabToolsInstaller
{
    public partial class MainForm : Form
    {
        private CancellationTokenSource _cts;
        private bool _installRunning;

        // 构造函数阶段释放内嵌 Payload 得到的实际文件路径，后续
        // btnStart_Click 里原来拼接 "Payload\..." 的两处都改用这里。
        // 用字段而不是每次现拼路径，是因为 Extract() 本身有实际的
        // 磁盘 I/O 开销（复制 52MB），不应该被重复调用。
        private PayloadExtractor.ExtractedPaths _payload;

        public MainForm()
        {
            InitializeComponent();
            RefreshUiTexts();
            txtInstallDir.Text = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                "SVS Lab Tools");
            radioTorchCpu.Checked = true;
            UpdateComponentSizeHints();

            // 尽早释放、尽早暴露问题：原来的设计是在用户点"开始安装"
            // 之前就校验 micromamba.exe / MSI 是否存在（避免装到一半才
            // 发现文件缺失），这里保持同样的"尽早"精神——程序集内嵌资源
            // 一旦损坏或缺失，属于安装包本身有问题，用户一打开窗口就
            // 应该看到明确提示，而不是等到点了按钮、甚至装到一半才报错。
            // 失败时提示后禁用安装按钮而不是直接退出整个进程——用户至少
            // 还能看清楚错误信息、通过界面截图反馈问题，比程序自己
            // 静默关闭更容易排查。
            try
            {
                _payload = PayloadExtractor.Extract();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    Strings.Get("Msg.PayloadExtractFailedBody", ex.Message),
                    Strings.Get("Msg.MissingFileTitle"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnStart.Enabled = false;
            }
        }

        // ────────────────────────────────────────────────────────────
        // 界面本地化：控件名 → 字符串表 key 的映射。
        //
        // 【为什么不直接在 MainForm.Designer.cs 里写 Strings.Get(...)】
        // 之前试过这样做，但 Visual Studio 设计器只要在设计视图里打开
        // 并保存过这个窗体一次，就会把它认为的"控件当前显示文本"重新
        // 生成回 .Text = "字面值" 这种形式，直接冲掉 Strings.Get(...)
        // 调用——设计器不理解、也不会保留任意 C# 表达式，只理解字面量。
        // 这不是一次性事故，只要还用设计器可视化编辑这个窗体，每次
        // 保存都会重新触发同样的覆盖。
        //
        // 所以改为：Designer.cs 保持"纯设计器生成物"，可以放心用设计器
        // 拖拽/编辑控件，不用担心冲掉本地化逻辑；真正的本地化统一在这
        // 张映射表 + RefreshUiTexts() 里做，构造函数里
        // InitializeComponent() 之后立即调用一次，把设计器写回的默认
        // 文本（不管是中文还是别的）按控件 Name 查表、整体覆盖成当前
        // 界面语言对应的文本。
        //
        // 【以后新增控件时怎么加】只需要两步：
        //   1. 在下面这张字典里加一行 "控件Name" -> "字符串表的key"
        //   2. 在 Core/Strings.*.cs 五个文件里给这个 key 补上五种语言的翻译
        // 不需要去 Designer.cs 或者别处再手写一行赋值代码——设计器改动
        // 控件本身（位置、大小、新增/删除控件）都不会影响这张映射表，
        // 只有"这个控件对应哪个翻译 key"这一件事记在这里。
        // ────────────────────────────────────────────────────────────
        private static readonly Dictionary<string, string> ControlTextKeys = new Dictionary<string, string>
        {
            ["grpComponents"] = "Ui.GroupComponents",
            ["chkCore"] = "Ui.ChkCore",
            ["grpLang"] = "Ui.GroupLang",
            ["chkLangCmn"] = "Ui.LangChinese",
            ["chkLangEng"] = "Ui.LangEnglish",
            ["chkLangJpn"] = "Ui.LangJapanese",
            ["chkLangKor"] = "Ui.LangKorean",
            ["chkLangYue"] = "Ui.LangCantonese",
            ["chkNemo"] = "Ui.ChkNemo",
            ["chkWhisperX"] = "Ui.ChkWhisperX",
            ["chkQwen3Tts"] = "Ui.ChkQwen3Tts",
            ["grpTorch"] = "Ui.GroupTorch",
            ["radioTorchCpu"] = "Ui.TorchCpu",
            ["radioTorchCuda118"] = "Ui.TorchCuda118",
            ["radioTorchCuda121"] = "Ui.TorchCuda121",
            ["lblInstallDir"] = "Ui.InstallDirLabel",
            ["btnBrowseDir"] = "Ui.BrowseButton",
            // btnStart 的"开始安装"/"取消安装"两种状态文案由
            // SetInstallingState() 在状态切换时用 Strings.Get 设置，
            // 但窗口刚打开、用户还未点击过按钮时 SetInstallingState
            // 从未被调用过，此时它显示的是 Designer.cs 里的默认字面值。
            // 这里补上初始状态（未安装）对应的 key，构造函数走完
            // RefreshUiTexts() 后按钮就有正确的初始文案；后续状态切换
            // 仍然由 SetInstallingState 接管，不冲突。
            ["btnStart"] = "Ui.StartInstall",
            // lblSizeHint 内容依赖 UpdateComponentSizeHints() 里的动态
            // 拼接逻辑，不放在这张表里，避免这里的固定文案覆盖掉它。
        };

        /// <summary>
        /// 按 <see cref="ControlTextKeys"/> 把窗体上（含所有层级嵌套，
        /// 比如 GroupBox 内部的 CheckBox/RadioButton）已注册的控件文本
        /// 刷新成当前界面语言对应的翻译。窗体标题（this.Text）单独处理，
        /// 因为窗体本身不在 Controls 递归范围内。
        /// </summary>
        private void RefreshUiTexts()
        {
            this.Text = Strings.Get("Ui.WindowTitle");
            RefreshControlTextsRecursive(this.Controls);
        }

        private static void RefreshControlTextsRecursive(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                string key;
                if (ControlTextKeys.TryGetValue(control.Name, out key))
                {
                    control.Text = Strings.Get(key);
                }
                if (control.Controls.Count > 0)
                {
                    RefreshControlTextsRecursive(control.Controls);
                }
            }
        }

        // ────────────────────────────────────────────────────────────
        // 组件联动提示：与原 installer.iss（Inno Setup 版本）里
        // NextButtonClick 对 tts/qwen3fa 的联动检查是同一个思路的延续
        // ——不过当前这批 bat 脚本迁移过来的功能集合里没有独立的
        // "TTS 跟读" 组件（那是另一版更早的 payload 设计），这里改成
        // 更贴合本项目实际功能集合的提示：勾了任意语言模型但没勾 Core，
        // 语言模型下载必然会失败（依赖 Core 建的 .mfa_env），在用户点
        // "开始安装" 之前就提醒，而不是等装到一半才报错。
        // ────────────────────────────────────────────────────────────
        private bool ValidateSelectionBeforeInstall(InstallSelection sel)
        {
            bool anyLangSelected = sel.LangChinese || sel.LangEnglish || sel.LangJapanese
                                    || sel.LangKorean || sel.LangCantonese;
            if (anyLangSelected && !sel.Core && !Directory.Exists(EnvironmentPlanner.EnvPrefixFor(txtInstallDir.Text.Trim(), ".mfa_env")))
            {
                var choice = MessageBox.Show(
                    Strings.Get("Msg.MissingCoreBody"),
                    Strings.Get("Msg.MissingCoreTitle"),
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);
                if (choice == DialogResult.Cancel) return false;
                if (choice == DialogResult.Yes) chkCore.Checked = true;
            }

            bool anySelected = sel.Core || anyLangSelected || sel.Nemo || sel.WhisperX || sel.Qwen3Tts;
            if (!anySelected)
            {
                MessageBox.Show(Strings.Get("Msg.NoSelectionBody"), Strings.Get("Msg.NoSelectionTitle"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            return true;
        }

        private InstallSelection CollectSelection()
        {
            return new InstallSelection
            {
                Core = chkCore.Checked,
                LangChinese = chkLangCmn.Checked,
                LangEnglish = chkLangEng.Checked,
                LangJapanese = chkLangJpn.Checked,
                LangKorean = chkLangKor.Checked,
                LangCantonese = chkLangYue.Checked,
                Nemo = chkNemo.Checked,
                WhisperX = chkWhisperX.Checked,
                Qwen3Tts = chkQwen3Tts.Checked,
                Torch = radioTorchCuda118.Checked ? TorchVariant.Cuda118
                       : radioTorchCuda121.Checked ? TorchVariant.Cuda121
                       : TorchVariant.Cpu,
            };
        }

        // 语言模型勾选框依赖 Core 复选框状态，随手更新一下大致的磁盘空间
        // 提示文字（粗略估算，不追求精确，只是给用户一个数量级概念）。
        private void UpdateComponentSizeHints()
        {
            lblSizeHint.Text = Strings.Get("Ui.SizeHint");
        }

        private async void btnBrowseDir_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.Description = Strings.Get("Ui.BrowseDialogDescription");
                if (Directory.Exists(txtInstallDir.Text.Trim()))
                    dlg.SelectedPath = txtInstallDir.Text.Trim();
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    txtInstallDir.Text = dlg.SelectedPath;
                }
            }
            await Task.CompletedTask;
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            if (_installRunning)
            {
                // 正在安装时这个按钮变成"取消"用途，见 SetInstallingState。
                _cts?.Cancel();
                return;
            }

            string appDir = txtInstallDir.Text.Trim();
            if (string.IsNullOrEmpty(appDir))
            {
                MessageBox.Show(Strings.Get("Msg.NoInstallDirBody"), Strings.Get("Msg.NoInstallDirTitle"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var sel = CollectSelection();
            if (!ValidateSelectionBeforeInstall(sel))
                return;
            // ValidateSelectionBeforeInstall 内部可能把 chkCore.Checked 改成
            // true（用户选了"一并勾选 Core"），重新采集一次选择状态，
            // 确保后面生成的安装计划里包含这个变更。
            sel = CollectSelection();

            // Payload 已在构造函数阶段释放到临时目录（见 _payload 字段）；
            // _payload 为 null 说明释放失败，构造函数里已经弹过错误提示并
            // 禁用了 btnStart，这里是双重保险（理论上禁用状态下点不到）。
            if (_payload == null)
            {
                return;
            }
            string micromambaExe = _payload.MicromambaExe;

            if (!File.Exists(micromambaExe))
            {
                MessageBox.Show(
                    Strings.Get("Msg.MissingMicromambaBody", micromambaExe),
                    Strings.Get("Msg.MissingFileTitle"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // backend/frontend 不再由本程序复制——那部分文件由随后自动
            // 调起的 "SVS Lab Tools.msi" 负责安装（见 MsiLauncher）。这里
            // 提前校验 MSI 是否存在，尽早暴露"安装包不完整"的问题，而不是
            // 等虚拟环境全部装完（可能耗时十几分钟）之后才发现装不了。
            string msiPath = _payload.MsiPath;
            if (!File.Exists(msiPath))
            {
                MessageBox.Show(
                    Strings.Get("Msg.MissingMsiBody", msiPath),
                    Strings.Get("Msg.MissingFileTitle"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            txtLog.Clear();
            SetInstallingState(true);
            _cts = new CancellationTokenSource();
            _msiNeedsReboot = false;

            try
            {
                Directory.CreateDirectory(appDir);
                Directory.CreateDirectory(Path.Combine(appDir, "_setup"));
                await Task.Run(() =>
                {
                    File.Copy(micromambaExe, Path.Combine(appDir, "_setup", "micromamba.exe"), overwrite: true);
                }, _cts.Token).ConfigureAwait(true);

                // envAssetsDir：mfa_env_sitecustomize.py / mfa_utils.py 的
                // 独立打包位置（Payload\EnvAssets\），不依赖 appDir\backend
                // 是否已经存在——backend 由随后调起的 MSI 安装，两条流程
                // 故意保持互不依赖，谁先跑都不会互相卡住。
                string envAssetsDir = _payload.EnvAssetsDir;
                var planner = new EnvironmentPlanner(appDir, Path.Combine(appDir, "_setup", "micromamba.exe"),
                    envAssetsDir);
                planner.LineReceived += line => AppendLogThreadSafe(line);

                List<InstallStep> plan = planner.BuildPlan(sel);

                // 虚拟环境全部建完后，最后追加一步：静默调起 MSI 安装
                // backend/frontend + 快捷方式 + 卸载信息。放在步骤列表
                // 末尾而不是单独写一套流程，是为了让进度条、失败重试、
                // 取消等现有机制原样覆盖到这一步，不需要额外处理。
                plan.Add(new InstallStep(Strings.Get("Step.InstallMsi"),
                    ct => InstallMsiStepAsync(msiPath, appDir, ct)));

                var results = new List<InstallStepResult>();

                progressBar.Minimum = 0;
                progressBar.Maximum = plan.Count;
                progressBar.Value = 0;

                foreach (InstallStep step in plan)
                {
                    _cts.Token.ThrowIfCancellationRequested();
                    lblCurrentStep.Text = Strings.Get("Ui.CurrentStepRunning", step.DisplayName);
                    AppendLog("");
                    AppendLog("================================================================================");
                    AppendLog(step.DisplayName);
                    AppendLog("================================================================================");

                    bool ok;
                    try
                    {
                        ok = await step.ExecuteAsync(_cts.Token).ConfigureAwait(true);
                    }
                    catch (OperationCanceledException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        AppendLog(Strings.Get("Log.StepException", ex.Message));
                        ok = false;
                    }

                    results.Add(new InstallStepResult(step.DisplayName, ok));
                    progressBar.Value = Math.Min(progressBar.Value + 1, progressBar.Maximum);
                }

                ShowSummary(results);
            }
            catch (OperationCanceledException)
            {
                AppendLog("");
                AppendLog(Strings.Get("Log.UserCancelled"));
                MessageBox.Show(Strings.Get("Msg.CancelledBody"), Strings.Get("Msg.CancelledTitle"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                AppendLog("");
                AppendLog(Strings.Get("Log.UnexpectedError", ex));
                MessageBox.Show(
                    Strings.Get("Msg.UnexpectedErrorBody", ex.Message),
                    Strings.Get("Msg.UnexpectedErrorTitle"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetInstallingState(false);
                _cts?.Dispose();
                _cts = null;
            }
        }

        // MSI 安装步骤的执行体：调用 MsiLauncher 静默安装，成功/失败都
        // 通过日志回显，并把"需要重启"的情况单独在汇总阶段提示用户。
        private bool _msiNeedsReboot;

        private async Task<bool> InstallMsiStepAsync(string msiPath, string appDir, CancellationToken ct)
        {
            MsiInstallResult result = await MsiLauncher.InstallSilentlyAsync(msiPath, appDir, AppendLogThreadSafe, ct)
                .ConfigureAwait(false);
            if (result.NeedsReboot) _msiNeedsReboot = true;
            return result.Success;
        }

        private void ShowSummary(List<InstallStepResult> results)
        {
            List<InstallStepResult> failed = results.Where(r => !r.Success).ToList();
            AppendLog("");
            AppendLog("================================================================================");
            if (failed.Count == 0)
            {
                AppendLog(Strings.Get("Log.AllDone"));
                AppendLog("================================================================================");
                lblCurrentStep.Text = Strings.Get("Ui.CurrentStepDone");
                string msg = Strings.Get("Msg.AllSuccessBody");
                if (_msiNeedsReboot)
                    msg += Strings.Get("Msg.AllSuccessRebootSuffix");
                MessageBox.Show(msg, Strings.Get("Msg.AllSuccessTitle"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                AppendLog(Strings.Get("Log.DoneWithFailuresHeader"));
                foreach (InstallStepResult r in failed)
                    AppendLog(Strings.Get("Log.FailedItemLine", r.DisplayName));
                AppendLog("================================================================================");
                lblCurrentStep.Text = Strings.Get("Ui.CurrentStepDoneWithFailures", failed.Count);

                var sb = new StringBuilder();
                sb.AppendLine(Strings.Get("Msg.PartialFailIntro"));
                sb.AppendLine();
                foreach (InstallStepResult r in failed)
                    sb.AppendLine(Strings.Get("Log.FailedItemLine", r.DisplayName));
                sb.AppendLine();
                sb.AppendLine(Strings.Get("Msg.PartialFailOutro"));
                MessageBox.Show(sb.ToString(), Strings.Get("Msg.PartialFailTitle"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SetInstallingState(bool installing)
        {
            _installRunning = installing;
            btnStart.Text = installing ? Strings.Get("Ui.CancelInstall") : Strings.Get("Ui.StartInstall");
            grpComponents.Enabled = !installing;
            grpTorch.Enabled = !installing;
            txtInstallDir.Enabled = !installing;
            btnBrowseDir.Enabled = !installing;
            if (!installing)
            {
                progressBar.Value = 0;
                lblCurrentStep.Text = string.Empty;
            }
        }

        // 【当前未被调用】backend/frontend 改由 MSI 安装后，本程序不再需要
        // 自己复制这两个目录。保留这个通用递归复制方法是为了以后如果要
        // 在 C# 侧额外复制别的内容（比如用户自定义词典/模型）时可以直接
        // 复用，但截至目前的安装流程里已经没有调用点。
        private static void CopyDirectory(string sourceDir, string destDir, CancellationToken ct)
        {
            if (!Directory.Exists(sourceDir))
                throw new DirectoryNotFoundException($"找不到源目录: {sourceDir}");

            Directory.CreateDirectory(destDir);
            foreach (string file in Directory.GetFiles(sourceDir))
            {
                ct.ThrowIfCancellationRequested();
                string destFile = Path.Combine(destDir, Path.GetFileName(file));
                File.Copy(file, destFile, overwrite: true);
            }
            foreach (string subDir in Directory.GetDirectories(sourceDir))
            {
                ct.ThrowIfCancellationRequested();
                string destSubDir = Path.Combine(destDir, Path.GetFileName(subDir));
                CopyDirectory(subDir, destSubDir, ct);
            }
        }

        // 日志追加：EnvironmentPlanner.LineReceived 是在后台 Task 线程上
        // 触发的，直接操作 txtLog（一个 WinForms 控件）会抛
        // InvalidOperationException（跨线程访问控件）。这里用
        // InvokeRequired + Invoke 做标准的线程封送。
        private void AppendLogThreadSafe(string line)
        {
            if (txtLog.InvokeRequired)
            {
                try
                {
                    txtLog.Invoke(new Action(() => AppendLog(line)));
                }
                catch (ObjectDisposedException)
                {
                    // 窗体可能已经在安装过程中被关闭，忽略这次日志写入。
                }
                catch (InvalidOperationException)
                {
                    // 窗体句柄可能尚未创建或已销毁，同样安全忽略。
                }
            }
            else
            {
                AppendLog(line);
            }
        }

        private void AppendLog(string line)
        {
            txtLog.AppendText(line + Environment.NewLine);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_installRunning)
            {
                var choice = MessageBox.Show(
                    Strings.Get("Msg.ClosingWhileInstallingBody"),
                    Strings.Get("Msg.ClosingWhileInstallingTitle"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);
                if (choice == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
                _cts?.Cancel();
            }
        }

        private void lblSizeHint_Click(object sender, EventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void chkCore_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
