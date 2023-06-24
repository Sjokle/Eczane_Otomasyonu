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

namespace tezprojesi
{
    
    
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }


        private void Form1_Load(object sender, EventArgs e)
        {
            ysifre.PasswordChar = '*';
            Random rnd = new Random();
            Random rnd1 = new Random();
            rndsayi1.Text = rnd.Next(1, 1).ToString();
            rndsayi2.Text = rnd1.Next(1, 1).ToString();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
           
        }

        private void button1_Click_2(object sender, EventArgs e)
        {

        }
        public string id;
        private void button3_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                SqlConnection baglanti = null;
                try
                {

                    baglanti = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=eczaneotomasyon;Integrated Security=True");
                    baglanti.Open();
                    SqlCommand SqlKomut = new SqlCommand("Select * from YoneticiTablo where yadi=@yadi and ysifre=@ysifre", baglanti);
                    SqlKomut.Parameters.AddWithValue("@yadi", yisim.Text);
                    SqlKomut.Parameters.AddWithValue("@ysifre", ysifre.Text);
                    SqlDataReader sqlDR = SqlKomut.ExecuteReader();
                    string kadi = "";
                    string sifre = "";
                    while (sqlDR.Read())
                    {
                        kadi = sqlDR[0].ToString();
                        sifre = sqlDR[1].ToString();
                    }

                    if (yisim.Text == kadi && ysifre.Text == sifre)
                    {
                        if (Convert.ToInt32(rndsayi1.Text) + Convert.ToInt32(rndsayi2.Text) == Convert.ToInt32(rndsonuc.Text))
                        {
                            YoneticiPaneli form2 = new YoneticiPaneli();
                            form2.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Birşeyler Yanlış Gitti");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Kullanıcı adı veya şifre yanlış");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Sql Query esnasında bir hata oluştu ! " + ex.ToString());
                }
                finally
                {
                    if (baglanti != null)
                        baglanti.Close();
                }
            }
            else if(radioButton2.Checked==true)
            {
                SqlConnection baglanti = null;
                try
                {

                    baglanti = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=eczaneotomasyon;Integrated Security=True");
                    baglanti.Open();
                    SqlCommand SqlKomut = new SqlCommand("Select * from KullaniciTablo where kkadi=@kadi and ksifre=@ksifre", baglanti);
                    SqlKomut.Parameters.AddWithValue("@kadi", yisim.Text);
                    SqlKomut.Parameters.AddWithValue("@ksifre", ysifre.Text);
                    SqlDataReader sqlDR = SqlKomut.ExecuteReader();
                    string kadi = "";
                    string sifre = "";
                    while (sqlDR.Read())
                    {
                        kadi = sqlDR[1].ToString();
                        sifre = sqlDR[2].ToString();
                        id = sqlDR[0].ToString();
                    }
                    string kullaniciAdi = kadi;
                    string kid = id;
                    if (yisim.Text == kadi && ysifre.Text == sifre)
                    {
                        if (Convert.ToInt32(rndsayi1.Text) + Convert.ToInt32(rndsayi2.Text) == Convert.ToInt32(rndsonuc.Text))
                        {
                            KullaniciPaneli form5 = new KullaniciPaneli(kadi, id);
                            form5.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Birşeyler Yanlış Gitti");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Kullanıcı Adı veya Şifre yanlış");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Sql Query esnasında bir hata oluştu ! " + ex.ToString());
                }
                finally
                {
                    if (baglanti != null)
                        baglanti.Close();
                }
            }
        }

        private void ysifre_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
