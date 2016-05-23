namespace JmsTranslator
{
    partial class TranslatorForm
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
            this.tbTranslator = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tbTranslator
            // 
            this.tbTranslator.Location = new System.Drawing.Point(13, 13);
            this.tbTranslator.Multiline = true;
            this.tbTranslator.Name = "tbTranslator";
            this.tbTranslator.ReadOnly = true;
            this.tbTranslator.Size = new System.Drawing.Size(259, 236);
            this.tbTranslator.TabIndex = 0;
            // 
            // TranslatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.tbTranslator);
            this.Name = "TranslatorForm";
            this.Text = "TranslatorForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbTranslator;
    }
}

