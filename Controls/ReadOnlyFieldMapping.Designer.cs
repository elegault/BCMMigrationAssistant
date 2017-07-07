namespace BCM_Migration_Tool.Controls
{
    partial class ReadOnlyFieldMapping
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblDestinationField = new System.Windows.Forms.Label();
            this.lblSourceField = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::BCM_Migration_Tool.Properties.Resources.rightbutton;
            this.pictureBox1.Location = new System.Drawing.Point(205, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(26, 29);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // lblDestinationField
            // 
            this.lblDestinationField.Location = new System.Drawing.Point(237, 7);
            this.lblDestinationField.Name = "lblDestinationField";
            this.lblDestinationField.Size = new System.Drawing.Size(195, 22);
            this.lblDestinationField.TabIndex = 5;
            // 
            // lblSourceField
            // 
            this.lblSourceField.Location = new System.Drawing.Point(3, 7);
            this.lblSourceField.Name = "lblSourceField";
            this.lblSourceField.Size = new System.Drawing.Size(195, 22);
            this.lblSourceField.TabIndex = 6;
            // 
            // ReadOnlyFieldMapping
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lblSourceField);
            this.Controls.Add(this.lblDestinationField);
            this.Controls.Add(this.pictureBox1);
            this.Name = "ReadOnlyFieldMapping";
            this.Size = new System.Drawing.Size(466, 31);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblDestinationField;
        private System.Windows.Forms.Label lblSourceField;
    }
}
