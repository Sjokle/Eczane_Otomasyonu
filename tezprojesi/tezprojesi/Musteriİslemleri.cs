using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tezprojesi
{
    public partial class Musteriİslemleri : Form
    {
        private string kullaniciAdi;
        private string kid;
        public Musteriİslemleri(string kullaniciAdi, string kid)
        {
            InitializeComponent();
            this.kullaniciAdi = kullaniciAdi;
            this.kid = kid;
        }



        private SqlConnection baglanti = null;

        public void verileriGoster(string veriler, SqlConnection baglanti)
        {
            SqlDataAdapter da = new SqlDataAdapter(veriler, baglanti);
            DataSet ds = new DataSet();
            da.Fill(ds);

            ds.Tables[0].Columns["MusteriTC"].ColumnName = "Müşteri TC";
            ds.Tables[0].Columns["MusteriAd"].ColumnName = "Müşteri Ad";
            ds.Tables[0].Columns["MusteriTelefon"].ColumnName = "Müşteri Telefon";
            ds.Tables[0].Columns["MusteriAdres"].ColumnName = "Müşteri Adres";
            ds.Tables[0].Columns["MusteriKayitTarihi"].ColumnName = "Müşteri Kayıt Tarihi";
            ds.Tables[0].Columns["MusteriEposta"].ColumnName = "Müşteri E-Posta";
            ds.Tables[0].Columns["VKN"].ColumnName = "VKN";
            ds.Tables[0].Columns["VergiDairesi"].ColumnName = "Vergi Dairesi";

            MusteriGridView1.DataSource = ds.Tables[0];

        }


        private void Musteriİslemleri_Load(object sender, EventArgs e)
        {
            try
            {
                baglanti = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=eczaneotomasyon;Integrated Security=True");
                baglanti.Open();
                verileriGoster("Select * From MusteriTablo", baglanti);

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
            label9.Text = kid + ", " + kullaniciAdi;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=eczaneotomasyon;Integrated Security=True");
                baglanti.Open();
                verileriGoster("Select * From MusteriTablo", baglanti);
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
                string tarih = mtarih.Text.ToString();
                int sayac = 0;
                if (mtel.Text.Length == 16)
                {
                    char[] tarihdizi = tarih.ToCharArray();
                    if (tarih.Length == 10)
                    {
                        for (int i = 0; i < tarihdizi.Length; i++)
                        {
                            if (tarihdizi[i].ToString() == "-")
                            {
                                sayac++;
                            }
                        }
                        if (sayac > 1 && sayac < 3)
                        {

                            if (msicil.Text.Length == 11 || msicil.Text.Length == 4)
                            {
                                if (msicil.Text != "" && misim.Text != "" && mtel.Text != "" && madres.Text != "" && mtarih.Text != "" && mmail.Text != "" && mvkn.Text != "" && mvergidairesi.Text != "")
                                {

                                    string sayiString = msicil.Text.ToString();
                                    string ilkIkiRakam = sayiString.Substring(0, 2);
                                    string sonIkiRakam = sayiString.Substring(sayiString.Length - 2);

                                    string ilk = ilkIkiRakam + sonIkiRakam;
                                    Convert.ToInt32(ilk);


                                    baglanti = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=eczaneotomasyon;Integrated Security=True");
                                    baglanti.Open();
                                    SqlCommand komut = new SqlCommand("Insert Into MusteriTablo (MusteriTC,MusteriAd,MusteriTelefon,MusteriAdres,MusteriKayitTarihi,MusteriEposta,VKN,VergiDairesi) values (@sicil,@ad,@tel,@adres,@tarih,@mail,@vkn,@vergidairesi)", baglanti);
                                    komut.Parameters.AddWithValue("@sicil", ilk);
                                    komut.Parameters.AddWithValue("@ad", misim.Text);
                                    komut.Parameters.AddWithValue("@tel", mtel.Text);
                                    komut.Parameters.AddWithValue("@adres", madres.Text);
                                    komut.Parameters.AddWithValue("@tarih", mtarih.Text);
                                    komut.Parameters.AddWithValue("@mail", mmail.Text);
                                    komut.Parameters.AddWithValue("@vkn", mvkn.Text);
                                    komut.Parameters.AddWithValue("@vergidairesi", mvergidairesi.Text);
                                    komut.ExecuteNonQuery();

                                    SqlCommand idkomut = new SqlCommand("Select * from MusteriTablo where MusteriTC=@sicil and MusteriAd=@ad and MusteriTelefon=@tel", baglanti);
                                    idkomut.Parameters.AddWithValue("@sicil", ilk);
                                    idkomut.Parameters.AddWithValue("@ad", misim.Text);
                                    idkomut.Parameters.AddWithValue("@tel", mtel.Text);
                                    int sicil = (int)idkomut.ExecuteScalar();
                                    label7.Text = msicil.Text + " Numaralı TC'e sahip Müşteri başarıyla eklenmiştir.";
                                    verileriGoster("Select * from MusteriTablo", baglanti);


                                    DateTime today = DateTime.Today;
                                    string date = today.ToString("yyyy-MM-dd");
                                    string[] id = label9.Text.ToString().Split(',');
                                    string islem = ilk + ", " + misim.Text + ", " + mtel.Text + ", " + madres.Text + ", " + mtarih.Text + ", " + mmail.Text + ", " + mvkn.Text + ", " + mvergidairesi.Text + " Bilgileri sisteme eklenmiştir.";
                                    SqlCommand komut1 = new SqlCommand("Insert Into LogSistem (id,islemturu,islem,tarih) values (@id,@islemturu,@islem,@tarih)", baglanti);
                                    komut1.Parameters.AddWithValue("@id", id.First());
                                    komut1.Parameters.AddWithValue("@islemturu", "Müşteri Ekleme");
                                    komut1.Parameters.AddWithValue("@islem", islem);
                                    komut1.Parameters.AddWithValue("@tarih", today);
                                    komut1.ExecuteNonQuery();

                                    msicil.Clear();
                                    misim.Clear();
                                    mtel.Clear();
                                    madres.Clear();
                                    mtarih.Clear();
                                    mmail.Clear();
                                }
                                else
                                {
                                    MessageBox.Show("Eksik Giriş Yapıldı!");
                                }
                            }
                            else
                            {
                                MessageBox.Show("TC girerken eksik veya fazla girildi lütfen doğru girdiğinizden emin olunuz \n Girilen karakter sayısı 11 olmalıdır. Girilen karakter sayısı ise : " + msicil.Text.Length);
                            }


                        }
                        else
                        {
                            MessageBox.Show("Tarih girerken eksik girildi lütfen YYYY-AA-GG şeklinde girdiğinizden emin olunuz " + tarih);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Tarih girerken eksik veya yanlış girildi " + tarih);
                    }

                }
                else
                {
                    MessageBox.Show("Telefon numarası eksik veya hatalı girildi \n'+90 ___ ___ ____' şeklinde girdiğinizden emin olunuz");
                }




            }
            catch (Exception exception)
            {
                Console.WriteLine("Bir hata oluştu " + exception.ToString());
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
                string tarih = mtarih.Text.ToString();
                int sayac = 0;
                if (mtel.Text.Length == 16)
                {
                    char[] tarihdizi = tarih.ToCharArray();
                    if (tarih.Length == 10)
                    {
                        for (int i = 0; i < tarihdizi.Length; i++)
                        {
                            if (tarihdizi[i].ToString() == "-")
                            {
                                sayac++;
                            }
                        }

                        if (sayac > 1 && sayac < 3)
                        {
                            string sayiString = msicil.Text.ToString();
                            string ilkIkiRakam = sayiString.Substring(0, 2);
                            string sonIkiRakam = sayiString.Substring(sayiString.Length - 2);

                            string ilk = ilkIkiRakam + sonIkiRakam;
                            Convert.ToInt32(ilk);
                            if (msicil.Text.Length == 11 || msicil.Text.Length == 4)
                            {
                                if (msicil.Text != "" && misim.Text != "" && mtel.Text != "" && madres.Text != "" && mtarih.Text != "" && mmail.Text != "" && mvkn.Text != "" && mvergidairesi.Text != "")
                                {
                                    baglanti = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=eczaneotomasyon;Integrated Security=True");
                                    baglanti.Open();
                                    int sec = Convert.ToInt32(MusteriGridView1.CurrentRow.Cells[0].Value);
                                    SqlCommand komut = new SqlCommand("Update MusteriTablo set MusteriAd='" + misim.Text + "', MusteriTelefon='" + mtel.Text + "',MusteriAdres='" + madres.Text + "',MusteriKayitTarihi='" + mtarih.Text + "',MusteriEposta='" + mmail.Text + "', VKN='" + mvkn.Text + "', VergiDairesi='" + mvergidairesi.Text + "'    where MusteriTC='" + ilk + "' ", baglanti);
                                    komut.ExecuteNonQuery();
                                    label8.Text = msicil.Text + " TC numarasına sahip Müşteri güncellenmiştir";
                                    verileriGoster("Select * From MusteriTablo", baglanti);

                                    DateTime today = DateTime.Today;
                                    string date = today.ToString("yyyy-MM-dd");
                                    string[] id = label9.Text.ToString().Split(',');
                                    string islem = guncelleme + " bilgileri; " + ilk + ", " + misim.Text + ", " + mtel.Text + ", " + madres.Text + ", " + mtarih.Text + ", " + mmail.Text + ", " + mvkn.Text + ", " + mvergidairesi.Text + " olarak güncellenmiştir.";
                                    SqlCommand komut1 = new SqlCommand("Insert Into LogSistem (id,islemturu,islem,tarih) values (@id,@islemturu,@islem,@tarih)", baglanti);
                                    komut1.Parameters.AddWithValue("@id", id.First());
                                    komut1.Parameters.AddWithValue("@islemturu", "Müşteri Güncelleme");
                                    komut1.Parameters.AddWithValue("@islem", islem);
                                    komut1.Parameters.AddWithValue("@tarih", date);
                                    komut1.ExecuteNonQuery();
                                    msicil.Clear();
                                    misim.Clear();
                                    mtel.Clear();
                                    madres.Clear();
                                    mtarih.Clear();
                                    mmail.Clear();
                                }
                                else
                                {
                                    MessageBox.Show("Eksik Giriş Yapıldı!");
                                }
                            }
                            else
                            {
                                MessageBox.Show("TC girerken eksik veya fazla girildi lütfen doğru girdiğinizden emin olunuz \n Girilen karakter sayısı 11 olmalıdır. Girilen karakter sayısı ise : " + msicil.Text.Length);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Tarih girerken eksik girildi lütfen YYYY-AA-GG şeklinde girdiğinizden emin olunuz " + tarih);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Tarih girerken eksik veya yanlış girildi " + tarih + sayac);
                    }

                }
                else
                {
                    MessageBox.Show("Telefon numarası eksik veya hatalı girildi \n'+90 ___ ___ ____' şeklinde girdiğinizden emin olunuz");
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

        private void MusteriGridView1_Click(object sender, EventArgs e)
        {

        }
        string guncelleme = "";
        private void MusteriGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            int sec = MusteriGridView1.SelectedCells[0].RowIndex;
            string sicil = MusteriGridView1.Rows[sec].Cells[0].Value.ToString();
            string ad = MusteriGridView1.Rows[sec].Cells[1].Value.ToString();
            string tel = MusteriGridView1.Rows[sec].Cells[2].Value.ToString();
            string adres = MusteriGridView1.Rows[sec].Cells[3].Value.ToString();
            string tarih = MusteriGridView1.Rows[sec].Cells[4].Value.ToString();
            string mail = MusteriGridView1.Rows[sec].Cells[5].Value.ToString();
            string vkn = MusteriGridView1.Rows[sec].Cells[6].Value.ToString();
            string vergidairesi = MusteriGridView1.Rows[sec].Cells[7].Value.ToString();


            string formattedDate = "null";
            string newdate = tarih.Substring(0, tarih.Length - 9);


            if (newdate.Length == 10)
            {
                DateTime date = DateTime.ParseExact(newdate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                formattedDate = date.ToString("yyyy-MM-dd");
            }
            else if (newdate.Length == 9)
            {
                DateTime date = DateTime.ParseExact(newdate, "d.MM.yyyy", CultureInfo.InvariantCulture);
                formattedDate = date.ToString("yyyy-MM-0d");
            }

            msicil.Text = sicil;
            misim.Text = ad;
            mtel.Text = tel;
            madres.Text = adres;
            mtarih.Text = formattedDate;
            mmail.Text = mail;
            mvkn.Text = vkn;
            mvergidairesi.Text = vergidairesi;


            guncelleme = sicil + "," + ad + "," + tel + "," + adres + "," + formattedDate + "," + mail + "," + vkn + "," + vergidairesi;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string sayiString = msicil.Text.ToString();
                string ilkIkiRakam = sayiString.Substring(0, 2);
                string sonIkiRakam = sayiString.Substring(sayiString.Length - 2);

                string ilk = ilkIkiRakam + sonIkiRakam;
                Convert.ToInt32(ilk);
                baglanti = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=eczaneotomasyon;Integrated Security=True");
                baglanti.Open();

                SqlCommand komut = new SqlCommand("Select * From MusteriTablo where MusteriTC = " + ilk, baglanti);
                SqlDataAdapter da = new SqlDataAdapter(komut);
                DataSet ds = new DataSet();
                da.Fill(ds);
                MusteriGridView1.DataSource = ds.Tables[0];


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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }



        private void button4_Click(object sender, EventArgs e)
        {
            KullaniciPaneli kpaneli = new KullaniciPaneli(kullaniciAdi, kid);
            kpaneli.Show();
            this.Hide();
        }
    }
}
