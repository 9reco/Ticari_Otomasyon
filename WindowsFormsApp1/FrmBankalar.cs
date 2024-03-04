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
using DevExpress.Data.Linq.Helpers;

namespace WindowsFormsApp1
{
    public partial class FrmBankalar : Form
    {
        public FrmBankalar()
        {
            InitializeComponent();
        }

        sqlbaglantisi bgl = new sqlbaglantisi();

        void il()
        {
            SqlCommand komut = new SqlCommand("Select SEHIR from TBL_ILLER", bgl.baglanti());
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                Cmbil.Properties.Items.Add(dr[0]);
            }
        }
        void listele()
        {
            SqlDataAdapter dr = new SqlDataAdapter("execute Bankabilgi", bgl.baglanti());
            DataTable dt = new DataTable();
            dr.Fill(dt);
            gridControl1.DataSource = dt;
        }
        void temizle()
        {
            TxtID.Text = "";
            CmbBankaAd.Text = "";
            Cmbil.Text = "";
            Cmbilce.Text = "";
            TxtSube.Text = "";
            TxtIBAN.Text = "";
            MskHesapNo.Text = "";
            TxtYetkılı.Text = "";
            MskTelefon.Text = "";
            MskTarih.Text = "";
            TxtHesapTur.Text = "";
            lookUpEdit1.Text = "";

        }
        void firma()
        {
            SqlDataAdapter da3 = new SqlDataAdapter("Select ID,AD from TBL_FIRMALAR", bgl.baglanti());
            DataTable dt3 = new DataTable();
            da3.Fill(dt3);
            lookUpEdit1.Properties.ValueMember = "ID";
            lookUpEdit1.Properties.DisplayMember = "AD";
            lookUpEdit1.Properties.DataSource = dt3;
        }
        private void FrmBankalar_Load(object sender, EventArgs e)
        {
            il();
            listele();
            firma();
            temizle();
            gridView1.OptionsBehavior.Editable = false;
        }

        private void Cmbil_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cmbilce.Properties.Items.Clear();
            Cmbilce.Text = "";

