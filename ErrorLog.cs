using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDM
{
    public enum ErrorLogType : int
    {
        FailConnectToTodus = 200,
        NotAutorized = 233,
        ConectionLost = 234,
        ReadSocketStream = 100,
        TxTLoad = 210,
        CompleteDownloadError = 333,
        Other = 90
    }
    public class ErrorLog
    {
        const string LogPath = "Errors";
        public static void CreateError(ErrorLogType type,string log)
        {
            if (!Directory.Exists(LogPath)) Directory.CreateDirectory(LogPath);
            File.WriteAllText($"Errors/{(int)type}-{Environment.TickCount}.txt", log);
        }
    }
}
