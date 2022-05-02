using DarkUI.Docking;
using DarkUI.Forms;
using DarkUI.Win32;
using Pack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TDM.Dialogs;
using static TDM.TxtDownloader;

namespace TDM
{

    public enum MethodType
    {
        Download,
        Upload
    }

    public partial class Main : Form
    {
        public static Main Current;

        private List<DarkDockContent> _toolWindows = new List<DarkDockContent>();
        private Descargas downloadDocument;
        private Descargas uploadDocument;
        public static MethodType Method = MethodType.Download;

        public Main()
        {
            Current = this;
            InitializeComponent();


            Application.AddMessageFilter(new ControlScrollFilter());
            Application.AddMessageFilter(DockPanel.DockContentDragFilter);
            Application.AddMessageFilter(DockPanel.DockResizeFilter);


            _toolWindows.Add(uploadDocument = new Descargas() { DockText = "Subidas", Type = 1 });
            _toolWindows.Add(downloadDocument = new Descargas() { DockText = "Descargas" });


            foreach (var toolWindow in _toolWindows)
                DockPanel.AddContent(toolWindow);
        }


        public static List<LinkData> Links = new List<LinkData>();
        public static List<DownloadControl> DownControls = new List<DownloadControl>();
        public static List<DownloadControl> LastDownControls = new List<DownloadControl>();

        public static List<DownloadControl> UploadControls = new List<DownloadControl>();


        public static Panel MainContentPanel;
        public static Panel MainUploadContentPanel;
        public static int TotalDownloads => DownControls.Count;
        public static int CompleteDownloads = 0;
        public int CurrenDownloads = 0;
        public bool closed = false;

        public void Closing()
        {
            closed = true;
            Process.GetCurrentProcess().Kill();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            ServicePointManager.DefaultConnectionLimit = 1000;


            this.FormClosing += (s, ee) => { Closing(); };
            this.FormClosed += (s, ee) => { Closing(); };

            MainContentPanel = downloadDocument.GetContentPanel();
            MainUploadContentPanel = uploadDocument.GetContentPanel();

            CheckForIllegalCrossThreadCalls = false;


            string adbDir = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/adb";
            if (!Directory.Exists(adbDir))
            {
                Directory.CreateDirectory(adbDir);
                Package.Extract(Properties.Resources.adb, adbDir);
            }


            CreateNotifyIcon();

            LoadDefaulsTelegramTxt();

            numericUpDown1.Value = (decimal)Properties.Settings.Default.MaxDownloads;

            UpdateTimer.Start();
            UpdateSpeedTimer.Start();


        }

        public static NotifyIcon notify = new NotifyIcon();
        public static bool showing = true;
        private void CreateNotifyIcon()
        {


            var menu = new ContextMenu();
            notify.Visible = true;
            notify.BalloonTipIcon = ToolTipIcon.Info;
            notify.ShowBalloonTip(5000, "Bienvenidos a TxTLinkDownload", "HI", ToolTipIcon.Info);

            menu.MenuItems.Add("Opciones", options_Click);
            menu.MenuItems.Add("E&xit", (s, e) => { Application.Exit(); });

            notify.ContextMenu = menu;
            notify.Icon = this.Icon;
            notify.Text = "TLD";
            notify.Visible = true;



            notify.DoubleClick += (s, e) => {
                showing = !showing;
                this.Visible = showing;
            };
        }
        private void btnStart_MouseHover(object sender, EventArgs e)
        {
            btnStart.Location -= new Size(1, 1);
            btnStart.Size += new Size(2, 2);
        }
        private void btnStart_MouseLeave(object sender, EventArgs e)
        {
            btnStart.Location += new Size(1, 1);
            btnStart.Size -= new Size(2, 2);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (DownControls.Count > 0 || UploadControls.Count > 0)
            {
               if (!Directory.Exists(Properties.Settings.Default.DownloadPath))
                {
                    Directory.CreateDirectory(Properties.Settings.Default.DownloadPath);
                }

                if (Pause)
                {
                    foreach (var d in DownControls)
                    {
                        if (d.pause)
                        {
                            d.Reanudate();
                        }
                    }
                    btnStart.Image = Properties.Resources.ic_pause_action_light;
                    RealoadList.Enabled = false;
                }
                else
                {
                    foreach (var d in DownControls)
                    {
                        if(d.Downloading && !d.IsComplete)
                        d.AbortDownload();
                    }
                    btnStart.Image = Properties.Resources.ic_play_arrow_action_light;
                    RealoadList.Enabled = true;
                }
                Pause = !Pause;

                MaxUploads = UploadControls.Count;
            }
        }
        int MaxUploads = 0;

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            LoadTxt();
        }

