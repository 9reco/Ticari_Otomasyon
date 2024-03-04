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
    public partial class FrmAyarlar : Form
    {
        public FrmAyarlar()
        {
            InitializeComponent();
        }
        sqlbaglantisi bgl = new sqlbaglantisi();

        void listele()
        {
            SqlDataAdapter da = new SqlDataAdapter("Select * from TBL_ADMİN",bgl.baglanti());
            DataTable dt = new DataTable();
            da.Fill(dt);
            gridControl1.DataSource = dt;
        }
        void temizle()
        {
            TxtKullanıcı.Text=string.Empty;
            TxtSifre.Text=string.Empty;
        }
        private void FrmAyarlar_Load(object sender, EventArgs e)
        {
            listele();
            temizle();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(TxtKullanıcı.Text) || string.IsNullOrWhiteSpace(TxtSifre.Text))
            {
                MessageBox.Show("Lütfen alanları doldurunuz","Bilgi",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (Btnislem.Text == "Kaydet")
            {
                if (MessageBox.Show("Eklemek istediğinizden emin misiniz ?", "Bilgi", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    SqlCommand komut = new SqlCommand("insert into TBL_Admin values (@p1,@p2)", bgl.baglanti());
                    komut.Parameters.AddWithValue("@p1", TxtKullanıcı.Text);
                    komut.Parameters.AddWithValue("@p2", TxtSifre.Text);
                    komut.ExecuteNonQuery();
                    bgl.baglanti().Close();
                    listele();
                    MessageBox.Show("Sisteme kayıt başarıyla yapıldı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    temizle();
                    return;
                }
                else
                {
                    MessageBox.Show("Sisteme kayıt yapılmadı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    temizle();
                }
            }
            if (Btnislem.Text == "Güncelle")
            {
                SqlCommand komut2 = new SqlCommand("update TBL_Admin set Sifre=@p2 where Kullaniciad=@p1", bgl.baglanti());
                komut2.Parameters.AddWithValue("@p1", TxtKullanıcı.Text);
                komut2.Parameters.AddWithValue("@p2", TxtSifre.Text);
                komut2.ExecuteNonQuery();
                bgl.baglanti().Close();
                listele();
                MessageBox.Show("Güncelleme başarıyla yapıldı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                temizle();
                return;
            }
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (dr != null)
            {
                TxtKullanıcı.Text = dr["Kullaniciad"].ToString();
                TxtSifre.Text = dr["Sifre"].ToString();
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            temizle();
        }

        private void TxtKullanıcı_EditValueChanged(object sender, EventArgs e)
        {
            if(TxtKullanıcı.Text != "")
            {
                Btnislem.Text = "Güncelle";
                Btnislem.BackColor = Color.GreenYellow;
            }
            else
            {
                Btnislem.Text = "Ekle";
                Btnislem.BackColor = Color.MediumAquamarine;
            }
        }
    }
}
