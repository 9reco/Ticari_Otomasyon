using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using DevExpress.CodeParser;

namespace WindowsFormsApp1
{
    public partial class FrmMail : Form
    {
        public FrmMail()
        {
            InitializeComponent();
        }
        public string mail;
        private void FrmMail_Load(object sender, EventArgs e)
        {
            TxtMailAdres.Text = mail;
        }

        private void BtnGonder_Click(object sender, EventArgs e)
        {


            string mailAdresi = TxtMailAdres.Text;
            string konu = TxtKonu.Text;
            string mesaj = RchMesaj.Text;

            try
            {
                // MailMessage sınıfı kullanarak bir e-posta oluşturun
                MailMessage eposta = new MailMessage();
                eposta.From = new MailAddress("gonderen@example.com"); // Gönderen adresini buraya yazın
                eposta.To.Add(mailAdresi); // Alıcı adresini ekleyin
                eposta.Subject = konu; // Konuyu ayarlayın
                eposta.Body = mesaj; // Mesajı ayarlayın

                // SmtpClient sınıfı kullanarak SMTP sunucusuna bağlanın ve e-postayı gönderin
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"); // SMTP sunucu adresini buraya yazın
                smtpClient.Port = 587; // SMTP portunu ayarlayın (genellikle 587 veya 465)
                smtpClient.Credentials = new System.Net.NetworkCredential("kullanici@gmail.com", "sifre"); // E-posta hesabınızın kimlik bilgilerini buraya yazın
                smtpClient.EnableSsl = true; // SSL kullanıyorsa true olarak ayarlayın
                smtpClient.Send(eposta); // E-postayı gönderin

                MessageBox.Show("E-posta başarıyla gönderildi.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("E-posta gönderilirken bir hata oluştu: " + ex.Message);
            }
        }
    }
}
