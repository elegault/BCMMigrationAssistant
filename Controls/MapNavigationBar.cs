using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BCM_Migration_Tool.Controls
{
    public partial class MapNavigationBar : UserControl
    {
        #region Fields
        public event EventHandler Button1Clicked;
        public event EventHandler Button2Clicked;
        public event EventHandler Button3Clicked;
        private MapNavigationBarStates navigationBarState;
        public enum MapNavigationBarStates
        {
            Accounts,
            BusinessContacts,
            Opportunities,
            AccountsInvalid,
            BusinessContactsInvalid,
            OpportunitiesInvalid
        }
        #endregion
        #region Constructors
        public MapNavigationBar()
        {
            InitializeComponent();
        }
#endregion
        #region Methods
        protected virtual void OnButton1Clicked(EventArgs e)
        {
            Button1Clicked?.Invoke(this, e);
        }

        protected virtual void OnButton2Clicked(EventArgs e)
        {
            Button2Clicked?.Invoke(this, e);
        }

        protected virtual void OnButton3Clicked(EventArgs e)
        {
            Button3Clicked?.Invoke(this, e);
        }
        #endregion
        #region Properties
        [
            Category("BCM"),
            Description("Sets the active Nav Bar button/state")
        ]
        public MapNavigationBarStates NavigationBarState
        {
            get { return navigationBarState; }
            set
            {
                System.Drawing.Color buttonColour = System.Drawing.Color.FromArgb(((int) (((byte) (46)))), ((int) (((byte) (117)))), ((int) (((byte) (182)))));

                switch (value)
                {
                    case MapNavigationBarStates.Accounts:
                        lblAccounts.ForeColor = buttonColour;
                        lblBusinessContacts.ForeColor = Color.Black;
                        lblOpportunities.ForeColor = Color.Black;
                        panel2.Visible = false;
                        panel1.Visible = true;
                        panel3.Visible = false;
                        break;
                    case MapNavigationBarStates.BusinessContacts:
                        lblAccounts.ForeColor = Color.Black;
                        lblBusinessContacts.ForeColor = buttonColour;
                        lblOpportunities.ForeColor = Color.Black;
                        panel2.Visible = true;
                        panel1.Visible = false;
                        panel3.Visible = false;
                        break;
                    case MapNavigationBarStates.Opportunities:
                        lblAccounts.ForeColor = Color.Black;
                        lblBusinessContacts.ForeColor = Color.Black;
                        lblOpportunities.ForeColor = buttonColour;
                        panel2.Visible = false;
                        panel1.Visible = false;
                        panel3.Visible = true;
                        break;
                    case MapNavigationBarStates.AccountsInvalid:
                        lblAccounts.ForeColor = Color.Red;
                        break;
                    case MapNavigationBarStates.BusinessContactsInvalid:
                        lblBusinessContacts.ForeColor = Color.Red;
                        break;
                    case MapNavigationBarStates.OpportunitiesInvalid:
                        lblOpportunities.ForeColor = Color.Red;
                        break;
                }
                Invalidate();
            }
        }
        #endregion
        #region Control Events
        private void lblAccounts_Click(object sender, EventArgs e)
        {
            NavigationBarState = MapNavigationBarStates.Accounts;
            OnButton1Clicked(EventArgs.Empty);
        }

        private void lblBusinessContacts_Click(object sender, EventArgs e)
        {
            NavigationBarState = MapNavigationBarStates.BusinessContacts;
            OnButton2Clicked(EventArgs.Empty);
        }

        private void lblOpportunities_Click(object sender, EventArgs e)
        {
            NavigationBarState = MapNavigationBarStates.Opportunities;
            OnButton3Clicked(EventArgs.Empty);
        }
        #endregion
    }
}
