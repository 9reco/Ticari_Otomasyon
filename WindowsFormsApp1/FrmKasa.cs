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
using DevExpress.Charts;

namespace WindowsFormsApp1
{
    public partial class FrmKasa : Form
    {
        public FrmKasa()
        {
            InitializeComponent();
        }
        sqlbaglantisi bgl = new sqlbaglantisi();

        void firmahareket()
        {
            SqlDataAdapter dr = new SqlDataAdapter("Execute FirmaHareket1",bgl.baglanti());
            DataTable dt = new DataTable();
            dr.Fill(dt);
            gridControl1.DataSource = dt;
        }
        void musterihareket()
        {
            SqlDataAdapter dr1 = new SqlDataAdapter("Execute MusteriHareket", bgl.baglanti());
            DataTable dt1 = new DataTable();
            dr1.Fill(dt1);
            gridControl3.DataSource = dt1;
        }
        void gider()
        {
            SqlDataAdapter dr2 = new SqlDataAdapter("Select * from TBL_GIDERLER", bgl.baglanti());
            DataTable dt2 = new DataTable();
            dr2.Fill(dt2);
            gridControl2.DataSource = dt2;
        }
        public string ad;
        
        private void FrmKasa_Load(object sender, EventArgs e)
        {
            LblAktifKullanıcı.Text = ad;
            firmahareket();
            musterihareket();
            gider();

            //Toplam Tutar
            SqlCommand komut = new SqlCommand("Select Sum(Tutar) from TBL_FATURADETAY",bgl.baglanti());
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                LblToplamTutar.Text = dr[0].ToString() + " TL";
            }
            bgl.baglanti().Close();

            //Ödemeler
            SqlCommand komut1 = new SqlCommand("Select (ELEKTRIK+SU+DOGALGAZ+INTERNET+EKSTRA) from TBL_GIDERLER order by ID asc", bgl.baglanti());
            SqlDataReader dr1 = komut1.ExecuteReader();
            while (dr1.Read())
            {
                LblOdemeler.Text = dr1[0].ToString() + " TL";
            }
            bgl.baglanti().Close();

            //Personel Maaşları
            SqlCommand komut2 = new SqlCommand("select MAASLAR from TBL_GIDERLER order by ID asc", bgl.baglanti());
            SqlDataReader dr2 = komut2.ExecuteReader();
            while (dr2.Read())
            {
                LblPMaas.Text = dr2[0].ToString() + " TL";
            }
            bgl.baglanti().Close();

            //Müşteri Sayısı
            SqlCommand komut3 = new SqlCommand("select Count(*) from TBL_MUSTERILER", bgl.baglanti());
            SqlDataReader dr3 = komut3.ExecuteReader();
            while (dr3.Read())
            {
                LblMusteriSayisi.Text = dr3[0].ToString();
            }
            bgl.baglanti().Close();

            //Firma Sayısı
            SqlCommand komut4 = new SqlCommand("select Count(*) from TBL_FIRMALAR", bgl.baglanti());
            SqlDataReader dr4 = komut4.ExecuteReader();
            while (dr4.Read())
            {
                LblFirmaSayisi.Text = dr4[0].ToString();
            }
            bgl.baglanti().Close();

            //Stok Sayısı
            SqlCommand komut5 = new SqlCommand("select Sum(ADET) from TBL_URUNLER", bgl.baglanti());
            SqlDataReader dr5 = komut5.ExecuteReader();
            while (dr5.Read())
            {
                LblStokSayisi.Text = dr5[0].ToString();
            }
            bgl.baglanti().Close();

