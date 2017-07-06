namespace BCM_Migration_Tool.Controls
{
    partial class Opportunities
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblNewFieldMapping = new System.Windows.Forms.Label();
            this.cmdNewMapping = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(4, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(497, 222);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "Opportunities";
            this.label1.Visible = false;
            // 
            // lblNewFieldMapping
            // 
            this.lblNewFieldMapping.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblNewFieldMapping.AutoSize = true;
            this.lblNewFieldMapping.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblNewFieldMapping.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNewFieldMapping.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(117)))), ((int)(((byte)(182)))));
            this.lblNewFieldMapping.Location = new System.Drawing.Point(43, 241);
            this.lblNewFieldMapping.Name = "lblNewFieldMapping";
            this.lblNewFieldMapping.Size = new System.Drawing.Size(145, 21);
            this.lblNewFieldMapping.TabIndex = 4;
            this.lblNewFieldMapping.Text = "New Field Mapping";
            // 
            // cmdNewMapping
            // 
            this.cmdNewMapping.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdNewMapping.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cmdNewMapping.FlatAppearance.BorderSize = 0;
            this.cmdNewMapping.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdNewMapping.Image = global::BCM_Migration_Tool.Properties.Resources.plusbutton;
            this.cmdNewMapping.Location = new System.Drawing.Point(4, 234);
            this.cmdNewMapping.Name = "cmdNewMapping";
            this.cmdNewMapping.Size = new System.Drawing.Size(35, 31);
            this.cmdNewMapping.TabIndex = 3;
            this.cmdNewMapping.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.cmdNewMapping.UseVisualStyleBackColor = true;
            this.cmdNewMapping.Click += new System.EventHandler(this.cmdNewMapping_Click);
            // 
            // Opportunities
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdNewMapping);
            this.Controls.Add(this.lblNewFieldMapping);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Opportunities";
            this.Size = new System.Drawing.Size(504, 282);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cmdNewMapping;
        private System.Windows.Forms.Label lblNewFieldMapping;
    }
}
