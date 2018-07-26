using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BCM_Migration_Tool.Controls
{
    public partial class Testing : UserControl
    {
        public Testing()
        {
            InitializeComponent();
        }

        private void chkTestMode_CheckedChanged(object sender, EventArgs e)
        {
            chkLBDebugMode.Enabled = chkTestMode.Checked;
            chkGetOnly.Enabled = chkTestMode.Checked;
            chkRepeatBatch.Enabled = chkTestMode.Checked;
            numericUpDownMaxRecords.Enabled = chkTestMode.Checked;
            numericUpDownRepeatCount.Enabled = chkTestMode.Checked;
            numericUpDownRetryDelay.Enabled = chkTestMode.Checked;
            numericUpDownRetryMax.Enabled = chkTestMode.Checked;
        }
    }
}
