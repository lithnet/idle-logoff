using System;
using System.Windows.Forms;

namespace Lithnet.idlelogoff
{
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {

            RefreshUI();
        }

        private void RefreshUI()
        {

            lbProductName.Text = "Lithnet.idlelogoff";
            lbProductVersion.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(3);

            ckEnableIdleLogoff.Checked = Settings.Enabled;

            if (Settings.IsSettingFromPolicy("Enabled"))
                ckEnableIdleLogoff.Enabled = false;
            else
                ckEnableIdleLogoff.Enabled = true;

            try
            {
                udMinutes.Value = Settings.IdleLimit;
            }
            catch
            {
                udMinutes.Value = 60;
            }

            if (Settings.IsSettingFromPolicy("IdleLimit"))
                udMinutes.Enabled = false;
            else
                udMinutes.Enabled = true;

            if ((!udMinutes.Enabled) | (!ckEnableIdleLogoff.Enabled))
                lbGPControlled.Visible = true;
            
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (ckEnableIdleLogoff.Enabled)
                    Settings.Enabled = ckEnableIdleLogoff.Checked;

                if (udMinutes.Enabled)
                    Settings.IdleLimit = (int)udMinutes.Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occured when trying to save the settings.\n\n" + ex.Message);
            }

            if (!EventLogging.IsEventSourceRegistered())
            {
                try
                {
                    EventLogging.RegisterEventSource();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occured registering the event source. Event log messages will not be logged for application events\n\n" + ex.Message);
                }
            }

            try
            {
                Settings.CreateStartupRegKey();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured while configuring the tool to automatically run at logon. The tool must be started with the '/start' switch for each user that logs on to take effect\n\n" + ex.Message);
            }

            Environment.Exit(0);

        }

        private void lbHiddenRefresh_Click(object sender, EventArgs e) 
        {
            RefreshUI();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

             
    }
}