        public void LoadTxt()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "TxtFile|*.txt*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                loadText(ofd.FileName);
            }
        }

        public void loadText(string path)
        {
            TxtDownloader dw = new TxtDownloader(path);
            dw.Foreach(data => {
                AddDownloaderControl(data);
            });
            LastDownControls.AddRange(DownControls);
        }
        public void loadTld(string path)
        {
            TxtDownloader dw = new TxtDownloader(path,true);
            dw.Foreach(data => {
                AddDownloaderControl(data);
            });
            LastDownControls.AddRange(DownControls);
        }

        int yStep = 0;
        int yOffcet = 5;
        int wStep = 10;
        public void AddDownloaderControl(LinkData data)
        {
            DownloadControl control = new DownloadControl();
            control.Location = new Point(wStep, yStep);
            control.Size = new Size(MainContentPanel.Size.Width - wStep * 2  - 20, control.Size.Height);
            control.setName(data.Name);
            control.setSize("esperando...");
            control.data = data;
            control.Charge();
            yStep += control.Height + yOffcet;
            DownControls.Add(control);
            MainContentPanel.Controls.Add(control);
            control.LoadDownImage();
        }

        int yStep2 = 0;
        int yOffcet2 = 5;
        int wStep2 = 10;
        public void AddUploaderControl(LinkData data)
        {
            DownloadControl control = new DownloadControl();
            control.Location = new Point(wStep2, yStep2);
            control.Size = new Size(MainContentPanel.Size.Width - wStep2 * 2 - 20, control.Size.Height);
            control.setName(data.Name);
            control.setSize("esperando...");
            control.data = data;
            yStep2 += control.Height + yOffcet2;
            UploadControls.Add(control);
            MainUploadContentPanel.Controls.Add(control);
            control.LoadImage();
        }


        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadTxt();
        }

        private void options_Click(object sender, EventArgs e)
        {
            AuthDialog dialog = new AuthDialog();
            dialog.StartPosition = FormStartPosition.CenterParent;
            dialog.SetAuth(Properties.Settings.Default.authorization);
            dialog.SetTel(Properties.Settings.Default.NumberPhone);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                dialog.SaveToSetings();
                if (Directory.Exists("temp"))
                {
                    DirectoryInfo di = new DirectoryInfo("temp");
                    foreach (var f in di.GetFiles())
                        f.Delete();
                    di.Delete();
                }
            }
        }

        

        private void btnClose_Click(object sender, EventArgs e)
        {
            // this.Visible = false;

          
        }

       
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            DownControls.Clear();
            Links.Clear();
            UploadControls.Clear();
            downloadDocument.Clear();
            uploadDocument.Clear();


            notify.Visible = false;
            notify?.Dispose();
            notify = default;
            Close();
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

        private void todusLinkerDownloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("TxT Link Downloader: \nEs una app desarrolada para las descargas S3 a trves del todus\n" +
                "El Jefaso: @Lioner\n" +
                "Desarrollo:\n@Obed Garcia (Obysoft)\n" +
                "@Nyan\n" +
                "Ayuda Visual:\n" +
                "@EL_MAS_TAIGER\n" +
                "@Elier\n" +
                "Canal Oficial : https://t.me/TLD_Oficial_Ayuda_y_Soporte",
                "Acerca De",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Pause = true;
            CurrenDownloads = 0;
            yStep = 0;
            yStep2 = 0;
            Links.Clear();
            DownControls.Clear();
            UploadControls.Clear();
            LastDownControls.Clear();
            MainContentPanel.Controls.Clear();
            MainUploadContentPanel.Controls.Clear();
        }

        private void btnClear_MouseEnter(object sender, EventArgs e)
        {
            btnClear.Size += new Size(2, 2);
            btnClear.Location -= new Size(1, 1);
        }

        private void btnClear_MouseLeave(object sender, EventArgs e)
        {
            btnClear.Size -= new Size(2, 2);
            btnClear.Location += new Size(1, 1);
        }
        Console c = new Console();
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            c.Show();
        }


        int actualSign = 0;
        bool Pause = true;
        List<DownloadControl> FinishUploads = new List<DownloadControl>();
        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                int max = Properties.Settings.Default.MaxDownloads;
                if (DownControls.Count > 0)
                {
                    for(int i=0;i< DownControls.Count; i++)
                    {
                        if (DownControls[i].Closing)
                        {
                            DownControls.RemoveAt(i);
                            MainContentPanel.Controls.RemoveAt(i);
                            int step = DownControls[i].Height + yOffcet;
                            for (int j = i; j < DownControls.Count; j++)
                            {
                                DownControls[j].Location -= new Size(0, step);
                            }

                            yStep -= step;
                            if (DownControls.Count <= 0)
                                yStep = 0;
                        }
                    }
                }
                if (!Pause)
                {
                    if (Method== MethodType.Download)
                    for (int i = 0; i < max; i++)
                    {
                        if (i >= DownControls.Count)
                        {
                            btnStart.Image = Properties.Resources.ic_play_arrow_action_light;
                            Pause = true;
                            continue;
                        }
                        DownloadControl dc = DownControls[i];
                        if (!dc.Downloading)
                        {
                            if (!dc.IsComplete && actualSign <= 0)
                            {
                                dc.onValidate += (s) =>
                                {
                                    actualSign = 0;
                                };
                                dc.onComplete += (s) =>
                                {
                                    DownControls.Remove(s);
                                    actualSign = 0;
                                };
                                dc.onFaild += (s, err) =>
                                {
                                    actualSign = 0;
                                    DownControls.Remove(s);
                                    RealoadList.Enabled = true;
                                };
                                dc.StartDownload(ConfigWin.authorizationType,
                                Properties.Settings.Default.authorization, ConfigWin.userAgent);
                                actualSign = 1;
                            }
                        }
                    }
                    if (Method == MethodType.Upload)
                    {
                        for (int i = 0; i < max; i++)
                        {

                            if (i >= UploadControls.Count)
                            {
                                btnStart.Image = Properties.Resources.ic_play_arrow_action_light;
                                Pause = true;
                                continue;
                            }
                            DownloadControl dc = UploadControls[i];

                            if (dc.Upload) continue;

                            if (!dc.Downloading)
                            {
                                if (!dc.IsComplete && actualSign <= 0)
                                {
                                    dc.onValidate += (s) =>
                                    {
                                        actualSign = 0;
                                    };
                                    dc.onComplete += (s) =>
                                    {
                                        FinishUploads.Add(s);
                                        if (FinishUploads.Count >= MaxUploads)
                                        {
                                            SaveFileDialog sfd = new SaveFileDialog();
                                            sfd.Title = "Guarde Su Txt Creado Como";
                                            string saveName = "";
                                            if (sfd.ShowDialog() == DialogResult.OK)
                                                saveName = sfd.FileName;

                                            if (!ModoTLD)
                                            {
                                                if (saveName == "")
                                                    if (!saveName.EndsWith(".txt"))
                                                    {
                                                        saveName = $"Txts/{FinishUploads[0].data.Name}.txt";
                                                    }
                                                    else
                                                    {
                                                        saveName = $"Txts/{FinishUploads[0].data.Name}";
                                                    }
                                                else
                                                        if (!saveName.EndsWith(".txt"))
                                                {
                                                    saveName += $".txt";
                                                }
                                            }
                                            else
                                            {
                                                if (saveName != "")
                                                {
                                                    if (!saveName.EndsWith(".txt"))
                                                        saveName += ".txt";
                                                }
                                                else
                                                {
                                                    if (!saveName.EndsWith(".txt"))
                                                        saveName = $"Txts/{FinishUploads[0].data.Name}.txt";
                                                    else
                                                        saveName = $"Txts/{FinishUploads[0].data.Name}";
                                                }
                                            }
                                            

                                            string content = "";
                                            if (!ModoTLD)
                                            {
                                                foreach (var c in FinishUploads)
                                                {
                                                    if (c.getUrl != "")
                                                    {
                                                        content += $"{c.getUrl}\t{c.data.Name}\n";
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                foreach (var c in FinishUploads)
                                                {
                                                    if (c.getUrl != "")
                                                    {
                                                        string name = c.data.Name;
                                                        string url = c.getUrl;
                                                        long size = c.data.Size;
                                                        DateTime date = DateTime.Now;
                                                        string timespan = $"{date.Day+1}-{date.Month}-{date.Year}:{date.Hour}-{date.Minute}-{date.Second}";
                                                        using (JsonWriter writer = new JsonWriter())
                                                        {
                                                            writer.Begin();
                                                            writer.Write("name", name);
                                                            writer.Write("url", url);
                                                            writer.Write("size", size);
                                                            writer.Write("timespan", timespan);
                                                            writer.End();
                                                            content += writer.Writer+"\n";
                                                        }
                                                    }
                                                }
                                            }

                                            if (!Directory.Exists("Txts"))
                                                Directory.CreateDirectory("Txts");

                                            File.WriteAllText(saveName, content);
                                            FinishUploads.Clear();
                                        }
                                        actualSign = 0;
                                        UploadControls.Remove(s);
                                    };
                                    dc.onFaild += (s, err) =>
                                    {
                                        actualSign = 0;
                                        UploadControls.Remove(s);
                                        RealoadList.Enabled = true;
                                    };
                                    dc.StartUpload();
                                    actualSign = 1;
                                }
                            }
                        }
                    }
                }
                SearchNewTxtInTelegram();
                FixedAllDownloads();
                this.Update();
            }
            catch (Exception ex){ ErrorLog.CreateError( ErrorLogType.Other, ex.Message); }
        }

        private void FixedAllDownloads()
        {
            if (!Directory.Exists(Properties.Settings.Default.DownloadPath)) return;
            DirectoryInfo di = new DirectoryInfo(Properties.Settings.Default.DownloadPath);
            foreach (var fi in di.GetFiles())
            {
                if (fi.Name.EndsWith(".temp"))
                {
                    bool isread = false;
                    try
                    {
                        Stream stream = fi.OpenRead();
                        stream.Close();
                        isread = true;
                    }
                    catch { }
                    if (isread)
                    {
                        if (File.Exists(fi.Directory.FullName + $"\\{fi.Name.Remove(fi.Name.LastIndexOf("."))}"))
                        {
                            using (Stream temp = File.OpenRead(fi.FullName))
                            {
                                using (Stream original = File.OpenWrite(fi.Directory.FullName + $"\\{fi.Name.Remove(fi.Name.LastIndexOf("."))}"))
                                {
                                    original.Position = original.Length;
                                    using (BinaryReader reader = new BinaryReader(temp))
                                    {
                                        using (BinaryWriter writer = new BinaryWriter(original))
                                        {
                                            writer.Write(reader.ReadBytes((int)temp.Length));
                                        }
                                    }
                                }
                            }
                            File.Delete(fi.FullName);
                        }
                        else
                        {
                            File.Move(fi.FullName, fi.Directory.FullName + $"\\{fi.Name.Remove(fi.Name.LastIndexOf("."))}");
                        }
                    }
                }
            }
        }

        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {
            RealoadList.Size += new Size(2, 2);
            RealoadList.Location -= new Size(1, 1);
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            RealoadList.Size -= new Size(2, 2);
            RealoadList.Location += new Size(1, 1);
        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            Pause = true;
            actualSign = 0;
            btnStart.Image = Properties.Resources.ic_play_arrow_action_light;
            yStep = 0;
            foreach (var d in DownControls)
            {
                d.AbortDownload();
            }
            DownControls.Clear();
            MainContentPanel.Controls.Clear();
            for(int i=0;i< LastDownControls.Count;i++)
                AddDownloaderControl(LastDownControls[i].data);
        }

        private void pictureBox3_MouseEnter(object sender, EventArgs e)
        {
            pictureBox3.Size += new Size(2, 2);
            pictureBox3.Location -= new Size(1, 1);
        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            pictureBox3.Size -= new Size(2, 2);
            pictureBox3.Location += new Size(1, 1);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if(Directory.Exists(Properties.Settings.Default.DownloadPath))
            Process.Start(Properties.Settings.Default.DownloadPath);
            else
            {
                if(MessageBox.Show("Carpeta de descarga no establecida \nDesea Establecer su Ruta de Descarga?","Ruta de Descarga", MessageBoxButtons.YesNo,MessageBoxIcon.Information)==DialogResult.Yes)
                {
                    SetDownloadPath();
                }
            }
        }

        public void SetDownloadPath()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog()==DialogResult.OK)
            {
                if (!Directory.Exists(fbd.SelectedPath))
                    Directory.CreateDirectory(fbd.SelectedPath);
                Properties.Settings.Default.DownloadPath = fbd.SelectedPath;
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Upgrade();
            }
        }

        private void btnTelegram_MouseEnter(object sender, EventArgs e)
        {
            btnTelegram.Size += new Size(2, 2);
            btnTelegram.Location -= new Size(1, 1);
        }

        private void btnTelegram_MouseLeave(object sender, EventArgs e)
        {
            btnTelegram.Size -= new Size(2, 2);
            btnTelegram.Location += new Size(1, 1);
        }

        private void btnTelegram_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = "TxtFile|*.txt*";
            ofd.InitialDirectory = GetTelegramPath();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string[] files = ofd.FileNames;
                foreach (var f in files)
                    loadText(f);
            }
        }

        public string GetTelegramPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) +
                "\\Downloads\\Telegram Desktop";
        }


        //Move Windows
        public bool moveWindows = false;
        public Point lasPoint = new Point();
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

        private void btnAddLink_MouseEnter(object sender, EventArgs e)
        {
            btnAddLink.Size += new Size(2, 2);
            btnAddLink.Location -= new Size(1, 1);
        }

        private void btnAddLink_MouseLeave(object sender, EventArgs e)
        {
            btnAddLink.Size -= new Size(2, 2);
            btnAddLink.Location += new Size(1, 1);
        }

        private void btnAddLink_Click(object sender, EventArgs e)
        {
            string clipboardText = Clipboard.GetText();
            if (clipboardText != "")
            {
                LinkData data = TxtDownloader.GetDataFromLink(clipboardText);
                if (data != null)
                    AddDownloaderControl(data);
                else
                {
                    //Aca abro la ventana
                }
            }
        }

        public List<string> TelegramTxts = new List<string>();
        public void LoadDefaulsTelegramTxt()
        {
            // notify
            DirectoryInfo di = new DirectoryInfo(GetTelegramPath());
            foreach (var txt in di.GetFiles())
            {
                if (txt.Name.Contains(".txt"))
                {
                    if (!TelegramTxts.Contains(txt.FullName))
                    {
                        TelegramTxts.Add(txt.FullName);
                    }
                }
            }
        }
        public void SearchNewTxtInTelegram()
        {
            // notify
            DirectoryInfo di = new DirectoryInfo(GetTelegramPath());
            if(di.Exists)
            foreach (var txt in di.GetFiles())
            {
                if (txt.Name.Contains(".txt"))
                {
                    if (!TelegramTxts.Contains(txt.FullName))
                    {
                        bool isread = false;
                        FileInfo fi = new FileInfo(txt.FullName);
                        try
                        {
                            fi.OpenRead();
                            isread = true;
                        }
                        catch (Exception ex){ /*ErrorLog.CreateError(this.GetType().Name,ex.Message);*/ }
                        if (isread)
                        {
                            TelegramTxts.Add(txt.FullName);
                            notify.ShowBalloonTip(3000, $"Nuevo Txt Encontrado {txt.Name}",
                                "TLD a encontrado un nuevo archivo de descarga\n" +
                                "y lo a adicionado a sus descargas", ToolTipIcon.Info);
                            loadText(txt.FullName);
                        }
                    }
                }
            }
        }

        private void UpdateSpeedTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < DownControls.Count; i++)
                DownControls[i].UpdateSpeedPerSecond();
        }

        public static bool notifyMe = true;
        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        public static void Notify(int time,string head,string content)
        {
            if(notifyMe)
            notify.ShowBalloonTip(time, head, content, ToolTipIcon.Info);
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int value = (int)numericUpDown1.Value;
            Properties.Settings.Default.MaxDownloads = value;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Upgrade();
        }

        private void rutaDeDescargaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetDownloadPath();
        }

        private void pictureBox4_Click_1(object sender, EventArgs e)
        {
            notifyMe = !notifyMe;
            if (notifyMe)
                pictureBox4.Image = Properties.Resources.ic_notification_history_white;
            else
                pictureBox4.Image = Properties.Resources.ic_notification_history;
        }


        public static bool ModoTLD = false;

        private void newTxtActivator_Click(object sender, EventArgs e)
        {
            ModoTLD = !ModoTLD;

            if (ModoTLD)
            {
                newTxtActivator.Image = Properties.Resources.logoTxt_on;
                Notify(30000, "Modo Json TxT", "este modo es un formato estandarizado q soluciona la mayoria de los errores de lectura y agrega informacion extra.");
            }
            else
            {
                newTxtActivator.Image = Properties.Resources.logoTxt;
            }

        }

        private void autenticacionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AuthDialog auth = new AuthDialog();
            if (auth.ShowDialog() == DialogResult.OK) {
                auth.SaveToSetings();
            }
        }

        private void newTxtActivator_MouseEnter(object sender, EventArgs e)
        {
            newTxtActivator.Size += new Size(2, 2);
            newTxtActivator.Location -= new Size(1, 1);
        }

        private void newTxtActivator_MouseLeave(object sender, EventArgs e)
        {
            newTxtActivator.Size -= new Size(2, 2);
            newTxtActivator.Location += new Size(1, 1);
        }

        private void pictureBox4_MouseEnter(object sender, EventArgs e)
        {
            pictureBox4.Size += new Size(2, 2);
            pictureBox4.Location -= new Size(1, 1);
        }

        private void pictureBox4_MouseLeave(object sender, EventArgs e)
        {
            pictureBox4.Size -= new Size(2, 2);
            pictureBox4.Location += new Size(1, 1);
        }
    }
    }
