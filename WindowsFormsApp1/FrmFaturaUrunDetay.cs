using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsFormsApp1
{
    public partial class FrmFaturaUrunDetay : Form
    {
        public FrmFaturaUrunDetay()
        {
            InitializeComponent();
        }
        sqlbaglantisi bgl = new sqlbaglantisi();


        public string ıd;

        void listele()
        {
            SqlDataAdapter da = new SqlDataAdapter("Select * from TBL_FATURADETAY where FATURAID='"+ıd+"'",bgl.baglanti());
            DataTable dt = new DataTable();
            da.Fill(dt);
            gridControl1.DataSource = dt;
        }
        private void FrmFaturaUrunDetay_Load(object sender, EventArgs e)
        {
            listele();
            gridView1.OptionsBehavior.Editable = false;

        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            FrmFaturaUrunEkle fr1=new FrmFaturaUrunEkle();
            DataRow dr1 = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            if(dr1 != null )
            {
                fr1.urunıd = dr1["FATURAURUNID"].ToString();
            }
            fr1.Show();
        }
    }
}
