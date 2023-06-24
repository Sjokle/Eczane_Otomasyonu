using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tezprojesi
{
    public partial class SilinenKullanicilar : Form
    {
        private SqlConnection baglanti = null;
        public SilinenKullanicilar()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Kullaniciİslemleri kislemleri = new Kullaniciİslemleri();
            kislemleri.Show();
            this.Hide();
        }

        public void verileriGoster(string veriler, SqlConnection baglanti)
        {
            SqlDataAdapter da = new SqlDataAdapter(veriler, baglanti);
            DataSet ds = new DataSet();
            da.Fill(ds);

            ds.Tables[0].Columns["kid"].ColumnName = "Kullanıcı ID";
            ds.Tables[0].Columns["kkadi"].ColumnName = "Kullanıcı Adı";
            ds.Tables[0].Columns["ksifre"].ColumnName = "Kullanıcı Şifre";
            SilinenGridView.DataSource = ds.Tables[0];

        }

        private void SilinenKullanicilar_Load(object sender, EventArgs e)
        {
            try
            {
                baglanti = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=eczaneotomasyon;Integrated Security=True");
                baglanti.Open();
                verileriGoster("Select * From SilinenKullanici", baglanti);

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
        }

        private void button6_Click(object sender, EventArgs e)
        {
            
            try
            {
                if(label2.Text!="")
                {
                    baglanti = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=eczaneotomasyon;Integrated Security=True");
                    baglanti.Open();

                    SqlCommand updatekomut = new SqlCommand("Select * From SilinenKullanici where kid =@kid", baglanti);
                    updatekomut.Parameters.AddWithValue("@kid", degerata());
                    SqlDataReader reader = updatekomut.ExecuteReader();
                    ArrayList Isimler = new ArrayList();
                    while (reader.Read())
                    {
                        Isimler.Add(reader["kkadi"]);
                        Isimler.Add(reader["ksifre"]);
                    }
                    reader.Close();

                    SqlCommand updatekomut2 = new SqlCommand("Insert Into KullaniciTablo (kkadi,ksifre) values (@adi,@sifre)", baglanti);
                    updatekomut2.Parameters.AddWithValue("@adi", Isimler[0].ToString());
                    updatekomut2.Parameters.AddWithValue("@sifre", Isimler[1].ToString());
                    updatekomut2.ExecuteNonQuery();

                    label4.Text = degerata() + " Numaralı ID ye sahip kullanıcı geri işe alınmıştır.";
                    SqlCommand komut = new SqlCommand("Delete from SilinenKullanici where kid=@id", baglanti);
                    komut.Parameters.AddWithValue("@id", degerata());
                    komut.ExecuteNonQuery();
                    verileriGoster("Select * from SilinenKullanici", baglanti);
                }
                else
                {
                    MessageBox.Show("Lütfen bir kayıt seçiniz!");
                }
                

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
        }

        private void SilinenGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        public string degerata()
        {
            int sec = SilinenGridView.SelectedCells[0].RowIndex;
            string id = SilinenGridView.Rows[sec].Cells[0].Value.ToString();
            return id;
        }
        private void SilinenGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int sec = SilinenGridView.SelectedCells[0].RowIndex;
            string isim = SilinenGridView.Rows[sec].Cells[1].Value.ToString();
            string sifre = SilinenGridView.Rows[sec].Cells[2].Value.ToString();
            
            label2.Text = isim;
            label3.Text = sifre;
        }
    }
}
