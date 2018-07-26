using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace BCM_Migration_Tool.Controls
{
    public partial class Migrate : UserControl
    {
        #region Events
        public event EventHandler StartClicked;
        public event EventHandler StopClicked;

        #endregion

        #region Constructors
        public Migrate()
        {
            InitializeComponent();
        }
        #endregion

        #region Control Events
        private void cmdStart_Click(object sender, EventArgs e)
        {
            //lblWarnings.Visible = false;
            StartClicked?.Invoke(this,e);           
        }

        private void cmdStop_Click(object sender, EventArgs e)
        {
            StopClicked?.Invoke(this, e);
            UpdateStatus("Cancelled!");
        }

        #endregion
        #region Methods

        public void UpdateText(string text)
        {
            //txtLog.AppendText(String.Format("{0}{1}{2}", txtLog.Text, Environment.NewLine, text));
            try
            {
                System.Diagnostics.Debug.WriteLine(text);
                txtLog.AppendText(Environment.NewLine + text);
                Invalidate();
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(String.Format("Error in BCM_Migration_Tool.Controls.Migrate.UpdateText(): {0}", ex.Message));
            }
        }

        public void UpdateStatus(string text)
        {
            try
            {
                lblMigrationStatus.Text = text;
                lblMigrationStatus.Visible = true;
                Invalidate();
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(String.Format("Error in BCM_Migration_Tool.Controls.Migrate.UpdateStatus(): {0}", ex.Message));
            }
        }
        #endregion
    }
}
