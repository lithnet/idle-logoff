﻿namespace Lithnet.idlelogoff
{
    partial class frmSettings
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
            ((System.ComponentModel.ISupportInitialize)(this.udMinutes)).BeginInit();
            this.SuspendLayout();
            // 
            // ckEnableIdleLogoff
            // 
            this.ckEnableIdleLogoff.AutoSize = true;
            this.ckEnableIdleLogoff.Location = new System.Drawing.Point(100, 54);
            this.ckEnableIdleLogoff.Name = "ckEnableIdleLogoff";
            this.ckEnableIdleLogoff.Size = new System.Drawing.Size(118, 17);
            this.ckEnableIdleLogoff.TabIndex = 0;
            this.ckEnableIdleLogoff.Text = "Enable idle logoff";
            this.ckEnableIdleLogoff.UseVisualStyleBackColor = true;
            // 
            // udMinutes
            // 
            this.udMinutes.Location = new System.Drawing.Point(227, 72);
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
            this.udMinutes.Size = new System.Drawing.Size(93, 22);
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
            this.label1.Location = new System.Drawing.Point(97, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Log off after (minutes):";
            // 
            // btOK
            // 
            this.btOK.Location = new System.Drawing.Point(344, 142);
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
            this.btCancel.Location = new System.Drawing.Point(267, 142);
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
            this.lbProductName.Location = new System.Drawing.Point(7, 144);
            this.lbProductName.Name = "lbProductName";
            this.lbProductName.Size = new System.Drawing.Size(99, 13);
            this.lbProductName.TabIndex = 4;
            this.lbProductName.Text = "Lithnet.IdleLogoff";
            // 
            // lbProductVersion
            // 
            this.lbProductVersion.Location = new System.Drawing.Point(6, 158);
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
            this.lbGPControlled.Size = new System.Drawing.Size(427, 15);
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
            // frmSettings
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(427, 178);
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
            this.Name = "frmSettings";
            this.Text = "IdleLogoff Settings";
            this.Load += new System.EventHandler(this.frmSettings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.udMinutes)).EndInit();
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
    }
}