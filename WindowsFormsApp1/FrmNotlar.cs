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
    public partial class FrmNotlar : Form
    {
        public FrmNotlar()
        {
            InitializeComponent();
        }
        sqlbaglantisi bgl = new sqlbaglantisi();

        void listele()
        {
            SqlDataAdapter da = new SqlDataAdapter("Select * from TBL_NOTLAR order by ID asc", bgl.baglanti());
            DataTable dt = new DataTable();
            da.Fill(dt);
            gridControl1.DataSource = dt;
        }
        void temizle()
        {
            TxtID.Text = string.Empty;
            MskTarih.Text = string.Empty;
            MskSaat.Text = string.Empty;
            TxtBaslik.Text = string.Empty;
            TxtOlusturan.Text = string.Empty;
            TxtHitap.Text = string.Empty;
            RchDetay.Text = string.Empty;

        }
        private void FrmNotlar_Load(object sender, EventArgs e)
        {
            listele();
            temizle();
            gridView1.OptionsBehavior.Editable = false;
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(MskTarih.Text) || string.IsNullOrWhiteSpace(MskSaat.Text) || string.IsNullOrWhiteSpace(TxtBaslik.Text) ||
                string.IsNullOrWhiteSpace(TxtHitap.Text) || string.IsNullOrWhiteSpace(TxtOlusturan.Text) || string.IsNullOrWhiteSpace(RchDetay.Text))
            {
                MessageBox.Show("Lütfen alanları doldurunuz","Bilgi",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            if(TxtBaslik.Text.Length > 20 || RchDetay.Text.Length > 250 || TxtOlusturan.Text.Length > 20 || TxtHitap.Text.Length > 20)
            {
                MessageBox.Show("Lütfen alanları doğru şekilde kullanınız.","Bilgi",MessageBoxButtons.OK,MessageBoxIcon.Hand); 
                return;
            }
            if(MessageBox.Show("Notları eklemek istiyor musunuz ?","Bilgi",MessageBoxButtons.OKCancel,MessageBoxIcon.Information) == DialogResult.OK)
            {
                SqlCommand komut = new SqlCommand("insert into TBL_NOTLAR (TARIH,SAAT,BASLIK,DETAY,OLUSTURAN,HITAP) values (@p1,@p2,@p3,@p4,@p5,@p6)",bgl.baglanti());
                komut.Parameters.AddWithValue("@p1",MskTarih.Text);
                komut.Parameters.AddWithValue("@p2", MskSaat.Text);
                komut.Parameters.AddWithValue("@p3", TxtBaslik.Text);
                komut.Parameters.AddWithValue("@p4", RchDetay.Text);
                komut.Parameters.AddWithValue("@p5", TxtOlusturan.Text);
                komut.Parameters.AddWithValue("@p6", TxtHitap.Text);
                komut.ExecuteNonQuery();
                bgl.baglanti().Close();
                listele();
                MessageBox.Show("Notlar sisteme kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                temizle();

            }
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            if (dr != null )
            {
                TxtID.Text = dr["ID"].ToString();
                MskTarih.Text = dr["TARIH"].ToString();
                MskSaat.Text = dr["SAAT"].ToString();
                TxtBaslik.Text = dr["BASLIK"].ToString();
                TxtOlusturan.Text = dr["OLUSTURAN"].ToString();
                TxtHitap.Text = dr["HITAP"].ToString();
                RchDetay.Text = dr["DETAY"].ToString();
            }
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Silmek istediğinizden emin misiniz?", "Bilgi", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                SqlCommand komutsil = new SqlCommand("delete from TBL_NOTLAR where ID=@k1", bgl.baglanti());
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
            if (MessageBox.Show("Not bilgilerini güncellemek istiyor musunuz?", "Bilgi", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                SqlCommand komut2 = new SqlCommand("update TBL_NOTLAR set TARIH=@P1,BASLIK=@P2,SAAT=@P3,DETAY=@P4,OLUSTURAN=@P5,HITAP=@P6 where ID=@P7", bgl.baglanti());
                komut2.Parameters.AddWithValue("@P1", MskTarih.Text);
                komut2.Parameters.AddWithValue("@P2", TxtBaslik.Text);
                komut2.Parameters.AddWithValue("@P3", MskSaat.Text);
                komut2.Parameters.AddWithValue("@P4", RchDetay.Text);
                komut2.Parameters.AddWithValue("@P5", TxtOlusturan.Text);
                komut2.Parameters.AddWithValue("@P6", TxtHitap.Text);
                komut2.Parameters.AddWithValue("@P7", TxtID.Text);
                komut2.ExecuteNonQuery();
                bgl.baglanti().Close();
                listele();
                MessageBox.Show("Güncelleme başarıyla yapıldı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                temizle();
            }
            else
            {
                MessageBox.Show("Not güncellenmesi iptal edildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                temizle();
            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            FrmNotDetay fr = new FrmNotDetay();

            DataRow dr1 = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (dr1 != null )
            {
                fr.notdetay = dr1["Detay"].ToString();
            }
            fr.Show();
        }
    }
}
