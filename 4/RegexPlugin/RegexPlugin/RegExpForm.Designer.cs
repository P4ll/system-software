namespace RegexPlugin
{
    partial class RegExpForm
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
            this.button = new System.Windows.Forms.Button();
            this.richTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // button
            // 
            this.button.Location = new System.Drawing.Point(12, 75);
            this.button.Name = "button";
            this.button.Size = new System.Drawing.Size(259, 28);
            this.button.TabIndex = 1;
            this.button.Text = "Exec";
            this.button.UseVisualStyleBackColor = true;
            this.button.Click += new System.EventHandler(this.button_Click);
            // 
            // richTextBox
            // 
            this.richTextBox.Location = new System.Drawing.Point(12, 12);
            this.richTextBox.Name = "richTextBox";
            this.richTextBox.Size = new System.Drawing.Size(259, 48);
            this.richTextBox.TabIndex = 2;
            this.richTextBox.Text = "";
            // 
            // RegExpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(281, 115);
            this.Controls.Add(this.richTextBox);
            this.Controls.Add(this.button);
            this.MaximumSize = new System.Drawing.Size(297, 154);
            this.MinimumSize = new System.Drawing.Size(297, 154);
            this.Name = "RegExpForm";
            this.Text = "RegExpForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.RegExpForm_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button;
        private System.Windows.Forms.RichTextBox richTextBox;
    }
}