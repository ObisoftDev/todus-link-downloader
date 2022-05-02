using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


public enum ErrorType
{
    Signed,
    Connection
}


public class TodusSigned
    {

        public delegate void OnSigned(string signedUrl);
        public delegate void OnError(ErrorType err);

        const string host = "im.todus.cu";
        const int port = 1756;

        public byte[] asynBuff;
        public SslStream SSLStream;
        TcpClient cli = new TcpClient();

        public string Auth = "";
        public string Phone = "";
        public string Url = "";
        public string Sid = "";

        public bool isSigned { get; private set; } = false;
        public string SignedUrl { get; private set; } = "";
        public string Error { get; private set; } = "";

        public OnSigned onSigned { get; set; }
        public OnError onErr { get; set; }

        public TodusSigned(string phone,string auth,string url)
        {
            Auth = auth;
            Phone = phone;
            Url = url;
        }

        public void StartSigned()
        {
         Sid = GenerateSessionID();
        try
        {
            cli.ExclusiveAddressUse = true;
            
            cli.BeginConnect(host, port, new AsyncCallback(OnConnect), cli);
        }
        catch (Exception ex) { if (onErr != null) onErr(ErrorType.Connection); }
        }
        private void OnConnect(IAsyncResult ar)
        {
        try
        {
            TcpClient cli = (TcpClient)ar.AsyncState;
            SSLStream = new SslStream(cli.GetStream(), true, new RemoteCertificateValidationCallback((s, c, ch, err) =>
            {
                if (err == SslPolicyErrors.None)
                    return true;
                return false;
            }), null);

            SSLStream.AuthenticateAsClient(host);
            asynBuff = new byte[4096];
            SSLStream.BeginRead(asynBuff, 0, asynBuff.Length, new AsyncCallback(OnRead), null);

            Console.WriteLine("Conected!");
            SendData("<stream:stream xmlns='jc' o='im.todus.cu' xmlns:stream='x1' v='1.0'>");
        }
        catch (Exception ex){ if (onErr != null) onErr(ErrorType.Connection); 
            TDM.ErrorLog.CreateError(TDM.ErrorLogType.FailConnectToTodus, ex.Message);}
    }
        private void OnRead(IAsyncResult ar)
        {
            try
            {
                int read = SSLStream.EndRead(ar);
                try
                {
                    if (read > 0)
                    {
                        byte[] readBuff = new byte[read];
                        Array.Copy(asynBuff, 0, readBuff, 0, read);
                        HandleData(readBuff);
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); 
            
            }
                SSLStream.BeginRead(asynBuff, 0, asynBuff.Length, new AsyncCallback(OnRead), null);
            }
            catch (Exception ex){TDM.ErrorLog.CreateError(TDM.ErrorLogType.ReadSocketStream, ex.Message);}
        }
        private void HandleData(byte[] readBuff)
        {
            string str = Encoding.UTF8.GetString(readBuff);

            Console.WriteLine("Server -> Client :");
            Console.WriteLine(str); Console.WriteLine("");
            bool pasoFinal = false;
            if (str == "<stream:features><es xmlns='x2'><e>PLAIN</e><e>X-OAUTH2</e></es><register xmlns='http://jabber.org/features/iq-register'/></stream:features>")
            {
                string char0 = Convert.ToChar(0).ToString();
                string authBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(char0 + Phone + char0 + Auth));
                SendData("<ah xmlns='ah:ns' e='PLAIN'>" + authBase64 + "</ah>");
            }
            else if (str == "<ok xmlns='x2'/>")
            {
                SendData("<stream:stream xmlns='jc' o='im.todus.cu' xmlns:stream='x1' v='1.0'>");
            }
            else if (str.Contains("<stream:features><b1 xmlns='x4'/>"))
            {
                SendData("<iq i='" + Sid + "-1' t='set'><b1 xmlns='x4'></b1></iq>");
            }
            else if (str.Contains("t='result' i='" + Sid + "-1'>"))
            {
                pasoFinal = true;
                SendData("<iq i='" + Sid + "-2' t='get'><query xmlns='todus:gurl' url='" + Url + "'></query></iq>");
            }
            else if (str.Contains("t='result' i='" + Sid + "-2'>") & str.Contains("status='200'"))
            {
                string[] tokens = str.Split(' ');
                SignedUrl = tokens[5].Replace("du='", "").Replace("'", "").Replace("amp;", "");
                isSigned = true;
               if (onSigned != null) onSigned(SignedUrl);
                CloseSocket();
            }else  if (str== "<failure xmlns='x2'><not-authorized/><text xml:lang='en'>Invalid username or password</text></failure>")
            {
            CloseSocket();
            if (onErr != null) if (onErr != null) onErr(ErrorType.Signed);
            }
        }
             public void CloseSocket()
             {
                 SSLStream?.Dispose();
                 SSLStream = default;
                 cli?.Close();
                 cli = default;
             }
        public void SendData(string text)
        {
            Console.WriteLine("Client -> Server :");
            Console.WriteLine(text); Console.WriteLine("");
            byte[] data = Encoding.UTF8.GetBytes(text);
            SSLStream.BeginWrite(data, 0, data.Length, (ar) => {
                SSLStream.EndWrite(ar);
            }, null);
        }
    public static void SendData(string text,SslStream stream)
    {
        Console.WriteLine("Client -> Server :");
        Console.WriteLine(text); Console.WriteLine("");
        byte[] data = Encoding.UTF8.GetBytes(text);
        stream.BeginWrite(data, 0, data.Length, (ar) => {
            stream.EndWrite(ar);
        }, null);
    }
    public static string GenerateSessionID()
        {
            string charset = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string newSid = "";
            Random rnd = new Random(Environment.TickCount);
            for (int i = 0; i < 5; i++)
                newSid += $"{charset[rnd.Next(0, charset.Length)]}";
            return newSid;
        }

    }