            //Personel Sayısı
            SqlCommand komut6 = new SqlCommand("select Count(*) from TBL_PERSONELLER", bgl.baglanti());
            SqlDataReader dr6 = komut6.ExecuteReader();
            while (dr6.Read())
            {
                LblPersonelSayisi.Text = dr6[0].ToString();
            }
            bgl.baglanti().Close();

            
        }
        int sayac = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            sayac++;
            //Elektrik
            if(sayac >0 && sayac <= 5)
            {
                groupControl9.Text = "ELEKTRİK";
                chartControl1.Series["Aylar"].Points.Clear();
                SqlCommand komut7 = new SqlCommand("Select Top 4 AY,ELEKTRIK from TBL_GIDERLER order by ID desc", bgl.baglanti());
                SqlDataReader dr7 = komut7.ExecuteReader();
                while (dr7.Read())
                {
                    chartControl1.Series["Aylar"].Points.Add(new DevExpress.XtraCharts.SeriesPoint(dr7[0], dr7[1]));
                }
                bgl.baglanti().Close();
            }
            //Su
            if(sayac >6 && sayac <= 10)
            {
                groupControl9.Text = "SU";
                chartControl1.Series["Aylar"].Points.Clear();
                SqlCommand komut8 = new SqlCommand("Select Top 4 AY,SU from TBL_GIDERLER order by ID desc", bgl.baglanti());
                SqlDataReader dr8 = komut8.ExecuteReader();
                while (dr8.Read())
                {
                    chartControl1.Series["Aylar"].Points.Add(new DevExpress.XtraCharts.SeriesPoint(dr8[0], dr8[1]));
                }
                bgl.baglanti().Close();
            }
            if (sayac >11 && sayac <= 15)
            {
                groupControl9.Text = "DOGALGAZ";
                chartControl1.Series["Aylar"].Points.Clear();
                SqlCommand komut9 = new SqlCommand("Select Top 4 AY,DOGALGAZ from TBL_GIDERLER order by ID desc", bgl.baglanti());
                SqlDataReader dr9 = komut9.ExecuteReader();
                while (dr9.Read())
                {
                    chartControl1.Series["Aylar"].Points.Add(new DevExpress.XtraCharts.SeriesPoint(dr9[0], dr9[1]));
                }
                bgl.baglanti().Close();
            }
            if(sayac >16 && sayac <= 20)
            {
                groupControl9.Text = "INTERNET";
                chartControl1.Series["Aylar"].Points.Clear();
                SqlCommand komut10 = new SqlCommand("Select Top 4 AY,INTERNET from TBL_GIDERLER order by ID desc", bgl.baglanti());
                SqlDataReader dr10 = komut10.ExecuteReader();
                while (dr10.Read())
                {
                    chartControl1.Series["Aylar"].Points.Add(new DevExpress.XtraCharts.SeriesPoint(dr10[0], dr10[1]));
                }
                bgl.baglanti().Close();
            }
            if(sayac >21 && sayac <=25)
            {
                groupControl9.Text = "EKSTRA";
                chartControl1.Series["Aylar"].Points.Clear();
                SqlCommand komut11 = new SqlCommand("Select Top 4 AY,EKSTRA from TBL_GIDERLER order by ID desc", bgl.baglanti());
                SqlDataReader dr11 = komut11.ExecuteReader();
                while (dr11.Read())
                {
                    chartControl1.Series["Aylar"].Points.Add(new DevExpress.XtraCharts.SeriesPoint(dr11[0], dr11[1]));
                }
                bgl.baglanti().Close();
            }
            if(sayac == 26)
            {
                sayac = 0;
            }
        }
        int sayac2 = 0;
        private void timer2_Tick(object sender, EventArgs e)
        {
            sayac2++;
            //Elektrik
            if (sayac2 > 0 && sayac2 <= 5)
            {
                groupControl10.Text = "ELEKTRİK";
                chartControl2.Series["Aylar"].Points.Clear();
                SqlCommand komut7 = new SqlCommand("Select Top 4 AY,ELEKTRIK from TBL_GIDERLER order by ID desc", bgl.baglanti());
                SqlDataReader dr7 = komut7.ExecuteReader();
                while (dr7.Read())
                {
                    chartControl2.Series["Aylar"].Points.Add(new DevExpress.XtraCharts.SeriesPoint(dr7[0], dr7[1]));
                }
                bgl.baglanti().Close();
            }
            //Su
            if (sayac2 > 6 && sayac2 <= 10)
            {
                groupControl10.Text = "SU";
                chartControl2.Series["Aylar"].Points.Clear();
                SqlCommand komut8 = new SqlCommand("Select Top 4 AY,SU from TBL_GIDERLER order by ID desc", bgl.baglanti());
                SqlDataReader dr8 = komut8.ExecuteReader();
                while (dr8.Read())
                {
                    chartControl2.Series["Aylar"].Points.Add(new DevExpress.XtraCharts.SeriesPoint(dr8[0], dr8[1]));
                }
                bgl.baglanti().Close();
            }
            if (sayac2 > 11 && sayac2 <= 15)
            {
                groupControl10.Text = "DOGALGAZ";
                chartControl2.Series["Aylar"].Points.Clear();
                SqlCommand komut9 = new SqlCommand("Select Top 4 AY,DOGALGAZ from TBL_GIDERLER order by ID desc", bgl.baglanti());
                SqlDataReader dr9 = komut9.ExecuteReader();
                while (dr9.Read())
                {
                    chartControl2.Series["Aylar"].Points.Add(new DevExpress.XtraCharts.SeriesPoint(dr9[0], dr9[1]));
                }
                bgl.baglanti().Close();
            }
            if (sayac2 > 16 && sayac2 <= 20)
            {
                groupControl10.Text = "INTERNET";
                chartControl2.Series["Aylar"].Points.Clear();
                SqlCommand komut10 = new SqlCommand("Select Top 4 AY,INTERNET from TBL_GIDERLER order by ID desc", bgl.baglanti());
                SqlDataReader dr10 = komut10.ExecuteReader();
                while (dr10.Read())
                {
                    chartControl2.Series["Aylar"].Points.Add(new DevExpress.XtraCharts.SeriesPoint(dr10[0], dr10[1]));
                }
                bgl.baglanti().Close();
            }
            if (sayac2 > 21 && sayac2 <= 25)
            {
                groupControl10.Text = "EKSTRA";
                chartControl2.Series["Aylar"].Points.Clear();
                SqlCommand komut11 = new SqlCommand("Select Top 4 AY,EKSTRA from TBL_GIDERLER order by ID desc", bgl.baglanti());
                SqlDataReader dr11 = komut11.ExecuteReader();
                while (dr11.Read())
                {
                    chartControl2.Series["Aylar"].Points.Add(new DevExpress.XtraCharts.SeriesPoint(dr11[0], dr11[1]));
                }
                bgl.baglanti().Close();
            }
            if (sayac2 == 26)
            {
                sayac2 = 0;
            }
        }
    }
}
