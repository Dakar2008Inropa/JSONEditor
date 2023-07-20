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
    public partial class SearchMainTreeForm : Form
    {
        public string SearchData { get; set; }
        public SearchMainTreeForm()
        {
            InitializeComponent();
        }

        private void SearchTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                SearchData = SearchTextbox.Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}