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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;
using System.Reflection.Emit;
using System.Collections;

namespace tezprojesi
{
    public partial class Kullaniciİslemleri : Form
    {
        SqlConnection baglanti = null;
        public Kullaniciİslemleri()
        {
            InitializeComponent();
        }
        
        public void verileriGoster(string veriler, SqlConnection baglanti)
        {
            SqlDataAdapter da = new SqlDataAdapter(veriler,baglanti);
            DataSet ds = new DataSet();
            da.Fill(ds);

            ds.Tables[0].Columns["kid"].ColumnName = "Kullanıcı ID";
            ds.Tables[0].Columns["kkadi"].ColumnName = "Kullanıcı Adı";
            ds.Tables[0].Columns["ksifre"].ColumnName = "Kullanıcı Şifre";
            KullaniciDataGrid.DataSource = ds.Tables[0];
            
        }

        public void Form6_Load(object sender, EventArgs e)
        {
            
            try
            {
                baglanti = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=eczaneotomasyon;Integrated Security=True");
                baglanti.Open();
                verileriGoster("Select * From KullaniciTablo",baglanti);
                
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

        private void button1_Click(object sender, EventArgs e)
        {
            
            try
            {
                baglanti = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=eczaneotomasyon;Integrated Security=True");
                baglanti.Open();
                verileriGoster("Select * From KullaniciTablo", baglanti);
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

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (progressBar1.Value == 4)
                {
                    if (kisim.Text != "" && ksifre.Text != "")
                    {
                        baglanti = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=eczaneotomasyon;Integrated Security=True");
                        baglanti.Open();
                        SqlCommand komut = new SqlCommand("Insert Into KullaniciTablo (kkadi,ksifre) values (@adi,@sifre)", baglanti);
                        komut.Parameters.AddWithValue("@adi", kisim.Text);
                        komut.Parameters.AddWithValue("@sifre", ksifre.Text);
                        komut.ExecuteNonQuery();

                        SqlCommand idkomut = new SqlCommand("Select max(kid) from KullaniciTablo", baglanti);
                        int kid = (int)idkomut.ExecuteScalar();
                        label8.Text = kid.ToString() + " Numaralı ID ye sahip kullanıcı başarıyla eklenmiştir.";
                        verileriGoster("Select * from KullaniciTablo", baglanti);
                        kisim.Clear();
                        ksifre.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Eksik Giriş Yapıldı!");
                    }

                }

                //MessageBox.Show("Şifre en az 8 karakterden oluşmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    MessageBox.Show("Şifre en az 8 karakterden oluşmalı ve içinde en az 1 rakam bulunmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception exception)
            {
                Console.WriteLine("Bir hata oluştu "+ exception.ToString());
                throw;
            }
            finally
            {
                if (baglanti != null)
                    baglanti.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (sifredurum==1)
                {
                    if (kidsil.Text != "")
                    {
                        baglanti = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=eczaneotomasyon;Integrated Security=True");
                        baglanti.Open();

                        SqlCommand updatekomut = new SqlCommand("Select * From KullaniciTablo where kid =@kid", baglanti);
                        updatekomut.Parameters.AddWithValue("@kid", Convert.ToInt32(kidsil.Text));
                        SqlDataReader reader = updatekomut.ExecuteReader();
                        ArrayList Isimler = new ArrayList();
                        while (reader.Read())
                        {
                            Isimler.Add(reader["kkadi"]);
                            Isimler.Add(reader["ksifre"]);
                        }
                        reader.Close();


                        SqlCommand updatekomut2 = new SqlCommand("Insert Into SilinenKullanici (kid,kkadi,ksifre) values (@kid,@adi,@sifre)", baglanti);
                        updatekomut2.Parameters.AddWithValue("@kid", kidsil.Text);
                        updatekomut2.Parameters.AddWithValue("@adi", Isimler[0].ToString());
                        updatekomut2.Parameters.AddWithValue("@sifre", Isimler[1].ToString());
                        updatekomut2.ExecuteNonQuery();
                        label7.Text = kidsil.Text + " Numaralı ID ye sahip kullanıcı başarıyla silinmiştir.";
                        SqlCommand komut = new SqlCommand("Delete from KullaniciTablo where kid=@id", baglanti);
                        komut.Parameters.AddWithValue("@id", kidsil.Text);
                        komut.ExecuteNonQuery();
                        verileriGoster("Select * from KullaniciTablo", baglanti);
                        kisim.Clear();
                        ksifre.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Eksik Giriş Yapıldı!");
                    }
                }
                else
                {
                    MessageBox.Show("Şifre en az 8 karakterden oluşmalı ve içinde en az 1 rakam ve 1 karakter bulunmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Bir hata oluştu ", exception.ToString());
                throw;
            }
            finally
            {
                if (baglanti != null)
                    baglanti.Close();
            }
        }

        private void KullaniciDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int sec = KullaniciDataGrid.SelectedCells[0].RowIndex;
            string id = KullaniciDataGrid.Rows[sec].Cells[0].Value.ToString();
            string ad = KullaniciDataGrid.Rows[sec].Cells[1].Value.ToString();
            string sifre = KullaniciDataGrid.Rows[sec].Cells[2].Value.ToString();

            kidsil.Text = id;
            kisim.Text= ad;
            ksifre.Text= sifre;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=eczaneotomasyon;Integrated Security=True");
                baglanti.Open();

                SqlCommand komut = new SqlCommand("Select * From KullaniciTablo where kid = "+kidsil.Text,baglanti);
                SqlDataAdapter da = new SqlDataAdapter(komut);
                DataSet ds = new DataSet();
                da.Fill(ds);
                KullaniciDataGrid.DataSource = ds.Tables[0];


            }
            catch (Exception exception)
            {
                MessageBox.Show("Bir hata oluştu ", exception.ToString());
                throw;
            }
            finally
            {
                if (baglanti != null)
                    baglanti.Close();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (sifredurum == 1)
                {
                    if (kisim.Text != "" && ksifre.Text != "" && kidsil.Text != "")
                    {
                        baglanti = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=eczaneotomasyon;Integrated Security=True");
                        baglanti.Open();

                        SqlCommand komut = new SqlCommand("Update KullaniciTablo set kkadi='" + kisim.Text + "',ksifre='" + ksifre.Text + "' where kid='" + kidsil.Text + "' ", baglanti);
                        komut.ExecuteNonQuery();
                        verileriGoster("Select * From KullaniciTablo", baglanti);
                    }
                    else
                    {
                        MessageBox.Show("Eksik Giriş Yapıldı!");
                    }
                }
                else
                {
                    MessageBox.Show("Şifre en az 8 karakterden oluşmalı ve içinde en az 1 rakam ve 1 karakter bulunmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception exception)
            {
                MessageBox.Show("Bir hata oluştu ", exception.ToString());
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
            SilinenKullanicilar skullanici = new SilinenKullanicilar();
            skullanici.Show();
            this.Hide();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            YoneticiPaneli ypaneli = new YoneticiPaneli();
            ypaneli.Show();
            this.Hide();
        }

        private void KullaniciDataGrid_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
        private int progress = 0;
        private int sifredurum = 0;
        private void UpdateProgressBar()
        {
            progressBar1.Value = progress;
        }

        private void ksifre_TextChanged(object sender, EventArgs e)
        {
            sifredurum = 0;
            progress = 0; // Her metin değiştiğinde progress sıfırlanır

            if (ksifre.Text.Length >= 1)
            {
                progress++;
            }

            if (ksifre.Text.Length >= 8)
            {
                progress++;
            }

            bool containsDigit = false;
            foreach (char character in ksifre.Text)
            {
                if (Char.IsDigit(character))
                {
                    containsDigit = true;
                    break;
                }
            }

            if (containsDigit)
            {
                progress++;
            }

            if (ksifre.Text.Any(char.IsSymbol) || ksifre.Text.Any(char.IsPunctuation))
            {
                progress++;
            }

            if (progress == 4)
            {
                sifredurum = 1;
            }
            progressBar1.Value = progress;
            UpdateProgressBar();

            //if (progressBar1.Value == 1)
            //{
            //    progressBar1.ForeColor = Color.Red;
            //}
            //else if (progressBar1.Value == 2)
            //{
            //    progressBar1.ForeColor = Color.Red;
            //}
            //else if (progressBar1.Value == 3)
            //{
            //    progressBar1.ForeColor = Color.Yellow;
            //}
            //else if (progressBar1.Value == 4)
            //{
            //    progressBar1.ForeColor = Color.Green;
            //}
            
        }

        private void kidsil_TextChanged(object sender, EventArgs e)
        {

        }
    }
}