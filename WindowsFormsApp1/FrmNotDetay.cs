using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class FrmNotDetay : Form
    {
        public FrmNotDetay()
        {
            InitializeComponent();
        }
        public string notdetay;
        private void FrmNotDetay_Load(object sender, EventArgs e)
        {
            RchDetay.Text = notdetay;
        }
    }
}
