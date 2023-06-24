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
using MessagingToolkit.QRCode.Codec;

namespace tezprojesi
{
    public partial class ReceteCikti : Form
    {
        private string kullaniciAdi;
        private string kid;

        public ReceteCikti(string kullaniciAdi, string kid)
        {
            InitializeComponent();
            this.kullaniciAdi = kullaniciAdi;
            this.kid = kid;
        }

        SqlConnection baglanti = null;

        private void ReceteIslemleri_Load(object sender, EventArgs e)
        {
            
            try
            {
                baglanti = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=eczaneotomasyon;Integrated Security=True");
                baglanti.Open();
                SqlCommand komut = new SqlCommand("Select MusteriTc,MusteriAd From MusteriTablo",baglanti);
                SqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    string birlestir = dr[0].ToString() +" "+ dr[1].ToString();
                    HastaBox1.Items.Add(birlestir);
                }
                baglanti.Close();
                label10.Text = "İşlemi Yapan Kullanıcı; " + kid + ", " + kullaniciAdi;

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

        private void HastaBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ReceteBox1.Items.Clear();
                ReceteBox1.Text = "";
                    baglanti = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=eczaneotomasyon;Integrated Security=True");
                baglanti.Open();

                string[] a = HastaBox1.SelectedItem.ToString().Split();
                string b = a[0];

                SqlCommand komud = new SqlCommand("Select * from ReceteTablo where MusteriTC=@p1", baglanti);
                komud.Parameters.AddWithValue("@p1", b);
                SqlDataReader reader = komud.ExecuteReader();
                while (reader.Read())
                {
                    ReceteBox1.Items.Add(reader[1].ToString()+" "+ reader[3].ToString()+" "+ reader[6].ToString());
                }
                reader.Close();




                
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
            KullaniciPaneli kpaneli = new KullaniciPaneli(kullaniciAdi, kid);
            kpaneli.Show();
            this.Hide();
        }
        public int labelsayac= 5;
        public int ilaclenght = 0;

        private void ReceteBox1_SelectedIndexChanged(object sender, EventArgs e)
        {


            label15.Text = "";
            string ilacid = "";

                baglanti = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=eczaneotomasyon;Integrated Security=True");
                baglanti.Open();

                string[] a = ReceteBox1.SelectedItem.ToString().Split();

                label15.Text = "Ödenecek Ücret: ";

                SqlCommand komut2 = new SqlCommand("Select * From ReceteTablo where Barkod=@barkod", baglanti);
                komut2.Parameters.AddWithValue("@barkod", a.Last());
                SqlDataReader dr2 = komut2.ExecuteReader();
                while (dr2.Read())
                {
                    label4.Text = dr2[1].ToString();
                    label7.Text = dr2[4].ToString();
                    label9.Text = dr2[5].ToString();
                    label6.Text = dr2[3].ToString();
                    ilacid= dr2[2].ToString();
                    label13.Text= dr2[6].ToString();
                    label15.Text+= dr2[7].ToString();
                }

                dr2.Close();
                string[] tc = HastaBox1.Text.Split(' ');
                
                

                SqlCommand komut = new SqlCommand("Select * From DoktorTablo where DoktorDiplomaNo=@komut", baglanti);
                komut.Parameters.AddWithValue("@komut", label9.Text);
                SqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    label5.Text = dr[1].ToString();
                }

                dr.Close();



                SqlCommand komut3 = new SqlCommand("Select * From MusteriTablo where MusteriTC=@komut", baglanti);
                komut3.Parameters.AddWithValue("@komut", tc[0]);
                SqlDataReader dr3 = komut3.ExecuteReader();

                while (dr3.Read())
                {
                    label8.Text = dr3[1].ToString();
                    label3.Text = dr3[4].ToString();
                }
                dr3.Close();



            
            string[] ilac = ilacid.ToString().Replace(" ","").Split('-');
            string ilacad="";
            string talimat = "";
            string SKT = "";
            int x=40; 
            int y=300;
            string[] yazdirma = new string[3];
            
