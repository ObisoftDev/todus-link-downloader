using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TDM
{
    public partial class ConfigWin : Form
    {
        public static string authorizationType= "Bearer";
        public static string userAgent = "ToDus 0.37.29 HTTP-Download";


        public bool moveWindows = false;
        public Point lasPoint = new Point();


        public ConfigWin()
        {
            InitializeComponent();
        }

        private void ConfigWin_Load(object sender, EventArgs e)
        {
            txtDownloadPath.Text = Properties.Settings.Default.DownloadPath;
            txtType.Text = authorizationType;
            txtAutorization.Text = Properties.Settings.Default.authorization;
            txtNumberPhone.Text = Properties.Settings.Default.NumberPhone;
            txtUserAgent.Text = userAgent;

            Downloads.Value = (Properties.Settings.Default.MaxDownloads>Downloads.Maximum)?Downloads.Maximum:Properties.Settings.Default.MaxDownloads;
            this.FormClosing += new FormClosingEventHandler(ClosingForm);
        }

        private void ClosingForm(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.MaxDownloads = (int)Downloads.Value;
            Properties.Settings.Default.DownloadPath = txtDownloadPath.Text;
            authorizationType = txtType.Text;
            Properties.Settings.Default.authorization = txtAutorization.Text;

            string phone = txtNumberPhone.Text;
            if (phone[0]=='5' && phone[1]=='3')
            Properties.Settings.Default.NumberPhone = txtNumberPhone.Text;
            else
            Properties.Settings.Default.NumberPhone = $"53{txtNumberPhone.Text}";

            userAgent = txtUserAgent.Text;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Upgrade();
        }

        public static void SaveConfig(string phone,string token)
        {
            if (phone[0] == '5' && phone[1] == '3')
                Properties.Settings.Default.NumberPhone = phone;
            else
                Properties.Settings.Default.NumberPhone = $"53{phone}";
            Properties.Settings.Default.authorization = token;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Upgrade();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtDownloadPath.Text = fbd.SelectedPath;
            }
        }

        private void Downloads_ValueChanged(object sender, EventArgs e)
        {
            if (Downloads.Value > 5)
                txtDescargas.Visible = true;
            else
                txtDescargas.Visible = false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            moveWindows = true;
            lasPoint = e.Location;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (moveWindows)
            {
                this.Location = new Point(
                this.Location.X - lasPoint.X + e.X, this.Location.Y - lasPoint.Y + e.Y);
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            moveWindows = false;
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            pictureBox1.Size += new Size(2, 2);
            pictureBox1.Location -= new Size(1, 1);
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.Size -= new Size(2, 2);
            pictureBox1.Location += new Size(1, 1);
        }
    }
}
