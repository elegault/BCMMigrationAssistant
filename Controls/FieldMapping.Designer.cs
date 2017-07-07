namespace BCM_Migration_Tool.Controls
{
    partial class FieldMapping
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
            this.cboSourceField = new System.Windows.Forms.ComboBox();
            this.cboDestinationField = new System.Windows.Forms.ComboBox();
            this.pictureBoxDelete = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.chkInclude = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDelete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // cboSourceField
            // 
            this.cboSourceField.BackColor = System.Drawing.SystemColors.Window;
            this.cboSourceField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSourceField.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cboSourceField.FormattingEnabled = true;
            this.cboSourceField.Location = new System.Drawing.Point(3, 5);
            this.cboSourceField.Name = "cboSourceField";
            this.cboSourceField.Size = new System.Drawing.Size(195, 21);
            this.cboSourceField.Sorted = true;
            this.cboSourceField.TabIndex = 0;
            this.cboSourceField.SelectedIndexChanged += new System.EventHandler(this.cboSourceField_SelectedIndexChanged);
            // 
            // cboDestinationField
            // 
            this.cboDestinationField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDestinationField.FormattingEnabled = true;
            this.cboDestinationField.Location = new System.Drawing.Point(237, 5);
            this.cboDestinationField.Name = "cboDestinationField";
            this.cboDestinationField.Size = new System.Drawing.Size(195, 21);
            this.cboDestinationField.Sorted = true;
            this.cboDestinationField.TabIndex = 1;
            this.cboDestinationField.SelectedIndexChanged += new System.EventHandler(this.cboDestinationField_SelectedIndexChanged);
            // 
            // pictureBoxDelete
            // 
            this.pictureBoxDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxDelete.Image = global::BCM_Migration_Tool.Properties.Resources.trash;
            this.pictureBoxDelete.Location = new System.Drawing.Point(438, 2);
            this.pictureBoxDelete.Name = "pictureBoxDelete";
            this.pictureBoxDelete.Size = new System.Drawing.Size(23, 26);
            this.pictureBoxDelete.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxDelete.TabIndex = 3;
            this.pictureBoxDelete.TabStop = false;
            this.pictureBoxDelete.Click += new System.EventHandler(this.pictureBoxDelete_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Image = global::BCM_Migration_Tool.Properties.Resources.rightbutton;
            this.pictureBox1.Location = new System.Drawing.Point(205, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(26, 29);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // chkInclude
            // 
            this.chkInclude.AutoSize = true;
            this.chkInclude.Location = new System.Drawing.Point(469, 9);
            this.chkInclude.Name = "chkInclude";
            this.chkInclude.Size = new System.Drawing.Size(15, 14);
            this.chkInclude.TabIndex = 2;
            this.chkInclude.UseVisualStyleBackColor = true;
            this.chkInclude.Visible = false;
            // 
            // FieldMapping
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.chkInclude);
            this.Controls.Add(this.pictureBoxDelete);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.cboDestinationField);
            this.Controls.Add(this.cboSourceField);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FieldMapping";
            this.Size = new System.Drawing.Size(493, 31);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDelete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboSourceField;
        private System.Windows.Forms.ComboBox cboDestinationField;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBoxDelete;
        private System.Windows.Forms.CheckBox chkInclude;
    }
}
