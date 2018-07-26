namespace BCM_Migration_Tool.Controls
{
    partial class Configure
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
            this.label11 = new System.Windows.Forms.Label();
            this.txtRedirectURI = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtClientID = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtDomain = new System.Windows.Forms.TextBox();
            this.cmdOpenDir = new System.Windows.Forms.Button();
            this.cmdDeleteDeals = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cboDeleteDealsOptions = new System.Windows.Forms.ComboBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.chkDealCreationDate = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkFullRESTLogging = new System.Windows.Forms.CheckBox();
            this.chkLogRecordNames = new System.Windows.Forms.CheckBox();
            this.chkEnableTestMode = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(5, 155);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(80, 17);
            this.label11.TabIndex = 9;
            this.label11.Text = "Redirect URI";
            // 
            // txtRedirectURI
            // 
            this.txtRedirectURI.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRedirectURI.Location = new System.Drawing.Point(8, 175);
            this.txtRedirectURI.Name = "txtRedirectURI";
            this.txtRedirectURI.Size = new System.Drawing.Size(357, 25);
            this.txtRedirectURI.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(4, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(210, 21);
            this.label3.TabIndex = 0;
            this.label3.Text = "Application Configuration";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(5, 55);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(89, 17);
            this.label10.TabIndex = 3;
            this.label10.Text = "Application ID";
            // 
            // txtClientID
            // 
            this.txtClientID.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtClientID.Location = new System.Drawing.Point(8, 75);
            this.txtClientID.Name = "txtClientID";
            this.txtClientID.Size = new System.Drawing.Size(358, 25);
            this.txtClientID.TabIndex = 5;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(5, 130);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(44, 17);
            this.label12.TabIndex = 7;
            this.label12.Text = "http://";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(5, 107);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(158, 17);
            this.label9.TabIndex = 6;
            this.label9.Text = "Office 365 Tenant Domain";
            // 
            // txtDomain
            // 
            this.txtDomain.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDomain.Location = new System.Drawing.Point(50, 127);
            this.txtDomain.Name = "txtDomain";
            this.txtDomain.Size = new System.Drawing.Size(316, 25);
            this.txtDomain.TabIndex = 8;
            // 
            // cmdOpenDir
            // 
            this.cmdOpenDir.Location = new System.Drawing.Point(10, 238);
            this.cmdOpenDir.Name = "cmdOpenDir";
            this.cmdOpenDir.Size = new System.Drawing.Size(232, 34);
            this.cmdOpenDir.TabIndex = 14;
            this.cmdOpenDir.Text = "Send Logs (the *.tx1 files)";
            this.cmdOpenDir.UseVisualStyleBackColor = true;
            this.cmdOpenDir.Click += new System.EventHandler(this.cmdOpenDir_Click);
            // 
            // cmdDeleteDeals
            // 
            this.cmdDeleteDeals.Enabled = false;
            this.cmdDeleteDeals.Location = new System.Drawing.Point(10, 278);
            this.cmdDeleteDeals.Name = "cmdDeleteDeals";
            this.cmdDeleteDeals.Size = new System.Drawing.Size(232, 34);
            this.cmdDeleteDeals.TabIndex = 17;
            this.cmdDeleteDeals.Text = "Delete Deals";
            this.cmdDeleteDeals.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 207);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 21);
            this.label1.TabIndex = 13;
            this.label1.Text = "Utilities";
            // 
            // cboDeleteDealsOptions
            // 
            this.cboDeleteDealsOptions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDeleteDealsOptions.FormattingEnabled = true;
            this.cboDeleteDealsOptions.Items.AddRange(new object[] {
            "All",
            "Private",
            "Shared"});
            this.cboDeleteDealsOptions.Location = new System.Drawing.Point(248, 281);
            this.cboDeleteDealsOptions.Name = "cboDeleteDealsOptions";
            this.cboDeleteDealsOptions.Size = new System.Drawing.Size(121, 29);
            this.cboDeleteDealsOptions.TabIndex = 18;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker1.Location = new System.Drawing.Point(501, 275);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(129, 29);
            this.dateTimePicker1.TabIndex = 16;
            // 
            // chkDealCreationDate
            // 
            this.chkDealCreationDate.AutoSize = true;
            this.chkDealCreationDate.Location = new System.Drawing.Point(384, 278);
            this.chkDealCreationDate.Name = "chkDealCreationDate";
            this.chkDealCreationDate.Size = new System.Drawing.Size(111, 25);
            this.chkDealCreationDate.TabIndex = 15;
            this.chkDealCreationDate.Text = "Created On:";
            this.chkDealCreationDate.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(399, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 21);
            this.label2.TabIndex = 4;
            this.label2.Text = "Logging Options";
            // 
            // chkFullRESTLogging
            // 
            this.chkFullRESTLogging.AutoSize = true;
            this.chkFullRESTLogging.Checked = true;
            this.chkFullRESTLogging.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFullRESTLogging.Location = new System.Drawing.Point(403, 179);
            this.chkFullRESTLogging.Name = "chkFullRESTLogging";
            this.chkFullRESTLogging.Size = new System.Drawing.Size(150, 25);
            this.chkFullRESTLogging.TabIndex = 12;
            this.chkFullRESTLogging.Text = "Full REST logging";
            this.chkFullRESTLogging.UseVisualStyleBackColor = true;
            // 
            // chkLogRecordNames
            // 
            this.chkLogRecordNames.Checked = true;
            this.chkLogRecordNames.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLogRecordNames.Location = new System.Drawing.Point(405, 94);
            this.chkLogRecordNames.Name = "chkLogRecordNames";
            this.chkLogRecordNames.Size = new System.Drawing.Size(239, 79);
            this.chkLogRecordNames.TabIndex = 10;
            this.chkLogRecordNames.Text = "Log existing Company/Contact names to output window";
            this.chkLogRecordNames.UseVisualStyleBackColor = true;
            // 
            // chkEnableTestMode
            // 
            this.chkEnableTestMode.AutoSize = true;
            this.chkEnableTestMode.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkEnableTestMode.Location = new System.Drawing.Point(403, 13);
            this.chkEnableTestMode.Name = "chkEnableTestMode";
            this.chkEnableTestMode.Size = new System.Drawing.Size(163, 25);
            this.chkEnableTestMode.TabIndex = 1;
            this.chkEnableTestMode.Text = "Enable Test Mode";
            this.chkEnableTestMode.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(7, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(171, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "(Do not change unless directed)";
            // 
            // Configure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chkEnableTestMode);
            this.Controls.Add(this.chkLogRecordNames);
            this.Controls.Add(this.chkFullRESTLogging);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkDealCreationDate);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.cboDeleteDealsOptions);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdDeleteDeals);
            this.Controls.Add(this.cmdOpenDir);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtDomain);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtRedirectURI);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtClientID);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Configure";
            this.Size = new System.Drawing.Size(680, 330);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label11;
        internal System.Windows.Forms.TextBox txtRedirectURI;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label10;
        internal System.Windows.Forms.TextBox txtClientID;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label9;
        internal System.Windows.Forms.TextBox txtDomain;
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Button cmdDeleteDeals;
        internal System.Windows.Forms.ComboBox cboDeleteDealsOptions;
        internal System.Windows.Forms.Button cmdOpenDir;
        internal System.Windows.Forms.DateTimePicker dateTimePicker1;
        internal System.Windows.Forms.CheckBox chkDealCreationDate;
        private System.Windows.Forms.Label label2;
        internal System.Windows.Forms.CheckBox chkFullRESTLogging;
        internal System.Windows.Forms.CheckBox chkLogRecordNames;
        internal System.Windows.Forms.CheckBox chkEnableTestMode;
        private System.Windows.Forms.Label label4;
    }
}
