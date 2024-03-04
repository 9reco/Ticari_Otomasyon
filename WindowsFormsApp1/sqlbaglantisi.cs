using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace WindowsFormsApp1
{
    internal class sqlbaglantisi
    {
        public SqlConnection baglanti()
        {
            SqlConnection bgl = new SqlConnection(@"Data Source=DESKTOP-PKBOPDL\SQLEXPRESS;Initial Catalog=DboTicariOtomasyon;User ID=sa;Trusted_Connection=True");
            bgl.Open();
            return bgl;
        }
    }
}
