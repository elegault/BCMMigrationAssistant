using System;
using System.Collections.Generic;
using BCM_Migration_Tool.Objects;

namespace BCM_Migration_Tool.Interfaces
{
    interface IFieldMapper
    {
        //bool Collapsed { get; set; }
        int NumberOfDefaultFieldMappingControls { get; set; }
        int NumberOfFieldMappingControls { get; set; }
        void AddNewMapping(List<FieldMappings.BCMField> bcmFields, List<FieldMappings.OCMField> ocmFields);
        void AddReadOnlyMappings();
        event EventHandler<EventArgs> FieldMappingAdded;
        void OnFieldMappingAdded();
        //void ToggleUIState(bool collapsed);
        FieldMapperWrapper FieldMapperWrapper { get; set; }
    }
}
