namespace Lithnet.idlelogoff
{
    partial class FrmSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ckEnableIdleLogoff = new System.Windows.Forms.CheckBox();
            this.udMinutes = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.btOK = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.lbProductName = new System.Windows.Forms.Label();
            this.lbProductVersion = new System.Windows.Forms.Label();
            this.lbGPControlled = new System.Windows.Forms.Label();
            this.lbHiddenRefresh = new System.Windows.Forms.Label();
            this.ckIgnoreDisplayRequested = new System.Windows.Forms.CheckBox();
            this.cbAction = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.udWarning = new System.Windows.Forms.NumericUpDown();
            this.ckShowWarning = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtWarningMessage = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.udMinutes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udWarning)).BeginInit();
            this.SuspendLayout();
            // 
            // ckEnableIdleLogoff
            // 
            this.ckEnableIdleLogoff.AutoSize = true;
            this.ckEnableIdleLogoff.Location = new System.Drawing.Point(38, 35);
            this.ckEnableIdleLogoff.Name = "ckEnableIdleLogoff";
            this.ckEnableIdleLogoff.Size = new System.Drawing.Size(135, 23);
            this.ckEnableIdleLogoff.TabIndex = 0;
            this.ckEnableIdleLogoff.Text = "Enable idle logoff";
            this.ckEnableIdleLogoff.UseVisualStyleBackColor = true;
            // 
            // udMinutes
            // 
            this.udMinutes.Location = new System.Drawing.Point(323, 53);
            this.udMinutes.Maximum = new decimal(new int[] {
            35791,
            0,
            0,
            0});
            this.udMinutes.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udMinutes.Name = "udMinutes";
            this.udMinutes.Size = new System.Drawing.Size(93, 26);
            this.udMinutes.TabIndex = 1;
            this.udMinutes.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "Log off after (minutes):";
            // 
            // btOK
            // 
            this.btOK.Location = new System.Drawing.Point(366, 297);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(71, 24);
            this.btOK.TabIndex = 4;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(289, 297);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(71, 24);
            this.btCancel.TabIndex = 3;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // lbProductName
            // 
            this.lbProductName.AutoSize = true;
            this.lbProductName.Location = new System.Drawing.Point(8, 299);
            this.lbProductName.Name = "lbProductName";
            this.lbProductName.Size = new System.Drawing.Size(116, 19);
            this.lbProductName.TabIndex = 4;
            this.lbProductName.Text = "Lithnet.IdleLogoff";
            // 
            // lbProductVersion
            // 
            this.lbProductVersion.Location = new System.Drawing.Point(8, 312);
            this.lbProductVersion.Name = "lbProductVersion";
            this.lbProductVersion.Size = new System.Drawing.Size(119, 13);
            this.lbProductVersion.TabIndex = 5;
            // 
            // lbGPControlled
            // 
            this.lbGPControlled.BackColor = System.Drawing.SystemColors.Info;
            this.lbGPControlled.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbGPControlled.Location = new System.Drawing.Point(0, 0);
            this.lbGPControlled.Name = "lbGPControlled";
            this.lbGPControlled.Size = new System.Drawing.Size(453, 15);
            this.lbGPControlled.TabIndex = 6;
            this.lbGPControlled.Text = "Some settings are currently configured by group policy and cannot be modified";
            this.lbGPControlled.Visible = false;
            // 
            // lbHiddenRefresh
            // 
            this.lbHiddenRefresh.Location = new System.Drawing.Point(1, 15);
            this.lbHiddenRefresh.Name = "lbHiddenRefresh";
            this.lbHiddenRefresh.Size = new System.Drawing.Size(23, 13);
            this.lbHiddenRefresh.TabIndex = 7;
            this.lbHiddenRefresh.Click += new System.EventHandler(this.lbHiddenRefresh_Click);
            // 
            // ckIgnoreDisplayRequested
            // 
            this.ckIgnoreDisplayRequested.Location = new System.Drawing.Point(38, 232);
            this.ckIgnoreDisplayRequested.Name = "ckIgnoreDisplayRequested";
            this.ckIgnoreDisplayRequested.Size = new System.Drawing.Size(378, 34);
            this.ckIgnoreDisplayRequested.TabIndex = 8;
            this.ckIgnoreDisplayRequested.Text = "Ignore sleep prevention requests from applications such as media playback";
            this.ckIgnoreDisplayRequested.UseVisualStyleBackColor = true;
            // 
            // cbAction
            // 
            this.cbAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAction.FormattingEnabled = true;
            this.cbAction.Location = new System.Drawing.Point(250, 76);
            this.cbAction.Name = "cbAction";
            this.cbAction.Size = new System.Drawing.Size(166, 27);
            this.cbAction.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 19);
            this.label2.TabIndex = 11;
            this.label2.Text = "Action";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(37, 144);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(282, 20);
            this.label3.TabIndex = 13;
            this.label3.Text = "Duration of warning message (seconds)";
            // 
            // udWarning
            // 
            this.udWarning.Location = new System.Drawing.Point(323, 142);
            this.udWarning.Maximum = new decimal(new int[] {
            35791,
            0,
            0,
            0});
            this.udWarning.Name = "udWarning";
            this.udWarning.Size = new System.Drawing.Size(93, 26);
            this.udWarning.TabIndex = 12;
            this.udWarning.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // ckShowWarning
            // 
            this.ckShowWarning.AutoSize = true;
            this.ckShowWarning.Location = new System.Drawing.Point(38, 122);
            this.ckShowWarning.Name = "ckShowWarning";
            this.ckShowWarning.Size = new System.Drawing.Size(294, 23);
            this.ckShowWarning.TabIndex = 14;
            this.ckShowWarning.Text = "Show a warning message before idle action";
            this.ckShowWarning.UseVisualStyleBackColor = true;
            this.ckShowWarning.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(37, 168);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(454, 19);
            this.label4.TabIndex = 15;
            this.label4.Text = "Custom warning message (Use {0} as the placeholder for remaining time)";
            // 
            // txtWarningMessage
            // 
            this.txtWarningMessage.Location = new System.Drawing.Point(40, 189);
            this.txtWarningMessage.Name = "txtWarningMessage";
            this.txtWarningMessage.Size = new System.Drawing.Size(378, 26);
            this.txtWarningMessage.TabIndex = 16;
            // 
            // FrmSettings
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(453, 334);
            this.Controls.Add(this.txtWarningMessage);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ckShowWarning);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.udWarning);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbAction);
            this.Controls.Add(this.ckIgnoreDisplayRequested);
            this.Controls.Add(this.lbHiddenRefresh);
            this.Controls.Add(this.lbGPControlled);
            this.Controls.Add(this.lbProductVersion);
            this.Controls.Add(this.lbProductName);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.udMinutes);
            this.Controls.Add(this.ckEnableIdleLogoff);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSettings";
            this.Text = "IdleLogoff Settings";
            this.Load += new System.EventHandler(this.frmSettings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.udMinutes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udWarning)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox ckEnableIdleLogoff;
        private System.Windows.Forms.NumericUpDown udMinutes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Label lbProductName;
        private System.Windows.Forms.Label lbProductVersion;
        private System.Windows.Forms.Label lbGPControlled;
        private System.Windows.Forms.Label lbHiddenRefresh;
        private System.Windows.Forms.CheckBox ckIgnoreDisplayRequested;
        private System.Windows.Forms.ComboBox cbAction;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown udWarning;
        private System.Windows.Forms.CheckBox ckShowWarning;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtWarningMessage;
    }
}