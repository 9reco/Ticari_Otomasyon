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
    public partial class FrmFaturaUrunEkle : Form
    {
        public FrmFaturaUrunEkle()
        {
            InitializeComponent();
        }

        sqlbaglantisi bgl = new sqlbaglantisi();

        public string urunıd;
        private void FrmFaturaUrunEkle_Load(object sender, EventArgs e)
        {
            TxtUrunID.Text = urunıd;

            SqlCommand komut = new SqlCommand("Select * from TBL_FATURADETAY where FATURAURUNID=@p1", bgl.baglanti());
            komut.Parameters.AddWithValue("@p1", TxtUrunID.Text);
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                TxtTutar.Text = dr[4].ToString();
                TxtFiyat.Text = dr[3].ToString();
                TxtMiktar.Text= dr[2].ToString();
                TxtUrunAd.Text= dr[1].ToString();
                bgl.baglanti().Close();
            }
            
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
           


            if (MessageBox.Show("Fatura bilgilerini güncellemek istiyor musunuz?", "Bilgi", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                double fiyat, tutar, miktar;
                fiyat = Convert.ToDouble(TxtFiyat.Text);
                miktar = Convert.ToDouble(TxtMiktar.Text);
                tutar = fiyat * miktar;
                TxtTutar.Text = tutar.ToString();

                SqlCommand komutg = new SqlCommand("update TBL_FATURADETAY set URUN=@P1,MIKTAR=@P2,FIYAT=@P3,TUTAR=@P4 where FATURAURUNID=@P5", bgl.baglanti());
                komutg.Parameters.AddWithValue("@P1", TxtUrunAd.Text);
                komutg.Parameters.AddWithValue("@P2", TxtMiktar.Text);
                komutg.Parameters.AddWithValue("@P3", decimal.Parse(TxtFiyat.Text));
                komutg.Parameters.AddWithValue("@P4", decimal.Parse(TxtTutar.Text));
                komutg.Parameters.AddWithValue("@P5", TxtUrunID.Text);
                komutg.ExecuteNonQuery();
                bgl.baglanti().Close();
                MessageBox.Show("Güncelleme başarıyla yapıldı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Fatura güncellenmesi iptal edildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
            }
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Silmek istediğinizden emin misiniz?", "Bilgi", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                SqlCommand komutsil = new SqlCommand("delete from TBL_FATURADETAY where FATURAURUNID=@k1", bgl.baglanti());
                komutsil.Parameters.AddWithValue("@k1", TxtUrunID.Text);
                komutsil.ExecuteNonQuery();
                bgl.baglanti().Close();              
                MessageBox.Show("Silme işlemi başarılı", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
               
            }
            else
            {
                MessageBox.Show("Silme işlemi iptal edildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
    }
}
