using System.Windows.Forms;
using BCM_Migration_Tool.Objects;

namespace BCM_Migration_Tool.Controls
{
    public partial class ReadOnlyFieldMapping : UserControl
    {
        #region Fields
        private FieldMappings.BCMField sourceField;
        #endregion
        #region Constructors
        public ReadOnlyFieldMapping(FieldMappings.BCMField sourceField)
        {            
            InitializeComponent();
            SourceField = sourceField;
        }
        public ReadOnlyFieldMapping()
        {
            InitializeComponent();         
        }
        #endregion
        #region Properties
        public FieldMappings.OCMField DestinationField
        {
            get { return SourceField.OCMFieldMapping; }
        }
        public FieldMappings.BCMField SourceField
        {
            get { return sourceField; }
            set
            {
                sourceField = value;
                lblSourceField.Text = value.Name;
                lblDestinationField.Text = DestinationField?.Name;
                Invalidate();
            }
        }
        #endregion
    }
}
