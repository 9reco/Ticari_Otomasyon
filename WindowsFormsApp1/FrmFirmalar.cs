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
using DevExpress.CodeParser;

namespace WindowsFormsApp1
{
    public partial class FrmFirmalar : Form
    {
        public FrmFirmalar()
        {
            InitializeComponent();
        }

        sqlbaglantisi bgl = new sqlbaglantisi();

        void listele()
        {
            SqlDataAdapter dr = new SqlDataAdapter("Select * from TBL_FIRMALAR",bgl.baglanti());
            DataTable dt = new DataTable();
            dr.Fill(dt);
            gridControl1.DataSource = dt;
        }
        void temizle()
        {
            TxtID.Text = "";
            TxtAd.Text = "";
            TxtYetkili.Text = "";
            TxtYGorev.Text = "";
            MskTC.Text = "";
            TxtSektor.Text = "";
            MskTel1.Text = "";
            MskTel2.Text = "";
            MskTel3.Text = "";
            TxtMail.Text = "";
            MskFax.Text = "";
            Cmbil.Text = "";
            Cmbilce.Text = "";
            RchAdres.Text = "";
            TxtKod1.Text = "";
            TxtKod2.Text = "";
            TxtKod3.Text = "";
            TxtAd.Focus();
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
        void kodlar()
        {
            SqlCommand cmd = new SqlCommand("Select FIRMAKOD1 from TBL_KODLAR", bgl.baglanti());
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                RchKod1.Text = dr[0].ToString();
            }
        }

