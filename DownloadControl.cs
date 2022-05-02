using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using static TDM.TxtDownloader;
using System.Net.Cache;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Net.Http;

namespace TDM
{
    public partial class DownloadControl : UserControl
    {
        public static DownloadControl Selected { get; private set; }

        public LinkData data = null;
        public bool IsComplete { get; private set; } = false;
        public bool Downloading = false;

        public void setName(string name) => fileName.Text = name;
        public void setSize(string size) => fileSize.Text = size;

        public long TotalBytes = 0;

        WebClient web = new WebClient();

        public DownloadControl()
        {
            InitializeComponent();
        }

        public delegate void OnValidate(DownloadControl sender);
        public delegate void OnComplete(DownloadControl sender);
        public delegate void OnFaild(DownloadControl sender, string err);
        public OnComplete onComplete { get; set; }
        public OnValidate onValidate { get; set; }
        public OnFaild onFaild { get; set; }




        public Image GetImageType(string filename,string path = "")
        {
            string ext = (filename.Contains('.'))?filename.Substring(filename.LastIndexOf(".")+1):"";
            switch (ext)
            {
                case "apk": return Properties.Resources.android;
                case "exe": return Icon.ExtractAssociatedIcon(path).ToBitmap();
                case "msi": return Properties.Resources.software_installer_uninstall;
                case "rar": return Properties.Resources._12;
                case "zip": return Properties.Resources._12;
                case "mp4": return Properties.Resources.type_video;
                case "avi": return Properties.Resources.type_video;
                case "3gp": return Properties.Resources.type_video;
                case "mpg": return Properties.Resources.type_video;
                case "png": return Properties.Resources.image;
                case "jpg": return Properties.Resources.image;
                case "gif": return Properties.Resources.image;
                default: return Properties.Resources.link;
            }
        }
        public Image GetImageDownType(string filename, string path = "")
        {
            string ext = (filename.Contains('.')) ? filename.Substring(filename.LastIndexOf(".") + 1) : "";
            switch (ext)
            {
                case "apk": return Properties.Resources.android;
                case "msi": return Properties.Resources.software_installer_uninstall;
                case "rar": return Properties.Resources._12;
                case "zip": return Properties.Resources._12;
                case "mp4": return Properties.Resources.type_video;
                case "avi": return Properties.Resources.type_video;
                case "3gp": return Properties.Resources.type_video;
                case "mpg": return Properties.Resources.type_video;
                case "png": return Properties.Resources.image;
                case "jpg": return Properties.Resources.image;
                case "gif": return Properties.Resources.image;
                default: return Properties.Resources.link;
            }
        }



        TodusSigned signer;
        public void StartDownload(string authorizationType,string authorization,string userAgent)
        {
            this.authorizationType = authorizationType;
            this.authorization = authorization;
            this.userAgent = userAgent;

            fileSize.Text = "conectando...";
         
           signer = new TodusSigned(Properties.Settings.Default.NumberPhone,
                   Properties.Settings.Default.authorization, data.Link);
            signer.onErr += OnErr;
            signer.onSigned += OnSigned;
            signer.StartSigned();
          
           // DownloadWebClientFile(data.Link, $"{Properties.Settings.Default.DownloadPath}/{data.Name}");
        }


        public string getUrl = "";
        public bool Upload = false;
        public void StartUpload()
        {
            fileSize.Text = "conectando...";

            FileInfo fi = new FileInfo(data.Link);
            if (fi.Exists) {
                TodusGeter geter = new TodusGeter(Properties.Settings.Default.NumberPhone, Properties.Settings.Default.authorization);
               
                geter.onGeter += (put, get) => {
                    UploadWebClientFile(fi.FullName, put);
                    getUrl = get;
                    if (onValidate != null)
                        onValidate(this);
                };
                geter.onErr += (err) => {
                    fileSize.ForeColor = Color.Red;
                    if (err == ErrorType.Signed)
                    {
                        fileSize.Text = "La Autenticacion no es Valida!";
                        ErrorLog.CreateError(ErrorLogType.NotAutorized, err.ToString());
                    }
                    if (err == ErrorType.Connection)
                    {
                        fileSize.Text = "Error de Coneccion!";
                        ErrorLog.CreateError(ErrorLogType.FailConnectToTodus, err.ToString());
                    }
                    if (onFaild != null)
                        onFaild(this, "Error");
                    Imagen.Image = Properties.Resources.load_more_warning;
                    ErrorLog.CreateError(ErrorLogType.FailConnectToTodus, err.ToString());
                };
                geter.StartGeter(fi.Length);
            }
        }


