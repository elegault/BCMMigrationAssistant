using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using BCM_Migration_Tool.Objects;
using Microsoft.Data.ConnectionUI;
using TracerX;

namespace BCM_Migration_Tool.Controls
{
    public partial class Connect : UserControl
    {
        #region Constructors
        public Connect()
        {
            InitializeComponent();
            cboAuthentication.SelectedIndex = 0;
#if DEBUG
            lblLogin.Enabled = true;
#endif
        }
        #endregion
        #region Fields

        private bool _connected;
        private SqlFileConnectionProperties cp;
        private SqlConnectionUIControl uic;
        private DataConnectionDialog dcd;
        private static Logger Log = Logger.GetLogger("Connect");
        public event EventHandler<ConnectEventArgs> AuthenticationChanged;
        public event EventHandler LoginClicked;
        public event EventHandler ConnectToDBClicked;
        #endregion
        #region Properties        
        public bool Connected
        {
            get { return _connected; }
            set
            {
                _connected = value;
                if (value)
                {
                    lblLogin.Text = "Connected!";
                }
                else
                {
                    lblLogin.Text = "Login to Office 365";
                }
                Invalidate();
            }
        }
        public string ConnectionString { get; set; }
        public string ManualConnectionString
        {
            get
            {
                return txtConnectionString.Text;
            }
            set
            {
                txtConnectionString.Text = value;
            }
        }
        public bool UseManualConnectionString
        {
            get
            {
                return chkUseConnectionString.Checked;
            }
            set
            {
                chkUseConnectionString.Checked = value;
            }
        }
        #endregion
        #region Control Events
        private void cboAuthentication_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConnectEventArgs.AuthenticationTypes authType = ConnectEventArgs.AuthenticationTypes.Windows;
            //Windows Authentication; SQL Server Authentication
            switch (cboAuthentication.Text)
            {
                case "Windows Authentication":
                    authType = ConnectEventArgs.AuthenticationTypes.Windows;
                    txtUserName.Enabled = false;
                    txtPassword.Enabled = false;
                    panelDBLogin.Visible = false;
                    break;
                case "SQL Server Authentication":
                    authType = ConnectEventArgs.AuthenticationTypes.SQL;
                    txtUserName.Enabled = true;
                    txtPassword.Enabled = true;
                    panelDBLogin.Visible = true;
                    break;
            }
            ConnectEventArgs args = new ConnectEventArgs(authType, txtUserName.Text, txtPassword.Text, txtDatabase.Text, txtServer.Text);
            OnAuthenticationChanged(args);
        }

        private void chkUseConnectionString_CheckedChanged(object sender, EventArgs e)
        {
            //UseManualConnectionString = chkUseConnectionString.Checked;
        }

        private void lblAppRegHelp_Click(object sender, EventArgs e)
        {
            Process.Start("https://developer.microsoft.com/en-us/graph/docs/concepts/auth_register_app_v2");
        }

        private void lblAdvancedConfiguration_Click(object sender, EventArgs e)
        {
            try
            {
                dcd = new DataConnectionDialog();

                chkUseConnectionString.Visible = true;
                txtConnectionString.Visible = true;
                dcd.DataSources.Add(DataSource.SqlDataSource);
                if (Microsoft.Data.ConnectionUI.DataConnectionDialog.Show(dcd) == System.Windows.Forms.DialogResult.OK)
                {
                    //ConnectionString = dcd.ConnectionString;
                    chkUseConnectionString.Checked = true;
                    txtConnectionString.Text = ConnectionString;
                    Log.InfoFormat("Advanced connection string: {0}", ConnectionString);
                }
            }
            catch (System.Exception ex)
            {
                Log.Error(ex);
            }
            finally
            { }
        }

        private void lblConnect_Click(object sender, EventArgs e)
        {
            //lblConnectionString.Visible = !lblConnectionString.Visible;
            //lblBuildConnectionString.Visible = !lblBuildConnectionString.Visible;
        }
        private void lblLogin_Click(object sender, EventArgs e)
        {
            OnLoginClicked();
        }

        private void lblConnectToDB_Click(object sender, EventArgs e)
        {
            OnConnectToDBClicked();
        }
        #endregion
        #region Methods
        public string GetConnectionString()
        {
            string database = txtDatabase.Text;//cbDatabases.Text;
            string server = txtServer.Text;//cbServers.Text;
            bool windowsAuth = (cboAuthentication.SelectedIndex == 0);

            if (windowsAuth)
            {
                //Windows Auth
                return string.Format("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog={0};Data Source={1}",
                    database, server);
            }
            else
            {
                //SQL Auth
                return string.Format("Provider=SQLNCLI.1;Password={0};Persist Security Info=True;User ID={1};Initial Catalog={2};Data Source={3}",
                    txtPassword.Text, txtUserName.Text, database, server);
            }
        }
        protected virtual void OnAuthenticationChanged(ConnectEventArgs e)
        {
            //ConnectEventArgs.AuthenticationTypes authType = ConnectEventArgs.AuthenticationTypes.Windows;
            ////Windows Authentication; SQL Server Authentication
            //switch (cboAuthentication.Text)
            //{
            //    case "Windows Authentication":            
            //        authType = ConnectEventArgs.AuthenticationTypes.Windows;        
            //        break;
            //    case "SQL Server Authentication":
            //        authType = ConnectEventArgs.AuthenticationTypes.SQL;
            //        break;
            //}
            //ConnectEventArgs args = new ConnectEventArgs(authType, txtUserName.Text, txtPassword.Text, txtDatabase.Text, txtServer.Text);
            AuthenticationChanged?.Invoke(this, e);
        }
        protected virtual void OnLoginClicked()
        {
            LoginClicked?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnConnectToDBClicked()
        {
            ConnectToDBClicked?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
