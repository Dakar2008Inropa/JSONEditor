namespace JSONEditor.Forms
{
    partial class SearchMainTreeForm
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
            SearchTextbox = new TextBox();
            SuspendLayout();
            // 
            // SearchTextbox
            // 
            SearchTextbox.BorderStyle = BorderStyle.FixedSingle;
            SearchTextbox.Dock = DockStyle.Fill;
            SearchTextbox.Location = new Point(0, 0);
            SearchTextbox.Margin = new Padding(0);
            SearchTextbox.Name = "SearchTextbox";
            SearchTextbox.Size = new Size(636, 23);
            SearchTextbox.TabIndex = 0;
            SearchTextbox.TextAlign = HorizontalAlignment.Center;
            // 
            // SearchMainTreeForm
            // 
            AutoScaleDimensions = new SizeF(8F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Gainsboro;
            ClientSize = new Size(636, 23);
            Controls.Add(SearchTextbox);
            Font = new Font("Verdana", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "SearchMainTreeForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Search";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox SearchTextbox;
    }
}