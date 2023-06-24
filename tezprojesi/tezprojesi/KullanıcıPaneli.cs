using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tezprojesi
{
    public partial class KullaniciPaneli : Form
    {
        

        public KullaniciPaneli(string kullaniciAdi, string kid)
        {
            InitializeComponent();
            this.kullaniciAdi = kullaniciAdi;
            this.kid = kid;
        }

        private string kullaniciAdi;
        private string kid;
        private void button1_Click(object sender, EventArgs e)
        {
            Musteriİslemleri mislemleri = new Musteriİslemleri(kullaniciAdi, kid);
            mislemleri.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ReceteCikti rislemleri = new ReceteCikti(kullaniciAdi,kid);
            rislemleri.Show();
            this.Hide();
        }

        private void KullaniciPaneli_Load(object sender, EventArgs e)
        {
            label2.Text ="İşlemi Yapan Kullanıcı; "+ kid+", "+kullaniciAdi;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ReceteGiris rgiris = new ReceteGiris(kullaniciAdi, kid);
            rgiris.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Main main = new Main();
            main.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            IlacStok istok = new IlacStok(kullaniciAdi, kid);
            istok.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Fatura form = new Fatura(kullaniciAdi, kid);
            form.Show();
            this.Hide();
        }
    }
}
