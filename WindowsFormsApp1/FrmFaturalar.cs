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
    public partial class FrmFaturalar : Form
    {
        public FrmFaturalar()
        {
            InitializeComponent();
        }
        sqlbaglantisi bgl = new sqlbaglantisi();

        void listele()
        {
            SqlDataAdapter sd = new SqlDataAdapter("Select * from TBL_FATURABILGI", bgl.baglanti());
            DataTable dt = new DataTable();
            sd.Fill(dt);
            gridControl1.DataSource = dt;
            
        }
        void temizle()
        {
            TxtID.Text = "";
            TxtSeri.Text = "";
            TxtSıraNo.Text = "";
            MskSaat.Text = "";
            MskTarih.Text = "";
            TxtVDaire.Text = "";
            TxtAlıcı.Text = "";
            TxtTEden.Text = "";
            TxtTAlan.Text = "";
        }
        void temizle2()
        {
            TxtUrunAd.Text = "";
            TxtMiktar.Text = "";
            TxtFiyat.Text = "";
            TxtTutar.Text = "";
            lookUpEdit1.EditValue = "";
            LookPersonel.EditValue = "";
            TxtUrunID.Text = "";
            TxtFaturaID.Text = "";
            lookMusteri.EditValue = "";
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
        void personel()
        {
            SqlDataAdapter da4 = new SqlDataAdapter("Select ID,AD + ' '+ SOYAD as TAM_AD from TBL_PERSONELLER", bgl.baglanti());
            DataTable dt4 = new DataTable();
            da4.Fill(dt4);
            LookPersonel.Properties.ValueMember= "ID";
            LookPersonel.Properties.DisplayMember= "TAM_AD";
            LookPersonel.Properties.DataSource = dt4;
        }
        void musteri()
        {
            SqlDataAdapter da5 = new SqlDataAdapter("Select ID,AD + ' ' + SOYAD as TAM_AD1 from TBL_MUSTERILER", bgl.baglanti());
            DataTable dt5 = new DataTable();
            da5.Fill(dt5);
            lookMusteri.Properties.ValueMember = "ID";
            lookMusteri.Properties.DisplayMember = "TAM_AD1";
            lookMusteri.Properties.DataSource = dt5;
        }
        private void FrmFaturalar_Load(object sender, EventArgs e)
        {
            listele();
            temizle();
            firma();
            personel();
            musteri();
            lookUpEdit1.Enabled = false;
            lookMusteri.Enabled = false;
            gridView1.OptionsBehavior.Editable = false;
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtSeri.Text) || string.IsNullOrWhiteSpace(TxtSıraNo.Text) || string.IsNullOrWhiteSpace(MskTarih.Text) || string.IsNullOrWhiteSpace(MskSaat.Text) ||
              string.IsNullOrWhiteSpace(TxtVDaire.Text) ||
              string.IsNullOrWhiteSpace(TxtAlıcı.Text) || string.IsNullOrWhiteSpace(TxtTEden.Text) ||
               string.IsNullOrWhiteSpace(TxtTAlan.Text))
            {
                MessageBox.Show("Lütfen alanları doldorunuz", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(CmbTür.Text))
            {
                MessageBox.Show("Lütfen Cari Türünü seçiniz..", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (TxtSıraNo.Text.Length > 10 || TxtVDaire.Text.Length > 40 || TxtVDaire.Text.Length > 40 || TxtTAlan.Text.Length > 40)
            {
                MessageBox.Show("Lütfen alanları doğru şekilde kullanınız", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                temizle();
                return;
            }
            //Firma hareketlere ekleme
            if (TxtFaturaID.Text != "" && CmbTür.Text=="Firma")
            {
                if(string.IsNullOrWhiteSpace(TxtMiktar.Text) || string.IsNullOrWhiteSpace(LookPersonel.Text))
                {
                    MessageBox.Show("Lütfen Miktar ve Personel seçiniz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                   
                    return;
                }
                double fiyat, tutar, miktar;
                fiyat = Convert.ToDouble(TxtFiyat.Text);
                miktar = Convert.ToDouble(TxtMiktar.Text);
                tutar = fiyat * miktar;
                TxtTutar.Text = tutar.ToString();

                

                SqlCommand komut2 = new SqlCommand("insert into TBL_FATURADETAY (URUN,MIKTAR,FIYAT,TUTAR,FATURAID) values (@p1,@p2,@p3,@p4,@p5)", bgl.baglanti());
                komut2.Parameters.AddWithValue("@p1", TxtUrunAd.Text);
                komut2.Parameters.AddWithValue("@p2", TxtMiktar.Text);
                komut2.Parameters.AddWithValue("@p3", decimal.Parse(TxtFiyat.Text));
                komut2.Parameters.AddWithValue("@p4", decimal.Parse(TxtTutar.Text));
                komut2.Parameters.AddWithValue("@p5", TxtFaturaID.Text);
                komut2.ExecuteNonQuery();
                bgl.baglanti().Close();
                listele();
                
                //hareket tablosuna veri girişi
                SqlCommand komut3 = new SqlCommand("insert into TBL_FIRMAHAREKET (URUNID,ADET,PERSONEL,FIRMA,FIYAT,TOPLAM,FATURAID,TARIH) values (@h1,@h2,"+
                    "@h3,@h4,@h5,@h6,@h7,@h8)", bgl.baglanti());
                komut3.Parameters.AddWithValue("@h1", TxtUrunID.Text);
                komut3.Parameters.AddWithValue("@h2", TxtMiktar.Text);
                komut3.Parameters.AddWithValue("@h3", LookPersonel.EditValue);
                komut3.Parameters.AddWithValue("@h4", lookUpEdit1.EditValue);
                komut3.Parameters.AddWithValue("@h5", decimal.Parse(TxtFiyat.Text));
                komut3.Parameters.AddWithValue("@h6", decimal.Parse(TxtTutar.Text));
                komut3.Parameters.AddWithValue("@h7", TxtFaturaID.Text);
                komut3.Parameters.AddWithValue("@h8", MskTarih.Text);
                komut3.ExecuteNonQuery();
                

                //stok sayısını azaltma
                SqlCommand komut4 = new SqlCommand("update TBL_URUNLER set ADET=ADET-@k1 where ID=@k2", bgl.baglanti());
                komut4.Parameters.AddWithValue("@k1", TxtMiktar.Text);
                komut4.Parameters.AddWithValue("@k2", TxtUrunID.Text);
                komut4.ExecuteNonQuery();
                bgl.baglanti().Close();
                MessageBox.Show("Faturaya ait ürün kaydedildi. \n Fatura No:" + TxtFaturaID.Text
                    + "\n Ürün:" + TxtUrunAd.Text + "\n Firma:" + lookUpEdit1.Text+ "\n Toplam Tutar:" + TxtTutar.Text, "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                temizle2();
                return;

            }
            
            
            if (MessageBox.Show("Sisteme kaydedilsin mi?", "Bilgi", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                if (TxtFaturaID.Text == "")
                {
                    SqlCommand komut = new SqlCommand("insert into TBL_FATURABILGI (SERI,SIRANO,TARIH,SAAT,VERGIDAIRE,ALICI,TESLIMEDEN,TESLIMALAN) values (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8)", bgl.baglanti());
                    komut.Parameters.AddWithValue("@p1", TxtSeri.Text);
                    komut.Parameters.AddWithValue("@p2", TxtSıraNo.Text);
                    komut.Parameters.AddWithValue("@p3", MskTarih.Text);
                    komut.Parameters.AddWithValue("@p4", MskSaat.Text);
                    komut.Parameters.AddWithValue("@p5", TxtVDaire.Text);
                    komut.Parameters.AddWithValue("@p6", TxtAlıcı.Text);
                    komut.Parameters.AddWithValue("@p7", TxtTEden.Text);
                    komut.Parameters.AddWithValue("@p8", TxtTAlan.Text);
                    komut.ExecuteNonQuery();
                    bgl.baglanti().Close();
                    listele();
                    MessageBox.Show("Sisteme kayıt başarıyla yapıldı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    temizle();
                    
                }
              
            }
            else
            {
                MessageBox.Show("Sisteme kayıt yapılmadı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                temizle();
                
            }
            if (TxtFaturaID.Text != "" && CmbTür.Text == "Müşteri")
            {
                if (string.IsNullOrWhiteSpace(TxtMiktar.Text) || string.IsNullOrWhiteSpace(LookPersonel.Text))
                {
                    MessageBox.Show("Lütfen Miktar ve Personel seçiniz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                double fiyat, tutar, miktar;
                fiyat = Convert.ToDouble(TxtFiyat.Text);
                miktar = Convert.ToDouble(TxtMiktar.Text);
                tutar = fiyat * miktar;
                TxtTutar.Text = tutar.ToString();



                SqlCommand komut2 = new SqlCommand("insert into TBL_FATURADETAY (URUN,MIKTAR,FIYAT,TUTAR,FATURAID) values (@p1,@p2,@p3,@p4,@p5)", bgl.baglanti());
                komut2.Parameters.AddWithValue("@p1", TxtUrunAd.Text);
                komut2.Parameters.AddWithValue("@p2", TxtMiktar.Text);
                komut2.Parameters.AddWithValue("@p3", decimal.Parse(TxtFiyat.Text));
                komut2.Parameters.AddWithValue("@p4", decimal.Parse(TxtTutar.Text));
                komut2.Parameters.AddWithValue("@p5", TxtFaturaID.Text);
                komut2.ExecuteNonQuery();
                bgl.baglanti().Close();
                listele();

                //hareket tablosuna veri girişi
                SqlCommand komut3 = new SqlCommand("insert into TBL_MUSTERIHAREKET (URUNID,ADET,PERSONEL,MUSTERI,FIYAT,TOPLAM,FATURAID,TARIH) values (@h1,@h2," +
                    "@h3,@h4,@h5,@h6,@h7,@h8)", bgl.baglanti());
                komut3.Parameters.AddWithValue("@h1", TxtUrunID.Text);
                komut3.Parameters.AddWithValue("@h2", TxtMiktar.Text);
                komut3.Parameters.AddWithValue("@h3", LookPersonel.EditValue);
                komut3.Parameters.AddWithValue("@h4", lookMusteri.EditValue);
                komut3.Parameters.AddWithValue("@h5", decimal.Parse(TxtFiyat.Text));
                komut3.Parameters.AddWithValue("@h6", decimal.Parse(TxtTutar.Text));
                komut3.Parameters.AddWithValue("@h7", TxtFaturaID.Text);
                komut3.Parameters.AddWithValue("@h8", MskTarih.Text);
                komut3.ExecuteNonQuery();


                //stok sayısını azaltma
                SqlCommand komut4 = new SqlCommand("update TBL_URUNLER set ADET=ADET-@k1 where ID=@k2", bgl.baglanti());
                komut4.Parameters.AddWithValue("@k1", TxtMiktar.Text);
                komut4.Parameters.AddWithValue("@k2", TxtUrunID.Text);
                komut4.ExecuteNonQuery();
                bgl.baglanti().Close();
                MessageBox.Show("Faturaya ait ürün kaydedildi. \n Fatura No:" + TxtFaturaID.Text
                    + "\n Ürün:" + TxtUrunAd.Text + "\n Müşteri:" + lookMusteri.Text + "\n Toplam Tutar:" + TxtTutar.Text, "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                temizle2();
                return;

            }

        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (dr != null)
            {
                TxtID.Text = dr["FATURABILGIID"].ToString();
                TxtFaturaID.Text = dr["FATURABILGIID"].ToString();
                TxtSeri.Text = dr["SERI"].ToString();
                TxtSıraNo.Text = dr["SIRANO"].ToString();
                MskTarih.Text = dr["TARIH"].ToString();
                MskSaat.Text = dr["SAAT"].ToString();
                TxtVDaire.Text = dr["VERGIDAIRE"].ToString();
                TxtAlıcı.Text = dr["ALICI"].ToString();
                TxtTEden.Text = dr["TESLIMEDEN"].ToString();
                TxtTAlan.Text = dr["TESLIMALAN"].ToString();
                
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            temizle();
        }

        private void TxtFiyat_EditValueChanged(object sender, EventArgs e)
        {
            
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Silmek istediğinizden emin misiniz?", "Bilgi", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                SqlCommand komutsil = new SqlCommand("delete from TBL_FATURABILGI where ID=@k1", bgl.baglanti());
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
            if (MessageBox.Show("Fatura bilgilerini güncellemek istiyor musunuz?", "Bilgi", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                SqlCommand komutg = new SqlCommand("update TBL_FATURABILGI set SERI=@P1,SIRANO=@P2,TARIH=@P3,SAAT=@P4,VERGIDAIRE=@P5,ALICI=@P6,TESLIMEDEN=@P7,TESLIMALAN=@P8 where FATURABILGIID=@P9", bgl.baglanti()); ;
                komutg.Parameters.AddWithValue("@P1",TxtSeri.Text);
                komutg.Parameters.AddWithValue("@P2", TxtSıraNo.Text);
                komutg.Parameters.AddWithValue("@P3", MskTarih.Text);
                komutg.Parameters.AddWithValue("@P4", MskSaat.Text);
                komutg.Parameters.AddWithValue("@P5", TxtVDaire.Text);
                komutg.Parameters.AddWithValue("@P6", TxtAlıcı.Text);
                komutg.Parameters.AddWithValue("@P7", TxtTEden.Text);
                komutg.Parameters.AddWithValue("@P8", TxtTAlan.Text);
                komutg.Parameters.AddWithValue("@P9", TxtID.Text);
                komutg.ExecuteNonQuery();
                bgl.baglanti().Close();
                listele();
                MessageBox.Show("Güncelleme başarıyla yapıldı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                temizle();
            }
            else
            {
                MessageBox.Show("Fatura güncellenmesi iptal edildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                temizle();
            }
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            FrmFaturaUrunDetay fr = new FrmFaturaUrunDetay();
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            if (dr != null)
            {
                TxtFaturaID.Text = dr["FATURABILGIID"].ToString();
                fr.ıd = dr["FATURABILGIID"].ToString();
                
            }
            fr.Show();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            
            SqlCommand komut = new SqlCommand("Select URUNAD,SATISFIYAT,ADET from TBL_URUNLER where ID=@p1",bgl.baglanti());          
            komut.Parameters.AddWithValue("@p1", TxtUrunID.Text);
            SqlDataReader dr = komut.ExecuteReader();
            if (dr.Read())
            {
                TxtUrunAd.Text = dr[0].ToString();
                TxtFiyat.Text = dr[1].ToString();
                MessageBox.Show("Ürün Stoğu:" + dr[2].ToString(), "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
            }
            else
            {
                MessageBox.Show("Ürün bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TxtUrunID.Text = string.Empty;
                return;          
            }

            if (int.TryParse(TxtUrunID.Text, out int result))
            {
                MessageBox.Show("Ürün Bulundu. \n Ürün No:" + result + "\n Ürün:" + dr[0].ToString(), "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Geçerli bir tamsayı giriniz", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TxtUrunID.Text=string.Empty;
            }
        }

        private void CmbTür_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CmbTür.Text == "Firma")
            {
                lookMusteri.Enabled = false;
                lookUpEdit1.Enabled = true;
            }
            else
            {
                lookMusteri.Enabled = true;
                lookUpEdit1.Enabled = false;
            }
        }
    }
    }

