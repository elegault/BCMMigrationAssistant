using System;
using System.Diagnostics;
using System.Windows.Forms;
using BCM_Migration_Tool.Objects;

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
        public event EventHandler<ConnectEventArgs> AuthenticationChanged;
        public event EventHandler LoginClicked;
        public event EventHandler TestDBConnectionClicked;
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

        private void lblAppRegHelp_Click(object sender, EventArgs e)
        {
            Process.Start("https://developer.microsoft.com/en-us/graph/docs/concepts/auth_register_app_v2");
        }

        private void lblConnect_Click(object sender, EventArgs e)
        {
            lblConnectionString.Visible = !lblConnectionString.Visible;
        }
        private void lblLogin_Click(object sender, EventArgs e)
        {
            OnLoginClicked();
        }

        private void lblTestConnection_Click(object sender, EventArgs e)
        {
            OnTestDBConnectionClicked();
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

        protected virtual void OnTestDBConnectionClicked()
        {
            TestDBConnectionClicked?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