            SqlCommand komut1 = new SqlCommand("Select ILCE from TBL_ILCELER where SEHIR=@p1",bgl.baglanti());
            komut1.Parameters.AddWithValue("@p1", Cmbil.SelectedIndex+1);
            SqlDataReader da1 = komut1.ExecuteReader();
            while (da1.Read())
            {
                Cmbilce.Properties.Items.Add(da1[0]);
            }
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CmbBankaAd.Text) || string.IsNullOrWhiteSpace(Cmbil.Text) || string.IsNullOrWhiteSpace(Cmbilce.Text) || string.IsNullOrWhiteSpace(TxtSube.Text) ||
               string.IsNullOrWhiteSpace(TxtIBAN.Text) ||
               string.IsNullOrWhiteSpace(MskHesapNo.Text) || string.IsNullOrWhiteSpace(TxtYetkılı.Text) || string.IsNullOrWhiteSpace(MskTelefon.Text) || string.IsNullOrWhiteSpace(MskTarih.Text) ||
               string.IsNullOrWhiteSpace(TxtHesapTur.Text) || string.IsNullOrWhiteSpace(lookUpEdit1.Text))
            {
                MessageBox.Show("Lütfen alanları doldorunuz", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (TxtSube.Text.Length > 30 || TxtIBAN.Text.Length > 30 || TxtYetkılı.Text.Length > 30 || TxtHesapTur.Text.Length > 20)
            {
                MessageBox.Show("Lütfen alanları doğru şekilde kullanınız", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                temizle();
                return;
            }
            if (MessageBox.Show("Sisteme kaydedilsin mi?", "Bilgi", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                SqlCommand komut = new SqlCommand("insert into TBL_BANKALAR (BANKAADI,IL,ILCE,SUBE,IBAN,HESAPNO,YETKILI,TELEFON,TARIH,HESAPTURU,FIRMAID) values " +
                    "(@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11)", bgl.baglanti());
                komut.Parameters.AddWithValue("@p1", CmbBankaAd.Text);
                komut.Parameters.AddWithValue("@p2", Cmbil.Text);
                komut.Parameters.AddWithValue("@p3", Cmbilce.Text);
                komut.Parameters.AddWithValue("@p4", TxtSube.Text);
                komut.Parameters.AddWithValue("@p5", TxtIBAN.Text);
                komut.Parameters.AddWithValue("@p6", MskHesapNo.Text);
                komut.Parameters.AddWithValue("@p7", TxtYetkılı.Text);
                komut.Parameters.AddWithValue("@p8", MskTelefon.Text);
                komut.Parameters.AddWithValue("@p9", MskTarih.Text);
                komut.Parameters.AddWithValue("@p10", TxtHesapTur.Text);
                komut.Parameters.AddWithValue("@p11", lookUpEdit1.EditValue);
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
                CmbBankaAd.Text = dr["Banka Adı"].ToString();
                Cmbil.Text = dr["İl"].ToString();
                Cmbilce.Text = dr["İlçe"].ToString();
                TxtSube.Text = dr["Şube"].ToString();
                TxtIBAN.Text = dr["IBAN"].ToString();
                MskHesapNo.Text = dr["Hesap No"].ToString();
                TxtYetkılı.Text = dr["YETKILI"].ToString();
                MskTelefon.Text = dr["TELEFON"].ToString();
                MskTarih.Text = dr["TARIH"].ToString();
                TxtHesapTur.Text = dr["Hesap Türü"].ToString();
                lookUpEdit1.EditValue = lookUpEdit1.Properties.GetKeyValueByDisplayText(dr["Firma Ad"].ToString());
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            temizle();
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Silmek istediğinizden emin misiniz?", "Bilgi", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                SqlCommand komutsil = new SqlCommand("delete from TBL_BANKALAR where ID=@k1", bgl.baglanti());
                komutsil.Parameters.AddWithValue("@k1", TxtID.Text);
                komutsil.ExecuteNonQuery();
                bgl.baglanti().Close();
                listele();
                MessageBox.Show("Silme işlemi başarılı", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                temizle();
            }
            else
            {
                MessageBox.Show("Silme işlemi iptal edildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Gider bilgilerini güncellemek istiyor musunuz?", "Bilgi", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                SqlCommand cmd = new SqlCommand("Update TBL_BANKALAR set BANKAADI=@P1,IL=@P2,ILCE=@P3,SUBE=@P4,IBAN=@P5,HESAPNO=@P6," +
                    "YETKILI=@P7,TELEFON=@P8,TARIH=@P9,HESAPTURU=@P10,FIRMAID=@P11 where ID=@P12", bgl.baglanti());
                cmd.Parameters.AddWithValue("@P1", CmbBankaAd.Text);
                cmd.Parameters.AddWithValue("@P2", Cmbil.Text);
                cmd.Parameters.AddWithValue("@P3", Cmbilce.Text);
                cmd.Parameters.AddWithValue("@P4", TxtSube.Text);
                cmd.Parameters.AddWithValue("@P5", TxtIBAN.Text);
                cmd.Parameters.AddWithValue("@P6", MskHesapNo.Text);
                cmd.Parameters.AddWithValue("@P7", TxtYetkılı.Text);
                cmd.Parameters.AddWithValue("@P8", MskTelefon.Text);
                cmd.Parameters.AddWithValue("@P9", MskTarih.Text);
                cmd.Parameters.AddWithValue("@P10", TxtHesapTur.Text);
                cmd.Parameters.AddWithValue("@P11", lookUpEdit1.EditValue);
                cmd.Parameters.AddWithValue("@P12", TxtID.Text);
                cmd.ExecuteNonQuery();
                bgl.baglanti().Close();
                listele();
                MessageBox.Show("Güncelleme başarıyla yapıldı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                temizle();
            }
            else
            {
                MessageBox.Show("Bilgi güncellenmesi iptal edildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                temizle();
            }
        }

    }
}
