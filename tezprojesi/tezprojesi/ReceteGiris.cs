using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using System.Reflection.Emit;
using iTextSharp.text.pdf.parser;

namespace tezprojesi
{
    public partial class ReceteGiris : Form
    {
        private string kullaniciAdi;
        private string kid;

        public ReceteGiris(string kullaniciAdi, string kid)
        {
            InitializeComponent();
            this.kullaniciAdi = kullaniciAdi;
            this.kid = kid;
        }
       

        private SqlConnection baglanti = null;

        public int indirimhesap(int yuzde)
        {
            int realucret = 0;
            if (yuzde != 0)
            {
                realucret = ucrethesap - (yuzde * ucrethesap / 100);

            }
            else
            {
                realucret = ucrethesap;
            }

            return realucret;
        }
        private void ReceteGiris_Load(object sender, EventArgs e)
        {
            baglanti = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=eczaneotomasyon;Integrated Security=True");
            baglanti.Open();
            SqlCommand komut1 = new SqlCommand("Select * From IlacListe", baglanti);
            SqlDataReader dr = komut1.ExecuteReader();
            while (dr.Read())
            {
                ilacBox1.Items.Add(dr[0].ToString() +" ; "+ dr[4].ToString());
            }

            dr.Close();
            SqlCommand komut2 = new SqlCommand("Select * From DoktorTablo", baglanti);
            SqlDataReader dr2 = komut2.ExecuteReader();
            while (dr2.Read())
            {
                doktorBox2.Items.Add(dr2[2].ToString() + " ; " + dr2[0].ToString());
            }
            dr2.Close();
            string barkod = DateTime.Now.ToString("dd/MM/yy");
            tarih.Text = barkod;

           
            SqlCommand komut3 = new SqlCommand("Select * From MusteriTablo", baglanti);
            dr = komut3.ExecuteReader();
            while (dr.Read())
            {
                musteriBox1.Items.Add(dr[0].ToString() + " ; " + dr[1].ToString());
            }



            dr.Close();
            dr2.Close();
            SqlCommand komut4 = new SqlCommand("Select * From SigortaTablo", baglanti);
            SqlDataReader dr1 = komut4.ExecuteReader();
            while (dr1.Read())
            {
                sigortaBox1.Items.Add(dr1[0].ToString() + " ; " + dr1[1].ToString());
            }
            dr1.Close();
            baglanti.Close();
            label8.Text =kid + ", " + kullaniciAdi;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (musteriBox1.Text != "" && protokolgiris.Text != "" && tanigiris.Text != "" && doktorBox2.Text != "" && ilacBox1.Text != "" && sigortaBox1.Text != "")
                {

                    string barkod = DateTime.Now.ToString("ddMMyy");
                    if (barkod.Length != 6)
                    {
                        barkod = "0" + barkod;
                    }
                    else
                    {

                    }
                    string[] a = ilacBox1.SelectedItem.ToString().Split();
                    string[] b = doktorBox2.SelectedItem.ToString().Split();
                    string[] c = musteriBox1.SelectedItem.ToString().Split();
                    string sayiString = c[0];
                    string sonIkiRakam = sayiString.Substring(sayiString.Length - 2);
                    barkod = barkod + protokolgiris.Text + sonIkiRakam;
                    string tc = c[0];
                    string ilac = a[0];
                    string diploma = b.Last();
                    string[] d = label1.Text.ToString().Split();
                    string[] sigortadizi = sigortaBox1.SelectedItem.ToString().Split();
                    int sigorta = Convert.ToInt32(sigortadizi.Last());
                    string sigortaisim = "";
                    for (int i = 0; i < sigortadizi.Length - 2; i++)
                    {
                        sigortaisim += sigortadizi[i] + " ";
                    }



                    baglanti = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=eczaneotomasyon;Integrated Security=True");
                    baglanti.Open();

                    //string[] ilac = ilacid.ToString().Replace(" ", "").Split('-');

                    SqlCommand komut = new SqlCommand("Insert Into ReceteTablo (MusteriTC,ilacid,Teshis,ProtokolNo,DoktorDiplomaNo,Barkod,ReceteUcret,Sigorta) values (@tc,@ilacid,@teshis,@protokol,@diploma,@barkod,@ucret,@sigorta)", baglanti);
                    komut.Parameters.AddWithValue("@tc", tc);
                    komut.Parameters.AddWithValue("@ilacid", label6.Text);
                    komut.Parameters.AddWithValue("@teshis", tanigiris.Text);
                    komut.Parameters.AddWithValue("@protokol", protokolgiris.Text);
                    komut.Parameters.AddWithValue("@diploma", diploma);
                    komut.Parameters.AddWithValue("@barkod", barkod);
                    komut.Parameters.AddWithValue("@ucret", indirimhesap(sigorta));
                    komut.Parameters.AddWithValue("@sigorta", sigortaisim);
                    komut.ExecuteNonQuery();

                    string[] id = label8.Text.Split(',');
                    DateTime today = DateTime.Today;
                    string date = today.ToString("yyyy-MM-dd");
                    string islem = tc + "," + label6.Text + "," + tanigiris.Text + "," + protokolgiris.Text + "," + diploma + "," + barkod + "," + indirimhesap(sigorta).ToString() + "," + sigortaisim + " Bilgileri sisteme eklenmiştir.";
                    SqlCommand komut1 = new SqlCommand("Insert Into LogSistem (id,islemturu,islem,tarih) values (@id,@islemturu,@islem,@tarih)", baglanti);
                    komut1.Parameters.AddWithValue("@id", id.First());
                    komut1.Parameters.AddWithValue("@islemturu", "Reçete Ekeleme");
                    komut1.Parameters.AddWithValue("@islem", islem);
                    komut1.Parameters.AddWithValue("@tarih", date);
                    komut1.ExecuteNonQuery();


                    string[] depodus = label6.Text.ToString().Replace(" ", "").Split('-');
                    Array.Resize(ref depodus, depodus.Length - 1);


                    if (depodus.Length == 1)
                    {
                        MessageBox.Show(barkod + " Barkod numaralı reçeteniz sisteme başarılı bir şekilde eklenmiştir.");
                        SqlCommand komut4 = new SqlCommand("UPDATE IlacListe SET StokAdedi = StokAdedi - 1 WHERE ilacid = " + depodus[0], baglanti);
                        komut4.ExecuteNonQuery();
                    }
                    else
                    {
                        for (int i = 0; i < depodus.Length; i++)
                        {
                            SqlCommand komut4 = new SqlCommand("UPDATE IlacListe SET StokAdedi = StokAdedi - 1 WHERE ilacid = " + depodus[i], baglanti);
                            komut4.ExecuteNonQuery();
                        }
                        MessageBox.Show(barkod + " QR numaralı reçeteniz sisteme başarılı bir şekilde eklenmiştir.");
                    }



                }
                else
                {
                    MessageBox.Show("Eksik Giriş Yapıldı!");
                    
                }
            }
            catch (Exception exception)
            {

                throw;
            }
            finally
            {
                if (baglanti != null)
                    baglanti.Close();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            baglanti = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=eczaneotomasyon;Integrated Security=True");
            baglanti.Open();
            SqlCommand komut2 = new SqlCommand("Select * From DoktorTablo", baglanti);
            SqlDataReader dr = komut2.ExecuteReader();
            ArrayList Isimler = new ArrayList();
            while (dr.Read())
            {
                Isimler.Add(dr["DoktorHastane"]);
            }
            
            hastane.Text=Isimler[Convert.ToInt32(doktorBox2.SelectedIndex)].ToString();
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            KullaniciPaneli kpaneli = new KullaniciPaneli(kullaniciAdi, kid);
            kpaneli.Show();
            this.Hide();
        }

