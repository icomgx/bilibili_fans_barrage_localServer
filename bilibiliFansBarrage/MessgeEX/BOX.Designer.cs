namespace bilibiliFansBarrage.MessageEX
{
    partial class BOX
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BOX));
            this.ami_Button_11 = new EASkins.Ami_Button_1();
            this.ami_RichTextBox1 = new EASkins.Ami_RichTextBox();
            this.SuspendLayout();
            // 
            // ami_Button_11
            // 
            this.ami_Button_11.BackColor = System.Drawing.Color.Transparent;
            this.ami_Button_11.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.ami_Button_11.Image = null;
            this.ami_Button_11.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ami_Button_11.Location = new System.Drawing.Point(63, 191);
            this.ami_Button_11.Name = "ami_Button_11";
            this.ami_Button_11.Size = new System.Drawing.Size(177, 30);
            this.ami_Button_11.TabIndex = 0;
            this.ami_Button_11.Text = "确定";
            this.ami_Button_11.TextAlignment = System.Drawing.StringAlignment.Center;
            this.ami_Button_11.Click += new System.EventHandler(this.ami_Button_11_Click);
            // 
            // ami_RichTextBox1
            // 
            this.ami_RichTextBox1.AutoWordSelection = false;
            this.ami_RichTextBox1.BackColor = System.Drawing.Color.Transparent;
            this.ami_RichTextBox1.Font = new System.Drawing.Font("Tahoma", 15F);
            this.ami_RichTextBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(76)))), ((int)(((byte)(76)))));
            this.ami_RichTextBox1.Location = new System.Drawing.Point(12, 73);
            this.ami_RichTextBox1.Name = "ami_RichTextBox1";
            this.ami_RichTextBox1.ReadOnly = true;
            this.ami_RichTextBox1.Size = new System.Drawing.Size(293, 112);
            this.ami_RichTextBox1.TabIndex = 1;
            this.ami_RichTextBox1.WordWrap = true;
            // 
            // BOX
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(317, 233);
            this.Controls.Add(this.ami_RichTextBox1);
            this.Controls.Add(this.ami_Button_11);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BOX";
            this.Sizable = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "提示";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private EASkins.Ami_Button_1 ami_Button_11;
        private EASkins.Ami_RichTextBox ami_RichTextBox1;
    }
}