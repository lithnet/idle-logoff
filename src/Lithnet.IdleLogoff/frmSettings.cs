using System;
using System.Windows.Forms;

namespace Lithnet.idlelogoff
{
    public partial class FrmSettings : Form
    {
        public FrmSettings()
        {
            this.InitializeComponent();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            this.RefreshUI();
        }

        private void RefreshUI()
        {
            this.lbProductName.Text = "Lithnet Idle Logoff";
            this.lbProductVersion.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(3);

            this.ckEnableIdleLogoff.Checked = Settings.Enabled;
            this.ckEnableIdleLogoff.Enabled = !Settings.IsSettingFromPolicy(nameof(Settings.Enabled));

            this.ckIgnoreDisplayRequested.Checked = Settings.IgnoreDisplayRequested;
            this.ckIgnoreDisplayRequested.Enabled = !Settings.IsSettingFromPolicy(nameof(Settings.IgnoreDisplayRequested));

            this.cbAction.Items.Clear();

            foreach (string item in Enum.GetNames(typeof(IdleTimeoutAction)))
            {
                this.cbAction.Items.Add(item);
            }

            this.cbAction.SelectedItem = Settings.Action.ToString();
            this.cbAction.Enabled = !Settings.IsSettingFromPolicy(nameof(Settings.Action));

            try
            {
                this.udMinutes.Value = Settings.IdleLimit;
            }
            catch
            {
                this.udMinutes.Value = 60;
            }

            this.udMinutes.Enabled = !Settings.IsSettingFromPolicy(nameof(Settings.IdleLimit));

            this.ckShowWarning.Checked = Settings.WarningEnabled;
            this.ckShowWarning.Enabled = !Settings.IsSettingFromPolicy(nameof(Settings.WarningEnabled));

            this.udWarning.Enabled = !Settings.IsSettingFromPolicy(nameof(Settings.WarningPeriod));
            this.udWarning.Value = Settings.WarningPeriod;

            this.txtWarningMessage.Enabled = !Settings.IsSettingFromPolicy(nameof(Settings.WarningMessage));
            this.txtWarningMessage.Text = Settings.WarningMessage;

            if (!this.udMinutes.Enabled | !this.ckEnableIdleLogoff.Enabled | !this.ckIgnoreDisplayRequested.Enabled | !this.udWarning.Enabled | !this.txtWarningMessage.Enabled | !this.ckShowWarning.Enabled)
            {
                this.lbGPControlled.Visible = true;
            }
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ckEnableIdleLogoff.Enabled)
                {
                    Settings.Enabled = this.ckEnableIdleLogoff.Checked;
                }

                if (this.udMinutes.Enabled)
                {
                    Settings.IdleLimit = (int)this.udMinutes.Value;
                }

                if (this.ckIgnoreDisplayRequested.Enabled)
                {
                    Settings.IgnoreDisplayRequested = this.ckIgnoreDisplayRequested.Checked;
                }

                if (this.ckShowWarning.Enabled)
                {
                    Settings.WarningEnabled = this.ckShowWarning.Checked;
                }

                if (this.txtWarningMessage.Enabled)
                {
                    Settings.WarningMessage = this.txtWarningMessage.Text;
                }

                if (this.udWarning.Enabled)
                {
                    Settings.WarningPeriod = (int)this.udWarning.Value;
                }

                if (this.cbAction.Enabled)
                {
                    Settings.Action = (IdleTimeoutAction)Enum.Parse(typeof(IdleTimeoutAction), (string)this.cbAction.SelectedItem, true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred when trying to save the settings.\n\n" + ex);
            }

            if (!EventLogging.IsEventSourceRegistered())
            {
                try
                {
                    EventLogging.RegisterEventSource();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred registering the event source. Event log messages will not be logged for application events\n\n" + ex.Message);
                }
            }
          
            Environment.Exit(0);
        }

        private void lbHiddenRefresh_Click(object sender, EventArgs e)
        {
            this.RefreshUI();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
