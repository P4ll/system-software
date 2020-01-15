namespace Plugin
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
            this.label1 = new System.Windows.Forms.Label();
            this.button = new System.Windows.Forms.Button();
            this.upperTextBox = new System.Windows.Forms.TextBox();
            this.lowerTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Введите возраст";
            // 
            // button
            // 
            this.button.Location = new System.Drawing.Point(12, 77);
            this.button.Name = "button";
            this.button.Size = new System.Drawing.Size(93, 23);
            this.button.TabIndex = 2;
            this.button.Text = "Exec";
            this.button.UseVisualStyleBackColor = true;
            this.button.Click += new System.EventHandler(this.button_Click);
            // 
            // upperTextBox
            // 
            this.upperTextBox.Location = new System.Drawing.Point(12, 25);
            this.upperTextBox.Name = "upperTextBox";
            this.upperTextBox.Size = new System.Drawing.Size(93, 20);
            this.upperTextBox.TabIndex = 3;
            // 
            // lowerTextBox
            // 
            this.lowerTextBox.Location = new System.Drawing.Point(12, 51);
            this.lowerTextBox.Name = "lowerTextBox";
            this.lowerTextBox.Size = new System.Drawing.Size(93, 20);
            this.lowerTextBox.TabIndex = 4;
            // 
            // RegExpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(159, 132);
            this.Controls.Add(this.lowerTextBox);
            this.Controls.Add(this.upperTextBox);
            this.Controls.Add(this.button);
            this.Controls.Add(this.label1);
            this.Name = "RegExpForm";
            this.Text = "RegExpForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button;
        private System.Windows.Forms.TextBox upperTextBox;
        private System.Windows.Forms.TextBox lowerTextBox;
    }
}