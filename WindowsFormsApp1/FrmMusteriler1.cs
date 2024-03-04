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
    public partial class FrmMusteriler1 : Form
    {
        public FrmMusteriler1()
        {
            InitializeComponent();
        }

        sqlbaglantisi bgl = new sqlbaglantisi();

        void listele()
        {
            SqlDataAdapter dt = new SqlDataAdapter("Select * from TBL_MUSTERILER",bgl.baglanti());
            DataTable dr = new DataTable();
            dt.Fill(dr);
            gridControl1.DataSource = dr;
        }

        void iller()
        {
            
            SqlCommand komut = new SqlCommand("Select SEHIR from TBL_ILLER",bgl.baglanti());
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                Cmbil.Properties.Items.Add(dr[0]);
            }
            
        }
        void temizle()
        {
            TxtID.Text = "";
            TxtAd.Text = "";
            TxtSoyad.Text = "";
            MskTel1.Text = "";
            MskTel2.Text = "";
            MskTC.Text = "";
            TxtMail.Text = "";
            Cmbil.Text = "";
            Cmbilce.Text = "";
            RchAdres.Text = "";
        }

        private void FrmMusteriler1_Load(object sender, EventArgs e)
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
            
            SqlCommand komut1 = new SqlCommand("Select ILCE from TBL_ILCELER where SEHIR=@p1", bgl.baglanti());
            komut1.Parameters.AddWithValue("@p1", Cmbil.SelectedIndex + 1);
            SqlDataReader da1 = komut1.ExecuteReader();
            while (da1.Read())
            {
                Cmbilce.Properties.Items.Add(da1[0]);
            }
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(TxtAd.Text) || string.IsNullOrWhiteSpace(TxtSoyad.Text) || string.IsNullOrWhiteSpace(MskTel1.Text) || string.IsNullOrWhiteSpace(MskTel2.Text) ||
               string.IsNullOrWhiteSpace(MskTC.Text) || string.IsNullOrWhiteSpace(TxtMail.Text) || string.IsNullOrWhiteSpace(Cmbil.Text) || string.IsNullOrWhiteSpace(Cmbilce.Text) || string.IsNullOrWhiteSpace(RchAdres.Text))
            {
                MessageBox.Show("Lütfen alanları doldorunuz", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(TxtAd.Text.Length > 20 || TxtSoyad.Text.Length > 20 || TxtMail.Text.Length > 40 || Cmbil.Text.Length > 13 || Cmbilce.Text.Length > 15 || RchAdres.Text.Length > 100)
            {
                MessageBox.Show("Lütfen alanları doğru şekilde kullanınız", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                temizle();
                return;
            }
            if(MessageBox.Show("Sisteme kaydedilsin mi?", "Bilgi", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                SqlCommand komut1 = new SqlCommand("insert into TBL_MUSTERILER (AD,SOYAD,TELEFON,TELEFON2,TC,MAIL,IL,ILCE,ADRES) values " +
                    "(@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9)", bgl.baglanti());
                komut1.Parameters.AddWithValue("@p1", TxtAd.Text);
                komut1.Parameters.AddWithValue("@p2", TxtSoyad.Text);
                komut1.Parameters.AddWithValue("@p3", MskTel1.Text);
                komut1.Parameters.AddWithValue("@p4", MskTel2.Text);
                komut1.Parameters.AddWithValue("@p5", MskTC.Text);
                komut1.Parameters.AddWithValue("@p6", TxtMail.Text);
                komut1.Parameters.AddWithValue("@p7", Cmbil.Text);
                komut1.Parameters.AddWithValue("@p8", Cmbilce.Text);
                komut1.Parameters.AddWithValue("@p9", RchAdres.Text);
                komut1.ExecuteNonQuery();
                bgl.baglanti().Close();
                listele();
                MessageBox.Show("Sisteme kayıt başarıyla yapıldı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);            
                temizle();
            }
            else
            {
                MessageBox.Show("Sisteme kayıt yapılmadı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            temizle();
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
                MskTel2.Text = dr["TELEFON2"].ToString();
                MskTC.Text = dr["TC"].ToString();
                TxtMail.Text = dr["MAIL"].ToString();
                Cmbil.Text = dr["IL"].ToString();
                Cmbilce.Text = dr["ILCE"].ToString();
                RchAdres.Text = dr["ADRES"].ToString();
            }
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Müşteriyi silmek istediğinizden emin misiniz?","Bilgi",MessageBoxButtons.OKCancel,MessageBoxIcon.Information) == DialogResult.OK) 
            {
                SqlCommand komut2 = new SqlCommand("Delete from TBL_MUSTERILER where ID=@p1", bgl.baglanti());
                komut2.Parameters.AddWithValue("@p1",TxtID.Text);
                komut2.ExecuteNonQuery();
                bgl.baglanti().Close();
                listele();
                MessageBox.Show("Müşteri başarıyla silindi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Müşteri silme işlemi iptal edildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Müşteri bilgilerini güncellemek istiyor musunuz?", "Bilgi", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                SqlCommand guncelle = new SqlCommand("update TBL_MUSTERILER set AD=@P1,SOYAD=@P2,TELEFON=@P3,TELEFON2=@P4,TC=@P5,MAIL=@P6,IL=@P7,ILCE=@P8,ADRES=@P9 where ID=@P10",bgl.baglanti());
                guncelle.Parameters.AddWithValue("@P1", TxtAd.Text);
                guncelle.Parameters.AddWithValue("@P2", TxtSoyad.Text);
                guncelle.Parameters.AddWithValue("@P3", MskTel1.Text);
                guncelle.Parameters.AddWithValue("@P4", MskTel2.Text);
                guncelle.Parameters.AddWithValue("@P5", MskTC.Text);
                guncelle.Parameters.AddWithValue("@P6", TxtMail.Text);
                guncelle.Parameters.AddWithValue("@P7", Cmbil.Text);
                guncelle.Parameters.AddWithValue("@P8", Cmbilce.Text);
                guncelle.Parameters.AddWithValue("@P9", RchAdres.Text);
                guncelle.Parameters.AddWithValue("@P10",TxtID.Text);
                guncelle.ExecuteNonQuery();
                bgl.baglanti().Close();
                listele();
                MessageBox.Show("Müşteri başarıyla güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);               
            }
            else
            {
                MessageBox.Show("Müşteri bilgi güncellenmesi iptal edildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                temizle();
            }
        }
    }
}