        private void FrmFirmalar_Load(object sender, EventArgs e)
        {
            gridView1.OptionsBehavior.Editable = false;
            listele();
            iller();
            temizle();
            kodlar();
            
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (dr != null)
            {
                TxtID.Text = dr["ID"].ToString();
                TxtAd.Text = dr["AD"].ToString();
                TxtYetkili.Text = dr["YETKILIADSOYAD"].ToString();
                TxtYGorev.Text = dr["YETKILISTATU"].ToString();
                MskTC.Text = dr["YETKILITC"].ToString();
                TxtSektor.Text = dr["SEKTOR"].ToString();
                MskTel1.Text = dr["TELEFON1"].ToString();
                MskTel2.Text = dr["TELEFON2"].ToString();
                MskTel3.Text = dr["TELEFON3"].ToString();
                TxtMail.Text = dr["MAIL"].ToString();
                MskFax.Text = dr["FAX"].ToString();
                Cmbil.Text = dr["IL"].ToString();
                Cmbilce.Text = dr["ILCE"].ToString();
                RchAdres.Text = dr["ADRES"].ToString();
                TxtKod1.Text = dr["OZELKOD1"].ToString();
                TxtKod2.Text = dr["OZELKOD2"].ToString();
                TxtKod3.Text = dr["OZELKOD3"].ToString();

            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            temizle();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtAd.Text) || string.IsNullOrWhiteSpace(TxtSektor.Text) || string.IsNullOrWhiteSpace(TxtYetkili.Text) || string.IsNullOrWhiteSpace(MskTC.Text) ||
                string.IsNullOrWhiteSpace(MskTel1.Text) || string.IsNullOrWhiteSpace(MskTel2.Text) || 
                string.IsNullOrWhiteSpace(TxtMail.Text) || string.IsNullOrWhiteSpace(Cmbil.Text) || string.IsNullOrWhiteSpace(Cmbilce.Text) || string.IsNullOrWhiteSpace(RchAdres.Text))
            {
                MessageBox.Show("Lütfen alanları doldorunuz", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(TxtAd.Text.Length > 30 || TxtYGorev.Text.Length > 30 || TxtYetkili.Text.Length > 30 || TxtSektor.Text.Length > 15 || TxtMail.Text.Length > 40
                || RchAdres.Text.Length > 100)
            {
                MessageBox.Show("Lütfen alanları doğru şekilde kullanınız", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                temizle();
                return;
            }
            if(TxtKod1.Text.Length > 10 || TxtKod2.Text.Length > 10 || TxtKod3.Text.Length > 10)
            {
                MessageBox.Show("Özel kod 10 karakteri aşmamalıdır!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (MessageBox.Show("Firma sisteme kaydedilsin mi?", "Bilgi", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                SqlCommand komut = new SqlCommand("insert into TBL_FIRMALAR (AD,YETKILISTATU,YETKILIADSOYAD,YETKILITC,SEKTOR,TELEFON1,TELEFON2,TELEFON3,MAIL,FAX,IL,ILCE,ADRES,OZELKOD1,OZELKOD2,OZELKOD3)" +
                    " values (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13,@p14,@p15,@p16)",bgl.baglanti());
                komut.Parameters.AddWithValue("@p1",TxtAd.Text);
                komut.Parameters.AddWithValue("@p2", TxtYGorev.Text);
                komut.Parameters.AddWithValue("@p3", TxtYetkili.Text);
                komut.Parameters.AddWithValue("@p4", MskTC.Text);
                komut.Parameters.AddWithValue("@p5", TxtSektor.Text);
                komut.Parameters.AddWithValue("@p6", MskTel1.Text);
                komut.Parameters.AddWithValue("@p7", MskTel2.Text);
                komut.Parameters.AddWithValue("@p8", MskTel3.Text);
                komut.Parameters.AddWithValue("@p9", TxtMail.Text);
                komut.Parameters.AddWithValue("@p10", MskFax.Text);
                komut.Parameters.AddWithValue("@p11", Cmbil.Text);
                komut.Parameters.AddWithValue("@p12", Cmbilce.Text);
                komut.Parameters.AddWithValue("@p13", RchAdres.Text);
                komut.Parameters.AddWithValue("@p14", TxtKod1.Text);
                komut.Parameters.AddWithValue("@p15", TxtKod2.Text);
                komut.Parameters.AddWithValue("@p16", TxtKod3.Text);
                komut.ExecuteNonQuery();
                bgl.baglanti().Close();
                listele();
                MessageBox.Show("Sisteme kayıt başarıyla yapıldı.","Bilgi",MessageBoxButtons.OK,MessageBoxIcon.Information);
                temizle();              
            }
            else
            {
                MessageBox.Show("Sisteme kayıt yapılmadı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                temizle();
            }
        }

        private void Cmbil_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cmbilce.Properties.Items.Clear();
            Cmbilce.Text = "";
            SqlCommand komut2 = new SqlCommand("Select ILCE from TBL_ILCELER where SEHIR=@P1", bgl.baglanti());
            komut2.Parameters.AddWithValue("@P1",Cmbil.SelectedIndex+1);
            SqlDataReader dr = komut2.ExecuteReader();
            while (dr.Read())
            {
                Cmbilce.Properties.Items.Add(dr[0]);
            }
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Firmayı silmek istediğinizden emin misiniz?", "Bilgi", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                SqlCommand cmd1 = new SqlCommand("Delete from TBL_FIRMALAR where ID=@k1",bgl.baglanti());
                cmd1.Parameters.AddWithValue("@k1", TxtID.Text);
                cmd1.ExecuteNonQuery();
                bgl.baglanti().Close();
                listele();
                MessageBox.Show("Firma başarıyla silindi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                SqlCommand komut3 = new SqlCommand("update TBL_FIRMALAR set AD=@P1,YETKILISTATU=@P2,YETKILIADSOYAD=@P3,YETKILITC=@P4,SEKTOR=@P5,TELEFON1=@P6,TELEFON2=@P7,TELEFON3=@P8,MAIL=@P9,FAX=@P10" +
                    ",IL=@P11,ILCE=@P12,ADRES=@P13,OZELKOD1=@P14,OZELKOD2=@P15,OZELKOD3=@P16 where ID=@P17", bgl.baglanti());
                komut3.Parameters.AddWithValue("@P1", TxtAd.Text);
                komut3.Parameters.AddWithValue("@P2", TxtYGorev.Text);
                komut3.Parameters.AddWithValue("@P3", TxtYetkili.Text);
                komut3.Parameters.AddWithValue("@P4", MskTC.Text);
                komut3.Parameters.AddWithValue("@P5", TxtSektor.Text);
                komut3.Parameters.AddWithValue("@P6", MskTel1.Text);
                komut3.Parameters.AddWithValue("@P7", MskTel2.Text);
                komut3.Parameters.AddWithValue("@P8", MskTel3.Text);
                komut3.Parameters.AddWithValue("@P9", TxtMail.Text);
                komut3.Parameters.AddWithValue("@P10", MskFax.Text);
                komut3.Parameters.AddWithValue("@P11", Cmbil.Text);
                komut3.Parameters.AddWithValue("@P12", Cmbilce.Text);
                komut3.Parameters.AddWithValue("@P13", RchAdres.Text);
                komut3.Parameters.AddWithValue("@P14", TxtKod1.Text);
                komut3.Parameters.AddWithValue("@P15", TxtKod2.Text);
                komut3.Parameters.AddWithValue("@P16", TxtKod3.Text);
                komut3.Parameters.AddWithValue("@P17", TxtID.Text);
                komut3.ExecuteNonQuery();
                bgl.baglanti().Close();
                listele();
                MessageBox.Show("Firma başarıyla güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Firma bilgi güncellenmesi iptal edildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                temizle();
            }
        }
    }
}
