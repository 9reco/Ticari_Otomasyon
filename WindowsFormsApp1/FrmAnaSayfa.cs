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
using System.Xml;
using System.Diagnostics;

namespace WindowsFormsApp1
{
    public partial class FrmAnaSayfa : Form
    {
        public FrmAnaSayfa()
        {
            InitializeComponent();
        }
        sqlbaglantisi bgl = new sqlbaglantisi();

        void stok()
        {
            SqlDataAdapter da = new SqlDataAdapter("Select Urunad,Sum(ADET) as 'Adet' from TBL_URUNLER group by URUNAD having SUM(ADET) <= 30 order by SUM(ADET)", bgl.baglanti());
            DataTable dt = new DataTable();
            da.Fill(dt);
            GridStok.DataSource = dt;
        }
        void ajanda()
        {
            DataTable dt1 = new DataTable();
            SqlDataAdapter da1 = new SqlDataAdapter("Select top 10 TARIH,SAAT,BASLIK,HITAP from TBL_NOTLAR order by ID desc",bgl.baglanti());
            da1.Fill(dt1);
            gridControl3.DataSource = dt1;
        }
        void hareket()
        {
            SqlDataAdapter da2 = new SqlDataAdapter("execute FirmaHareket2", bgl.baglanti());
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            GridHareket.DataSource = dt2;
        }
        void firma()
        {
            SqlDataAdapter da3 = new SqlDataAdapter("Select AD,TELEFON1 from TBL_FIRMALAR", bgl.baglanti());
            DataTable dt3 = new DataTable();
            da3.Fill(dt3);
            GridFirma.DataSource = dt3;
        }
        void haber()
        {
            XmlTextReader xmloku = new XmlTextReader("https://www.hurriyet.com.tr/rss/anasayfa");
            while (xmloku.Read())
            {
                if (xmloku.Name == "title")
                {
                    listBox1.Items.Add(xmloku.ReadString());
                }
            }

        }
        private void FrmAnaSayfa_Load(object sender, EventArgs e)
        {
            stok();
            ajanda();
            hareket();
            firma();
            haber();

            string bugün = "https://www.tcmb.gov.tr/kurlar/today.xml";
            var xmldosya = new XmlDocument();
            xmldosya.Load(bugün);

            string dolaralis = xmldosya.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteBuying").InnerXml;
            LblDolarAlis.Text = dolaralis;

            string dolarsatis = xmldosya.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteSelling").InnerXml;
            LblDolarSatis.Text = dolarsatis;

            string euroalis = xmldosya.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteBuying").InnerXml;
            LblEuroAlis.Text = euroalis;

            string eurosatis = xmldosya.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteSelling").InnerXml;
            LblEuroSatis.Text = eurosatis;

            gridView1.OptionsBehavior.Editable = false;
            gridView2.OptionsBehavior.Editable = false;
            gridView3.OptionsBehavior.Editable = false;
            gridView4.OptionsBehavior.Editable = false;

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.hurriyet.com.tr/");
        }
    }
}
