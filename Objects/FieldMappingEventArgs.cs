using System;

namespace BCM_Migration_Tool.Objects
{
    public class FieldMappingEventArgs : EventArgs
    {
        #region Fields
        public enum ChangeTypes
        {
            Added,
            Removed,
            MappingDeleted
        }
        #endregion
        public FieldMappingEventArgs(ChangeTypes changeType, FieldMappings.BCMField bcmField)
        {
            BCMField = bcmField;
            ChangeType = changeType;
        }
        public FieldMappingEventArgs(ChangeTypes changeType, FieldMappings.OCMField ocmField)
        {
            OCMField = ocmField;
            ChangeType = changeType;
        }
        public FieldMappingEventArgs(ChangeTypes changeType, FieldMappings.OCMField ocmField, FieldMappings.BCMField bcmField)
        {
            OCMField = ocmField; BCMField = bcmField;
            ChangeType = changeType;
        }
        
        #region Properties
        public FieldMappings.BCMField BCMField { get; set; }
        public ChangeTypes ChangeType { get; set; }
        public FieldMappings.OCMField OCMField { get; set; }

        #endregion
    }
}
