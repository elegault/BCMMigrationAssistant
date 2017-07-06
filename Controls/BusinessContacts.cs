using BCM_Migration_Tool.Interfaces;
using BCM_Migration_Tool.Objects;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BCM_Migration_Tool.Controls
{
    public partial class BusinessContacts : UserControl, IFieldMapper
    {
        #region Fields
        //private bool collapsed;

        public event EventHandler<EventArgs> FieldMappingAdded;
        #endregion


        #region Constructors
        public BusinessContacts()
        {
            InitializeComponent();
            FieldMapperWrapper = new FieldMapperWrapper();
            lblNewFieldMapping.Click += cmdNewMapping_Click;
        }
        #endregion

        #region Methods
        public void AddNewMapping(List<FieldMappings.BCMField> bcmFields, List<FieldMappings.OCMField> ocmFields)
        {
            FieldMapping fieldMapping = new FieldMapping();
            FieldMapperWrapper.AddNewMapping(ref fieldMapping, bcmFields, ocmFields, NumberOfFieldMappingControls, this, flowLayoutPanel1);
            //fieldMapping.Name = String.Format("fieldMappingControl{0}", NumberOfFieldMappingControls);
            //fieldMapping.SetSourceFields(bcmFields);
            //fieldMapping.SetDestinationFields(ocmFields);
            fieldMapping.DestinationFieldChanged += fieldMapping_DestinationFieldChanged;
            fieldMapping.SourceFieldChanged += fieldMapping_SourceFieldChanged;
            fieldMapping.MappingRemoved += fieldMapping_MappingRemoved;
            //fieldMapping.Size = new System.Drawing.Size(493, 31);
            //this.SuspendLayout();
            //flowLayoutPanel1.Controls.Add(fieldMapping);
            //this.ResumeLayout(false);
            //this.PerformLayout();
            //Invalidate();
            NumberOfFieldMappingControls += 1;
            OnFieldMappingAdded();
        }

        public void AddReadOnlyMappings()
        {
            flowLayoutPanel1.Controls.Add(new ReadOnlyFieldMapping(FieldMappings.BCMBusinessContactFields.Birthday));
            flowLayoutPanel1.Controls.Add(new ReadOnlyFieldMapping(FieldMappings.BCMBusinessContactFields.WorkAddressStreet));
            flowLayoutPanel1.Controls.Add(new ReadOnlyFieldMapping(FieldMappings.BCMBusinessContactFields.WorkPhoneNum));
            flowLayoutPanel1.Controls.Add(new ReadOnlyFieldMapping(FieldMappings.BCMBusinessContactFields.CompanyName));
            flowLayoutPanel1.Controls.Add(new ReadOnlyFieldMapping(FieldMappings.BCMBusinessContactFields.ContactNotes));
            flowLayoutPanel1.Controls.Add(new ReadOnlyFieldMapping(FieldMappings.BCMBusinessContactFields.Email1Address));
            //flowLayoutPanel1.Controls.Add(new ReadOnlyFieldMapping(FieldMappings.BCMBusinessContactFields.Account));
            flowLayoutPanel1.Controls.Add(new ReadOnlyFieldMapping(FieldMappings.BCMBusinessContactFields.FullName));            
            flowLayoutPanel1.Controls.Add(new ReadOnlyFieldMapping(FieldMappings.BCMBusinessContactFields.HomePhoneNum));
            flowLayoutPanel1.Controls.Add(new ReadOnlyFieldMapping(FieldMappings.BCMBusinessContactFields.JobTitle));
            flowLayoutPanel1.Controls.Add(new ReadOnlyFieldMapping(FieldMappings.BCMBusinessContactFields.MobilePhoneNum));
            Invalidate();
        }

        public void OnFieldMappingAdded()
        {
            FieldMappingAdded?.Invoke(this, EventArgs.Empty);
        }
        public bool ValidateFields()
        {
            foreach (Control c in flowLayoutPanel1.Controls)
            {
                if (c.GetType() == typeof(FieldMapping))
                {
                    FieldMapping fm = (FieldMapping) c;
                    //ignore default unmapped fields
                    if (fm.DestinationField == null && fm.SourceField == null)
                        continue;
                    if (fm.Invalid || fm.DestinationField == null || fm.SourceField == null || fm.SourceField?.OCMFieldMapping == null)
                        return false;
                }
            }

            return true;
        }
        //public void ToggleUIState(bool collapsed)
        //{
        //    if (collapsed)
        //    {
        //        Height = 35;
        //        cmdToggle.Image = Resources.downbutton;
        //        flowLayoutPanel1.Visible = false;
        //        lblNewFieldMapping.Visible = false;
        //        cmdNewMapping.Visible = false;
        //    }
        //    else
        //    {
        //        Height = 347; //267
        //        cmdToggle.Image = Resources.upbutton;
        //        flowLayoutPanel1.Visible = true;
        //        lblNewFieldMapping.Visible = true;
        //        cmdNewMapping.Visible = true;
        //    }
        //    Invalidate();
        //}
        #endregion
        //[
        //    Category("BCM"),
        //    Description("Sets the collapsed/expanded state of the controls.")
        //]
        //public bool Collapsed
        //{
        //    get { return collapsed; }
        //    set
        //    {
        //        collapsed = value;
        //        ToggleUIState(value);
        //        Invalidate();
        //    }
        //}        #region Methods
        #region Properties
        //public bool Collapsed
        //{
        //    get { return collapsed; }
        //    set
        //    {
        //        collapsed = value;
        //        ToggleUIState(value);
        //        //Invalidate();
        //    }
        //}

        public FieldMapperWrapper FieldMapperWrapper { get; set; }
        public int NumberOfDefaultFieldMappingControls { get; set; }
        public int NumberOfFieldMappingControls { get; set; }
        #endregion
        #region Control Events
        private void cmdNewMapping_Click(object sender, EventArgs e)
        {
            AddNewMapping(FieldMappings.BCMBusinessContactFields.BCMFields.UnMappedFields, FieldMappings.OCMBusinessContactFields.OCMFields.UnMappedFields);
        }
        private void fieldMapping_DestinationFieldChanged(object sender, Objects.FieldMappingEventArgs e)
        {
            //Remove the passed field from the master collection (Mapped Fields) if it was "added" (thus should no longer be available in other controls), or add the passed field back to the master collection of UnMappedFields
            FieldMapping currentFieldMappingControl = (FieldMapping) sender;
            FieldMapperWrapper.ProcessDestinationFieldChange(currentFieldMappingControl, flowLayoutPanel1.Controls, e);
        }
        private void fieldMapping_MappingRemoved(object sender, Objects.FieldMappingEventArgs e)
        {
            //Add back any selected mappings to the relevant master collection, and remove them as well
            FieldMapping currentFieldMappingControl = (FieldMapping) sender;
            FieldMapperWrapper.ProcessMappingRemoval(currentFieldMappingControl, flowLayoutPanel1.Controls, e);
        }
        private void fieldMapping_SourceFieldChanged(object sender, Objects.FieldMappingEventArgs e)
        {
            //Remove the current field from the master collection, add the old field back to the master collection
            FieldMapping currentFieldMappingControl = (FieldMapping) sender;
            FieldMapperWrapper.ProcessSourceFieldChange(currentFieldMappingControl, flowLayoutPanel1.Controls, e);
        }
        #endregion
    }
}
