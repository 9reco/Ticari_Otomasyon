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
    public partial class FrmGiderler : Form
    {
        public FrmGiderler()
        {
            InitializeComponent();
        }

        sqlbaglantisi bgl = new sqlbaglantisi();

        void listele()
        {
            SqlDataAdapter da = new SqlDataAdapter("Select * from TBL_GIDERLER Order By ID Asc",bgl.baglanti());
            DataTable dt = new DataTable();
            da.Fill(dt);
            gridControl1.DataSource = dt;
        }
        void temizle()
        {
            TxtID.Text = "";
            CmbAy.Text = "";
            CmbYıl.Text = "";
            TxtElektrik.Text = "";
            TxtSu.Text = "";
            TxtDogalgaz.Text = "";
            TxtInternet.Text = "";
            TxtMaaslar.Text = "";
            TxtEkstra.Text = "";
            RchNotlar.Text = "";

        }

        private void FrmGiderler_Load(object sender, EventArgs e)
        {
            listele();
            temizle();
            gridView1.OptionsBehavior.Editable = false;
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CmbAy.Text) || string.IsNullOrWhiteSpace(CmbYıl.Text) || string.IsNullOrWhiteSpace(TxtElektrik.Text) || string.IsNullOrWhiteSpace(TxtSu.Text) ||
                string.IsNullOrWhiteSpace(TxtDogalgaz.Text) ||
                string.IsNullOrWhiteSpace(TxtInternet.Text) || string.IsNullOrWhiteSpace(TxtMaaslar.Text) || string.IsNullOrWhiteSpace(TxtEkstra.Text) || string.IsNullOrWhiteSpace(RchNotlar.Text))
            {
                MessageBox.Show("Lütfen alanları doldorunuz", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(TxtElektrik.Text.Length > 8 || TxtSu.Text.Length > 8 || TxtDogalgaz.Text.Length > 8 || TxtInternet.Text.Length > 8 || TxtMaaslar.Text.Length > 10 || TxtEkstra.Text.Length > 8 || TxtSu.Text.Length > 500)
            {
                MessageBox.Show("Lütfen alanları doğru şekilde kullanınız", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                temizle();
                return;
            }
            if (MessageBox.Show("Giderler sisteme kaydedilsin mi?", "Bilgi", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                SqlCommand komut = new SqlCommand("insert into TBL_GIDERLER (AY,YIL,ELEKTRIK,SU,DOGALGAZ,INTERNET,MAASLAR,EKSTRA,NOTLAR) values (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9)", bgl.baglanti());
                komut.Parameters.AddWithValue("@p1",CmbAy.Text);
                komut.Parameters.AddWithValue("@p2", CmbYıl.Text);
                komut.Parameters.AddWithValue("@p3", decimal.Parse(TxtElektrik.Text));
                komut.Parameters.AddWithValue("@p4",decimal.Parse(TxtSu.Text));
                komut.Parameters.AddWithValue("@p5", decimal.Parse(TxtDogalgaz.Text));
                komut.Parameters.AddWithValue("@p6",decimal.Parse(TxtInternet.Text));
                komut.Parameters.AddWithValue("@p7",decimal.Parse(TxtMaaslar.Text));
                komut.Parameters.AddWithValue("@p8",decimal.Parse( TxtEkstra.Text));
                komut.Parameters.AddWithValue("@p9",RchNotlar.Text);
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
                CmbAy.Text = dr["AY"].ToString();
                CmbYıl.Text = dr["YIL"].ToString();
                TxtElektrik.Text = dr["ELEKTRIK"].ToString();
                TxtDogalgaz.Text = dr["DOGALGAZ"].ToString();
                TxtSu.Text = dr["SU"].ToString();
                TxtInternet.Text = dr["INTERNET"].ToString();
                TxtMaaslar.Text = dr["MAASLAR"].ToString();
                TxtEkstra.Text = dr["EKSTRA"].ToString();
                RchNotlar.Text = dr["NOTLAR"].ToString();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            temizle();
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Silmek istediğinizden emin misiniz?","Bilgi",MessageBoxButtons.OKCancel,MessageBoxIcon.Information) == DialogResult.OK)
            {
                SqlCommand komutsil = new SqlCommand("delete from TBL_GIDERLER where ID=@k1", bgl.baglanti());
                komutsil.Parameters.AddWithValue("@k1", TxtID.Text);
                komutsil.ExecuteNonQuery();
                bgl.baglanti().Close();
                listele();
                MessageBox.Show("Silme işlemi başarılı","Bilgi",MessageBoxButtons.OK,MessageBoxIcon.Information);
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
                SqlCommand komut1 = new SqlCommand("update TBL_GIDERLER set AY=@P1,YIL=@P2,ELEKTRIK=@P3,SU=@P4,DOGALGAZ=@P5,INTERNET=@P6,MAASLAR=@P7,EKSTRA=@P8,NOTLAR=@P9 where ID=@P10", bgl.baglanti());
                komut1.Parameters.AddWithValue("@P1", CmbAy.Text);
                komut1.Parameters.AddWithValue("@P2", CmbYıl.Text);
                komut1.Parameters.AddWithValue("@P3", decimal.Parse(TxtElektrik.Text));
                komut1.Parameters.AddWithValue("@P4", decimal.Parse(TxtSu.Text));
                komut1.Parameters.AddWithValue("@P5", decimal.Parse(TxtDogalgaz.Text));
                komut1.Parameters.AddWithValue("@P6", decimal.Parse(TxtInternet.Text));
                komut1.Parameters.AddWithValue("@P7", decimal.Parse(TxtMaaslar.Text));
                komut1.Parameters.AddWithValue("@P8", decimal.Parse(TxtEkstra.Text));
                komut1.Parameters.AddWithValue("@P9", RchNotlar.Text);
                komut1.Parameters.AddWithValue("@P10", TxtID.Text);
                komut1.ExecuteNonQuery();
                bgl.baglanti().Close();
                listele();
                MessageBox.Show("Güncelleme başarıyla yapıldı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                temizle();
            }
            else
            {
                MessageBox.Show("Gider bilgi güncellenmesi iptal edildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                temizle();
            }
        }
    }
}
