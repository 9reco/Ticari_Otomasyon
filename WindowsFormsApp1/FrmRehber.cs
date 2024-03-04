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
    public partial class FrmRehber : Form
    {
        public FrmRehber()
        {
            InitializeComponent();
        }

        sqlbaglantisi bgl = new sqlbaglantisi();

        void musteriler()
        {
            SqlDataAdapter da = new SqlDataAdapter("Select AD,SOYAD,TELEFON,TELEFON2,MAIL from TBL_MUSTERILER",bgl.baglanti());
            DataTable dt = new DataTable();
            da.Fill(dt);
            gridControl1.DataSource = dt;
        }
        void firmalar()
        {
            SqlDataAdapter da1 = new SqlDataAdapter("Select AD 'Firma Ad',YETKILIADSOYAD 'Yetkili Ad Soyad',TELEFON1,TELEFON2,TELEFON3,MAIL,FAX from TBL_FIRMALAR",bgl.baglanti());
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);
            gridControl2.DataSource = dt1;
        }

        private void FrmRehber_Load(object sender, EventArgs e)
        {
            musteriler();
            firmalar();
            gridView1.OptionsBehavior.Editable = false;
            gridView2.OptionsBehavior.Editable = false;
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            FrmMail frm = new FrmMail();
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            if (dr != null )
            {
                frm.mail = dr["MAIL"].ToString();
            }
            frm.Show();
        }

        private void gridView2_DoubleClick(object sender, EventArgs e)
        {
            FrmMail fr1 = new FrmMail();
            DataRow dr1 = gridView2.GetDataRow(gridView2.FocusedRowHandle);

            if (dr1 != null )
            {
                fr1.mail = dr1["MAIL"].ToString();
            }
            fr1.Show();
        }
    }
}
