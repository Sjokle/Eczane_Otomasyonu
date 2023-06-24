using System;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace tezprojesi
{
    public partial class IlacStok : Form
    {
        private SqlConnection baglanti = null;

        public void verileriGoster(string veriler, SqlConnection baglanti)
        {
            SqlDataAdapter da = new SqlDataAdapter(veriler, baglanti);
            DataSet ds = new DataSet();
            da.Fill(ds);

            ds.Tables[0].Columns["ilacid"].ColumnName = "İlaç ID";
            ds.Tables[0].Columns["StokAdedi"].ColumnName = "Stok Adedi";
            ds.Tables[0].Columns["Kategorisi"].ColumnName = "Kategorisi";
            ds.Tables[0].Columns["SistemeGirisTarihi"].ColumnName = "Sisteme Giriş Tarihi";
            ds.Tables[0].Columns["ilacismi"].ColumnName = "İlaç İsmi";
            ds.Tables[0].Columns["kullanimtalimati"].ColumnName = "Kullanım Talimatı";
            ds.Tables[0].Columns["SKT"].ColumnName = "SKT";
            ds.Tables[0].Columns["ucret"].ColumnName = "Ücret";
            ilacGridView1.DataSource = ds.Tables[0];

        }
        private string kullaniciAdi;
        private string kid;

        public IlacStok(string kullaniciAdi, string kid)
        {
            InitializeComponent();
            this.kullaniciAdi = kullaniciAdi;
            this.kid = kid;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            KullaniciPaneli kpaneli = new KullaniciPaneli(kullaniciAdi, kid);
            kpaneli.Show();
            this.Hide();
        }

        private void IlacStok_Load(object sender, EventArgs e)
        {
            try
            {
                baglanti = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=eczaneotomasyon;Integrated Security=True");
                baglanti.Open();
                verileriGoster("Select * From IlacListe", baglanti);
                string today = DateTime.Now.ToString("yyyy-MM-dd");
                label7.Text = today;
                sktbox.Text = "YYYY-AA-GG";
                sktbox.ForeColor = Color.LightGray;
                label14.Text = kid + ", " + kullaniciAdi;
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
                baglanti = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=eczaneotomasyon;Integrated Security=True");
                baglanti.Open();
                verileriGoster("Select * From IlacListe", baglanti);
                
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

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (stokadedi.Text != "" && kategori.Text != "" && isim.Text != "" && sktbox.Text != "" && ucret.Text != "" && talimat.Text != "")
                {
                
                string today = DateTime.Now.ToString("yyyy-MM-dd");
                baglanti = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=eczaneotomasyon;Integrated Security=True");
                baglanti.Open();
                
                SqlCommand komut = new SqlCommand("Insert Into IlacListe (StokAdedi,kategorisi,SistemeGirisTarihi,ilacismi,kullanimtalimati,SKT,ucret) values (@sadedi,@kategorisi,@giristarihi,@ilacismi,@talimat,@skt,@ucret)", baglanti);
                komut.Parameters.AddWithValue("@sadedi", stokadedi.Text);
                komut.Parameters.AddWithValue("@kategorisi", kategori.Text);
                komut.Parameters.AddWithValue("@giristarihi", today);
                komut.Parameters.AddWithValue("@ilacismi", isim.Text);
                komut.Parameters.AddWithValue("@talimat", talimat.Text);
                komut.Parameters.AddWithValue("@skt", sktbox.Text);
                komut.Parameters.AddWithValue("@ucret", ucret.Text);
                komut.ExecuteNonQuery();

                string[] id = label14.Text.Split(',');
                string islem=stokadedi.Text + " , " + kategori.Text + " , " + today + " , " + isim.Text + " , " + talimat.Text + " , " + sktbox.Text + " , " + ucret.Text+ " Bilgileri sisteme eklenmiştir";

                SqlCommand komut1 = new SqlCommand("Insert Into LogSistem (id,islemturu,islem,tarih) values (@id,@islemturu,@islem,@tarih)", baglanti);
                komut1.Parameters.AddWithValue("@id", id.First());
                komut1.Parameters.AddWithValue("@islemturu", "Stok İlaç Ekleme");
                komut1.Parameters.AddWithValue("@islem", islem);
                komut1.Parameters.AddWithValue("@tarih", today);
                komut1.ExecuteNonQuery();


                MessageBox.Show(isim.Text+" İsimli ilaç sisteme başarılı bir şekilde eklenmiştir.");
                stokadedi.Clear();
                kategori.Clear();
                isim.Clear();
                talimat.Clear();
                sktbox.Clear();
                }
                else
                { 
                    MessageBox.Show("Eksik Giriş Yapıldı!"); 
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

        private void textBox6_Enter(object sender, EventArgs e)
        {
            if (sktbox.Text=="YYYY-AA-GG")
            {
                sktbox.Text = "";
                sktbox.ForeColor = Color.Black;
            }
        }

        private void textBox6_Leave(object sender, EventArgs e)
        {
            if (sktbox.Text == "")
            {
                sktbox.Text = "YYYY-AA-GG";
                sktbox.ForeColor = Color.LightGray;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            baglanti = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=eczaneotomasyon;Integrated Security=True");
            baglanti.Open();
            if(textBox1.Text!="" && label9.Text!="")
            {
                SqlCommand komut4 = new SqlCommand("UPDATE IlacListe SET StokAdedi = StokAdedi + " + textBox1.Text + " WHERE ilacid = " + label9.Text, baglanti);
                komut4.ExecuteNonQuery();

                int sayi = Convert.ToInt32(textBox1.Text);
                stokadedi.Text = (Convert.ToInt32(stokadedi.Text) + sayi).ToString();
                stokguncelleme = stokadedi.Text;

                string islem = stokguncelleme + " Adet olan stok adedi " + textBox1.Text + " kadar arttırılmıştır";
                string[] id = label14.Text.Split(',');
                DateTime today = DateTime.Today;
                string date = today.ToString("yyyy-MM-dd");
                
                SqlCommand komut1 = new SqlCommand("Insert Into LogSistem (id,islemturu,islem,tarih) values (@id,@islemturu,@islem,@tarih)", baglanti);
                komut1.Parameters.AddWithValue("@id", id.First());
                komut1.Parameters.AddWithValue("@islemturu", "Stok Güncelleme");
                komut1.Parameters.AddWithValue("@islem", islem);
                komut1.Parameters.AddWithValue("@tarih", today);
                komut1.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show(id.First() + " id li ilaca "+ textBox1.Text+ " kadar stok eklenmiştir.");
            }
            else
            {
                MessageBox.Show("Eksik Giriş Yapıldı!");
            }

        }
        private void button5_Click(object sender, EventArgs e)
        {
            baglanti = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=eczaneotomasyon;Integrated Security=True");
            baglanti.Open();
            if (textBox1.Text != "" && label9.Text != "")
            {
                SqlCommand komut4 = new SqlCommand("UPDATE IlacListe SET StokAdedi = StokAdedi - " + textBox1.Text + " WHERE ilacid = " + label9.Text, baglanti);
                komut4.ExecuteNonQuery();


                string islem = stokguncelleme + " Adet olan stok adedi " + textBox1.Text + " kadar azaltılmıştır";
                string[] id = label14.Text.Split(',');
                DateTime today = DateTime.Today;
                string date = today.ToString("yyyy-MM-dd");


                int sayi = Convert.ToInt32(textBox1.Text);
                stokadedi.Text = (Convert.ToInt32(stokadedi.Text) - sayi).ToString();
                stokguncelleme = stokadedi.Text;

                SqlCommand komut1 = new SqlCommand("Insert Into LogSistem (id,islemturu,islem,tarih) values (@id,@islemturu,@islem,@tarih)", baglanti);
                komut1.Parameters.AddWithValue("@id", id.First());
                komut1.Parameters.AddWithValue("@islemturu", "Stok Güncelleme");
                komut1.Parameters.AddWithValue("@islem", islem);
                komut1.Parameters.AddWithValue("@tarih", today);
                komut1.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show(id.First() + " id li ilaca " + textBox1.Text + " kadar stok eklenmiştir.");
            }
            else
            {
                MessageBox.Show("Eksik Giriş Yapıldı!");
            }
            

        }

        private void ilacGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            


        }

        private void ilacGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int sec = ilacGridView1.SelectedCells[0].RowIndex;
            label13.Text= ilacGridView1.Rows[sec].Cells[0].Value.ToString()+" Id li İlaca ";
            label9.Text = ilacGridView1.Rows[sec].Cells[0].Value.ToString();
            stokadedi.Text = ilacGridView1.Rows[sec].Cells[1].Value.ToString();
            kategori.Text = ilacGridView1.Rows[sec].Cells[2].Value.ToString();
            isim.Text = ilacGridView1.Rows[sec].Cells[4].Value.ToString();
            talimat.Text = ilacGridView1.Rows[sec].Cells[5].Value.ToString();
            string skt= ilacGridView1.Rows[sec].Cells[6].Value.ToString();
            ucret.Text = ilacGridView1.Rows[sec].Cells[7].Value.ToString();
            string[] sskt=skt.Split(' ');
            string yeniTarih = "0";
            if (sskt[0].Length == 10)
            {
                DateTime dateTime = DateTime.ParseExact(sskt[0], "dd.MM.yyyy", null);
                yeniTarih = dateTime.ToString("yyyy-MM-dd");
            }
            else
            {
                DateTime dateTime = DateTime.ParseExact(sskt[0], "d.MM.yyyy", null);
                yeniTarih = dateTime.ToString("yyyy-MM-0d");
            }

            this.sktbox.Text = yeniTarih;
            this.sktbox.ForeColor = Color.Black;

            guncelleme = label9.Text + "idli ürünün , " + stokadedi.Text + " , " + kategori.Text + " , " + label7.Text + " , " + isim.Text + " , " + sktbox.Text + " , " + ucret.Text + " , " +talimat.Text;
            stokguncelleme = stokadedi.Text;
        }
        public string guncelleme;
        public string stokguncelleme;
        private void button6_Click(object sender, EventArgs e)
        {
            if (stokadedi.Text != "" && kategori.Text != "" && isim.Text != "" && sktbox.Text != "" && ucret.Text != "" && talimat.Text != "")
            {
                baglanti = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=eczaneotomasyon;Integrated Security=True");
                baglanti.Open();
                int sec = Convert.ToInt32(ilacGridView1.CurrentRow.Cells[0].Value);
                SqlCommand komut = new SqlCommand("Update IlacListe set StokAdedi='" + stokadedi.Text + "',kategorisi='" + kategori.Text + "', ilacismi='" + isim.Text + "',kullanimtalimati='" + talimat.Text + "',SKT='" + sktbox.Text + "',ucret='" + ucret.Text + "' where ilacid='" + sec + "' ", baglanti);
                komut.ExecuteNonQuery();
                verileriGoster("Select * From IlacListe", baglanti);

                string islem = " bilgileri " + guncelleme + sec + " idli ürünün" + stokadedi.Text + " , " + kategori.Text + " , " + isim.Text + " , " + talimat.Text + " , " + sktbox.Text + " , " + ucret.Text + " Bilgileri ile güncellenmiştir";
                string[] id = label14.Text.Split(',');
                DateTime today = DateTime.Today;
                string date = today.ToString("yyyy-MM-dd");

                SqlCommand komut1 = new SqlCommand("Insert Into LogSistem (id,islemturu,islem,tarih) values (@id,@islemturu,@islem,@tarih)", baglanti);
                komut1.Parameters.AddWithValue("@id", id.First());
                komut1.Parameters.AddWithValue("@islemturu", "Stok İlaç Güncelleme");
                komut1.Parameters.AddWithValue("@islem", islem);
                komut1.Parameters.AddWithValue("@tarih", today);
                komut1.ExecuteNonQuery();
            }
            else
            {
                MessageBox.Show("Eksik Giriş Yapıldı!");
            }

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }
    }
}
