namespace BCM_Migration_Tool
{
    partial class MigrationAssistant
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MigrationAssistant));
            this.labelApp = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageConnect = new System.Windows.Forms.TabPage();
            this.connect1 = new BCM_Migration_Tool.Controls.Connect();
            this.tabPageMap = new System.Windows.Forms.TabPage();
            this.map1 = new BCM_Migration_Tool.Controls.Map();
            this.tabPageMigrate = new System.Windows.Forms.TabPage();
            this.lblRESTRetries = new System.Windows.Forms.Label();
            this.numericUpDownRetryMax = new System.Windows.Forms.NumericUpDown();
            this.lblRetryDelay = new System.Windows.Forms.Label();
            this.numericUpDownRetryDelay = new System.Windows.Forms.NumericUpDown();
            this.lblMaxRecords = new System.Windows.Forms.Label();
            this.chkLBDebugMode = new System.Windows.Forms.CheckedListBox();
            this.chkGetOnly = new System.Windows.Forms.CheckBox();
            this.chkTestMode = new System.Windows.Forms.CheckBox();
            this.numericUpDownMaxRecords = new System.Windows.Forms.NumericUpDown();
            this.migrate1 = new BCM_Migration_Tool.Controls.Migrate();
            this.tabPageConfig = new System.Windows.Forms.TabPage();
            this.configure1 = new BCM_Migration_Tool.Controls.Configure();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblWebSite = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblCurrentUser = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.navigationBar1 = new BCM_Migration_Tool.Controls.NavigationBar();
            this.tabControl1.SuspendLayout();
            this.tabPageConnect.SuspendLayout();
            this.tabPageMap.SuspendLayout();
            this.tabPageMigrate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRetryMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRetryDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxRecords)).BeginInit();
            this.tabPageConfig.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelApp
            // 
            this.labelApp.AutoSize = true;
            this.labelApp.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelApp.Location = new System.Drawing.Point(58, 9);
            this.labelApp.Name = "labelApp";
            this.labelApp.Size = new System.Drawing.Size(225, 25);
            this.labelApp.TabIndex = 0;
            this.labelApp.Text = "BCM Migration Assistant";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageConnect);
            this.tabControl1.Controls.Add(this.tabPageMap);
            this.tabControl1.Controls.Add(this.tabPageMigrate);
            this.tabControl1.Controls.Add(this.tabPageConfig);
            this.tabControl1.Location = new System.Drawing.Point(-5, 113);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(0, 0);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(719, 481);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPageConnect
            // 
            this.tabPageConnect.BackColor = System.Drawing.Color.White;
            this.tabPageConnect.Controls.Add(this.connect1);
            this.tabPageConnect.Location = new System.Drawing.Point(4, 22);
            this.tabPageConnect.Name = "tabPageConnect";
            this.tabPageConnect.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageConnect.Size = new System.Drawing.Size(711, 455);
            this.tabPageConnect.TabIndex = 0;
            this.tabPageConnect.Text = "Connect";
            // 
            // connect1
            // 
            this.connect1.BackColor = System.Drawing.Color.White;
            this.connect1.Connected = false;
            this.connect1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.connect1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.connect1.Location = new System.Drawing.Point(3, 3);
            this.connect1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.connect1.Name = "connect1";
            this.connect1.Size = new System.Drawing.Size(705, 449);
            this.connect1.TabIndex = 0;
            // 
            // tabPageMap
            // 
            this.tabPageMap.AutoScroll = true;
            this.tabPageMap.Controls.Add(this.map1);
            this.tabPageMap.Location = new System.Drawing.Point(4, 22);
            this.tabPageMap.Name = "tabPageMap";
            this.tabPageMap.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMap.Size = new System.Drawing.Size(711, 455);
            this.tabPageMap.TabIndex = 1;
            this.tabPageMap.Text = "Map";
            this.tabPageMap.UseVisualStyleBackColor = true;
            // 
            // map1
            // 
            this.map1.AutoScroll = true;
            this.map1.AutoSize = true;
            this.map1.BackColor = System.Drawing.Color.White;
            this.map1.ConnectionString = null;
            this.map1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.map1.Location = new System.Drawing.Point(3, 3);
            this.map1.Name = "map1";
            this.map1.Size = new System.Drawing.Size(705, 449);
            this.map1.TabIndex = 0;
            // 
            // tabPageMigrate
            // 
            this.tabPageMigrate.Controls.Add(this.lblRESTRetries);
            this.tabPageMigrate.Controls.Add(this.numericUpDownRetryMax);
            this.tabPageMigrate.Controls.Add(this.lblRetryDelay);
            this.tabPageMigrate.Controls.Add(this.numericUpDownRetryDelay);
            this.tabPageMigrate.Controls.Add(this.lblMaxRecords);
            this.tabPageMigrate.Controls.Add(this.chkLBDebugMode);
            this.tabPageMigrate.Controls.Add(this.chkGetOnly);
            this.tabPageMigrate.Controls.Add(this.chkTestMode);
            this.tabPageMigrate.Controls.Add(this.numericUpDownMaxRecords);
            this.tabPageMigrate.Controls.Add(this.migrate1);
            this.tabPageMigrate.Location = new System.Drawing.Point(4, 22);
            this.tabPageMigrate.Name = "tabPageMigrate";
            this.tabPageMigrate.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMigrate.Size = new System.Drawing.Size(711, 455);
            this.tabPageMigrate.TabIndex = 2;
            this.tabPageMigrate.Text = "Migrate";
            this.tabPageMigrate.UseVisualStyleBackColor = true;
            // 
            // lblRESTRetries
            // 
            this.lblRESTRetries.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRESTRetries.AutoSize = true;
            this.lblRESTRetries.Location = new System.Drawing.Point(459, 59);
            this.lblRESTRetries.Name = "lblRESTRetries";
            this.lblRESTRetries.Size = new System.Drawing.Size(166, 13);
            this.lblRESTRetries.TabIndex = 11;
            this.lblRESTRetries.Text = "Max REST call retries on failure:";
            this.toolTip1.SetToolTip(this.lblRESTRetries, "How many retries to attempt before aborting the call (this setting is also used w" +
        "hen Test Mode is off)");
            // 
            // numericUpDownRetryMax
            // 
            this.numericUpDownRetryMax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownRetryMax.Location = new System.Drawing.Point(631, 57);
            this.numericUpDownRetryMax.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownRetryMax.Name = "numericUpDownRetryMax";
            this.numericUpDownRetryMax.Size = new System.Drawing.Size(50, 22);
            this.numericUpDownRetryMax.TabIndex = 10;
            this.toolTip1.SetToolTip(this.numericUpDownRetryMax, "How many retries to attempt before aborting the call (this setting is also used w" +
        "hen Test Mode is off)");
            this.numericUpDownRetryMax.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // lblRetryDelay
            // 
            this.lblRetryDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRetryDelay.AutoSize = true;
            this.lblRetryDelay.Location = new System.Drawing.Point(511, 34);
            this.lblRetryDelay.Name = "lblRetryDelay";
            this.lblRetryDelay.Size = new System.Drawing.Size(115, 13);
            this.lblRetryDelay.TabIndex = 9;
            this.lblRetryDelay.Text = "REST retry delay (sec):";
            this.toolTip1.SetToolTip(this.lblRetryDelay, "The amount of time in seconds to wait before any failed REST calls are retried");
            // 
            // numericUpDownRetryDelay
            // 
            this.numericUpDownRetryDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownRetryDelay.Location = new System.Drawing.Point(631, 32);
            this.numericUpDownRetryDelay.Maximum = new decimal(new int[] {
            1800,
            0,
            0,
            0});
            this.numericUpDownRetryDelay.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownRetryDelay.Name = "numericUpDownRetryDelay";
            this.numericUpDownRetryDelay.Size = new System.Drawing.Size(50, 22);
            this.numericUpDownRetryDelay.TabIndex = 8;
            this.toolTip1.SetToolTip(this.numericUpDownRetryDelay, "The amount of time in seconds to wait before any failed REST calls are retried (t" +
        "his setting is also used when Test Mode is off)");
            this.numericUpDownRetryDelay.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // lblMaxRecords
            // 
            this.lblMaxRecords.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMaxRecords.AutoSize = true;
            this.lblMaxRecords.Location = new System.Drawing.Point(383, 34);
            this.lblMaxRecords.Name = "lblMaxRecords";
            this.lblMaxRecords.Size = new System.Drawing.Size(72, 13);
            this.lblMaxRecords.TabIndex = 7;
            this.lblMaxRecords.Text = "Max records:";
            this.toolTip1.SetToolTip(this.lblMaxRecords, "Limits the number of records to create (per checked item)");
            // 
            // chkLBDebugMode
            // 
            this.chkLBDebugMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkLBDebugMode.FormattingEnabled = true;
            this.chkLBDebugMode.Items.AddRange(new object[] {
            "Accounts",
            "Contacts",
            "Opportunities",
            "Deal Stages"});
            this.chkLBDebugMode.Location = new System.Drawing.Point(252, 34);
            this.chkLBDebugMode.Name = "chkLBDebugMode";
            this.chkLBDebugMode.Size = new System.Drawing.Size(120, 38);
            this.chkLBDebugMode.TabIndex = 6;
            // 
            // chkGetOnly
            // 
            this.chkGetOnly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkGetOnly.AutoSize = true;
            this.chkGetOnly.Location = new System.Drawing.Point(168, 57);
            this.chkGetOnly.Name = "chkGetOnly";
            this.chkGetOnly.Size = new System.Drawing.Size(69, 17);
            this.chkGetOnly.TabIndex = 2;
            this.chkGetOnly.Text = "Get only";
            this.toolTip1.SetToolTip(this.chkGetOnly, "Will not create records if checked");
            this.chkGetOnly.UseVisualStyleBackColor = true;
            // 
            // chkTestMode
            // 
            this.chkTestMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkTestMode.AutoSize = true;
            this.chkTestMode.Location = new System.Drawing.Point(168, 34);
            this.chkTestMode.Name = "chkTestMode";
            this.chkTestMode.Size = new System.Drawing.Size(78, 17);
            this.chkTestMode.TabIndex = 1;
            this.chkTestMode.Text = "Test Mode";
            this.toolTip1.SetToolTip(this.chkTestMode, "If checked, will limit the records created or retrieved to the items selected in " +
        "the list box (up to the Max records specified)");
            this.chkTestMode.UseVisualStyleBackColor = true;
            // 
            // numericUpDownMaxRecords
            // 
            this.numericUpDownMaxRecords.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownMaxRecords.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownMaxRecords.Location = new System.Drawing.Point(386, 50);
            this.numericUpDownMaxRecords.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownMaxRecords.Name = "numericUpDownMaxRecords";
            this.numericUpDownMaxRecords.Size = new System.Drawing.Size(50, 22);
            this.numericUpDownMaxRecords.TabIndex = 4;
            this.toolTip1.SetToolTip(this.numericUpDownMaxRecords, "Limits the number of records to create");
            // 
            // migrate1
            // 
            this.migrate1.BackColor = System.Drawing.Color.White;
            this.migrate1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.migrate1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.migrate1.Location = new System.Drawing.Point(3, 3);
            this.migrate1.Name = "migrate1";
            this.migrate1.Size = new System.Drawing.Size(705, 449);
            this.migrate1.TabIndex = 0;
            // 
            // tabPageConfig
            // 
            this.tabPageConfig.Controls.Add(this.configure1);
            this.tabPageConfig.Location = new System.Drawing.Point(4, 22);
            this.tabPageConfig.Name = "tabPageConfig";
            this.tabPageConfig.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageConfig.Size = new System.Drawing.Size(711, 455);
            this.tabPageConfig.TabIndex = 3;
            this.tabPageConfig.Text = "Configure";
            this.tabPageConfig.UseVisualStyleBackColor = true;
            // 
            // configure1
            // 
            this.configure1.BackColor = System.Drawing.Color.White;
            this.configure1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.configure1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.configure1.Location = new System.Drawing.Point(3, 3);
            this.configure1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.configure1.Name = "configure1";
            this.configure1.Size = new System.Drawing.Size(705, 449);
            this.configure1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.lblWebSite);
            this.panel1.Controls.Add(this.lblVersion);
            this.panel1.Controls.Add(this.lblCurrentUser);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.labelApp);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(709, 89);
            this.panel1.TabIndex = 0;
            // 
            // lblWebSite
            // 
            this.lblWebSite.AutoSize = true;
            this.lblWebSite.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblWebSite.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWebSite.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(117)))), ((int)(((byte)(182)))));
            this.lblWebSite.Location = new System.Drawing.Point(60, 37);
            this.lblWebSite.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.lblWebSite.Name = "lblWebSite";
            this.lblWebSite.Size = new System.Drawing.Size(355, 17);
            this.lblWebSite.TabIndex = 2;
            this.lblWebSite.Text = "Created by Rockin\' Software/Eric Legault Consulting Inc.";
            this.lblWebSite.Click += new System.EventHandler(this.lblWebSite_Click);
            // 
            // lblVersion
            // 
            this.lblVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblVersion.AutoEllipsis = true;
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(117)))), ((int)(((byte)(182)))));
            this.lblVersion.Location = new System.Drawing.Point(60, 59);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(72, 17);
            this.lblVersion.TabIndex = 3;
            this.lblVersion.Text = "Version 1.0";
            this.lblVersion.Click += new System.EventHandler(this.lblVersion_Click);
            // 
            // lblCurrentUser
            // 
            this.lblCurrentUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCurrentUser.AutoEllipsis = true;
            this.lblCurrentUser.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentUser.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(117)))), ((int)(((byte)(182)))));
            this.lblCurrentUser.Location = new System.Drawing.Point(414, 9);
            this.lblCurrentUser.Name = "lblCurrentUser";
            this.lblCurrentUser.Size = new System.Drawing.Size(283, 21);
            this.lblCurrentUser.TabIndex = 1;
            this.lblCurrentUser.Text = "Signed in as:";
            this.lblCurrentUser.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblCurrentUser.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::BCM_Migration_Tool.Properties.Resources.BCMtool_icon;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(40, 34);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // navigationBar1
            // 
            this.navigationBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.navigationBar1.BackColor = System.Drawing.Color.White;
            this.navigationBar1.ConfigureEnabled = true;
            this.navigationBar1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.navigationBar1.Location = new System.Drawing.Point(4, 92);
            this.navigationBar1.MapEnabled = false;
            this.navigationBar1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.navigationBar1.MigrateEnabled = false;
            this.navigationBar1.Name = "navigationBar1";
            this.navigationBar1.NavigationBarState = BCM_Migration_Tool.Controls.NavigationBar.NavigationBarStates.Configuration;
            this.navigationBar1.Size = new System.Drawing.Size(706, 42);
            this.navigationBar1.TabIndex = 1;
            // 
            // MigrationAssistant
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(709, 592);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.navigationBar1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MigrationAssistant";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BCM Migration Assistant";
            this.Load += new System.EventHandler(this.MigrationAssistant_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPageConnect.ResumeLayout(false);
            this.tabPageMap.ResumeLayout(false);
            this.tabPageMap.PerformLayout();
            this.tabPageMigrate.ResumeLayout(false);
            this.tabPageMigrate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRetryMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRetryDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxRecords)).EndInit();
            this.tabPageConfig.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelApp;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageMap;
        private System.Windows.Forms.TabPage tabPageMigrate;
        private System.Windows.Forms.TabPage tabPageConnect;
        private System.Windows.Forms.Panel panel1;
        private Controls.NavigationBar navigationBar1;
        private Controls.Connect connect1;
        private Controls.Migrate migrate1;
        private System.Windows.Forms.CheckBox chkTestMode;
        private Controls.Map map1;
        private System.Windows.Forms.CheckBox chkGetOnly;
        private System.Windows.Forms.Label lblCurrentUser;
        private System.Windows.Forms.TabPage tabPageConfig;
        private Controls.Configure configure1;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblWebSite;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckedListBox chkLBDebugMode;
        internal System.Windows.Forms.Label lblMaxRecords;
        internal System.Windows.Forms.Label lblRESTRetries;
        internal System.Windows.Forms.NumericUpDown numericUpDownRetryMax;
        internal System.Windows.Forms.Label lblRetryDelay;
        internal System.Windows.Forms.NumericUpDown numericUpDownRetryDelay;
        internal System.Windows.Forms.NumericUpDown numericUpDownMaxRecords;
    }
}