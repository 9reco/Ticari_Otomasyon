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
    public partial class FrmUrunler1 : Form
    {
        public FrmUrunler1()
        {
            InitializeComponent();
        }

        sqlbaglantisi bgl = new sqlbaglantisi();

        void listele()
        {
            SqlDataAdapter rb = new SqlDataAdapter("Select * From TBL_URUNLER",bgl.baglanti());
            DataTable dt = new DataTable();
            rb.Fill(dt);
            gridControl1.DataSource = dt;
        }
        void temizle()
        {
            TxtID.Text = "";
            TxtAd.Text = "";
            TxtMarka.Text = "";
            TxtModel.Text = "";
            MskYil.Text = "";
            NuAdet.Text = "";
            TxtAlisFiyat.Text = "";
            TxtSatisFiyat.Text = "";
            RchDetay.Text = "";
        }

        private void FrmUrunler1_Load(object sender, EventArgs e)
        {
            listele();
            temizle();
            gridView1.OptionsBehavior.Editable = false;
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            //Verileri kaydetme
        
            if(string.IsNullOrWhiteSpace(TxtAd.Text) || string.IsNullOrWhiteSpace(TxtMarka.Text) || string.IsNullOrWhiteSpace(TxtModel.Text) || string.IsNullOrWhiteSpace(MskYil.Text) ||
                string.IsNullOrWhiteSpace(NuAdet.Text) || string.IsNullOrWhiteSpace(TxtAlisFiyat.Text) || string.IsNullOrWhiteSpace(TxtSatisFiyat.Text) || string.IsNullOrWhiteSpace(RchDetay.Text))
            {
                MessageBox.Show("Lütfen alanları doldorunuz","Bilgi",MessageBoxButtons.OK,MessageBoxIcon.Error);
                
                return;
            }
            
            if(TxtAd.Text.Length > 20 || TxtMarka.Text.Length > 20 || TxtModel.Text.Length > 20 || RchDetay.Text.Length > 500)
            {
                MessageBox.Show("Lütfen alanları doğru şekilde kullanınız","Bilgi",MessageBoxButtons.OK,MessageBoxIcon.Error);
                temizle();
                return;
            }
            if (MessageBox.Show("Sisteme kaydedilsin mi?", "Bilgi", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                SqlCommand komut = new SqlCommand("insert into TBL_URUNLER (URUNAD,MARKA,MODEL,YIL,ADET,ALISFIYAT,SATISFIYAT,DETAY) " +
                 "values (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8)", bgl.baglanti());
                komut.Parameters.AddWithValue("@p1", TxtAd.Text);
                komut.Parameters.AddWithValue("@p2", TxtMarka.Text);
                komut.Parameters.AddWithValue("@p3", TxtModel.Text);
                komut.Parameters.AddWithValue("@p4", MskYil.Text);
                komut.Parameters.AddWithValue("@p5", int.Parse((NuAdet.Value).ToString()));
                komut.Parameters.AddWithValue("@p6", decimal.Parse(TxtAlisFiyat.Text));
                komut.Parameters.AddWithValue("@p7", decimal.Parse(TxtSatisFiyat.Text));
                komut.Parameters.AddWithValue("@p8", RchDetay.Text);
                komut.ExecuteNonQuery();
                bgl.baglanti().Close();
                listele();
                MessageBox.Show("Sisteme kayıt başarıyla yapıldı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                temizle();

            }
            else
            {
                MessageBox.Show("Sisteme kayıt yapılmadı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                temizle();
            }



        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Silmek istediğinizden emin misiniz?","Bilgi",MessageBoxButtons.OKCancel,MessageBoxIcon.Information)==DialogResult.OK)
            {
                SqlCommand komutsil = new SqlCommand("Delete from TBL_URUNLER where ID=@p1",bgl.baglanti());
                komutsil.Parameters.AddWithValue("@p1", TxtID.Text);
                komutsil.ExecuteNonQuery();
                bgl.baglanti().Close();             
                listele();
                MessageBox.Show("Ürün başarıyla silindi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Ürün silme işlemi iptal edildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                temizle();
            }
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            TxtID.Text = dr["ID"].ToString();
            TxtAd.Text = dr["URUNAD"].ToString();
            TxtMarka.Text = dr["MARKA"].ToString();
            TxtModel.Text = dr["MODEL"].ToString();
            MskYil.Text = dr["YIL"].ToString();
            NuAdet.Text = dr["ADET"].ToString();
            TxtAlisFiyat.Text = dr["ALISFIYAT"].ToString();
            TxtSatisFiyat.Text = dr["SATISFIYAT"].ToString();
            RchDetay.Text = dr["DETAY"].ToString();
        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            temizle();
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Ürünü güncellemek istiyor musunuz?","Bilgi",MessageBoxButtons.OKCancel,MessageBoxIcon.Information) == DialogResult.OK)
            {
                SqlCommand komut = new SqlCommand("Update TBL_URUNLER set URUNAD=@P1,MARKA=@P2,MODEL=@P3,YIL=@P4,ADET=@P5,ALISFIYAT=@P6,SATISFIYAT=@P7,DETAY=@P8 where ID=@P9", bgl.baglanti());
                komut.Parameters.AddWithValue("@P1", TxtAd.Text);
                komut.Parameters.AddWithValue("@P2", TxtMarka.Text);
                komut.Parameters.AddWithValue("@P3", TxtModel.Text);
                komut.Parameters.AddWithValue("@P4", MskYil.Text);
                komut.Parameters.AddWithValue("@P5", int.Parse((NuAdet.Value).ToString()));
                komut.Parameters.AddWithValue("@P6", decimal.Parse(TxtAlisFiyat.Text));
                komut.Parameters.AddWithValue("@P7", decimal.Parse(TxtSatisFiyat.Text));
                komut.Parameters.AddWithValue("@P8", RchDetay.Text);
                komut.Parameters.AddWithValue("@P9", TxtID.Text);
                komut.ExecuteNonQuery();
                bgl.baglanti().Close();
                listele();
                MessageBox.Show("Ürün başarıyla güncellendi.","Bilgi",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Ürün güncellenmesi iptal edildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                temizle();
            }
        }
    }
    }

