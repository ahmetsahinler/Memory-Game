using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Media;

namespace Eşleştirme_Oyunu
{
    public partial class Form1 : Form
    {
        SoundPlayer sound = new SoundPlayer();
        Image[] Images = new Image[] { Properties.Resources.volumeOn, Properties.Resources.volumeOff };
        List<Control> liste = new List<Control>();
        List<string> listcopy = new List<string>();
        List<string> list = new List<string>()
        {
            "a","b","c","d","e","f","g","h","ı","i","j","k",
            "a","b","c","d","e","f","g","h","ı","i","j","k"
        };
        
        
        Timer t = new Timer();
        Timer t2 = new Timer();
        
        public Form1()
        {
            InitializeComponent();
        }
      
        int saniye = 0;
        int dakika = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            saniye++;
            if (saniye == 600)
            {
                saniye = 0;
                dakika++;
            }
            label1.Text = "GEÇEN SÜRE:"+ dakika.ToString() + ":" + saniye.ToString();
            
        }
        
        
        int wrong = 0;
        int sayac = 0;
        Random rnd = new Random();
        int rndindex;
        Button btn;
        private void show()
        {

            listcopy.Clear();
            foreach (string item in list)
            {
                listcopy.Add(item);
            }
            foreach (var item in Controls)
            {
                if (button26 != item && button25 != item && item != pictureBox1 && item != label1 && item !=label5)
                {
                    liste.Add((Button)item);
                }
            }
            
            foreach (Button item in liste)
            {
                btn = item as Button;
                rndindex = rnd.Next(listcopy.Count);
                btn.Text = listcopy[rndindex];
                btn.ForeColor = Color.Black;
                listcopy.RemoveAt(rndindex);
                btn.Visible= true;
            }
            label5.Text = "HATA SAYISI: " + wrong.ToString();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (var item in Controls)
            {
                if (button26 != item && button25 != item && item != pictureBox1 && item != label1 && item != label5)
                {
                    btn = item as Button;
                    btn.Visible= false;
                }
            }
        }

        private void TimerEvent(object sender, EventArgs e)
        {
            t.Stop();
            foreach (Button item in liste)
            {
                item.ForeColor = item.BackColor;
            }
        }
        Button birinci, ikinci;
        private void Button_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (birinci==null)
            {
                birinci = btn;
                birinci.ForeColor= Color.Black;
                birinci.Enabled= false;
                return;
            }
            ikinci= btn;
            ikinci.ForeColor= Color.Black;
            ikinci.Enabled= false;
            if (birinci.Text==ikinci.Text)
            {
                birinci.ForeColor= Color.Black;
                ikinci.ForeColor= Color.Black;
                birinci.Visible= false;
                ikinci.Visible= false;
                birinci = null; ikinci = null;
                sayac++;
                if ((liste.Count/2) == sayac)
                {
                    timer1.Stop();
                    DialogResult dialogResult = MessageBox.Show(dakika.ToString() + ":" + saniye.ToString() + " SÜREDE " + wrong.ToString() + " HATAYLA BİTİRDİNİZ.\n TEKRAR DENEMEK İSTER MİSİN ?", "TEKBRİKLER", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        liste.Clear();
                        StartEvent(sender, e);
                        wrong = 0; saniye = 0; dakika = 0;
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        Application.Exit();
                    }
                }
            }
            else
            {
                wrong++;
                birinci.Enabled = true;
                ikinci.Enabled = true;
                t2.Start();
                t2.Interval= 300;
                label5.Text = "HATA SAYISI: " + wrong.ToString();
            }
        }
        private void TimerEvent2(object sender, EventArgs e)
        {
            t2.Stop();
            birinci.ForeColor = birinci.BackColor;
            ikinci.ForeColor = ikinci.BackColor;
            birinci = null; ikinci = null;
        }

        private void Restart(object sender, EventArgs e)
        {

            DialogResult dialogResult = MessageBox.Show("OYUNU BIRAKIP, YENİ BİR OYUN AÇMAK İSTEDİĞİNİZE EMİN MİSİNİZ ?", "TEKRAR DENEME", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                wrong = 0; saniye = 0; dakika = 0;
                foreach (Button item in liste)
                {
                    item.Visible = true;
                    item.Enabled = true;
                }
                liste.Clear();
                StartEvent(sender, e);
                
            }
        }
        int click=0;

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            PictureBox box = (PictureBox)sender;
            if (click % 2 == 1)
            {
                box.Image = Images[0];
                sound.Play();
            }
            else
            {
                box.Image = Images[1];
                sound.Stop();
            }
            click++;
        }

        private void StartEvent(object sender, EventArgs e)
        {
            button26.Enabled = true;
            t.Stop();
            sound.SoundLocation = "song.wav";
            sound.Play();
            button25.Enabled = false;
            timer1.Start(); 
            t.Tick += TimerEvent;
            t.Start();
            t.Interval = 2000;
            show();
            t2.Tick += TimerEvent2;
        }
    }
}
