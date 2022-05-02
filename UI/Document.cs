using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using DarkUI.Controls;
using DarkUI.Docking;
using DarkUI.Forms;
using DarkUI.Win32;
using System.IO;

namespace TDM
{
    public partial class Descargas : DarkDocument
    {

        public int Type = 0;


        public Descargas()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.DockArea = DarkDockArea.Document;
        }

        private void GameView_Load(object sender, EventArgs e)
        {
            
        }

        public Panel GetContentPanel() => ContentPanel;

        private void ContentPanel_Paint(object sender, PaintEventArgs e)
        {
            if (Type != 1)
            {
                Main.Method = MethodType.Download;
            }
            else
            {
                Main.Method = MethodType.Upload;
            }
        }

        private void ContentPanel_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void ContentPanel_DragDrop(object sender, DragEventArgs e)
        {
            if (Type == 0)
            {
                string[] paths = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (var p in paths)
                {
                    if(p.EndsWith(".txt"))
                    Main.Current.loadText(p);
                    if (p.EndsWith(".tlc"))
                    {
                        using (Stream stream = File.OpenRead(p))
                        {
                            using (BinaryReader reader = new BinaryReader(stream))
                            {
                                string phone = reader.ReadString();
                                string token = reader.ReadString();

                                ConfigWin.SaveConfig(phone, token);
                                Main.Notify(3000, "TLD Se A Actualizado!", "Se a cargado la actualizacion correctamente!");
                            }
                        }
                    }
                    if (p.EndsWith(".tld"))
                    {
                        Main.Current.loadTld(p);
                    }
                }
            }
            else
            {
                string[] paths = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (var p in paths)
                {
                    if (p.EndsWith(".tlc"))
                    {
                        using (Stream stream = File.OpenRead(p))
                        {
                            using (BinaryReader reader = new BinaryReader(stream))
                            {
                                string phone = reader.ReadString();
                                string token = reader.ReadString();
                                ConfigWin.SaveConfig(phone, token);
                                Main.Notify(3000, "TLD Se A Actualizado!", "Se a cargado la actualizacion correctamente!");
                            }
                        }
                        continue;
                    }
                    FileInfo fi = new FileInfo(p);
                    if(fi.Exists)
                    Main.Current.AddUploaderControl(new TxtDownloader.LinkData() {Name=fi .Name,Link=p,Size=fi.Length});
                }
            }
        }

        private void Descargas_Click(object sender, EventArgs e)
        {
            
        }

        internal void Clear()
        {
            ContentPanel.Controls.Clear();
        }
    }
}
