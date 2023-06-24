using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tezprojesi
{
    public partial class LogKonrol : Form
    {
        public LogKonrol()
        {
            InitializeComponent();
        }

        public void verileriGoster(string veriler, SqlConnection baglanti)
        {
            SqlDataAdapter da = new SqlDataAdapter(veriler, baglanti);
            DataSet ds = new DataSet();
            da.Fill(ds);

            ds.Tables[0].Columns["id"].ColumnName = "ID";
            ds.Tables[0].Columns["islemturu"].ColumnName = "İşlem Türü";
            ds.Tables[0].Columns["islem"].ColumnName = "İşlem";
            ds.Tables[0].Columns["tarih"].ColumnName = "Sisteme Giriş Tarihi";
            logGridView1.DataSource = ds.Tables[0];

        }
        SqlConnection baglanti = null;

        
        private void LogKonrol_Load(object sender, EventArgs e)
        {
            try
            {
                baglanti = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=eczaneotomasyon;Integrated Security=True");
                baglanti.Open();
                verileriGoster("Select * From LogSistem", baglanti);

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
            finally
            {
                if (baglanti != null)
                    baglanti.Close();
            }
            logGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            YoneticiPaneli main = new YoneticiPaneli();
            main.Show();
            this.Hide();
        }

        private void logGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
