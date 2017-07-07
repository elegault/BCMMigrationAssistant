namespace BCM_Migration_Tool.Controls
{
    partial class Map
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            BCM_Migration_Tool.Objects.FieldMapperWrapper fieldMapperWrapper1 = new BCM_Migration_Tool.Objects.FieldMapperWrapper();
            BCM_Migration_Tool.Objects.FieldMapperWrapper fieldMapperWrapper2 = new BCM_Migration_Tool.Objects.FieldMapperWrapper();
            BCM_Migration_Tool.Objects.FieldMapperWrapper fieldMapperWrapper3 = new BCM_Migration_Tool.Objects.FieldMapperWrapper();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageAccounts = new System.Windows.Forms.TabPage();
            this.accounts1 = new BCM_Migration_Tool.Controls.Accounts();
            this.tabPageBusinessContacts = new System.Windows.Forms.TabPage();
            this.businessContacts1 = new BCM_Migration_Tool.Controls.BusinessContacts();
            this.tabPageOpportunities = new System.Windows.Forms.TabPage();
            this.opportunities1 = new BCM_Migration_Tool.Controls.Opportunities();
            this.tabPageLog = new System.Windows.Forms.TabPage();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.mapNavigationBar1 = new BCM_Migration_Tool.Controls.MapNavigationBar();
            this.tabControl1.SuspendLayout();
            this.tabPageAccounts.SuspendLayout();
            this.tabPageBusinessContacts.SuspendLayout();
            this.tabPageOpportunities.SuspendLayout();
            this.tabPageLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageAccounts);
            this.tabControl1.Controls.Add(this.tabPageBusinessContacts);
            this.tabControl1.Controls.Add(this.tabPageOpportunities);
            this.tabControl1.Controls.Add(this.tabPageLog);
            this.tabControl1.Location = new System.Drawing.Point(-5, 18);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(680, 391);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPageAccounts
            // 
            this.tabPageAccounts.Controls.Add(this.accounts1);
            this.tabPageAccounts.Location = new System.Drawing.Point(4, 22);
            this.tabPageAccounts.Name = "tabPageAccounts";
            this.tabPageAccounts.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAccounts.Size = new System.Drawing.Size(672, 365);
            this.tabPageAccounts.TabIndex = 0;
            this.tabPageAccounts.Text = "Accounts";
            this.tabPageAccounts.UseVisualStyleBackColor = true;
            // 
            // accounts1
            // 
            this.accounts1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.accounts1.AutoScroll = true;
            this.accounts1.BackColor = System.Drawing.Color.White;
            this.accounts1.FieldMapperWrapper = fieldMapperWrapper1;
            this.accounts1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accounts1.Location = new System.Drawing.Point(6, 10);
            this.accounts1.Name = "accounts1";
            this.accounts1.NumberOfDefaultFieldMappingControls = 0;
            this.accounts1.NumberOfFieldMappingControls = 0;
            this.accounts1.Size = new System.Drawing.Size(660, 349);
            this.accounts1.TabIndex = 0;
            // 
            // tabPageBusinessContacts
            // 
            this.tabPageBusinessContacts.Controls.Add(this.businessContacts1);
            this.tabPageBusinessContacts.Location = new System.Drawing.Point(4, 22);
            this.tabPageBusinessContacts.Name = "tabPageBusinessContacts";
            this.tabPageBusinessContacts.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBusinessContacts.Size = new System.Drawing.Size(672, 365);
            this.tabPageBusinessContacts.TabIndex = 1;
            this.tabPageBusinessContacts.Text = "Business Contacts";
            this.tabPageBusinessContacts.UseVisualStyleBackColor = true;
            // 
            // businessContacts1
            // 
            this.businessContacts1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.businessContacts1.AutoScroll = true;
            this.businessContacts1.BackColor = System.Drawing.Color.White;
            this.businessContacts1.FieldMapperWrapper = fieldMapperWrapper2;
            this.businessContacts1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.businessContacts1.Location = new System.Drawing.Point(6, 10);
            this.businessContacts1.Name = "businessContacts1";
            this.businessContacts1.NumberOfDefaultFieldMappingControls = 0;
            this.businessContacts1.NumberOfFieldMappingControls = 0;
            this.businessContacts1.Size = new System.Drawing.Size(660, 349);
            this.businessContacts1.TabIndex = 1;
            // 
            // tabPageOpportunities
            // 
            this.tabPageOpportunities.Controls.Add(this.opportunities1);
            this.tabPageOpportunities.Location = new System.Drawing.Point(4, 22);
            this.tabPageOpportunities.Name = "tabPageOpportunities";
            this.tabPageOpportunities.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOpportunities.Size = new System.Drawing.Size(672, 365);
            this.tabPageOpportunities.TabIndex = 2;
            this.tabPageOpportunities.Text = "Opportunities";
            this.tabPageOpportunities.UseVisualStyleBackColor = true;
            // 
            // opportunities1
            // 
            this.opportunities1.AutoScroll = true;
            this.opportunities1.BackColor = System.Drawing.Color.White;
            this.opportunities1.FieldMapperWrapper = fieldMapperWrapper3;
            this.opportunities1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.opportunities1.Location = new System.Drawing.Point(6, 10);
            this.opportunities1.Name = "opportunities1";
            this.opportunities1.NumberOfDefaultFieldMappingControls = 0;
            this.opportunities1.NumberOfFieldMappingControls = 0;
            this.opportunities1.Size = new System.Drawing.Size(660, 349);
            this.opportunities1.TabIndex = 2;
            // 
            // tabPageLog
            // 
            this.tabPageLog.Controls.Add(this.txtLog);
            this.tabPageLog.Location = new System.Drawing.Point(4, 22);
            this.tabPageLog.Name = "tabPageLog";
            this.tabPageLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLog.Size = new System.Drawing.Size(672, 365);
            this.tabPageLog.TabIndex = 3;
            this.tabPageLog.Text = "Log";
            this.tabPageLog.UseVisualStyleBackColor = true;
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(117)))), ((int)(((byte)(182)))));
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.txtLog.Location = new System.Drawing.Point(3, 3);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(666, 359);
            this.txtLog.TabIndex = 4;
            // 
            // mapNavigationBar1
            // 
            this.mapNavigationBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mapNavigationBar1.BackColor = System.Drawing.Color.White;
            this.mapNavigationBar1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mapNavigationBar1.Location = new System.Drawing.Point(0, 0);
            this.mapNavigationBar1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.mapNavigationBar1.Name = "mapNavigationBar1";
            this.mapNavigationBar1.NavigationBarState = BCM_Migration_Tool.Controls.MapNavigationBar.MapNavigationBarStates.Accounts;
            this.mapNavigationBar1.Size = new System.Drawing.Size(670, 42);
            this.mapNavigationBar1.TabIndex = 3;
            // 
            // Map
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.mapNavigationBar1);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Map";
            this.Size = new System.Drawing.Size(670, 408);
            this.tabControl1.ResumeLayout(false);
            this.tabPageAccounts.ResumeLayout(false);
            this.tabPageBusinessContacts.ResumeLayout(false);
            this.tabPageOpportunities.ResumeLayout(false);
            this.tabPageLog.ResumeLayout(false);
            this.tabPageLog.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private BusinessContacts businessContacts1;
        private Opportunities opportunities1;
        private System.Windows.Forms.TabPage tabPageAccounts;
        private System.Windows.Forms.TabPage tabPageBusinessContacts;
        private System.Windows.Forms.TabPage tabPageOpportunities;
        private MapNavigationBar mapNavigationBar1;
        private Accounts accounts1;
        private System.Windows.Forms.TabPage tabPageLog;
        internal System.Windows.Forms.TabControl tabControl1;
        internal System.Windows.Forms.TextBox txtLog;
    }
}
