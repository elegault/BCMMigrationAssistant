using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BCM_Migration_Tool.Interfaces;
using BCM_Migration_Tool.Objects;
using TracerX;

namespace BCM_Migration_Tool.Controls
{
    public partial class Accounts : UserControl, IFieldMapper
    {
        #region Fields
        public event EventHandler<EventArgs> FieldMappingAdded;
        private static readonly Logger Log = Logger.GetLogger("Accounts");
        #endregion
        #region Constructors
        public Accounts()
        {
            InitializeComponent();
            FieldMapperWrapper = new FieldMapperWrapper();
            this.Load += accounts_Load;
            lblNewFieldMapping.Click += cmdNewMapping_Click;
        }
        private void accounts_Load(object sender, EventArgs e)
        {}
        #endregion
        #region Properties
        public FieldMapperWrapper FieldMapperWrapper { get; set; }
        public int NumberOfDefaultFieldMappingControls { get; set; }
        public int NumberOfFieldMappingControls { get; set; }
        #endregion
        #region Control Events
        private void cmdNewMapping_Click(object sender, EventArgs e)
        {
            AddNewMapping(FieldMappings.BCMAccountFields.BCMFields.UnMappedFields, FieldMappings.OCMAccountFields.OCMFields.UnMappedFields);
        }
        private void fieldMapping_DestinationFieldChanged(object sender, Objects.FieldMappingEventArgs e)
        {
            //Remove the passed field from the master collection (Mapped Fields) if it was "added" (thus should no longer be available in other controls), or add the passed field back to the master collection of UnMappedFields
            FieldMapping currentFieldMappingControl = (FieldMapping) sender;
            FieldMapperWrapper.ProcessDestinationFieldChange(currentFieldMappingControl, flowLayoutPanel1.Controls, e);

            //foreach (Control c in flowLayoutPanel1.Controls)
            //{
            //    if (c.GetType() == typeof(FieldMapping))
            //    {
            //        FieldMapping fm = (FieldMapping) c;
            //        if (fm != currentFieldMappingControl)
            //        {
            //            //Add or remove the field that was changed and update relevant DestinationFields/SourceFields collection and combo

            //            switch (e.ChangeType)
            //            {
            //                case FieldMappingEventArgs.ChangeTypes.Added:
            //                    fm.RemoveOCMField(e.OCMField);
            //                    break;
            //                case FieldMappingEventArgs.ChangeTypes.Removed:
            //                    fm.AddOCMField(e.OCMField);
            //                    break;
            //            }
            //        }
            //    }
            //}
        }
        private void fieldMapping_MappingRemoved(object sender, Objects.FieldMappingEventArgs e)
        {
            //Add back any selected mappings to the relevant master collection, and remove them as well
            FieldMapping currentFieldMappingControl = (FieldMapping) sender;
            FieldMapperWrapper.ProcessMappingRemoval(currentFieldMappingControl, flowLayoutPanel1.Controls, e);
            Invalidate();

            //Debug.WriteLine(String.Format("DestinationFieldChanged: Removing current OCM field from unmapped fields: {0}; Adding to mapped fields: {1}; Current mapped fields: {2}; Current unmapped fields: {3}; ", e.OCMField.Name, fieldMapping.DestinationField.Name, FieldMappings.OCMAccountFields.OCMFields.MappedFields.Count, FieldMappings.OCMAccountFields.OCMFields.UnMappedFields.Count));

            //FieldMappings.OCMAccountFields.OCMFields.MappedFields.Remove(currentFieldMappingControl.DestinationField);
            //FieldMappings.OCMAccountFields.OCMFields.UnMappedFields.Add(e.OCMField);

            //FieldMappings.BCMAccountFields.BCMFields.MappedFields.Remove(currentFieldMappingControl.SourceField);
            //FieldMappings.BCMAccountFields.BCMFields.UnMappedFields.Add(e.BCMField);

            //currentFieldMappingControl.UpdateSourceMappings(FieldMappings.BCMAccountFields.BCMFields.UnMappedFields);
            //currentFieldMappingControl.UpdateDestinationMappings(FieldMappings.OCMAccountFields.OCMFields.UnMappedFields);
            //foreach (Control c in flowLayoutPanel1.Controls)
            //{
            //    if (c.GetType() == typeof(FieldMapping))
            //    {
            //        FieldMapping fm = (FieldMapping) c;
            //        if (fm != currentFieldMappingControl)
            //        {
            //            if (e.OCMField != null)
            //                fm.AddOCMField(e.OCMField);
            //            if (e.BCMField != null)
            //                fm.AddBCMField(e.BCMField);
            //        }
            //    }
            //}
        }
        private void fieldMapping_SourceFieldChanged(object sender, Objects.FieldMappingEventArgs e)
        {
            //Remove the current field from the master collection, add the old field back to the master collection                
            FieldMapping currentFieldMappingControl = (FieldMapping) sender;
            FieldMapperWrapper.ProcessSourceFieldChange(currentFieldMappingControl, flowLayoutPanel1.Controls, e);

            //try
            //{
            //    foreach (Control c in flowLayoutPanel1.Controls)
            //    {
            //        if (c.GetType() == typeof(FieldMapping))
            //        {
            //            FieldMapping fm = (FieldMapping) c;
            //            if (fm != currentFieldMappingControl)
            //                //Add or remove the field that was changed and update relevant DestinationFields/SourceFields collection and combo
            //                switch (e.ChangeType)
            //                {
            //                    case FieldMappingEventArgs.ChangeTypes.Added:
            //                        fm.RemoveBCMField(e.BCMField);
            //                        break;
            //                    case FieldMappingEventArgs.ChangeTypes.Removed:
            //                        fm.AddBCMField(e.BCMField);
            //                        break;
            //                }
            //        }
            //    }
            //}
            //catch (System.Exception ex)
            //{
            //    //MessageBox.Show("Error in BCM_Migration_Tool.Controls.Accounts.fieldMapping_SourceFieldChanged()", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        #endregion
        #region Methods
        public void AddReadOnlyMappings()
        {
            ReadOnlyFieldMapping readOnlyFieldMapping;

            try
            {                
                //Business Address
                readOnlyFieldMapping = new ReadOnlyFieldMapping();
                readOnlyFieldMapping.SourceField = FieldMappings.BCMAccountFields.BusinessAddress;
                flowLayoutPanel1.Controls.Add(readOnlyFieldMapping);
                //WorkPhoneNum
                readOnlyFieldMapping = new ReadOnlyFieldMapping();
                readOnlyFieldMapping.SourceField = FieldMappings.BCMAccountFields.WorkPhoneNum;
                flowLayoutPanel1.Controls.Add(readOnlyFieldMapping);
                //Email
                readOnlyFieldMapping = new ReadOnlyFieldMapping();
                readOnlyFieldMapping.SourceField = FieldMappings.BCMAccountFields.Email;
                flowLayoutPanel1.Controls.Add(readOnlyFieldMapping);
                //Full Name
                readOnlyFieldMapping = new ReadOnlyFieldMapping();
                readOnlyFieldMapping.SourceField = FieldMappings.BCMAccountFields.FullName;
                flowLayoutPanel1.Controls.Add(readOnlyFieldMapping);

                //Business Phone
                //readOnlyFieldMapping = new ReadOnlyFieldMapping();
                //readOnlyFieldMapping.SourceField = FieldMappings.BCMAccountFields.BusinessPhone;
                //flowLayoutPanel1.Controls.Add(readOnlyFieldMapping);

                //Notes
                readOnlyFieldMapping = new ReadOnlyFieldMapping();
                readOnlyFieldMapping.SourceField = FieldMappings.BCMAccountFields.Notes;
                flowLayoutPanel1.Controls.Add(readOnlyFieldMapping);
                //Web page
                readOnlyFieldMapping = new ReadOnlyFieldMapping();
                readOnlyFieldMapping.SourceField = FieldMappings.BCMAccountFields.WebPageAddress;
                flowLayoutPanel1.Controls.Add(readOnlyFieldMapping);

                
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            Invalidate();            
        }
        public void AddNewMapping(List<FieldMappings.BCMField> bcmFields, List<FieldMappings.OCMField> ocmFields)
        {
         
            FieldMapping fieldMapping;
            fieldMapping = new FieldMapping();
            FieldMapperWrapper.AddNewMapping(ref fieldMapping, bcmFields, ocmFields, NumberOfFieldMappingControls, this, flowLayoutPanel1);

            fieldMapping.DestinationFieldChanged += fieldMapping_DestinationFieldChanged;
            fieldMapping.SourceFieldChanged += fieldMapping_SourceFieldChanged;
            fieldMapping.MappingRemoved += fieldMapping_MappingRemoved;
            
            //Mark as invalid until fields are set?
            //fieldMapping.Invalid = true;
            NumberOfFieldMappingControls += 1;
            OnFieldMappingAdded();
        }
        public void OnFieldMappingAdded()
        {
            FieldMappingAdded?.Invoke(this, EventArgs.Empty);
        }
        public bool ValidateFields()
        {
            bool result = true;
            foreach (Control c in flowLayoutPanel1.Controls)
            {
                if (c.GetType() == typeof(FieldMapping))
                {
                    FieldMapping fm = (FieldMapping) c;
                    if (fm.Invalid || fm.DestinationField == null || fm.SourceField == null || fm.SourceField?.OCMFieldMapping == null)
                    {
                        //ignore default unmapped fields
                        if (fm.DestinationField == null && fm.SourceField == null)
                            continue;
                        result = false;
                        break;
                    }                    
                }
            }

            return result;
        }
        #endregion
    }
}