            if (labelsayac == 5)
            {

            }
            else if (labelsayac == 1)
            {
                for (int i = 0; i < ilaclenght-1; i++)
                {
                    for (int j =0; j < 3; j++)
                    {
                        string labelisim=i.ToString()+j.ToString();
                        Label myLabel = (Label)this.Controls[labelisim];
                        this.Controls.Remove(myLabel);
                    }
                }
            }
            else if (labelsayac == 0)
            {
                for (int j = 0; j < 3; j++)
                {
                    string labelisim = j.ToString();
                    Label myLabel = (Label)this.Controls[labelisim];
                    this.Controls.Remove(myLabel);
                }
            }

            if (ilac.Length != 1)
            {
                ilaclenght = ilac.Length;
                Array.Resize(ref ilac, ilac.Length - 1);

                for (int i = 0; i <= ilac.Length - 1; i++)
                {
                    labelsayac = 1;
                    SqlCommand komut4 = new SqlCommand("Select * From IlacListe where ilacid=@komut", baglanti);
                    komut4.Parameters.AddWithValue("@komut", ilac[i]);
                    SqlDataReader dr4 = komut4.ExecuteReader();
                    while (dr4.Read())
                    {
                        yazdirma[0] = dr4[4].ToString();
                        yazdirma[1] = dr4[5].ToString();
                        yazdirma[2] = dr4[6].ToString();
                    }

                    char[] SKT1 = yazdirma[2].ToCharArray();
                    yazdirma[2] = "";
                    for (int z = 0; z < 10; z++)
                    {
                        yazdirma[2] += SKT1[z];

                    }

                    yazdirma[2] = "Son Kullanım Tarihi \n" + yazdirma[2];
                    yazdirma[1] = "Kullanım Talimatı \n" + yazdirma[1];
                    for (int j = 0; j < 3; j++)
                    {

                        Label label = new Label();
                        label.Name = i.ToString() + j.ToString();
                        label.Text = yazdirma[j];
                        label.Location = new Point(x, y);
                        this.Controls.Add(label);
                        label.Width = 120;
                        label.Height = 55;
                        x += 180;
                    }
                    
                    y += 70;
                    x = 40;
                    dr4.Close();
                }
            }
            else
            {
                labelsayac = 0;
                SqlCommand komut4 = new SqlCommand("Select * From IlacListe where ilacid=@komut", baglanti);
                komut4.Parameters.AddWithValue("@komut", ilac[0]);
                SqlDataReader dr4 = komut4.ExecuteReader();
                while (dr4.Read())
                {
                    yazdirma[0] = dr4[4].ToString();
                    yazdirma[1] = dr4[5].ToString();
                    yazdirma[2] = dr4[6].ToString();
                }
                char[] SKT1 = yazdirma[2].ToCharArray();
                yazdirma[2] = "";
                for (int z = 0; z < 10; z++)
                {
                    yazdirma[2] += SKT1[z];
                }
                yazdirma[2] = "Son Kullanım Tarihi \n" + yazdirma[2];
                yazdirma[1] = "Kullanım Talimatı \n" + yazdirma[1];
                for (int j =0; j < 3; j++)
                {

                    Label label = new Label();
                    label.Name=j.ToString();
                    label.Text = yazdirma[j];
                    label.Location = new Point(x, y);
                    this.Controls.Add(label);
                    label.Width = 120;
                    label.Height = 55;
                    x += 180;
                }

                y += 70;
                x = 40;
                dr4.Close();
            }

            string[] tarih = label3.Text.Split(' ');
                label3.Text = tarih[0];
                //label12.Text = "Kullanım Talimatları";
                //char[] SKT1 = SKT.ToCharArray();
                //SKT = "";
                //for (int i = 0; i < 10; i++)
                //{
                //    SKT += SKT1[i];
                    
                //}

                if (label13.Text.Length!=9)
                {
                    label13.Text = "0" + label13.Text;
                }
               


            Zen.Barcode.Code128BarcodeDraw brc = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
            pictureBox3.Image = brc.Draw(label13.Text,200);
            
            QRCodeEncoder encod = new QRCodeEncoder();
            pictureBox3.Image = encod.Encode(label13.Text);


            if (baglanti != null)
                    baglanti.Close();
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //120
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            

            
        
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }
    }
}
