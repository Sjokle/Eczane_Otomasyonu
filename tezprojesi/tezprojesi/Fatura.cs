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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace tezprojesi
{
    public partial class Fatura : Form
    {

        public Fatura(string kullaniciAdi, string kid)
        {
            InitializeComponent();
            this.kullaniciAdi = kullaniciAdi;
            this.kid = kid;
        }

        private string kullaniciAdi;
        private string kid;

        public void verileriGoster(string veriler, SqlConnection baglanti)
        {
            SqlDataAdapter da = new SqlDataAdapter(veriler, baglanti);
            DataSet ds = new DataSet();
            da.Fill(ds);


        }
        SqlConnection baglanti = null;
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                baglanti = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=eczaneotomasyon;Integrated Security=True");
                baglanti.Open();
                SqlCommand komut = new SqlCommand("Select * From MusteriTablo", baglanti);
                SqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    string birlestir = dr[0].ToString() + " " + dr[1].ToString();
                    hastaBox1.Items.Add(birlestir);
                }
                baglanti.Close();

                DateTime today = DateTime.Today;
                string date = today.ToString("MMMM dd.yyyy");
                label6.Text=date.ToString();

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
            label2.Text = "İşlemi Yapan Kullanıcı; " + kid + ", " + kullaniciAdi;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            baglanti = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=eczaneotomasyon;Integrated Security=True");
            baglanti.Open();

            receteBox2.Items.Clear();
            receteBox2.Text = "";
            string ilacid = "";
           
            string[] hasta = hastaBox1.SelectedItem.ToString().Split();
            
                SqlCommand komut = new SqlCommand("Select * From ReceteTablo Where MusteriTC=@hasta", baglanti);
                komut.Parameters.AddWithValue("@hasta", hasta.First());
                SqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    //ilacid = dr[2].ToString();
                    string birlestir = dr[3].ToString() + " " + dr[6].ToString();
                    receteBox2.Items.Add(birlestir);
                }

                dr.Close();
                label1.Text = hasta[1] + " " + hasta[2];

            SqlCommand komut1 = new SqlCommand("Select * From MusteriTablo Where MusteriTC=@hasta", baglanti);
            komut1.Parameters.AddWithValue("@hasta", hasta.First());
            SqlDataReader dr1 = komut1.ExecuteReader();
            while (dr1.Read())
            {
                adres.Text = dr1[3].ToString();
                mail.Text = dr1[5].ToString();
                tel.Text= dr1[2].ToString();
                vergi.Text= dr1[7].ToString();
                vkn.Text = dr1[6].ToString();
            }
            dr.Close();
            label16.Text = "0000000002356";
            label14.Text = "TR1.2.1";
            
            DateTime today = DateTime.Today;
            string date = today.ToString("dd.MM.yyyy");
            label17.Text = date;

            string date1 = DateTime.Now.ToString("HH:mm:ss");
            label18.Text = date1;


            if (baglanti != null)
                    baglanti.Close();
            
        }
        public int labelsayac = 5;
        public int ilaclenght = 0;
        //****************************************************************************************************************************
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            baglanti = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=eczaneotomasyon;Integrated Security=True");
            baglanti.Open();

            string[] barkod = receteBox2.SelectedItem.ToString().Split();
            string ilacid = "";
            string ucret = "";
            string sigortaisim = "";
            string sigortaoran = "";
            ondalıklabel.Text = "";
            label4.Text = barkod.Last();

            SqlCommand komut2 = new SqlCommand("Select * From ReceteTablo where Barkod=@barkod", baglanti);
            komut2.Parameters.AddWithValue("@barkod", barkod.Last());
            SqlDataReader dr2 = komut2.ExecuteReader();
            while (dr2.Read())
            {
                ilacid = dr2[2].ToString();
                //label13.Text = dr2[6].ToString();
                ucret = dr2[7].ToString();
                sigortaisim = dr2[8].ToString();

            }

            dr2.Close();

            string[] ilac = ilacid.ToString().Replace(" ", "").Split('-');
            string ilacad = "";
            string talimat = "";
            string SKT = "";
            
            string[] yazdirma = new string[3];

            if (labelsayac == 5)
            {

            }
            else if (labelsayac == 1)
            {
                for (int i = 0; i < ilaclenght - 1; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        string labelisim = i.ToString() + j.ToString();
                        System.Windows.Forms.Label myLabel = (System.Windows.Forms.Label)this.Controls[labelisim];
                        this.Controls.Remove(myLabel);

                        string labelisim2 = i.ToString() + j.ToString() + "a";
                        System.Windows.Forms.Label ourlabel = (System.Windows.Forms.Label)this.Controls[labelisim2];
                        this.Controls.Remove(ourlabel);
                    }
                    string labelisim3 = i.ToString() + "a";
                    System.Windows.Forms.Label islabel = (System.Windows.Forms.Label)this.Controls[labelisim3];
                    this.Controls.Remove(islabel);

                    string labelisim4 = i.ToString() + "b";
                    System.Windows.Forms.Label thislabel = (System.Windows.Forms.Label)this.Controls[labelisim4];
                    this.Controls.Remove(thislabel);

                    string labelisim5 = "sigorta";
                    System.Windows.Forms.Label thatlabel = (System.Windows.Forms.Label)this.Controls[labelisim5];
                    this.Controls.Remove(thatlabel);
                }
            }
            else if (labelsayac == 0)
            {
                for (int j = 0; j < 3; j++)
                {
                    string labelisim = j.ToString();
                    System.Windows.Forms.Label myLabel = (System.Windows.Forms.Label)this.Controls[labelisim];
                    this.Controls.Remove(myLabel);

                    string labelisim2 = j.ToString()+"a";
                    System.Windows.Forms.Label ourlabel = (System.Windows.Forms.Label)this.Controls[labelisim2];
                    this.Controls.Remove(ourlabel);
                }
                string labelisim3 = "a";
                System.Windows.Forms.Label islabel = (System.Windows.Forms.Label)this.Controls[labelisim3];
                this.Controls.Remove(islabel);

                string labelisim4 = "b";
                System.Windows.Forms.Label thislabel = (System.Windows.Forms.Label)this.Controls[labelisim4];
                this.Controls.Remove(thislabel);

                string labelisim5 = "sigorta";
                System.Windows.Forms.Label thatlabel = (System.Windows.Forms.Label)this.Controls[labelisim5];
                this.Controls.Remove(thatlabel);
            }

            int x = 70;
            int y = 500;
            int xadet = 310;
            int yadet = 500;
            if (ilac.Length != 1)
            {
                string ilacucret = "";
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
                        yazdirma[1] = dr4[7].ToString();
                        ilacucret = dr4[7].ToString();
                    }

                    for (int j = 0; j < 2; j++)
                    {

                        System.Windows.Forms.Label label = new System.Windows.Forms.Label();
                        label.Name = i.ToString() + j.ToString();
                        label.Text = yazdirma[j];
                        label.Location = new Point(x, y);
                        this.Controls.Add(label);
                        label.Width = 60;
                        label.Height = 20;
                        label.BackColor = Color.Transparent;
                        label.Font = new System.Drawing.Font(label.Font.FontFamily, 10);
                        x += 530;

                        
                    }
                    System.Windows.Forms.Label label1 = new System.Windows.Forms.Label();
                    label1.Name = i.ToString()+ "a";
                    label1.Text = "1";
                    label1.Location = new Point(xadet, yadet);
                    this.Controls.Add(label1);
                    label1.Width = 20;
                    label1.Height = 20;
                    label1.BackColor = Color.Transparent;
                    label1.Font = new System.Drawing.Font(label1.Font.FontFamily, 10);
                    xadet += 155;

                    System.Windows.Forms.Label label2 = new System.Windows.Forms.Label();
                    label2.Name = i.ToString() + "b";
                    label2.Text = ilacucret;
                    label2.Location = new Point(xadet, yadet);
                    this.Controls.Add(label2);
                    label2.BackColor = Color.Transparent;
                    label2.Font = new System.Drawing.Font(label2.Font.FontFamily, 10);
                    label2.Width = 20;
                    label2.Height = 20;
                    label2.AutoSize = true;

                    y += 35;
                    x = 70;
                    xadet = 310;
                    yadet += 35;
                    dr4.Close();
                }
                //215; 465

                SqlCommand komut5 = new SqlCommand("Select * From SigortaTablo where SigortaIsim=@sigorta", baglanti);
                komut5.Parameters.AddWithValue("@sigorta", sigortaisim);
                SqlDataReader dr5 = komut5.ExecuteReader();
                while (dr5.Read())
                {
                    sigortaoran = dr5[1].ToString();
                }
                System.Windows.Forms.Label label4 = new System.Windows.Forms.Label();
                label4.Name = "sigorta";
                label4.Text = sigortaisim + " sayesinde " + sigortaoran + " oranında indirim ile yeni ücret = " + ucret;
                label4.Location = new Point(47, 659);
                this.Controls.Add(label4);
                label4.BackColor = Color.Transparent;
                label4.Font = new System.Drawing.Font(label4.Font.FontFamily, 10);
                label4.AutoSize = true;


                double kdv = Convert.ToDouble(ucret) * 18 / 100;
                double hesap = Convert.ToDouble(ucret) - kdv;
                label3.Text = hesap.ToString();
                kdvlabel.Text =kdv.ToString();
                label5.Text = ucret;

                switch (Convert.ToInt32(ucret) / 1000)
                {
                    case 9: ondalıklabel.Text +=("Dokuzbin "); break;
                    case 8: ondalıklabel.Text +=("Sekizbin "); break;
                    case 7: ondalıklabel.Text +=("Yedibin "); break;
                    case 6: ondalıklabel.Text +=("Altıbin "); break;
                    case 5: ondalıklabel.Text +=("Beşbin "); break;
                    case 4: ondalıklabel.Text += ("Dörtbin "); break;
                    case 3: ondalıklabel.Text += ("Üçbin "); break;
                    case 2: ondalıklabel.Text += ("İkibin "); break;
                    case 1: ondalıklabel.Text += ("Bin "); break;
                }
                switch (Convert.ToInt32(ucret) % 1000 / 100)
                {
                    case 9: ondalıklabel.Text += ("Dokuzyüz "); break;
                    case 8: ondalıklabel.Text += ("Sekizyüz "); break;
                    case 7: ondalıklabel.Text += ("Yediyüz "); break;
                    case 6: ondalıklabel.Text += ("Altıyüz "); break;
                    case 5: ondalıklabel.Text += ("Beşyüz "); break;
                    case 4: ondalıklabel.Text += ("Dörtyüz "); break;
                    case 3: ondalıklabel.Text += ("Üçyüz "); break;
                    case 2: ondalıklabel.Text += ("İkiyüz "); break;
                    case 1: ondalıklabel.Text += ("Yüz "); break;
                }
                switch ((Convert.ToInt32(ucret) % 100) / 10)
                {
                    case 9: ondalıklabel.Text += ("Doksan "); break;
                    case 8: ondalıklabel.Text += ("Seksen "); break;
                    case 7: ondalıklabel.Text += ("Yetmiş "); break;
                    case 6: ondalıklabel.Text += ("Altmış "); break;
                    case 5: ondalıklabel.Text += ("Elli "); break;
                    case 4: ondalıklabel.Text += ("Kırk "); break;
                    case 3: ondalıklabel.Text += ("Otuz "); break;
                    case 2: ondalıklabel.Text += ("Yirmi "); break;
                    case 1: ondalıklabel.Text += ("On "); break;
                }
                switch ((Convert.ToInt32(ucret) % 10))
                {
                    case 9: ondalıklabel.Text += ("Dokuz "); break;
                    case 8: ondalıklabel.Text += ("Sekiz "); break;
                    case 7: ondalıklabel.Text += ("Yedi "); break;
                    case 6: ondalıklabel.Text += ("Altı "); break;
                    case 5: ondalıklabel.Text += ("Beş "); break;
                    case 4: ondalıklabel.Text += ("Dört "); break;
                    case 3: ondalıklabel.Text += ("Üç "); break;
                    case 2: ondalıklabel.Text += ("İki "); break;
                    case 1: ondalıklabel.Text += ("Bir "); break;
                }
                if (Convert.ToInt32(ucret) == 0)
                    ondalıklabel.Text += ("Sıfır ");
            }



            else
            {
                string ilacucret="";
                labelsayac = 0;
                SqlCommand komut4 = new SqlCommand("Select * From IlacListe where ilacid=@komut", baglanti);
                komut4.Parameters.AddWithValue("@komut", ilac[0]);
                SqlDataReader dr4 = komut4.ExecuteReader();
                while (dr4.Read())
                {
                    yazdirma[0] = dr4[4].ToString();
                    yazdirma[1] = dr4[7].ToString();
                    ilacucret = dr4[7].ToString();
                }
                
                for (int j = 0; j < 2; j++)
                {

                    System.Windows.Forms.Label label = new System.Windows.Forms.Label();
                    label.Name = j.ToString();
                    label.Text = yazdirma[j];
                    label.Location = new Point(x, y);
                    this.Controls.Add(label);
                    label.Width = 60;
                    label.BackColor = Color.Transparent;
                    label.Font = new System.Drawing.Font(label.Font.FontFamily, 10);
                    label.Height = 20;
                    x += 530;

                    

                }
                System.Windows.Forms.Label label1 = new System.Windows.Forms.Label();
                label1.Name ="a";
                label1.Text = "1";
                label1.Location = new Point(xadet, yadet);
                this.Controls.Add(label1);
                label1.Width = 20;
                label1.Height = 20;
                label1.BackColor = Color.Transparent;
                label1.Font = new System.Drawing.Font(label1.Font.FontFamily, 10);
                xadet += 70;

                System.Windows.Forms.Label label2 = new System.Windows.Forms.Label();
                label2.Name = "b";
                label2.Text = ilacucret;
                label2.Location = new Point(xadet, yadet);
                this.Controls.Add(label2);
                label2.BackColor = Color.Transparent;
                label2.Font = new System.Drawing.Font(label2.Font.FontFamily, 10);
                label2.Width = 20;
                label2.Height = 20;
                label2.AutoSize = true;


                y += 35;
                x = 70;
                xadet = 310;
                yadet += 35;
                dr4.Close();

                SqlCommand komut5 = new SqlCommand("Select * From SigortaTablo where SigortaIsim=@sigorta", baglanti);
                komut5.Parameters.AddWithValue("@sigorta", sigortaisim);
                SqlDataReader dr5 = komut5.ExecuteReader();
                while (dr5.Read())
                {
                    sigortaoran = dr5[1].ToString();
                }
                System.Windows.Forms.Label label4 = new System.Windows.Forms.Label();
                label4.Name = "sigorta";
                label4.Text = sigortaisim + " sayesinde "+sigortaoran+" oranında indirim ile yeni ücret = " + ucret;
                label4.Location = new Point(47, 659);
                this.Controls.Add(label4);
                label4.AutoSize = true;
                label4.BackColor = Color.Transparent;
                label4.Font = new System.Drawing.Font(label4.Font.FontFamily, 10);


                double kdv = Convert.ToDouble(ucret) * 18 / 100;
                double hesap = Convert.ToDouble(ucret) - kdv;
                label3.Text = hesap.ToString();
                kdvlabel.Text = kdv.ToString();
                label5.Text = ucret;

                switch (Convert.ToInt32(ucret) / 1000)
                {
                    case 9: ondalıklabel.Text += ("Dokuzbin "); break;
                    case 8: ondalıklabel.Text += ("Sekizbin "); break;
                    case 7: ondalıklabel.Text += ("Yedibin "); break;
                    case 6: ondalıklabel.Text += ("Altıbin "); break;
                    case 5: ondalıklabel.Text += ("Beşbin "); break;
                    case 4: ondalıklabel.Text += ("Dörtbin "); break;
                    case 3: ondalıklabel.Text += ("Üçbin "); break;
                    case 2: ondalıklabel.Text += ("İkibin "); break;
                    case 1: ondalıklabel.Text += ("Bin "); break;
                }
                switch (Convert.ToInt32(ucret) % 1000 / 100)
                {
                    case 9: ondalıklabel.Text += ("Dokuzyüz "); break;
                    case 8: ondalıklabel.Text += ("Sekizyüz "); break;
                    case 7: ondalıklabel.Text += ("Yediyüz "); break;
                    case 6: ondalıklabel.Text += ("Altıyüz "); break;
                    case 5: ondalıklabel.Text += ("Beşyüz "); break;
                    case 4: ondalıklabel.Text += ("Dörtyüz "); break;
                    case 3: ondalıklabel.Text += ("Üçyüz "); break;
                    case 2: ondalıklabel.Text += ("İkiyüz "); break;
                    case 1: ondalıklabel.Text += ("Yüz "); break;
                }
                switch ((Convert.ToInt32(ucret) % 100) / 10)
                {
                    case 9: ondalıklabel.Text += ("Doksan "); break;
                    case 8: ondalıklabel.Text += ("Seksen "); break;
                    case 7: ondalıklabel.Text += ("Yetmiş "); break;
                    case 6: ondalıklabel.Text += ("Altmış "); break;
                    case 5: ondalıklabel.Text += ("Elli "); break;
                    case 4: ondalıklabel.Text += ("Kırk "); break;
                    case 3: ondalıklabel.Text += ("Otuz "); break;
                    case 2: ondalıklabel.Text += ("Yirmi "); break;
                    case 1: ondalıklabel.Text += ("On "); break;
                }
                switch ((Convert.ToInt32(ucret) % 10))
                {
                    case 9: ondalıklabel.Text += ("Dokuz "); break;
                    case 8: ondalıklabel.Text += ("Sekiz "); break;
                    case 7: ondalıklabel.Text += ("Yedi "); break;
                    case 6: ondalıklabel.Text += ("Altı "); break;
                    case 5: ondalıklabel.Text += ("Beş "); break;
                    case 4: ondalıklabel.Text += ("Dört "); break;
                    case 3: ondalıklabel.Text += ("Üç "); break;
                    case 2: ondalıklabel.Text += ("İki "); break;
                    case 1: ondalıklabel.Text += ("Bir "); break;
                }
                if (Convert.ToInt32(ucret) == 0)
                    ondalıklabel.Text += ("Sıfır ");
            }
        }

        public void FormToPDF(string fileName)
        {
            
            Document doc = new Document(new iTextSharp.text.Rectangle(707, 1000), 0, 0, 0, 0);

            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(fileName, FileMode.Create));

            doc.Open();

            PdfContentByte cb = writer.DirectContent;

            Bitmap bmp = new Bitmap(this.Width-20, this.Height-40);
            Graphics graphics = Graphics.FromImage(bmp);
            graphics.CopyFromScreen(this.PointToScreen(Point.Empty), Point.Empty, this.Size);

            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(bmp, System.Drawing.Imaging.ImageFormat.Bmp);
            img.ScalePercent(100);
            img.SetAbsolutePosition(doc.PageSize.Width / 2 - img.ScaledWidth / 2, doc.PageSize.Height / 2 - img.ScaledHeight / 2);

            cb.AddImage(img);

            doc.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglanti = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=eczaneotomasyon;Integrated Security=True");
            baglanti.Open();
            string ad = "";
            string tc= "";
            SqlCommand komut2 = new SqlCommand("Select * From ReceteTablo where Barkod=@barkod", baglanti);
            komut2.Parameters.AddWithValue("@barkod", label4.Text);
            SqlDataReader dr2 = komut2.ExecuteReader();
            while (dr2.Read())
            {
                tc = dr2[1].ToString();
            }
            dr2.Close();
            SqlCommand komut3 = new SqlCommand("Select * From MusteriTablo where MusteriTC=@barkod", baglanti);
            komut3.Parameters.AddWithValue("@barkod", tc);
            SqlDataReader dr3 = komut3.ExecuteReader();
            while (dr3.Read())
            {
                ad = dr3[1].ToString();
            }
            dr3.Close();

            label2.Hide();
            hastaBox1.Hide();
            receteBox2.Hide();
            button1.Hide();
            button2.Hide();

            string pdfFilePath = @"C:\Users\Mehmet Can Koç\Desktop\pdf\"+ad+tc+label4.Text+".pdf";
            FormToPDF(pdfFilePath);

            label2.Show();
            hastaBox1.Show();
            receteBox2.Show();
            button1.Show();
            button2.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this.ClientSize.Width.ToString()+"+"+ this.ClientSize.Height.ToString());
        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            KullaniciPaneli kpaneli = new KullaniciPaneli(kullaniciAdi, kid);
            kpaneli.Show();
            this.Hide();
        }
    }
}