        private void OnSigned(string signedUrl)
        {
            data.Link = signedUrl;
            signer.CloseSocket();
            LoadDownImage();
            DownloadWebClientFile(signedUrl, $"{Properties.Settings.Default.DownloadPath}/{data.Name}");
        }

        private void OnErr(ErrorType err)
        {
            //Progres.Visible = false;
            fileSize.ForeColor = Color.Red;
            if (err == ErrorType.Signed)
            {
                fileSize.Text = "La Autenticacion no es Valida!";
                ErrorLog.CreateError(ErrorLogType.NotAutorized, err.ToString());
            }
            if (err == ErrorType.Connection)
            {
                fileSize.Text = "Error de Coneccion!";
                ErrorLog.CreateError(ErrorLogType.FailConnectToTodus, err.ToString());
            }
            if (onFaild != null)
                onFaild(this, "Error");

            Imagen.Image = Properties.Resources.load_more_warning;

            ErrorLog.CreateError(ErrorLogType.FailConnectToTodus, err.ToString());
        }

        public void Reanudate()
        {
            DownloadWebClientFile(data.Link, $"{Properties.Settings.Default.DownloadPath}/{data.Name}");
        }

        string authorizationType = "";
        string authorization = "";
        string userAgent = "";
        DateTime lastUpdate = DateTime.Now;
        long lasBytes = 0;
        private void DownloadControl_Load(object sender, EventArgs e)
        {
          
        }

        public void LoadDownImage()
        {
            Imagen.Image = GetImageDownType(data.Name, data.Link);
        }
        public void LoadImage()
        {
            Imagen.Image = GetImageType(data.Name, data.Link);
        }

        public void dispose()
        {
            web.Dispose();
        }
        public static string PrettySize(long size)
        {
            string type = "B";
            long len = size;
            long kilobit = 1024;
            long megabit = kilobit * kilobit;
            long gigabit = megabit * megabit;
            if (len > kilobit && len < megabit)
            {
                len = len / 1000;
                type = "KB";
            }
            if (len > megabit && len < gigabit)
            {
                len = len / 1000000;
                type = "MB";
            }
            if (len > gigabit)
            {
                len = len / 1000000;
                type = "GB";
            }
            return $"{len}{type}";
        }

        private void fileName_Click(object sender, EventArgs e)
        {

        }

        internal void Charge()
        {
            if (data != null)
            {
                if (data.TimeSpan != null)
                {
                    string date = data.TimeSpan.Split(':')[0];
                    date = date.Replace("-", "/");
                    string time = data.TimeSpan.Split(':')[1];
                    time = time.Replace("-", ":");

                    timespan.ForeColor = Color.Orange;
                    timespan.Text = $"Vence {date} {time}";
                }
                if (data.Size != 0)
                {
                    fileSize.Text = $"{PrettySize(data.Size)}";
                }
            }
        }

