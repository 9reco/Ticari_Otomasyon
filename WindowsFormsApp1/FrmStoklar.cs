using DevExpress.XtraBars;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WindowsFormsApp1
{
    public partial class FrmStoklar : Form
    {
        public FrmStoklar()
        {
            InitializeComponent();
        }
        sqlbaglantisi bgl = new sqlbaglantisi();
        private void FrmStoklar_Load(object sender, EventArgs e)
        {
            gridView1.OptionsBehavior.Editable = false;

            SqlDataAdapter da = new SqlDataAdapter("Select URUNAD,Sum(ADET) as 'Miktar' from TBL_URUNLER group by URUNAD",bgl.baglanti());
            DataTable dt = new DataTable();
            da.Fill(dt);
            gridControl1.DataSource = dt;

            //Chart1 Ürün-Miktar

            SqlCommand cmd = new SqlCommand("Select URUNAD,Sum(ADET) as 'Miktar' from TBL_URUNLER group by URUNAD",bgl.baglanti());
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                chartControl1.Series["Series 1"].Points.AddPoint(Convert.ToString(dr[0]), Convert.ToInt16(dr[1]));
            }
            bgl.baglanti().Close();

            //Firma-Şehir
            SqlCommand cmd1 = new SqlCommand("Select IL,Count(*) AS 'FİRMA' from TBL_FIRMALAR group by IL", bgl.baglanti());
            SqlDataReader dr1 = cmd1.ExecuteReader();
            while (dr1.Read())
            {
                chartControl3.Series["Series 1"].Points.AddPoint(Convert.ToString(dr1[0]), Convert.ToInt16(dr1[1]));
            }
            bgl.baglanti().Close();
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            FrmStokDetay fr = new FrmStokDetay();
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            if (dr != null )
            {
                fr.ad = dr["URUNAD"].ToString();
            }
            fr.Show();
        }
    }
}
