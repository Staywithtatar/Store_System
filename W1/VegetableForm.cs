using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace W1
{
    public partial class VegetableForm : Form
    {
        public VegetableForm()
        {
            InitializeComponent();
        }
        private void button_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if(btn.Name == "button1")
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else if(btn.Name == "button2")
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
    }
}
