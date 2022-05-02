using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DarkUI.Forms;
using System.IO;

namespace TDM.Dialogs
{
    public partial class AuthDialog : DarkDialog
    {
        public void SetAuth(string auth) => darkTextBox1.Text = auth;
        public void SetTel(string tel) => darkTextBox2.Text = tel;

        public string GetAuth() => darkTextBox1.Text;
        public string GetTel() => darkTextBox2.Text;

        public AuthDialog()
        {
            InitializeComponent();
        }

        private void AuthDialog_Load(object sender, EventArgs e)
        {
            SetAuth(Properties.Settings.Default.authorization);
            SetTel(Properties.Settings.Default.NumberPhone);
        }

        bool infoShow = false;
        private void darkButton2_Click(object sender, EventArgs e)
        {
            infoShow = !infoShow;
            if (infoShow)
            {
                InfoBtn.Image = Properties.Resources.ScrollBarArrowUp;
                this.MaximumSize = new Size(this.Width, 310);
                this.Height = 310;
                InfoTxt.Visible = true;
            }
            else
            {
                InfoBtn.Image = Properties.Resources.ScrollBarArrowDown;
                this.MaximumSize = new Size(this.Width, 198);
                this.Height = 198;
                InfoTxt.Visible = false;
            }
        }
        public void SaveToSetings()
        {
            Properties.Settings.Default.authorization = GetAuth();

            string phone = GetTel();
            if (phone[0] == '5' && phone[1] == '3')
                Properties.Settings.Default.NumberPhone = GetTel();
            else
                Properties.Settings.Default.NumberPhone = $"53{GetTel()}";

            Properties.Settings.Default.Save();
            Properties.Settings.Default.Upgrade();
        }

        private void ReloadAuth_Click(object sender, EventArgs e)
        {
            TodusUtil.ExtractDB(txtPakete.Text);
            string token = TodusUtil.GetToken("temp");
            string movil = TodusUtil.GetPhone("temp");
            if (token == "" && movil == "")
                MessageBox.Show("No se Pudo Recargar el Token Buelba a intentarlo");
            else
            {
                SetAuth(token);
                SetTel(movil);
            }
        }
    }
}
