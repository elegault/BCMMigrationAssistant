namespace BCM_Migration_Tool.Controls
{
    partial class MapNavigationBar
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
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblOpportunities = new System.Windows.Forms.Label();
            this.lblBusinessContacts = new System.Windows.Forms.Label();
            this.lblAccounts = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(117)))), ((int)(((byte)(182)))));
            this.panel3.Location = new System.Drawing.Point(344, 32);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(101, 5);
            this.panel3.TabIndex = 5;
            this.panel3.Visible = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(117)))), ((int)(((byte)(182)))));
            this.panel2.Location = new System.Drawing.Point(159, 32);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(126, 5);
            this.panel2.TabIndex = 4;
            this.panel2.Visible = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(117)))), ((int)(((byte)(182)))));
            this.panel1.Location = new System.Drawing.Point(6, 32);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(96, 5);
            this.panel1.TabIndex = 3;
            // 
            // lblOpportunities
            // 
            this.lblOpportunities.AutoSize = true;
            this.lblOpportunities.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblOpportunities.Location = new System.Drawing.Point(340, 6);
            this.lblOpportunities.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblOpportunities.Name = "lblOpportunities";
            this.lblOpportunities.Size = new System.Drawing.Size(106, 21);
            this.lblOpportunities.TabIndex = 2;
            this.lblOpportunities.Text = "Opportunities";
            this.lblOpportunities.Click += new System.EventHandler(this.lblOpportunities_Click);
            // 
            // lblBusinessContacts
            // 
            this.lblBusinessContacts.AutoSize = true;
            this.lblBusinessContacts.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblBusinessContacts.Location = new System.Drawing.Point(155, 6);
            this.lblBusinessContacts.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblBusinessContacts.Name = "lblBusinessContacts";
            this.lblBusinessContacts.Size = new System.Drawing.Size(134, 21);
            this.lblBusinessContacts.TabIndex = 1;
            this.lblBusinessContacts.Text = "Business Contacts";
            this.lblBusinessContacts.Click += new System.EventHandler(this.lblBusinessContacts_Click);
            // 
            // lblAccounts
            // 
            this.lblAccounts.AutoSize = true;
            this.lblAccounts.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblAccounts.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(117)))), ((int)(((byte)(182)))));
            this.lblAccounts.Location = new System.Drawing.Point(4, 6);
            this.lblAccounts.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblAccounts.Name = "lblAccounts";
            this.lblAccounts.Size = new System.Drawing.Size(73, 21);
            this.lblAccounts.TabIndex = 0;
            this.lblAccounts.Text = "Accounts";
            this.lblAccounts.Click += new System.EventHandler(this.lblAccounts_Click);
            // 
            // MapNavigationBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblOpportunities);
            this.Controls.Add(this.lblBusinessContacts);
            this.Controls.Add(this.lblAccounts);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MapNavigationBar";
            this.Size = new System.Drawing.Size(452, 42);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblOpportunities;
        private System.Windows.Forms.Label lblBusinessContacts;
        private System.Windows.Forms.Label lblAccounts;
    }
}
