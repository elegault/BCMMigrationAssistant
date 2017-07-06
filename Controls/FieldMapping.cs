using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BCM_Migration_Tool.Objects;
using BCM_Migration_Tool.Properties;
using TracerX;

namespace BCM_Migration_Tool.Controls
{
    public partial class FieldMapping : UserControl
    {
        #region Fields
        private FieldMappings.OCMField _DestinationField;
        private FieldMappings.BCMField _SourceField;
        private bool _Invalid;
        private bool isReadOnlyButOptional;
        private static readonly Logger Log = Logger.GetLogger("FieldMapping");
        #endregion
        #region Constructors
        public FieldMapping()
        {
            InitializeComponent();
            //LastSourceFieldIndex = -1;
            //LastDestinationFieldIndex = -1;
            cboSourceField.ValueMember = "Name";
            cboSourceField.DisplayMember = "Label";
            cboDestinationField.ValueMember = "Name";
            cboDestinationField.DisplayMember = "Label";
        }
        #endregion

        #region Control Events
        private void cboDestinationField_SelectedIndexChanged(object sender, EventArgs e)
        {
            FieldMappings.OCMField newField = null;
            FieldMappingEventArgs fieldMappingEventArgs;

            try
            {
                //Get the selected field
                if (cboDestinationField.SelectedIndex != -1)
                {
                    //newField = DestinationFields.Find(item => item.Name == cboDestinationField.Text);
                    //newField = DestinationFields.Find(item => item.Name == cboDestinationField.ValueMember); 
                    newField = (FieldMappings.OCMField)cboDestinationField.SelectedItem; //TESTED Getting field by SelectedItem
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return;
            }

            try
            {
                if (newField == null)
                    goto continueHere; //Null selection - an automatic mapping reset

                //Update list of mapped fields with the current field and remove it from the list of mapped fields
                //e.g. Business Fax is selected; add it to mapped list, remove it from unmapped list

                switch (newField.OCMDataSetType)
                {
                    case FieldMappings.OCMDataSetTypes.Accounts:
                        FieldMappings.OCMAccountFields.OCMFields.MappedFields.Add(newField);
                        FieldMappings.OCMAccountFields.OCMFields.UnMappedFields.Remove(newField);
                        break;
                    case FieldMappings.OCMDataSetTypes.BusinessContacts:
                        FieldMappings.OCMBusinessContactFields.OCMFields.MappedFields.Add(newField);
                        FieldMappings.OCMBusinessContactFields.OCMFields.UnMappedFields.Remove(newField);
                        break;
                    case FieldMappings.OCMDataSetTypes.Deals:
                        FieldMappings.OCMDealFields.OCMFields.MappedFields.Add(newField);
                        FieldMappings.OCMDealFields.OCMFields.UnMappedFields.Remove(newField);
                        break;
                }

                //Fire OnSourceFieldChange event so parent control (eg. Accounts) can handle it and force an update on all other FieldMapping controls it contains. The currently selected field should be removed from the source combo in all those controls
                fieldMappingEventArgs = new FieldMappingEventArgs(FieldMappingEventArgs.ChangeTypes.Added, newField, SourceField);
                OnDestinationFieldChange(fieldMappingEventArgs);

            continueHere:

                if (DestinationField != null)
                {
                    //A field was previously selected
                    //The current selection is changed to another field (e.g. Notes). The previous field (Business Fax) cached in DestinationField should be removed from mapped fields and added to unmapped fields.  Current selection is handled above

                    switch (newField.OCMDataSetType)
                    {
                        case FieldMappings.OCMDataSetTypes.Accounts:
                            FieldMappings.OCMAccountFields.OCMFields.MappedFields.Remove(DestinationField);
                            FieldMappings.OCMAccountFields.OCMFields.UnMappedFields.Add(DestinationField);
                            break;
                        case FieldMappings.OCMDataSetTypes.BusinessContacts:                            
                            FieldMappings.OCMBusinessContactFields.OCMFields.MappedFields.Remove(DestinationField);
                            FieldMappings.OCMBusinessContactFields.OCMFields.UnMappedFields.Add(DestinationField);
                            break;
                        case FieldMappings.OCMDataSetTypes.Deals:                            
                            FieldMappings.OCMDealFields.OCMFields.MappedFields.Remove(DestinationField);
                            FieldMappings.OCMDealFields.OCMFields.UnMappedFields.Add(DestinationField);
                            break;
                    }

                    fieldMappingEventArgs = new FieldMappingEventArgs(FieldMappingEventArgs.ChangeTypes.Removed, DestinationField);
                    OnDestinationFieldChange(fieldMappingEventArgs);

                    //If the previously selected field is an automatic field, we need to also nullify the selection in the other combo. Just set index to -1
                    if (DestinationField?.FieldMappingType == FieldMappings.FieldMappingTypes.OptionalAutomatic && newField != null)
                        cboSourceField.SelectedIndex = -1;
                }

                //NOW update/cache the selected field as the current DestinationField in the control
                DestinationField = newField;
                //LastDestinationFieldIndex = cboDestinationField.SelectedIndex;                
                if (DestinationField == null)
                {
                    Log.Warn("DestinationField is null");
                    return;
                }

                if (DestinationField.FieldMappingType == FieldMappings.FieldMappingTypes.OptionalAutomatic && DestinationField.OCMDataSetType == FieldMappings.OCMDataSetTypes.Accounts)
                {
                    //Special case for Account/Company Notes field only
                    IsReadOnlyButOptional = true;
                    //Find Notes field in SourceFields
                    int index = SourceFields.FindIndex(item => item.Name == FieldMappings.BCMAccountFields.Notes.Name);
                    //TESTED Setting to field
                    if (cboSourceField.SelectedItem != FieldMappings.BCMAccountFields.Notes)
                    {
                        //See if it is already mapped - this combo may have just been automapped from the other control so we don't need to automap the other control in return!
                        //cboSourceField.SelectedIndex = index;
                        //cboSourceField.Text = FieldMappings.BCMAccountFields.Notes.Name;                        
                        cboSourceField.SelectedValue = FieldMappings.BCMAccountFields.Notes;

                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.Error(ex);
            }
        }
        private void cboSourceField_SelectedIndexChanged(object sender, EventArgs e)
        {
            FieldMappings.BCMField newField = null;
            FieldMappingEventArgs fieldMappingEventArgs;

            try
            {
                //Get the selected field
                if (cboSourceField.SelectedIndex != -1)
                {
                    //TESTED Find by object instead of index
                    newField = (FieldMappings.BCMField) cboSourceField.SelectedItem;
                        // SourceFields[cboSourceField.SelectedIndex];                    
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return;
            }

            try
            {
                if (newField == null)
                    goto continueHere; //Null selection - an automatic mapping reset

                //Update list of mapped fields with the current field and remove it from the list of mapped fields
                //e.g. Business Fax is selected; add it to mapped list, remove it from unmapped list
                switch (newField.BCMDataSetType)
                {
                    case FieldMappings.BCMDataSetTypes.Accounts:
                        FieldMappings.BCMAccountFields.BCMFields.MappedFields.Add(newField);
                        FieldMappings.BCMAccountFields.BCMFields.UnMappedFields.Remove(newField);
                        break;
                    case FieldMappings.BCMDataSetTypes.BusinessContacts:
                        FieldMappings.BCMBusinessContactFields.BCMFields.MappedFields.Add(newField);
                        FieldMappings.BCMBusinessContactFields.BCMFields.UnMappedFields.Remove(newField);
                        break;
                    case FieldMappings.BCMDataSetTypes.Opportunities:
                        FieldMappings.BCMOpportunityFields.BCMFields.MappedFields.Add(newField);
                        FieldMappings.BCMOpportunityFields.BCMFields.UnMappedFields.Remove(newField);
                        break;
                }
                
                //Fire OnSourceFieldChange event so parent control (eg. Accounts) can handle it and force an update on all other FieldMapping controls it contains. The currently selected field should be removed from the source combo in all those controls
                fieldMappingEventArgs = new FieldMappingEventArgs(FieldMappingEventArgs.ChangeTypes.Added, DestinationField, newField);
                OnSourceFieldChange(fieldMappingEventArgs);

            continueHere:

                if (SourceField != null)
                {
                    //The current selection is changed to another field (e.g. Notes). The previous field (Business Fax) should be removed from mapped fields and added to unmapped fields.  Current selection is handled above

                    switch (newField.BCMDataSetType)
                    {
                        case FieldMappings.BCMDataSetTypes.Accounts:
                            FieldMappings.BCMAccountFields.BCMFields.MappedFields.Remove(SourceField);
                            FieldMappings.BCMAccountFields.BCMFields.UnMappedFields.Add(SourceField);
                            break;
                        case FieldMappings.BCMDataSetTypes.BusinessContacts:
                            FieldMappings.BCMBusinessContactFields.BCMFields.MappedFields.Remove(SourceField);
                            FieldMappings.BCMBusinessContactFields.BCMFields.UnMappedFields.Add(SourceField);
                            break;
                        case FieldMappings.BCMDataSetTypes.Opportunities:
                            FieldMappings.BCMOpportunityFields.BCMFields.MappedFields.Remove(SourceField);
                            FieldMappings.BCMOpportunityFields.BCMFields.UnMappedFields.Add(SourceField);
                            break;
                    }

                    fieldMappingEventArgs = new FieldMappingEventArgs(FieldMappingEventArgs.ChangeTypes.Removed, SourceField);
                    OnSourceFieldChange(fieldMappingEventArgs);

                    //NOTE If the previously selected field is an automatic field, we need to also nullify the selection in the other combo. Just set index to -1??. Also do NOT do if newField is null, otherwise we will be resetting the field that was just changed and triggered the change to THIS combo
                    if (SourceField?.FieldMappingType == FieldMappings.FieldMappingTypes.OptionalAutomatic && newField != null)
                        cboDestinationField.SelectedIndex = -1;
                }

                //NOW update/cache the selected field as the current SourceField in the control
                SourceField = newField;

                if (SourceField == null)
                {
                    //Happens when deleting a mappingRemoved
                    //Log.Verbose("SourceField is null");
                    return;
                }

                if (SourceField.FieldMappingType == FieldMappings.FieldMappingTypes.OptionalAutomatic && SourceField.BCMDataSetType == FieldMappings.BCMDataSetTypes.Accounts)
                {                    
                    //Special case for Account/Company Notes field only. Now only for Company notes because Account notes are now auto-mapped
                    IsReadOnlyButOptional = true;
                    //Find Notes field in SourceFields
                    int index = DestinationFields.FindIndex(item => item.Name == FieldMappings.OCMAccountFields.Notes.Name);
                    //TESTED Setting to field
                    if (cboDestinationField.ValueMember != FieldMappings.OCMAccountFields.Notes.Name)
                    {
                        //if (cboDestinationField.SelectedIndex != index) //See if it is already mapped - this combo may have just been automapped from the other control so we don't need to automap the other control in return!
                        //cboDestinationField.SelectedIndex = index;
                        //cboDestinationField.Text = FieldMappings.OCMAccountFields.Notes.Name;                        
                        cboDestinationField.SelectedValue = FieldMappings.OCMAccountFields.Notes; //TESTED Setting to field
                    }

                }
            }
            catch (System.Exception ex)
            {
                Log.Error(ex);
            }
        }
        private void pictureBoxDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (DestinationField != null)
                {
                    switch (DestinationField.OCMDataSetType)
                    {
                        case FieldMappings.OCMDataSetTypes.Accounts:
                            FieldMappings.OCMAccountFields.OCMFields.MappedFields.Remove(DestinationField);
                            FieldMappings.OCMAccountFields.OCMFields.UnMappedFields.Add(DestinationField);
                            break;
                        case FieldMappings.OCMDataSetTypes.BusinessContacts:
                            FieldMappings.OCMBusinessContactFields.OCMFields.MappedFields.Remove(DestinationField);
                            FieldMappings.OCMBusinessContactFields.OCMFields.UnMappedFields.Add(DestinationField);
                            break;
                        case FieldMappings.OCMDataSetTypes.Deals:
                            FieldMappings.OCMDealFields.OCMFields.MappedFields.Remove(DestinationField);
                            FieldMappings.OCMDealFields.OCMFields.UnMappedFields.Add(DestinationField);
                            break;
                    }
                }
                if (SourceField != null)
                {
                    switch (SourceField.BCMDataSetType)
                    {
                        case FieldMappings.BCMDataSetTypes.Accounts:
                            FieldMappings.BCMAccountFields.BCMFields.MappedFields.Remove(SourceField);
                            FieldMappings.BCMAccountFields.BCMFields.UnMappedFields.Add(SourceField);
                            break;
                        case FieldMappings.BCMDataSetTypes.BusinessContacts:
                            FieldMappings.BCMBusinessContactFields.BCMFields.MappedFields.Remove(SourceField);
                            FieldMappings.BCMBusinessContactFields.BCMFields.UnMappedFields.Add(SourceField);
                            break;
                        case FieldMappings.BCMDataSetTypes.Opportunities:
                            FieldMappings.BCMOpportunityFields.BCMFields.MappedFields.Remove(SourceField);
                            FieldMappings.BCMOpportunityFields.BCMFields.UnMappedFields.Add(SourceField);
                            break;
                    }
                }

                FieldMappingEventArgs fieldMappingEventArgs;
                fieldMappingEventArgs = new FieldMappingEventArgs(FieldMappingEventArgs.ChangeTypes.MappingDeleted, DestinationField, SourceField);
                DestinationField = null;
                SourceField = null;
                OnMappingRemoved(fieldMappingEventArgs);
                cboDestinationField.SelectedIndex = -1;
                cboSourceField.SelectedIndex = -1;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Error in BCM_Migration_Tool.Controls.FieldMapping.pictureBoxDelete_Click()", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.Error(ex);
            }
        }
        #endregion
        #region Object Events
        internal event EventHandler<FieldMappingEventArgs> DestinationFieldChanged;
        internal event EventHandler<FieldMappingEventArgs> MappingRemoved;
        internal event EventHandler<FieldMappingEventArgs> SourceFieldChanged;

        protected virtual void OnDestinationFieldChange(FieldMappingEventArgs e)
        {
            //NOTE For Source or Destination fields: if the combo that was just changed has already been filtered to only show fields with matching data types, then we don't want to filter the other combo in return - it should be left as is. So how to detect: if the other combo has a non-null *Field?

            try
            {
                Log.DebugFormat("SourceFields: {0}; DestinationFields: {1}. Current SourceField: {2}; Current DestinationField: {3}", SourceFields.Count, DestinationFields.Count, SourceField != null ? String.Format("{0} ({1}); mapping: {2}", SourceField.Name, SourceField.DataTypeLabel, SourceField.OCMFieldMapping != null ? String.Format("{0} ({1})", SourceField.OCMFieldMapping.Name, SourceField.OCMFieldMapping.DataTypeLabel) : "n/a/") : "n/a", DestinationField != null ? String.Format("{0} ({1})", DestinationField.Name, DestinationField.DataTypeLabel) : "n/a");
                Log.DebugFormat("e.BCMField: {0}; e.OCMField: {1}", e.BCMField != null, e.OCMField != null);

                Invalid = SourceField == null;

                if (SourceField != null && e.BCMField != null)
                {
                    //TESTED Set BCMField.OCMFieldMapping to the curent field
                    e.BCMField.OCMFieldMapping = e.OCMField;
                    if (e.OCMField != null)
                    {
                        Log.DebugFormat("BCM field '{0}' (Type: {1}) mapped to OCM field '{2}' (Type: {3})", e.BCMField.Name, e.BCMField.FieldType, e.OCMField.Name, e.OCMField.FieldType);
                    }
                    else
                    {
                        Log.DebugFormat("BCM field '{0}' (Type: {1}) mapped to null OCM field)", e.BCMField.Name, e.BCMField.FieldType);
                    }

                    return; //Don't update other control as it was set by the user //NOTE But should we add a BCMFieldMapping property to OCMField and set it here? Or is only BCMField.OCMFieldMapping needed?
                }

                if (e.BCMField == null)
                {
                    //Because source field is blank
                    //Log.Warn("Unexpected: BCM field is null");
                }

                DestinationFieldChanged?.Invoke(this, e);

                //Filter the list of destination fields (via SetDestinationFields) based on the data type and data set type of the selected field
                switch (e.OCMField.FieldType)
                {
                    case FieldMappings.OCMField.OCMFieldTypes.Text:
                        //"Text"
                        switch (e.OCMField.OCMDataSetType)
                        {
                            case FieldMappings.OCMDataSetTypes.Accounts:
                                SetSourceFields(FieldMappings.BCMAccountFields.UnMappedTextFields);
                                break;
                            case FieldMappings.OCMDataSetTypes.BusinessContacts:
                                SetSourceFields(FieldMappings.BCMBusinessContactFields.UnMappedTextFields);
                                break;
                            case FieldMappings.OCMDataSetTypes.Deals:
                                SetSourceFields(FieldMappings.BCMOpportunityFields.UnMappedTextFields);
                                break;
                        }
                        break;
                    case FieldMappings.OCMField.OCMFieldTypes.NumberOrText:
                        //"Number/Text"
                        switch (e.OCMField.OCMDataSetType)
                        {
                            case FieldMappings.OCMDataSetTypes.Accounts:
                                SetSourceFields(FieldMappings.BCMAccountFields.UnMappedNumberOrTextFields);
                                break;
                            case FieldMappings.OCMDataSetTypes.BusinessContacts:
                                SetSourceFields(FieldMappings.BCMBusinessContactFields.UnMappedNumberOrTextFields);
                                break;
                            case FieldMappings.OCMDataSetTypes.Deals:
                                SetSourceFields(FieldMappings.BCMOpportunityFields.UnMappedNumberOrTextFields);
                                break;
                        }

                        break;
                    case FieldMappings.OCMField.OCMFieldTypes.Date:
                        //"Date"
                        switch (e.OCMField.OCMDataSetType)
                        {
                            case FieldMappings.OCMDataSetTypes.Accounts:
                                SetSourceFields(FieldMappings.BCMAccountFields.UnMappedDateFields);
                                break;
                            case FieldMappings.OCMDataSetTypes.BusinessContacts:
                                SetSourceFields(FieldMappings.BCMBusinessContactFields.UnMappedDateFields);
                                break;
                            case FieldMappings.OCMDataSetTypes.Deals:
                                SetSourceFields(FieldMappings.BCMOpportunityFields.UnMappedDateFields);
                                break;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
        protected virtual void OnMappingRemoved(FieldMappingEventArgs e)
        {
            MappingRemoved?.Invoke(this, e);
        }
        protected virtual void OnSourceFieldChange(FieldMappingEventArgs e)
        {
            //REVIEW For Source or Destination fields: if the combo that was just changed has already been filtered to only show fields with matching data types, then we don't want to filter the other combo in return - it should be left as is. So how to detect: if the other combo has a non-null *Field?

            try
            {               
                Log.DebugFormat("SourceFields: {0}; DestinationFields: {1}. Current SourceField: {2}; Current DestinationField: {3}", SourceFields.Count, DestinationFields.Count, SourceField != null ? String.Format("{0} ({1}); mapping: {2}", SourceField.Name, SourceField.DataTypeLabel, SourceField.OCMFieldMapping != null ? String.Format("{0} ({1})", SourceField.OCMFieldMapping.Name, SourceField.OCMFieldMapping.DataTypeLabel) : "n/a/") : "n/a", DestinationField != null ? String.Format("{0} ({1})", DestinationField.Name, DestinationField.DataTypeLabel) : "n/a");
                Log.DebugFormat("e.BCMField: {0}; e.OCMField: {1}", e.BCMField != null, e.OCMField != null);

                Invalid = DestinationField == null;

                if (e.BCMField != null)
                {
                    //TESTED Set mapping - could be the destination field was set first while the source is blank
                    e.BCMField.OCMFieldMapping = DestinationField;
                    if (!Invalid)
                    {
                        Log.DebugFormat("BCM field '{0}' (Type: {1}) mapped to OCM field '{2}' (Type: {3})",
                            e.BCMField.Name, e.BCMField.FieldType, DestinationField.Name, DestinationField.FieldType);
                    }
                    else
                    {
                        Log.DebugFormat("BCM field '{0}' (Type: {1}) mapped to null OCM field)", e.BCMField.Name, e.BCMField.FieldType);
                    }

                    //REVIEW Leave now? OnDestinationFieldChange does...
                }

                if (DestinationField != null)
                {
                    Log.Warn("Leaving");
                    return; //Don't update other control as it was set by the user
                }

                SourceFieldChanged?.Invoke(this, e);

                FieldMappings.BCMField.BCMFieldTypes bcmFieldType = e.BCMField.FieldType;

                //Filter the list of destination fields (via SetDestinationFields) based on the data type and data set type of the selected field
                switch (bcmFieldType)
                {
                    case FieldMappings.BCMField.BCMFieldTypes.Text:
                    case FieldMappings.BCMField.BCMFieldTypes.YesNo:
                    case FieldMappings.BCMField.BCMFieldTypes.DropDownList:
                    case FieldMappings.BCMField.BCMFieldTypes.URL:
                    case FieldMappings.BCMField.BCMFieldTypes.Relationship:
                        //"Text"
                        switch (e.BCMField.BCMDataSetType)
                        {
                            case FieldMappings.BCMDataSetTypes.Accounts:
                                SetDestinationFields(FieldMappings.OCMAccountFields.UnMappedTextFields);
                                break;
                            case FieldMappings.BCMDataSetTypes.BusinessContacts:
                                SetDestinationFields(FieldMappings.OCMBusinessContactFields.UnMappedTextFields);
                                break;
                            case FieldMappings.BCMDataSetTypes.Opportunities:
                                SetDestinationFields(FieldMappings.OCMDealFields.UnMappedTextFields);
                                break;
                        }
                        break;
                    case FieldMappings.BCMField.BCMFieldTypes.Number:
                    case FieldMappings.BCMField.BCMFieldTypes.Percent:
                    case FieldMappings.BCMField.BCMFieldTypes.Currency:
                    case FieldMappings.BCMField.BCMFieldTypes.Integer:
                        //"Number/Text"
                        switch (e.BCMField.BCMDataSetType)
                        {
                            case FieldMappings.BCMDataSetTypes.Accounts:
                                SetDestinationFields(FieldMappings.OCMAccountFields.UnMappedNumberOrTextFields);
                                break;
                            case FieldMappings.BCMDataSetTypes.BusinessContacts:
                                SetDestinationFields(FieldMappings.OCMBusinessContactFields.UnMappedNumberOrTextFields);
                                break;
                            case FieldMappings.BCMDataSetTypes.Opportunities:
                                SetDestinationFields(FieldMappings.OCMDealFields.UnMappedNumberOrTextFields);
                                break;
                        }
                        break;
                    case FieldMappings.BCMField.BCMFieldTypes.DateTime:
                        //"Date"
                        switch (e.BCMField.BCMDataSetType)
                        {
                            case FieldMappings.BCMDataSetTypes.Accounts:
                                SetDestinationFields(FieldMappings.OCMAccountFields.UnMappedDateFields);
                                break;
                            case FieldMappings.BCMDataSetTypes.BusinessContacts:
                                SetDestinationFields(FieldMappings.OCMBusinessContactFields.UnMappedDateFields);
                                break;
                            case FieldMappings.BCMDataSetTypes.Opportunities:
                                SetDestinationFields(FieldMappings.OCMDealFields.UnMappedDateFields);
                                break;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
        #endregion
        #region Properties
        public FieldMappings.OCMField DestinationField
        {
            get
            {
                return _DestinationField;
            }
            private set
            {
                _DestinationField = value;
                if (SourceField != null && value != null)
                    Invalid = false; //Mapping is good
            }
        }
        public List<FieldMappings.OCMField> DestinationFields { get; private set; }
        public bool Invalid
        {
            get
            {
                return _Invalid;
            }
            set
            {
                if (value)
                {
                    //This doesn't paint the selected text, only text in the list!
                    //cboSourceField.ForeColor = Color.Red;
                    //cboDestinationField.ForeColor = Color.Red;
                    //pictureBox1.BackColor = Color.Red;        
                    //BackColor = Color.Red;
                    pictureBox1.Image = Resources.rightbuttonred;
                }
                else
                {
                    //cboSourceField.ForeColor = Color.Black;
                    //cboDestinationField.ForeColor = Color.Black;
                    //pictureBox1.BackColor = Color.White;
                    //BackColor = Color.White;
                    pictureBox1.Image = Resources.rightbutton;
                }
                Invalidate();
                _Invalid = value;
            }
        }
        public bool IsReadOnlyButOptional
        {
            get
            {
                return isReadOnlyButOptional;
            }
            set
            {
                isReadOnlyButOptional = value;
                //cboSourceField.Enabled = !value;
                //cboDestinationField.Enabled = !value;
                chkInclude.Visible = value;
                Invalidate();
            }
        }
        public FieldMappings.BCMField SourceField
        {
            get
            {
                return _SourceField;
            }
            private set
            {
                _SourceField = value;
                if (DestinationField != null && value != null)
                    Invalid = false; //Mapping is good
            }
        }
        public List<FieldMappings.BCMField> SourceFields { get; private set; }
        #endregion
        #region Methods
        public void AddBCMField(FieldMappings.BCMField field)
        {
            //TESTED Adding fields as objects instead of text
            SourceFields.Add(field);            
            cboSourceField.Items.Add(field);
        }

        public void AddOCMField(FieldMappings.OCMField field)
        {
            DestinationFields.Add(field);
            cboDestinationField.Items.Add(field);
        }
        public void RemoveBCMField(FieldMappings.BCMField field)
        {
            SourceFields.Remove(field);
            cboSourceField.Items.Remove(field);
        }
        public void RemoveOCMField(FieldMappings.OCMField field)
        {
            DestinationFields.Remove(field);
            cboDestinationField.Items.Remove(field);
        }
        public void SetDestinationFields(List<FieldMappings.OCMField> ocmFields)
        {
            //Update the combo with the passed field
            DestinationFields = new List<FieldMappings.OCMField>();
            DestinationFields.AddRange(ocmFields);
            cboDestinationField.Items.Clear();

            Log.VerboseFormat("Adding {0} destination fields:", ocmFields.Count);
            foreach (var field in DestinationFields)
            {
                Log.VerboseFormat("-{0} ({1})", field.Name, field.DataTypeLabel);
                cboDestinationField.Items.Add(field);
            }
        }
        public void SetSourceFields(List<FieldMappings.BCMField> bcmFields)
        {
            SourceFields = new List<FieldMappings.BCMField>();
            SourceFields.AddRange(bcmFields);
            cboSourceField.Items.Clear();

            Log.VerboseFormat("Adding {0} source fields:", bcmFields.Count);
            foreach (var field in SourceFields)
            {
                Log.VerboseFormat("-{0} ({1})", field.Name, field.DataTypeLabel);
                cboSourceField.Items.Add(field);
            }            
        }
        #endregion
    }
}
