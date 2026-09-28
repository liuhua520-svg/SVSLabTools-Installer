namespace SVSLabToolsInstaller
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.grpComponents = new System.Windows.Forms.GroupBox();
            this.chkQwen3Tts = new System.Windows.Forms.CheckBox();
            this.chkWhisperX = new System.Windows.Forms.CheckBox();
            this.chkNemo = new System.Windows.Forms.CheckBox();
            this.grpLang = new System.Windows.Forms.GroupBox();
            this.chkLangYue = new System.Windows.Forms.CheckBox();
            this.chkLangKor = new System.Windows.Forms.CheckBox();
            this.chkLangJpn = new System.Windows.Forms.CheckBox();
            this.chkLangEng = new System.Windows.Forms.CheckBox();
            this.chkLangCmn = new System.Windows.Forms.CheckBox();
            this.chkCore = new System.Windows.Forms.CheckBox();
            this.grpTorch = new System.Windows.Forms.GroupBox();
            this.radioTorchCuda121 = new System.Windows.Forms.RadioButton();
            this.radioTorchCuda118 = new System.Windows.Forms.RadioButton();
            this.radioTorchCpu = new System.Windows.Forms.RadioButton();
            this.lblInstallDir = new System.Windows.Forms.Label();
            this.txtInstallDir = new System.Windows.Forms.TextBox();
            this.btnBrowseDir = new System.Windows.Forms.Button();
            this.lblSizeHint = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblCurrentStep = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.grpComponents.SuspendLayout();
            this.grpLang.SuspendLayout();
            this.grpTorch.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpComponents
            // 
            this.grpComponents.Controls.Add(this.chkQwen3Tts);
            this.grpComponents.Controls.Add(this.chkWhisperX);
            this.grpComponents.Controls.Add(this.chkNemo);
            this.grpComponents.Controls.Add(this.grpLang);
            this.grpComponents.Controls.Add(this.chkCore);
            this.grpComponents.Location = new System.Drawing.Point(10, 10);
            this.grpComponents.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpComponents.Name = "grpComponents";
            this.grpComponents.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpComponents.Size = new System.Drawing.Size(402, 240);
            this.grpComponents.TabIndex = 0;
            this.grpComponents.TabStop = false;
            this.grpComponents.Text = "选择要安装的功能";
            // 
            // chkQwen3Tts
            // 
            this.chkQwen3Tts.AutoSize = true;
            this.chkQwen3Tts.Location = new System.Drawing.Point(14, 194);
            this.chkQwen3Tts.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkQwen3Tts.Name = "chkQwen3Tts";
            this.chkQwen3Tts.Size = new System.Drawing.Size(234, 16);
            this.chkQwen3Tts.TabIndex = 4;
            this.chkQwen3Tts.Text = "Qwen3-TTS（独立环境，语音合成服务）";
            this.chkQwen3Tts.UseVisualStyleBackColor = true;
            // 
            // chkWhisperX
            // 
            this.chkWhisperX.AutoSize = true;
            this.chkWhisperX.Location = new System.Drawing.Point(14, 174);
            this.chkWhisperX.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkWhisperX.Name = "chkWhisperX";
            this.chkWhisperX.Size = new System.Drawing.Size(228, 16);
            this.chkWhisperX.TabIndex = 3;
            this.chkWhisperX.Text = "WhisperX（独立环境，语音识别服务）";
            this.chkWhisperX.UseVisualStyleBackColor = true;
            // 
            // chkNemo
            // 
            this.chkNemo.AutoSize = true;
            this.chkNemo.Location = new System.Drawing.Point(14, 154);
            this.chkNemo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkNemo.Name = "chkNemo";
            this.chkNemo.Size = new System.Drawing.Size(222, 16);
            this.chkNemo.TabIndex = 2;
            this.chkNemo.Text = "NeMo-FA（独立环境，强制对齐服务）";
            this.chkNemo.UseVisualStyleBackColor = true;
            // 
            // grpLang
            // 
            this.grpLang.Controls.Add(this.chkLangYue);
            this.grpLang.Controls.Add(this.chkLangKor);
            this.grpLang.Controls.Add(this.chkLangJpn);
            this.grpLang.Controls.Add(this.chkLangEng);
            this.grpLang.Controls.Add(this.chkLangCmn);
            this.grpLang.Location = new System.Drawing.Point(14, 42);
            this.grpLang.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpLang.Name = "grpLang";
            this.grpLang.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpLang.Size = new System.Drawing.Size(382, 104);
            this.grpLang.TabIndex = 1;
            this.grpLang.TabStop = false;
            this.grpLang.Text = "MFA 语言模型（依赖 Core）";
            this.grpLang.Visible = false;
            // 
            // chkLangYue
            // 
            this.chkLangYue.AutoSize = true;
            this.chkLangYue.Location = new System.Drawing.Point(152, 40);
            this.chkLangYue.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkLangYue.Name = "chkLangYue";
            this.chkLangYue.Size = new System.Drawing.Size(48, 16);
            this.chkLangYue.TabIndex = 4;
            this.chkLangYue.Text = "粤语";
            this.chkLangYue.UseVisualStyleBackColor = true;
            this.chkLangYue.Visible = false;
            // 
            // chkLangKor
            // 
            this.chkLangKor.AutoSize = true;
            this.chkLangKor.Location = new System.Drawing.Point(152, 20);
            this.chkLangKor.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkLangKor.Name = "chkLangKor";
            this.chkLangKor.Size = new System.Drawing.Size(48, 16);
            this.chkLangKor.TabIndex = 3;
            this.chkLangKor.Text = "韩语";
            this.chkLangKor.UseVisualStyleBackColor = true;
            // 
            // chkLangJpn
            // 
            this.chkLangJpn.AutoSize = true;
            this.chkLangJpn.Location = new System.Drawing.Point(14, 60);
            this.chkLangJpn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkLangJpn.Name = "chkLangJpn";
            this.chkLangJpn.Size = new System.Drawing.Size(48, 16);
            this.chkLangJpn.TabIndex = 2;
            this.chkLangJpn.Text = "日语";
            this.chkLangJpn.UseVisualStyleBackColor = true;
            // 
            // chkLangEng
            // 
            this.chkLangEng.AutoSize = true;
            this.chkLangEng.Location = new System.Drawing.Point(14, 40);
            this.chkLangEng.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkLangEng.Name = "chkLangEng";
            this.chkLangEng.Size = new System.Drawing.Size(48, 16);
            this.chkLangEng.TabIndex = 1;
            this.chkLangEng.Text = "英语";
            this.chkLangEng.UseVisualStyleBackColor = true;
            // 
            // chkLangCmn
            // 
            this.chkLangCmn.AutoSize = true;
            this.chkLangCmn.Location = new System.Drawing.Point(14, 20);
            this.chkLangCmn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkLangCmn.Name = "chkLangCmn";
            this.chkLangCmn.Size = new System.Drawing.Size(84, 16);
            this.chkLangCmn.TabIndex = 0;
            this.chkLangCmn.Text = "中文普通话";
            this.chkLangCmn.UseVisualStyleBackColor = true;
            // 
            // chkCore
            // 
            this.chkCore.AutoSize = true;
            this.chkCore.Checked = true;
            this.chkCore.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCore.Enabled = false;
            this.chkCore.Location = new System.Drawing.Point(14, 20);
            this.chkCore.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkCore.Name = "chkCore";
            this.chkCore.Size = new System.Drawing.Size(252, 16);
            this.chkCore.TabIndex = 0;
            this.chkCore.Text = "Core（核心环境，主运行环境，建议勾选）";
            this.chkCore.UseVisualStyleBackColor = true;
            this.chkCore.CheckedChanged += new System.EventHandler(this.chkCore_CheckedChanged);
            // 
            // grpTorch
            // 
            this.grpTorch.Controls.Add(this.radioTorchCuda121);
            this.grpTorch.Controls.Add(this.radioTorchCuda118);
            this.grpTorch.Controls.Add(this.radioTorchCpu);
            this.grpTorch.Location = new System.Drawing.Point(418, 10);
            this.grpTorch.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpTorch.Name = "grpTorch";
            this.grpTorch.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpTorch.Size = new System.Drawing.Size(475, 104);
            this.grpTorch.TabIndex = 1;
            this.grpTorch.TabStop = false;
            this.grpTorch.Text = "PyTorch 硬件类型（影响所有环境的 torch 版本）";
            // 
            // radioTorchCuda121
            // 
            this.radioTorchCuda121.AutoSize = true;
            this.radioTorchCuda121.Location = new System.Drawing.Point(15, 68);
            this.radioTorchCuda121.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radioTorchCuda121.Name = "radioTorchCuda121";
            this.radioTorchCuda121.Size = new System.Drawing.Size(275, 16);
            this.radioTorchCuda121.TabIndex = 2;
            this.radioTorchCuda121.Text = "CUDA 12.1（较新独立显卡，如 RTX 30/40 系）";
            this.radioTorchCuda121.UseVisualStyleBackColor = true;
            // 
            // radioTorchCuda118
            // 
            this.radioTorchCuda118.AutoSize = true;
            this.radioTorchCuda118.Location = new System.Drawing.Point(15, 44);
            this.radioTorchCuda118.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radioTorchCuda118.Name = "radioTorchCuda118";
            this.radioTorchCuda118.Size = new System.Drawing.Size(305, 16);
            this.radioTorchCuda118.TabIndex = 1;
            this.radioTorchCuda118.Text = "CUDA 11.8（旧款显卡，如 GTX 10 系 Pascal 架构）";
            this.radioTorchCuda118.UseVisualStyleBackColor = true;
            // 
            // radioTorchCpu
            // 
            this.radioTorchCpu.AutoSize = true;
            this.radioTorchCpu.Location = new System.Drawing.Point(15, 20);
            this.radioTorchCpu.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radioTorchCpu.Name = "radioTorchCpu";
            this.radioTorchCpu.Size = new System.Drawing.Size(263, 16);
            this.radioTorchCpu.TabIndex = 0;
            this.radioTorchCpu.TabStop = true;
            this.radioTorchCpu.Text = "CPU（无独立显卡、非N卡，或不确定，推荐）";
            this.radioTorchCpu.UseVisualStyleBackColor = true;
            // 
            // lblInstallDir
            // 
            this.lblInstallDir.AutoSize = true;
            this.lblInstallDir.Location = new System.Drawing.Point(416, 124);
            this.lblInstallDir.Name = "lblInstallDir";
            this.lblInstallDir.Size = new System.Drawing.Size(65, 12);
            this.lblInstallDir.TabIndex = 2;
            this.lblInstallDir.Text = "安装目录：";
            // 
            // txtInstallDir
            // 
            this.txtInstallDir.Location = new System.Drawing.Point(418, 138);
            this.txtInstallDir.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtInstallDir.Name = "txtInstallDir";
            this.txtInstallDir.Size = new System.Drawing.Size(317, 21);
            this.txtInstallDir.TabIndex = 3;
            // 
            // btnBrowseDir
            // 
            this.btnBrowseDir.Location = new System.Drawing.Point(741, 138);
            this.btnBrowseDir.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnBrowseDir.Name = "btnBrowseDir";
            this.btnBrowseDir.Size = new System.Drawing.Size(152, 20);
            this.btnBrowseDir.TabIndex = 4;
            this.btnBrowseDir.Text = "浏览...";
            this.btnBrowseDir.UseVisualStyleBackColor = true;
            this.btnBrowseDir.Click += new System.EventHandler(this.btnBrowseDir_Click);
            // 
            // lblSizeHint
            // 
            this.lblSizeHint.Location = new System.Drawing.Point(418, 164);
            this.lblSizeHint.Name = "lblSizeHint";
            this.lblSizeHint.Size = new System.Drawing.Size(475, 72);
            this.lblSizeHint.TabIndex = 5;
            this.lblSizeHint.Click += new System.EventHandler(this.lblSizeHint_Click);
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnStart.Location = new System.Drawing.Point(418, 254);
            this.btnStart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(475, 32);
            this.btnStart.TabIndex = 6;
            this.btnStart.Text = "开始安装";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(10, 262);
            this.progressBar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(402, 18);
            this.progressBar.TabIndex = 7;
            // 
            // lblCurrentStep
            // 
            this.lblCurrentStep.AutoSize = true;
            this.lblCurrentStep.Location = new System.Drawing.Point(10, 283);
            this.lblCurrentStep.Name = "lblCurrentStep";
            this.lblCurrentStep.Size = new System.Drawing.Size(0, 12);
            this.lblCurrentStep.TabIndex = 8;
            // 
            // txtLog
            // 
            this.txtLog.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtLog.Location = new System.Drawing.Point(10, 300);
            this.txtLog.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(883, 209);
            this.txtLog.TabIndex = 9;
            this.txtLog.WordWrap = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(905, 518);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.lblCurrentStep);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lblSizeHint);
            this.Controls.Add(this.btnBrowseDir);
            this.Controls.Add(this.txtInstallDir);
            this.Controls.Add(this.lblInstallDir);
            this.Controls.Add(this.grpTorch);
            this.Controls.Add(this.grpComponents);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SVS Lab Tools 安装器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.grpComponents.ResumeLayout(false);
            this.grpComponents.PerformLayout();
            this.grpLang.ResumeLayout(false);
            this.grpLang.PerformLayout();
            this.grpTorch.ResumeLayout(false);
            this.grpTorch.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpComponents;
        private System.Windows.Forms.CheckBox chkCore;
        private System.Windows.Forms.GroupBox grpLang;
        private System.Windows.Forms.CheckBox chkLangCmn;
        private System.Windows.Forms.CheckBox chkLangEng;
        private System.Windows.Forms.CheckBox chkLangJpn;
        private System.Windows.Forms.CheckBox chkLangKor;
        private System.Windows.Forms.CheckBox chkLangYue;
        private System.Windows.Forms.CheckBox chkNemo;
        private System.Windows.Forms.CheckBox chkWhisperX;
        private System.Windows.Forms.CheckBox chkQwen3Tts;
        private System.Windows.Forms.GroupBox grpTorch;
        private System.Windows.Forms.RadioButton radioTorchCpu;
        private System.Windows.Forms.RadioButton radioTorchCuda118;
        private System.Windows.Forms.RadioButton radioTorchCuda121;
        private System.Windows.Forms.Label lblInstallDir;
        private System.Windows.Forms.TextBox txtInstallDir;
        private System.Windows.Forms.Button btnBrowseDir;
        private System.Windows.Forms.Label lblSizeHint;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblCurrentStep;
        private System.Windows.Forms.TextBox txtLog;
    }
}