        private void musteriBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] c = musteriBox1.SelectedItem.ToString().Split();
            musteri.Text = c[0];
        }

        private void ilacBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            baglanti = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=eczaneotomasyon;Integrated Security=True");
            baglanti.Open();
            label1.Text = "Ödenecek Ücret: ";
            string[] a = ilacBox1.SelectedItem.ToString().Split();
            SqlCommand komut1 = new SqlCommand("Select * From IlacListe where ilacid="+a[0], baglanti);
            SqlDataReader dr = komut1.ExecuteReader();
            while (dr.Read())
            {
                label1.Text+= dr[4].ToString()+" : "+dr[7].ToString()+", ";
            }
            baglanti.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            

        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            
        }

        private void sigortaBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label3.Text = "";
            string[] sigorta = sigortaBox1.SelectedItem.ToString().Split();
            string[] ilac=label1.Text.Split();
            string ucret = ilac.Last();

            for (int i = 0; i < sigorta.Length-2; i++)
            {
                label3.Text += sigorta[i]+" ";
            }
            label3.Text += "Şirketi Sayesinde Yeni Ücret ";

            label3.Text += indirimhesap(Convert.ToInt32(sigorta.Last()));
        }

        private void barkodgiris_Click(object sender, EventArgs e)
        {

        }


        private int ucrethesap=0;

        public ReceteGiris()
        {
            InitializeComponent();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            baglanti = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=eczaneotomasyon;Integrated Security=True");
            baglanti.Open();
            string[] ilacbox = ilacBox1.SelectedItem.ToString().Split();
            SqlCommand komut1 = new SqlCommand("Select * From IlacListe where ilacid=" + ilacbox[0], baglanti);
            SqlDataReader dr = komut1.ExecuteReader();
            string ucret = "";
            while (dr.Read())
            {
                ucret = dr[7].ToString();
            }
            baglanti.Close();
            label5.Text += ilacbox[2] +" "+ucret+ " , ";
            
            ucrethesap += Convert.ToInt32(ucret);
            label4.Text = "Ödenecek Toplam Ücret; " + ucrethesap;
            label6.Text += ilacbox[0] + "-";
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            string[] depodus = label6.Text.ToString().Replace(" ", "").Split('-');
            Array.Resize(ref depodus, depodus.Length - 1);
            for (int i = 0; i < depodus.Length; i++)
            {
                MessageBox.Show(depodus[i]);
            }

            MessageBox.Show(depodus.Length.ToString());
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
