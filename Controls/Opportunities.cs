using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BCM_Migration_Tool.Interfaces;
using BCM_Migration_Tool.Objects;

namespace BCM_Migration_Tool.Controls
{
    public partial class Opportunities : UserControl, IFieldMapper
    {
        #region Fields
        public event EventHandler<EventArgs> FieldMappingAdded;

        #endregion
        #region Constructors
        public Opportunities()
        {
            InitializeComponent();
            FieldMapperWrapper = new FieldMapperWrapper();
            lblNewFieldMapping.Click += cmdNewMapping_Click;
        }
        #endregion
        #region Control Events
        private void cmdNewMapping_Click(object sender, EventArgs e)
        {
            AddNewMapping(FieldMappings.BCMOpportunityFields.BCMFields.UnMappedFields, FieldMappings.OCMDealFields.OCMFields.UnMappedFields);
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
            currentFieldMappingControl.Invalidate();
        }
        #endregion
        #region Properties
        public FieldMapperWrapper FieldMapperWrapper { get; set; }

        public int NumberOfDefaultFieldMappingControls { get; set; }
        public int NumberOfFieldMappingControls { get; set; }
        #endregion
        #region Methods
        public void AddNewMapping(List<FieldMappings.BCMField> bcmFields, List<FieldMappings.OCMField> ocmFields)
        {
            FieldMapping fieldMapping = new FieldMapping();
            FieldMapperWrapper.AddNewMapping(ref fieldMapping, bcmFields, ocmFields, NumberOfFieldMappingControls, this, flowLayoutPanel1);
            fieldMapping.DestinationFieldChanged += fieldMapping_DestinationFieldChanged;
            fieldMapping.SourceFieldChanged += fieldMapping_SourceFieldChanged;
            fieldMapping.MappingRemoved += fieldMapping_MappingRemoved;

            NumberOfFieldMappingControls += 1;
            OnFieldMappingAdded();
        }

        public void AddReadOnlyMappings()
        {            
            flowLayoutPanel1.Controls.Add(new ReadOnlyFieldMapping(FieldMappings.BCMOpportunityFields.CloseDate));
            flowLayoutPanel1.Controls.Add(new ReadOnlyFieldMapping(FieldMappings.BCMOpportunityFields.Contact));
            flowLayoutPanel1.Controls.Add(new ReadOnlyFieldMapping(FieldMappings.BCMOpportunityFields.Account));
            flowLayoutPanel1.Controls.Add(new ReadOnlyFieldMapping(FieldMappings.BCMOpportunityFields.Title));
            flowLayoutPanel1.Controls.Add(new ReadOnlyFieldMapping(FieldMappings.BCMOpportunityFields.Stage));
            flowLayoutPanel1.Controls.Add(new ReadOnlyFieldMapping(FieldMappings.BCMOpportunityFields.TotalAmount));
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
        #endregion
    }
}
