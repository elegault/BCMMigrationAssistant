using System;
using System.Windows.Forms;
using BCM_Migration_Tool.Objects;
using TracerX;

namespace BCM_Migration_Tool.Controls
{
    public partial class Map : UserControl
    {
        private static readonly Logger Log = Logger.GetLogger("Map");

        #region Constructors
        public Map()
        {
            InitializeComponent();
            accounts1.FieldMappingAdded += accounts1_FieldMappingAdded;
            businessContacts1.FieldMappingAdded += businessContacts1_FieldMappingAdded;
            opportunities1.FieldMappingAdded += opportunities1_FieldMappingAdded;
            mapNavigationBar1.Button1Clicked += mapNavigationBar1_AccountsClicked;
            mapNavigationBar1.Button2Clicked += mapNavigationBar1_ContactsClicked;
            mapNavigationBar1.Button3Clicked += mapNavigationBar1_OpportunitiesClicked;            
        }
        #endregion
        #region Properties
        public string ConnectionString { get; set; }
        #endregion
        #region Control Events
        private void accounts1_FieldMappingAdded(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void businessContacts1_FieldMappingAdded(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void mapNavigationBar1_AccountsClicked(object sender, EventArgs e)
        {
            tabControl1.SelectTab(tabPageAccounts);
        }

        private void mapNavigationBar1_ContactsClicked(object sender, EventArgs e)
        {
            tabControl1.SelectTab(tabPageBusinessContacts);
        }

        private void mapNavigationBar1_OpportunitiesClicked(object sender, EventArgs e)
        {
            tabControl1.SelectTab(tabPageOpportunities);
        }
        private void opportunities1_FieldMappingAdded(object sender, EventArgs e)
        {
            Invalidate();
        }
        #endregion
        #region Methods
        public void InitializeFieldMappings(bool disableCustomFields)
        {
            FieldMappings.InitializeFieldMappings(ConnectionString, disableCustomFields);

            accounts1.AddReadOnlyMappings();

            //Add two blank field mapping controls, each initialized with the default list of manual field mappings
            accounts1.AddNewMapping(FieldMappings.BCMAccountFields.BCMFields.UnMappedFields, FieldMappings.OCMAccountFields.OCMFields.UnMappedFields);
            accounts1.AddNewMapping(FieldMappings.BCMAccountFields.BCMFields.UnMappedFields, FieldMappings.OCMAccountFields.OCMFields.UnMappedFields);

            businessContacts1.AddReadOnlyMappings();
            //Add one field mapping control for custom field
            businessContacts1.AddNewMapping(FieldMappings.BCMBusinessContactFields.BCMFields.UnMappedFields, FieldMappings.OCMBusinessContactFields.OCMFields.UnMappedFields);

            opportunities1.AddReadOnlyMappings();
            //Add one field mapping control for custom field
            opportunities1.AddNewMapping(FieldMappings.BCMOpportunityFields.BCMFields.UnMappedFields, FieldMappings.OCMDealFields.OCMFields.UnMappedFields);
            
            Invalidate();
        }
        public void UpdateText(string text)
        {
            txtLog.AppendText(Environment.NewLine + text);
            Invalidate();
        }

        public bool ValidateMappings()
        {
            if (accounts1.ValidateFields() == false)
            {
                mapNavigationBar1.NavigationBarState = MapNavigationBar.MapNavigationBarStates.AccountsInvalid;
                return false;
            }
            else
            {
                //mapNavigationBar1.NavigationBarState = MapNavigationBar.MapNavigationBarStates.Accounts;
            }
            if (businessContacts1.ValidateFields() == false)
            {
                mapNavigationBar1.NavigationBarState = MapNavigationBar.MapNavigationBarStates.BusinessContactsInvalid;
                return false;
            }
            if (opportunities1.ValidateFields() == false)
            {
                mapNavigationBar1.NavigationBarState = MapNavigationBar.MapNavigationBarStates.OpportunitiesInvalid;
                return false;
            }

            try
            {
                if (FieldMappings.BCMAccountFields.BCMFields != null)
                {
                    if (FieldMappings.BCMAccountFields.BCMFields.MappedFields.Count !=
            FieldMappings.OCMAccountFields.OCMFields.MappedFields.Count)
                        return false;
                }

                if (FieldMappings.BCMBusinessContactFields.BCMFields != null)
                {
                    if (FieldMappings.BCMBusinessContactFields.BCMFields.MappedFields.Count !=
            FieldMappings.OCMBusinessContactFields.OCMFields.MappedFields.Count)
                        return false;
                }

                if (FieldMappings.BCMOpportunityFields.BCMFields != null)
                {
                    if (FieldMappings.BCMOpportunityFields.BCMFields.MappedFields.Count !=
            FieldMappings.OCMDealFields.OCMFields.MappedFields.Count)
                        return false;
                }                
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            
            return true;        
        }
        #endregion
    }
}
