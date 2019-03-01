using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lithnet.idlelogoff;
using Timer = System.Windows.Forms.Timer;

namespace lithnet.idlelogoff
{
    public partial class LogoffWarning : Form
    {
        public DateTime LogoffDateTime
        {
            get => this.logoffDateTime;
            set
            {
                this.logoffDateTime = value;
                this.UpdateLabelText();
            }
        }

        private Timer timer;
        private DateTime logoffDateTime;

        public LogoffWarning()
        {
            this.InitializeComponent();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (this.Visible)
            {
                this.timer = new Timer();
                this.timer.Tick += this.Timer_Tick;
                this.timer.Interval = 1000;
                this.timer.Start();
            }
            else
            {
                this.timer?.Stop();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            this.UpdateLabelText();
        }

        private void UpdateLabelText()
        {
            if (this.LogoffDateTime > DateTime.Now)
            {
                this.lbWarning.Text = string.Format(Settings.WarningMessage, (int) ((this.LogoffDateTime.Subtract(DateTime.Now)).TotalSeconds));
            }
            else
            {
                this.lbWarning.Text = string.Empty;
            }
        }

        private void LogoffWarning_KeyPress(object sender, KeyPressEventArgs e)
        {
            Trace.WriteLine("Hiding warning window on key press");
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Trace.WriteLine("User pressed cancel button");
            this.Hide();
        }
    }
}
