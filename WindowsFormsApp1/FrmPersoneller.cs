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
    public partial class FrmPersoneller : Form
    {
        public FrmPersoneller()
        {
            InitializeComponent();
        }

        sqlbaglantisi bgl = new sqlbaglantisi();

        void listele()
        {
            SqlDataAdapter dr = new SqlDataAdapter("Select * from TBL_PERSONELLER",bgl.baglanti());
            DataTable dt = new DataTable();
            dr.Fill(dt);
            gridControl1.DataSource = dt;
        }
        void iller()
        {
            SqlCommand komut = new SqlCommand("Select SEHIR from TBL_ILLER", bgl.baglanti());
            SqlDataReader dt = komut.ExecuteReader();
            while (dt.Read())
            {
                Cmbil.Properties.Items.Add(dt[0]);
            }

        }
        void temizle()
        {
            TxtID.Text = "";
            TxtAd.Text = "";
            TxtSoyad.Text = "";
            MskTel1.Text = "";
            MskTC.Text = "";
            TxtMail.Text = "";
            Cmbil.Text = "";
            Cmbilce.Text = "";
            TxtGorev.Text = "";
            RchAdres.Text = "";
        }

        private void FrmPersoneller_Load(object sender, EventArgs e)
        {
            listele();
            iller();
            temizle();
            gridView1.OptionsBehavior.Editable = false;
        }

        private void Cmbil_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cmbilce.Properties.Items.Clear();
            Cmbilce.Text = "";
            SqlCommand komut = new SqlCommand("Select ILCE from TBL_ILCELER where SEHIR=@P1", bgl.baglanti());
            komut.Parameters.AddWithValue("@P1", Cmbil.SelectedIndex + 1);
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                Cmbilce.Properties.Items.Add(dr[0]);
            }
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(TxtAd.Text) || string.IsNullOrWhiteSpace(TxtSoyad.Text) || string.IsNullOrWhiteSpace(MskTel1.Text) || string.IsNullOrWhiteSpace(MskTC.Text) ||
                string.IsNullOrWhiteSpace(TxtGorev.Text) ||
                string.IsNullOrWhiteSpace(TxtMail.Text) || string.IsNullOrWhiteSpace(Cmbil.Text) || string.IsNullOrWhiteSpace(Cmbilce.Text) || string.IsNullOrWhiteSpace(RchAdres.Text))
            {
                MessageBox.Show("Lütfen alanları doldorunuz", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (TxtAd.Text.Length > 20 || TxtSoyad.Text.Length > 20 || TxtMail.Text.Length > 40 || TxtGorev.Text.Length > 20
                || RchAdres.Text.Length > 100)
            {
                MessageBox.Show("Lütfen alanları doğru şekilde kullanınız", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                temizle();
                return;
            }
            if (MessageBox.Show("Firma sisteme kaydedilsin mi?", "Bilgi", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                SqlCommand komut = new SqlCommand("insert into TBL_PERSONELLER (AD,SOYAD,TELEFON,TC,MAIL,IL,ILCE,ADRES,GOREV) values (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9) ",bgl.baglanti());
                komut.Parameters.AddWithValue("@p1",TxtAd.Text);
                komut.Parameters.AddWithValue("@p2", TxtSoyad.Text);
                komut.Parameters.AddWithValue("@p3", MskTel1.Text);
                komut.Parameters.AddWithValue("@p4", MskTC.Text);
                komut.Parameters.AddWithValue("@p5", TxtMail.Text);
                komut.Parameters.AddWithValue("@p6", Cmbil.Text);
                komut.Parameters.AddWithValue("@p7", Cmbilce.Text);
                komut.Parameters.AddWithValue("@p8", RchAdres.Text);
                komut.Parameters.AddWithValue("@p9", TxtGorev.Text);
                komut.ExecuteNonQuery();
                bgl.baglanti().Close();
                listele();
                MessageBox.Show("Sisteme kayıt başarıyla yapıldı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                temizle();
            }
            else
            {
                MessageBox.Show("Sisteme kayıt yapılmadı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                temizle();
            }

        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (dr != null)
            {
                TxtID.Text = dr["ID"].ToString();
                TxtAd.Text = dr["AD"].ToString();
                TxtSoyad.Text = dr["SOYAD"].ToString();
                MskTel1.Text = dr["TELEFON"].ToString();
                MskTC.Text = dr["TC"].ToString();
                TxtMail.Text = dr["MAIL"].ToString();
                Cmbil.Text = dr["IL"].ToString();
                Cmbilce.Text = dr["ILCE"].ToString();
                RchAdres.Text = dr["ADRES"].ToString();
                TxtGorev.Text = dr["GOREV"].ToString();

            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            temizle();
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Firmayı silmek istediğinizden emin misiniz?", "Bilgi", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                SqlCommand cmd1 = new SqlCommand("Delete from TBL_FIRMALAR where ID=@k1", bgl.baglanti());
                cmd1.Parameters.AddWithValue("@k1", TxtID.Text);
                cmd1.ExecuteNonQuery();
                bgl.baglanti().Close();
                listele();
                MessageBox.Show("Firma başarıyla silindi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                temizle();
            }
            else
            {
                MessageBox.Show("Firma silme işlemi iptal edildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Müşteri bilgilerini güncellemek istiyor musunuz?", "Bilgi", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                SqlCommand cmd1 = new SqlCommand("update TBL_PERSONELLER set AD=@P1,SOYAD=@P2,TELEFON=@P3,TC=@P4,MAIL=@P5,IL=@P6,ILCE=@P7,ADRES=@P8,GOREV=@P9 where ID=@P10", bgl.baglanti());
                cmd1.Parameters.AddWithValue("@P1",TxtAd.Text);
                cmd1.Parameters.AddWithValue("@P2", TxtSoyad.Text);
                cmd1.Parameters.AddWithValue("@P3", MskTel1.Text);
                cmd1.Parameters.AddWithValue("@P4", MskTC.Text);
                cmd1.Parameters.AddWithValue("@P5", TxtMail.Text);
                cmd1.Parameters.AddWithValue("@P6", Cmbil.Text);
                cmd1.Parameters.AddWithValue("@P7", Cmbilce.Text);
                cmd1.Parameters.AddWithValue("@P8", RchAdres.Text);
                cmd1.Parameters.AddWithValue("@P9", TxtGorev.Text);
                cmd1.Parameters.AddWithValue("@P10", TxtID.Text);
                cmd1.ExecuteNonQuery();
                bgl.baglanti().Close();
                listele();
                MessageBox.Show("Personel başarıyla güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Personel bilgi güncellenmesi iptal edildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                temizle();
            }

        }
    }
    }