        public long LastPosition = 0;
        public long SpeedPerSecond = 0;
        public async Task DownloadFile(string sSourceURL, string sDestinationPath)
        {
            if (Downloading) return;

            Downloading = true;
            long iFileSize = 0;
            int iBufferSize = 1024 * 20000;
            long iExistLen = 0;
            FileStream saveFileStream;
            if (File.Exists(sDestinationPath))
            {
                FileInfo fINfo = new System.IO.FileInfo(sDestinationPath);
                iExistLen = fINfo.Length;
            }
            if (iExistLen > 0)
                saveFileStream = new FileStream(sDestinationPath,FileMode.Append,FileAccess.Write,FileShare.ReadWrite);
            else
                saveFileStream = new FileStream(sDestinationPath,FileMode.Create,FileAccess.Write,FileShare.ReadWrite);

            HttpWebRequest hwRq;
            WebResponse hwRes;

           
            hwRq = HttpWebRequest.CreateHttp(sSourceURL);
            hwRq.Timeout = 50000;
            hwRq.AllowAutoRedirect = true;
            hwRq.ContinueDelegate += (s,e) => { };
            hwRq.Headers.Add("authorization", $"{authorizationType} {authorization}");
            hwRq.Headers.Add("accept-encoding", $"gzip");
            hwRq.UserAgent = $"{userAgent}";
            hwRq.AddRange((int)iExistLen);
            Stream smRespStream;
            try
            {
               var newResponse = await hwRq.GetResponseAsync();
               hwRes = newResponse;
            }
            catch (Exception ex)
            {
               // ErrorLog.CreateError(this.GetType().Name, ex.Message);
                IsComplete = true;
                Downloading = false;
                saveFileStream?.Close();
                fileSize.Text = "completado!";
                if (onComplete != null)
                    onComplete(this);
                return;
            }
           
            smRespStream = hwRes.GetResponseStream();

            long totalBytes = (iExistLen <= 0)? hwRes.ContentLength: hwRes.ContentLength+ iExistLen;

            if (saveFileStream.Position >= totalBytes)
            {
                fileSize.Text = $"Completado!";
                if (onComplete != null)
                    onComplete(this);
            }


            iFileSize = hwRes.ContentLength;

            int iByteSize;
            byte[] downBuffer = new byte[iBufferSize];
            try
            {
                btnClose.Enabled = false;
                while ((iByteSize = await smRespStream.ReadAsync(downBuffer, 0, iBufferSize)) > 0)
                {
                    Progres.Maximum = (int)totalBytes;
                    Progres.Value = (int)saveFileStream.Position;
                    //Progres.Visible = true;


                    actualPosition = saveFileStream.Position;

                    fileSize.Text = $"{PrettySize(saveFileStream.Position)} / {PrettySize(totalBytes)} {PrettySize(SpeedPerSecond)}/s";
                    saveFileStream.Write(downBuffer, 0, iByteSize);

                }
                IsComplete = true;
                Downloading = false;

               // Progres.Visible = false;
                btnClose.Enabled = true;
                if (saveFileStream.Position > 0)
                {
                    fileSize.Text = $"completado!";
                }
                else
                {
                    fileSize.ForeColor = Color.Red;
                    fileSize.Text = $"El archivo ya no existe!";
                }
                if (onComplete != null)
                    onComplete(this);
                saveFileStream.Close();
                smRespStream.Close();
                if (Main.notifyMe)
                {
                    Main.Notify(1000,"Descarga Finalizada!",$"La descarga de {data.Name} se a completado");
                }
            }
            catch(Exception ex){
                ErrorLog.CreateError(ErrorLogType.ConectionLost, ex.Message);
                await DownloadFile(data.Link, $"{Properties.Settings.Default.DownloadPath}/{data.Name}");
            }
        }


        WebClientEx cli;
        Stopwatch sw = new Stopwatch();
        long Position = 0;
        public void DownloadWebClientFile(string sSourceURL, string sDestinationPath)
        {
             if (Downloading && !pause) return;
             pause = false;     Downloading = true;
             Position = 0;
             if (File.Exists(sDestinationPath))
             Position = new FileInfo(sDestinationPath).Length;
              cli = new WebClientEx(Position);
              cli.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
              sw.Start();
              cli.Headers.Add("authorization", $"{authorizationType} {authorization}");
              cli.Headers.Add("accept-encoding", $"gzip");
              cli.Headers.Add(HttpRequestHeader.UserAgent, $"ToDus 0.38.35 HTTP-Download");
              cli.DownloadProgressChanged += ProgresChanged;
              cli.DownloadFileCompleted += OnDownloadComplete;
              cli.DownloadFileAsync(new Uri(sSourceURL), sDestinationPath+".temp");
        }

