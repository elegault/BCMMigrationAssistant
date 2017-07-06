using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BCM_Migration_Tool.Controls;
using TracerX;

namespace BCM_Migration_Tool.Objects
{
    public class FieldMapperWrapper
    {
        #region Fields
        private static readonly Logger Log = Logger.GetLogger("FieldMapperWrapper");
        #endregion

        #region Methods
        internal void AddNewMapping(ref FieldMapping fieldMapping, List<FieldMappings.BCMField> bcmFields, List<FieldMappings.OCMField> ocmFields, int NumberOfFieldMappingControls, UserControl parent, FlowLayoutPanel flowLayoutPanel)
        {

            try
            {
                //FieldMapping fieldMapping;
                //fieldMapping = new FieldMapping();
                fieldMapping.Name = String.Format("fieldMappingControl{0}", NumberOfFieldMappingControls);
                fieldMapping.SetSourceFields(bcmFields);
                fieldMapping.SetDestinationFields(ocmFields);
                //fieldMapping.DestinationFieldChanged += fieldMapping_DestinationFieldChanged;
                //fieldMapping.SourceFieldChanged += fieldMapping_SourceFieldChanged;
                //fieldMapping.MappingRemoved += fieldMapping_MappingRemoved;
                fieldMapping.Size = new System.Drawing.Size(493, 31);
                parent.SuspendLayout();
                flowLayoutPanel.Controls.Add(fieldMapping);
                parent.ResumeLayout(false);
                parent.PerformLayout();
                parent.Invalidate();
                //NumberOfFieldMappingControls += 1;
                //OnFieldMappingAdded();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        internal void ProcessDestinationFieldChange(FieldMapping currentFieldMappingControl, System.Windows.Forms.Control.ControlCollection parentContainer, FieldMappingEventArgs e)
        {
            try
            {
                foreach (Control c in parentContainer)
                    if (c.GetType() == typeof(FieldMapping))
                    {
                        FieldMapping fm = (FieldMapping) c;
                        if (fm != currentFieldMappingControl)
                            //Add or remove the field that was changed and update relevant DestinationFields/SourceFields collection and combo

                            switch (e.ChangeType)
                            {
                                case FieldMappingEventArgs.ChangeTypes.Added:
                                    fm.RemoveOCMField(e.OCMField);
                                    break;
                                case FieldMappingEventArgs.ChangeTypes.Removed:
                                    fm.AddOCMField(e.OCMField);
                                    break;
                            }
                    }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        internal void ProcessMappingRemoval(FieldMapping currentFieldMappingControl, System.Windows.Forms.Control.ControlCollection parentContainer, FieldMappingEventArgs e)
        {
            try
            {
                //Add back any selected mappings to the relevant master collection, and remove them as well

                foreach (Control c in parentContainer)
                    if (c.GetType() == typeof(FieldMapping))
                    {
                        FieldMapping fm = (FieldMapping) c;
                        if (fm != currentFieldMappingControl)
                        {
                            if (e.OCMField != null)
                                fm.AddOCMField(e.OCMField);
                            if (e.BCMField != null)
                                fm.AddBCMField(e.BCMField);
                        }
                    }

                parentContainer.Remove(currentFieldMappingControl);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        internal void ProcessSourceFieldChange(FieldMapping currentFieldMappingControl, System.Windows.Forms.Control.ControlCollection parentContainer, FieldMappingEventArgs e)
        {
            try
            {
                //Remove the current field from the master collection, add the old field back to the master collection

                foreach (Control c in parentContainer)
                    if (c.GetType() == typeof(FieldMapping))
                    {
                        FieldMapping fm = (FieldMapping) c;
                        if (fm != currentFieldMappingControl)
                            //Add or remove the field that was changed and update relevant DestinationFields/SourceFields collection and combo
                            switch (e.ChangeType)
                            {
                                case FieldMappingEventArgs.ChangeTypes.Added:
                                    fm.RemoveBCMField(e.BCMField);
                                    break;
                                case FieldMappingEventArgs.ChangeTypes.Removed:
                                    fm.AddBCMField(e.BCMField);
                                    break;
                            }
                    }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
        #endregion
    }
}
