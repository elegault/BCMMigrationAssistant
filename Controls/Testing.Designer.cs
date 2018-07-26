namespace BCM_Migration_Tool.Controls
{
    partial class Testing
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
            this.components = new System.ComponentModel.Container();
            this.numericUpDownRepeatCount = new System.Windows.Forms.NumericUpDown();
            this.chkRepeatBatch = new System.Windows.Forms.CheckBox();
            this.lblRESTRetries = new System.Windows.Forms.Label();
            this.numericUpDownRetryMax = new System.Windows.Forms.NumericUpDown();
            this.lblRetryDelay = new System.Windows.Forms.Label();
            this.numericUpDownRetryDelay = new System.Windows.Forms.NumericUpDown();
            this.lblMaxRecords = new System.Windows.Forms.Label();
            this.chkLBDebugMode = new System.Windows.Forms.CheckedListBox();
            this.chkGetOnly = new System.Windows.Forms.CheckBox();
            this.chkTestMode = new System.Windows.Forms.CheckBox();
            this.numericUpDownMaxRecords = new System.Windows.Forms.NumericUpDown();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRepeatCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRetryMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRetryDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxRecords)).BeginInit();
            this.SuspendLayout();
            // 
            // numericUpDownRepeatCount
            // 
            this.numericUpDownRepeatCount.Enabled = false;
            this.numericUpDownRepeatCount.Location = new System.Drawing.Point(340, 25);
            this.numericUpDownRepeatCount.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownRepeatCount.Name = "numericUpDownRepeatCount";
            this.numericUpDownRepeatCount.Size = new System.Drawing.Size(50, 20);
            this.numericUpDownRepeatCount.TabIndex = 20;
            // 
            // chkRepeatBatch
            // 
            this.chkRepeatBatch.AutoSize = true;
            this.chkRepeatBatch.Enabled = false;
            this.chkRepeatBatch.Location = new System.Drawing.Point(241, 26);
            this.chkRepeatBatch.Name = "chkRepeatBatch";
            this.chkRepeatBatch.Size = new System.Drawing.Size(93, 17);
            this.chkRepeatBatch.TabIndex = 19;
            this.chkRepeatBatch.Text = "Repeat Import";
            this.chkRepeatBatch.UseVisualStyleBackColor = true;
            // 
            // lblRESTRetries
            // 
            this.lblRESTRetries.AutoSize = true;
            this.lblRESTRetries.Location = new System.Drawing.Point(396, 26);
            this.lblRESTRetries.Name = "lblRESTRetries";
            this.lblRESTRetries.Size = new System.Drawing.Size(112, 13);
            this.lblRESTRetries.TabIndex = 21;
            this.lblRESTRetries.Text = "Max REST call retries:";
            this.toolTip1.SetToolTip(this.lblRESTRetries, "How many times to retry a failed Graph API call due to a timeout");
            // 
            // numericUpDownRetryMax
            // 
            this.numericUpDownRetryMax.Enabled = false;
            this.numericUpDownRetryMax.Location = new System.Drawing.Point(518, 24);
            this.numericUpDownRetryMax.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownRetryMax.Name = "numericUpDownRetryMax";
            this.numericUpDownRetryMax.Size = new System.Drawing.Size(50, 20);
            this.numericUpDownRetryMax.TabIndex = 22;
            this.numericUpDownRetryMax.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // lblRetryDelay
            // 
            this.lblRetryDelay.AutoSize = true;
            this.lblRetryDelay.Location = new System.Drawing.Point(396, 3);
            this.lblRetryDelay.Name = "lblRetryDelay";
            this.lblRetryDelay.Size = new System.Drawing.Size(116, 13);
            this.lblRetryDelay.TabIndex = 16;
            this.lblRetryDelay.Text = "REST retry delay (sec):";
            this.toolTip1.SetToolTip(this.lblRetryDelay, "Amount of time to wait if there is a timeout with Graph API calls");
            // 
            // numericUpDownRetryDelay
            // 
            this.numericUpDownRetryDelay.Enabled = false;
            this.numericUpDownRetryDelay.Location = new System.Drawing.Point(518, 1);
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
            this.numericUpDownRetryDelay.Size = new System.Drawing.Size(50, 20);
            this.numericUpDownRetryDelay.TabIndex = 17;
            this.numericUpDownRetryDelay.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // lblMaxRecords
            // 
            this.lblMaxRecords.AutoSize = true;
            this.lblMaxRecords.Location = new System.Drawing.Point(238, 4);
            this.lblMaxRecords.Name = "lblMaxRecords";
            this.lblMaxRecords.Size = new System.Drawing.Size(68, 13);
            this.lblMaxRecords.TabIndex = 14;
            this.lblMaxRecords.Text = "Max records:";
            this.toolTip1.SetToolTip(this.lblMaxRecords, "The maximum number of records to create");
            // 
            // chkLBDebugMode
            // 
            this.chkLBDebugMode.Enabled = false;
            this.chkLBDebugMode.FormattingEnabled = true;
            this.chkLBDebugMode.Items.AddRange(new object[] {
            "Accounts",
            "Contacts",
            "Opportunities",
            "Deal Stages",
            "Account Activities",
            "Contact Activities",
            "Opportunity Activities"});
            this.chkLBDebugMode.Location = new System.Drawing.Point(85, 3);
            this.chkLBDebugMode.Name = "chkLBDebugMode";
            this.chkLBDebugMode.Size = new System.Drawing.Size(138, 49);
            this.chkLBDebugMode.TabIndex = 13;
            // 
            // chkGetOnly
            // 
            this.chkGetOnly.AutoSize = true;
            this.chkGetOnly.Enabled = false;
            this.chkGetOnly.Location = new System.Drawing.Point(2, 26);
            this.chkGetOnly.Name = "chkGetOnly";
            this.chkGetOnly.Size = new System.Drawing.Size(65, 17);
            this.chkGetOnly.TabIndex = 18;
            this.chkGetOnly.Text = "Get only";
            this.toolTip1.SetToolTip(this.chkGetOnly, "Will only get and log BCM records and not import any records to OCM");
            this.chkGetOnly.UseVisualStyleBackColor = true;
            // 
            // chkTestMode
            // 
            this.chkTestMode.AutoSize = true;
            this.chkTestMode.Location = new System.Drawing.Point(2, 3);
            this.chkTestMode.Name = "chkTestMode";
            this.chkTestMode.Size = new System.Drawing.Size(77, 17);
            this.chkTestMode.TabIndex = 12;
            this.chkTestMode.Text = "Test Mode";
            this.chkTestMode.UseVisualStyleBackColor = true;
            this.chkTestMode.CheckedChanged += new System.EventHandler(this.chkTestMode_CheckedChanged);
            // 
            // numericUpDownMaxRecords
            // 
            this.numericUpDownMaxRecords.Enabled = false;
            this.numericUpDownMaxRecords.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownMaxRecords.Location = new System.Drawing.Point(340, 0);
            this.numericUpDownMaxRecords.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownMaxRecords.Name = "numericUpDownMaxRecords";
            this.numericUpDownMaxRecords.Size = new System.Drawing.Size(50, 20);
            this.numericUpDownMaxRecords.TabIndex = 15;
            // 
            // Testing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.numericUpDownRepeatCount);
            this.Controls.Add(this.chkRepeatBatch);
            this.Controls.Add(this.lblRESTRetries);
            this.Controls.Add(this.numericUpDownRetryMax);
            this.Controls.Add(this.lblRetryDelay);
            this.Controls.Add(this.numericUpDownRetryDelay);
            this.Controls.Add(this.lblMaxRecords);
            this.Controls.Add(this.chkLBDebugMode);
            this.Controls.Add(this.chkGetOnly);
            this.Controls.Add(this.chkTestMode);
            this.Controls.Add(this.numericUpDownMaxRecords);
            this.Name = "Testing";
            this.Size = new System.Drawing.Size(576, 56);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRepeatCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRetryMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRetryDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxRecords)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.NumericUpDown numericUpDownRepeatCount;
        private System.Windows.Forms.CheckBox chkRepeatBatch;
        internal System.Windows.Forms.Label lblRESTRetries;
        internal System.Windows.Forms.NumericUpDown numericUpDownRetryMax;
        internal System.Windows.Forms.Label lblRetryDelay;
        internal System.Windows.Forms.NumericUpDown numericUpDownRetryDelay;
        internal System.Windows.Forms.Label lblMaxRecords;
        private System.Windows.Forms.CheckedListBox chkLBDebugMode;
        private System.Windows.Forms.CheckBox chkGetOnly;
        private System.Windows.Forms.CheckBox chkTestMode;
        internal System.Windows.Forms.NumericUpDown numericUpDownMaxRecords;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