        public void UploadWebClientFile(string file,string putUrl)
        {
            if (Downloading && !pause) return;
            pause = false; Downloading = true;
            WebClient uploadcli = new WebClient();
            string auth = Properties.Settings.Default.authorization;
            uploadcli.Headers.Add("authorization", $"{ConfigWin.authorizationType} {auth}");
            uploadcli.Headers.Add("accept-encoding", $"gzip");
            sw.Start();
            uploadcli.UploadProgressChanged += (s, e) => {
                try
                {
                    Progres.Maximum = (int)(e.TotalBytesToSend);
                    Progres.Value = (int)(e.BytesSent);

                    Progres.Visible = true;

                    long bytesPerSecond = e.BytesReceived;
                    if (_startedAt == default(DateTime))
                    {
                        _startedAt = DateTime.Now;
                    }
                    else
                    {
                        var timeSpan = DateTime.Now - _startedAt;
                        if (timeSpan.TotalSeconds > 0)
                        {
                            bytesPerSecond = e.BytesSent / (long)timeSpan.TotalSeconds;
                        }
                    }


                    fileSize.Text = $"{PrettySize(Progres.Value)} / {PrettySize(Progres.Maximum)} {PrettySize(bytesPerSecond)}/s";
                }
                catch { }
            };
            uploadcli.UploadFileCompleted += (s, e) => {
                if (onComplete != null)
                    onComplete(this);
                Progres.Visible = false;
                IsComplete = true;
                Downloading = false;
               
                if (e.Error != null)
                {
                    if (!e.Error.Message.Contains("(416)"))
                    {
                        fileSize.ForeColor = Color.Red;
                        fileSize.Text = e.Error.Message;
                    }
                    else if (!e.Error.Message.Contains("(403)"))
                    {
                        fileSize.ForeColor = Color.Red;
                        fileSize.Text = "Se ha Exedido el Tiempo Limite De Subida Nesesita Mas Velicidad";
                    }
                    else
                    {
                        fileSize.Text = "El archivo ya esta Descargado";
                        Upload = true;
                    }
                }
                else
                {
                    fileSize.Text = "Completado!";
                    Main.Notify(1000, $"Archivo Subido Correctamente {data.Name}!", "YAP!");
                    Upload = true;
                }
                uploadcli.Dispose();
            };
            uploadcli.UploadFileAsync(new Uri(putUrl),"PUT",file);
        }



        bool Signed = false;
        long lastBytes = 0;
        DateTime _startedAt;
        private void ProgresChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (!Signed)
            {
                if (onValidate != null) onValidate(this);
                Signed = true;
            }

            try{
                    Progres.Maximum = (int)(e.TotalBytesToReceive + Position);
                    Progres.Value = (int)(e.BytesReceived + Position);

                    Progres.Visible = true;


                long bytesPerSecond = e.BytesReceived;
                if (_startedAt == default(DateTime))
                { 
                 _startedAt = DateTime.Now;
                }
                else
                {
                    var timeSpan = DateTime.Now - _startedAt;
                    if (timeSpan.TotalSeconds > 0)
                    {
                     bytesPerSecond = e.BytesReceived / (long)timeSpan.TotalSeconds;
                    }
                }

                    fileSize.Text = $"{PrettySize(Progres.Value)} / {PrettySize(Progres.Maximum)} {PrettySize(bytesPerSecond)}/s";

                if (fileSize.ForeColor == Color.Red)
                {
                    fileSize.ForeColor = Color.White;
                    LoadDownImage();
                }
            }
            catch { }
        }

        private void OnDownloadComplete(object sender, AsyncCompletedEventArgs e)
        {
            if (!pause) { 
            sw.Restart();
            if (onComplete != null)
                onComplete(this);
            Progres.Visible = false;
            IsComplete = true;
            Downloading = false;
            if (e.Error != null)
            {
                if (!e.Error.Message.Contains("(416)"))
                {
                    fileSize.ForeColor = Color.Red;
                    fileSize.Text = e.Error.Message;
                }
                else
                {
                    fileSize.Text = "El archivo ya esta Descargado";
                }
            }
            else
            {
                    fileSize.Text = "Completado!";
                    Main.Notify(1000, $"Descarga Finalizada {data.Name}!","Se a completado la descara!");
            }
            }
            else
            {
                fileSize.Text = "Descarga Pausada";
            }
            cli.Dispose();
        }

        long actualPosition = 0;
        public void UpdateSpeedPerSecond()
        {
            if (!Downloading) return;
            if (IsComplete) return;
            SpeedPerSecond = actualPosition - LastPosition;
            LastPosition = actualPosition;
        }


        bool Aborting = false;

        public void Abort() => Aborting = true;


        public bool Closing = false;
        private void btnClose_Click(object sender, EventArgs e)
        {
            btnClose.Enabled = false;
            Closing = true;
            AbortDownload();
        }

        public  bool pause = false;
        public void AbortDownload()
        {
            pause = true;
            cli?.CancelAsync();
        }

        private void btnClose_MouseEnter(object sender, EventArgs e)
        {
            btnClose.Size += new Size(2, 2);
            btnClose.Location -= new Size(1, 1);
        }

        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            btnClose.Size -= new Size(2, 2);
            btnClose.Location += new Size(1, 1);
        }

    }
}
