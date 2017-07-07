namespace BCM_Migration_Tool.Controls
{
    partial class NavigationBar
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
            this.lblConnect = new System.Windows.Forms.Label();
            this.lblMap = new System.Windows.Forms.Label();
            this.lblMigrate = new System.Windows.Forms.Label();
            this.panelConnect = new System.Windows.Forms.Panel();
            this.panelMap = new System.Windows.Forms.Panel();
            this.panelMigrate = new System.Windows.Forms.Panel();
            this.pictureBoxGear = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGear)).BeginInit();
            this.SuspendLayout();
            // 
            // lblConnect
            // 
            this.lblConnect.AutoSize = true;
            this.lblConnect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblConnect.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(117)))), ((int)(((byte)(182)))));
            this.lblConnect.Location = new System.Drawing.Point(4, 6);
            this.lblConnect.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblConnect.Name = "lblConnect";
            this.lblConnect.Size = new System.Drawing.Size(67, 21);
            this.lblConnect.TabIndex = 0;
            this.lblConnect.Text = "Connect";
            this.lblConnect.Click += new System.EventHandler(this.lblConnect_Click);
            // 
            // lblMap
            // 
            this.lblMap.AutoSize = true;
            this.lblMap.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblMap.Enabled = false;
            this.lblMap.Location = new System.Drawing.Point(142, 6);
            this.lblMap.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMap.Name = "lblMap";
            this.lblMap.Size = new System.Drawing.Size(41, 21);
            this.lblMap.TabIndex = 1;
            this.lblMap.Text = "Map";
            this.lblMap.Click += new System.EventHandler(this.lblMap_Click);
            // 
            // lblMigrate
            // 
            this.lblMigrate.AutoSize = true;
            this.lblMigrate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblMigrate.Enabled = false;
            this.lblMigrate.Location = new System.Drawing.Point(261, 6);
            this.lblMigrate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMigrate.Name = "lblMigrate";
            this.lblMigrate.Size = new System.Drawing.Size(64, 21);
            this.lblMigrate.TabIndex = 2;
            this.lblMigrate.Text = "Migrate";
            this.lblMigrate.Click += new System.EventHandler(this.lblMigrate_Click);
            // 
            // panelConnect
            // 
            this.panelConnect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(117)))), ((int)(((byte)(182)))));
            this.panelConnect.Location = new System.Drawing.Point(6, 32);
            this.panelConnect.Name = "panelConnect";
            this.panelConnect.Size = new System.Drawing.Size(64, 5);
            this.panelConnect.TabIndex = 3;
            // 
            // panelMap
            // 
            this.panelMap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(117)))), ((int)(((byte)(182)))));
            this.panelMap.Location = new System.Drawing.Point(146, 32);
            this.panelMap.Name = "panelMap";
            this.panelMap.Size = new System.Drawing.Size(40, 5);
            this.panelMap.TabIndex = 4;
            this.panelMap.Visible = false;
            // 
            // panelMigrate
            // 
            this.panelMigrate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(117)))), ((int)(((byte)(182)))));
            this.panelMigrate.Location = new System.Drawing.Point(265, 32);
            this.panelMigrate.Name = "panelMigrate";
            this.panelMigrate.Size = new System.Drawing.Size(60, 5);
            this.panelMigrate.TabIndex = 5;
            this.panelMigrate.Visible = false;
            // 
            // pictureBoxGear
            // 
            this.pictureBoxGear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxGear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxGear.Image = global::BCM_Migration_Tool.Properties.Resources.Settings_Gray;
            this.pictureBoxGear.Location = new System.Drawing.Point(430, 5);
            this.pictureBoxGear.Name = "pictureBoxGear";
            this.pictureBoxGear.Size = new System.Drawing.Size(32, 32);
            this.pictureBoxGear.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxGear.TabIndex = 6;
            this.pictureBoxGear.TabStop = false;
            this.pictureBoxGear.Click += new System.EventHandler(this.pictureBoxGear_Click);
            // 
            // NavigationBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pictureBoxGear);
            this.Controls.Add(this.panelMigrate);
            this.Controls.Add(this.panelMap);
            this.Controls.Add(this.panelConnect);
            this.Controls.Add(this.lblMigrate);
            this.Controls.Add(this.lblMap);
            this.Controls.Add(this.lblConnect);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "NavigationBar";
            this.Size = new System.Drawing.Size(465, 42);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGear)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblConnect;
        private System.Windows.Forms.Label lblMap;
        private System.Windows.Forms.Label lblMigrate;
        private System.Windows.Forms.Panel panelConnect;
        private System.Windows.Forms.Panel panelMap;
        private System.Windows.Forms.Panel panelMigrate;
        private System.Windows.Forms.PictureBox pictureBoxGear;
    }
}
