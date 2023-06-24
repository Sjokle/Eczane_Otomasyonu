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
    public partial class YoneticiPaneli : Form
    {
        public YoneticiPaneli()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Kullaniciİslemleri form6 = new Kullaniciİslemleri();
            form6.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Main main = new Main();
            main.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
          
        }

        private void YoneticiPaneli_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {

            LogKonrol lkontrol = new LogKonrol();
            lkontrol.Show();
            this.Hide();
        }
    }
}
