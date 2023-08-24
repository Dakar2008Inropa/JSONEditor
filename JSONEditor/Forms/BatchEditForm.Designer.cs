namespace JSONEditor.Forms
{
    partial class BatchEditForm
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
            SearchLabel = new Label();
            PropertyToFindTextbox = new TextBox();
            PropertyMatchLabel = new Label();
            PropertyMatchCombo = new ComboBox();
            WhatToDoLabel = new Label();
            WhatToDoCombo = new ComboBox();
            FactorLabe = new Label();
            FactorTextbox = new TextBox();
            ExecuteBtn = new Button();
            SuspendLayout();
            // 
            // SearchLabel
            // 
            SearchLabel.Location = new Point(5, 5);
            SearchLabel.Margin = new Padding(0, 0, 5, 5);
            SearchLabel.Name = "SearchLabel";
            SearchLabel.Size = new Size(402, 27);
            SearchLabel.TabIndex = 0;
            SearchLabel.Text = "Property To Find";
            SearchLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // PropertyToFindTextbox
            // 
            PropertyToFindTextbox.BorderStyle = BorderStyle.FixedSingle;
            PropertyToFindTextbox.Location = new Point(5, 37);
            PropertyToFindTextbox.Margin = new Padding(0, 0, 5, 5);
            PropertyToFindTextbox.Name = "PropertyToFindTextbox";
            PropertyToFindTextbox.Size = new Size(402, 23);
            PropertyToFindTextbox.TabIndex = 1;
            PropertyToFindTextbox.TextAlign = HorizontalAlignment.Center;
            // 
            // PropertyMatchLabel
            // 
            PropertyMatchLabel.Location = new Point(412, 5);
            PropertyMatchLabel.Margin = new Padding(0, 0, 0, 5);
            PropertyMatchLabel.Name = "PropertyMatchLabel";
            PropertyMatchLabel.Size = new Size(216, 27);
            PropertyMatchLabel.TabIndex = 2;
            PropertyMatchLabel.Text = "Match";
            PropertyMatchLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // PropertyMatchCombo
            // 
            PropertyMatchCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            PropertyMatchCombo.FlatStyle = FlatStyle.Flat;
            PropertyMatchCombo.FormattingEnabled = true;
            PropertyMatchCombo.Location = new Point(412, 37);
            PropertyMatchCombo.Margin = new Padding(0, 0, 0, 5);
            PropertyMatchCombo.Name = "PropertyMatchCombo";
            PropertyMatchCombo.Size = new Size(216, 24);
            PropertyMatchCombo.TabIndex = 3;
            // 
            // WhatToDoLabel
            // 
            WhatToDoLabel.Location = new Point(5, 66);
            WhatToDoLabel.Margin = new Padding(0, 0, 5, 5);
            WhatToDoLabel.Name = "WhatToDoLabel";
            WhatToDoLabel.Size = new Size(402, 26);
            WhatToDoLabel.TabIndex = 4;
            WhatToDoLabel.Text = "What To Do With The Value";
            WhatToDoLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // WhatToDoCombo
            // 
            WhatToDoCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            WhatToDoCombo.FlatStyle = FlatStyle.Flat;
            WhatToDoCombo.FormattingEnabled = true;
            WhatToDoCombo.Location = new Point(5, 97);
            WhatToDoCombo.Margin = new Padding(0, 0, 5, 10);
            WhatToDoCombo.Name = "WhatToDoCombo";
            WhatToDoCombo.Size = new Size(402, 24);
            WhatToDoCombo.TabIndex = 5;
            // 
            // FactorLabe
            // 
            FactorLabe.Location = new Point(412, 66);
            FactorLabe.Margin = new Padding(0, 0, 0, 5);
            FactorLabe.Name = "FactorLabe";
            FactorLabe.Size = new Size(216, 26);
            FactorLabe.TabIndex = 6;
            FactorLabe.Text = "Factor";
            FactorLabe.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // FactorTextbox
            // 
            FactorTextbox.BorderStyle = BorderStyle.FixedSingle;
            FactorTextbox.Location = new Point(412, 98);
            FactorTextbox.Margin = new Padding(0, 0, 0, 10);
            FactorTextbox.Name = "FactorTextbox";
            FactorTextbox.Size = new Size(216, 23);
            FactorTextbox.TabIndex = 7;
            FactorTextbox.TextAlign = HorizontalAlignment.Center;
            FactorTextbox.KeyPress += FactorTextbox_KeyPress;
            // 
            // ExecuteBtn
            // 
            ExecuteBtn.BackColor = Color.LightGray;
            ExecuteBtn.FlatStyle = FlatStyle.Flat;
            ExecuteBtn.Location = new Point(5, 131);
            ExecuteBtn.Margin = new Padding(0);
            ExecuteBtn.Name = "ExecuteBtn";
            ExecuteBtn.Size = new Size(623, 39);
            ExecuteBtn.TabIndex = 8;
            ExecuteBtn.Text = "Execute";
            ExecuteBtn.UseVisualStyleBackColor = false;
            ExecuteBtn.Click += ExecuteBtn_Click;
            // 
            // BatchEditForm
            // 
            AutoScaleDimensions = new SizeF(8F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DarkGray;
            ClientSize = new Size(633, 176);
            Controls.Add(ExecuteBtn);
            Controls.Add(FactorTextbox);
            Controls.Add(FactorLabe);
            Controls.Add(WhatToDoCombo);
            Controls.Add(WhatToDoLabel);
            Controls.Add(PropertyMatchCombo);
            Controls.Add(PropertyMatchLabel);
            Controls.Add(PropertyToFindTextbox);
            Controls.Add(SearchLabel);
            Font = new Font("Verdana", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "BatchEditForm";
            Padding = new Padding(5);
            StartPosition = FormStartPosition.CenterParent;
            Text = "Batch Edit";
            Load += BatchEditForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label SearchLabel;
        private TextBox PropertyToFindTextbox;
        private Label PropertyMatchLabel;
        private ComboBox PropertyMatchCombo;
        private Label WhatToDoLabel;
        private ComboBox WhatToDoCombo;
        private Label FactorLabe;
        private TextBox FactorTextbox;
        private Button ExecuteBtn;
    }
}