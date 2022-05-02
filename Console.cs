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
    public partial class Console : Form
    {
        public static Console Current { get; private set; }

        public Console()
        {
            Current = this;
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Clear();this.Hide();
        }

        private void Console_Load(object sender, EventArgs e)
        {
          
        }

        public void Write(string text)=>listBox1.Items.Add(text);
        public void Clear() => listBox1.Items.Clear();


        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            pictureBox1.Size += new Size(2, 2);
            pictureBox1.Location -= new Size(1, 1);
        }
    }
}
