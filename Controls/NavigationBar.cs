using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using BCM_Migration_Tool.Properties;

namespace BCM_Migration_Tool.Controls
{
    public partial class NavigationBar : UserControl
    {
        #region Constructors
        public NavigationBar()
        {
            InitializeComponent();
        }
        #endregion
        #region Fields
        public event EventHandler ConfigClicked;
        public event EventHandler ConnectClicked;
        public event EventHandler MapClicked;
        public event EventHandler MigrateClicked;
        private bool configureEnabled;
        private bool mapEnabled;
        private bool migrateEnabled;
        private NavigationBarStates navigationBarState;
        public enum NavigationBarStates
        {
            Configuration,
            Connect,
            Map,
            Migrate
        }
        #endregion
        #region Methods
        protected virtual void OnConfigClicked(EventArgs e)
        {
            ConfigClicked?.Invoke(this, e);
        }
        protected virtual void OnConnectClicked(EventArgs e)
        {
            ConnectClicked?.Invoke(this, e);
        }

        protected virtual void OnMapClicked(EventArgs e)
        {
            MapClicked?.Invoke(this, e);
        }

        protected virtual void OnMigrateClicked(EventArgs e)
        {
            MigrateClicked?.Invoke(this, e);
        }
        #endregion
        #region Properties
        [
            Category("BCM"),
            Description("Enables/disables the control")
        ]
        public bool ConfigureEnabled
        {
            get
            {
                return configureEnabled;
            }
            set
            {
                configureEnabled = value;
                pictureBoxGear.Enabled = value;
                Invalidate();
            }
        }
        [
            Category("BCM"),
            Description("Enables/disables the control")
        ]
        public bool MapEnabled
        {
            get
            {
                return mapEnabled;
            }
            set
            {
                mapEnabled = value;
                lblMap.Enabled = value;
                Invalidate();
            }
        }
        [
            Category("BCM"),
            Description("Enables/disables the control")
        ]
        public bool MigrateEnabled
        {
            get
            {
                return migrateEnabled;
            }
            set
            {
                migrateEnabled = value;
                lblMigrate.Enabled = value;
                Invalidate();
            }
        }
        
        [
            Category("BCM"),
            Description("Sets the active Nav Bar button/state")
        ]
        public NavigationBarStates NavigationBarState
        {
            get { return navigationBarState; }
            set
            {
                System.Drawing.Color buttonColour = System.Drawing.Color.FromArgb(((int) (((byte) (46)))), ((int) (((byte) (117)))), ((int) (((byte) (182)))));
                
                switch (value)
                {
                    case NavigationBarStates.Connect:
                        lblConnect.ForeColor = buttonColour;
                        lblMap.ForeColor = Color.Black;
                        lblMigrate.ForeColor = Color.Black;
                        panelMap.Visible = false;
                        panelConnect.Visible = true;
                        panelMigrate.Visible = false;
                        break;
                    case NavigationBarStates.Map:
                        lblMap.ForeColor = Color.Black;
                        lblMap.ForeColor = buttonColour;
                        lblMigrate.ForeColor = Color.Black;
                        panelMap.Visible = true;
                        panelConnect.Visible = false;
                        panelMigrate.Visible = false;
                        break;
                    case NavigationBarStates.Migrate:
                        lblConnect.ForeColor = Color.Black;
                        lblMap.ForeColor = Color.Black;
                        lblMigrate.ForeColor = buttonColour;
                        panelMap.Visible = false;
                        panelConnect.Visible = false;
                        panelMigrate.Visible = true;
                        break;
                    case NavigationBarStates.Configuration:
                        lblMap.ForeColor = Color.Black;
                        lblMap.ForeColor = Color.Black;
                        lblMigrate.ForeColor = Color.Black;
                        panelMap.Visible = false;
                        panelConnect.Visible = false;
                        panelMigrate.Visible = false;
                        break;

                }
                Invalidate();
            }
        }
        #endregion
        #region Control Events
        internal void lblConnect_Click(object sender, System.EventArgs e)
        {
            NavigationBarState = NavigationBarStates.Connect;
            pictureBoxGear.Image = Resources.Settings_Gray;
            OnConnectClicked(EventArgs.Empty);
        }

        internal void lblMap_Click(object sender, System.EventArgs e)
        {
            NavigationBarState = NavigationBarStates.Map;
            pictureBoxGear.Image = Resources.Settings_Gray;
            OnMapClicked(EventArgs.Empty);
        }

        internal void lblMigrate_Click(object sender, System.EventArgs e)
        {
            NavigationBarState = NavigationBarStates.Migrate;
            pictureBoxGear.Image = Resources.Settings_Gray;
            OnMigrateClicked(EventArgs.Empty);
        }


        private void pictureBoxGear_Click(object sender, EventArgs e)
        {
            NavigationBarState = NavigationBarStates.Configuration;
            pictureBoxGear.Image = Resources.Settings_Blue;
            OnConfigClicked(EventArgs.Empty);

        }
        #endregion
    }
}
