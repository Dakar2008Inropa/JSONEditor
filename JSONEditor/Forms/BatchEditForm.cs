﻿using JSONEditor.Classes.Application;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JSONEditor.Forms
{
    public partial class BatchEditForm : Form
    {
        public Settings AppSettings { get; set; }

        private List<BatchComboClass> MatchComboList { get; set; }
        private List<BatchComboClass> WhatToDoList { get; set; }

        public string SearchData { get; set; }
        public int SearchMatchValue { get; set; }
        public int WhatToDoValue { get; set; }
        public int FactorValue { get; set; }

        private string SelectedNodeText { get; set; }
        public BatchEditForm(Settings appSettings, string selectedNodeText = null)
        {
            MatchComboList = new List<BatchComboClass>();
            WhatToDoList = new List<BatchComboClass>();
            AppSettings = appSettings;
            SelectedNodeText = selectedNodeText;
            if(AppSettings.BatchEdit == null)
            {
                AppSettings.BatchEdit = new BatchEdit();
            }
            InitializeComponent();
        }

        private void PopulateMatchCombo()
        {
            MatchComboList = new List<BatchComboClass>
            {
                new BatchComboClass { Name = "Contains", Value = 0 },
                new BatchComboClass { Name = "Starts With", Value = 1 },
                new BatchComboClass { Name = "Ends With", Value = 2 },
                new BatchComboClass { Name = "Equals", Value = 3 }
            };
            PropertyMatchCombo.DataSource = MatchComboList;
            PropertyMatchCombo.DisplayMember = "Name";
            PropertyMatchCombo.ValueMember = "Value";
            PropertyMatchCombo.SelectedIndex = AppSettings.BatchEdit.LastUsedMatch;
        }

        private void PopulateWhatToDoCombo()
        {
            WhatToDoList = new List<BatchComboClass>
            {
                new BatchComboClass { Name = "Add", Value = 0 },
                new BatchComboClass { Name = "Subtract", Value = 1 },
                new BatchComboClass { Name = "Multiply", Value = 2 },
                new BatchComboClass { Name = "Divide", Value = 3 }
            };
            WhatToDoCombo.DataSource = WhatToDoList;
            WhatToDoCombo.DisplayMember = "Name";
            WhatToDoCombo.ValueMember = "Value";
            WhatToDoCombo.SelectedIndex = AppSettings.BatchEdit.LastUsedWhatToDoValue;
        }

        private void FactorTextbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Only allow numbers
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void BatchEditForm_Load(object sender, EventArgs e)
        {
            PopulateMatchCombo();
            PopulateWhatToDoCombo();
            if (!string.IsNullOrEmpty(SelectedNodeText))
            {
                PropertyToFindTextbox.Text = SelectedNodeText;
            }
            FactorTextbox.Text = AppSettings.BatchEdit.LastUsedFactor.ToString();
        }

        private void ExecuteBtn_Click(object sender, EventArgs e)
        {
            SearchData = PropertyToFindTextbox.Text;
            SearchMatchValue = (int)PropertyMatchCombo.SelectedValue;
            WhatToDoValue = (int)WhatToDoCombo.SelectedValue;
            FactorValue = int.Parse(FactorTextbox.Text);

            AppSettings.BatchEdit.LastUsedMatch = SearchMatchValue;
            AppSettings.BatchEdit.LastUsedWhatToDoValue = WhatToDoValue;
            AppSettings.BatchEdit.LastUsedFactor = FactorValue;

            if (string.IsNullOrEmpty(SearchData))
            {
                MessageBox.Show("Please enter a search value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (FactorValue == 0)
            {
                MessageBox.Show("Please enter a factor value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DialogResult = DialogResult.OK;
        }
    }
}