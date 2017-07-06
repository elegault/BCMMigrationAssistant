using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace BCM_Migration_Tool.Controls
{
    public partial class Configure : UserControl
    {
        public Configure()
        {
            InitializeComponent();
        }

        private void cmdOpenDir_Click(object sender, System.EventArgs e)
        {
            try
            {
                Process.Start(AppDomain.CurrentDomain.BaseDirectory);
            }
            catch (Exception exception)
            {
                
            }            
        }
    }
}
