using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDM
{
    public class Log
    {
        public static string LogText { get; private set; } = "";
        public static void Write(string text) => LogText += text + "\n";
        public static void Save() => File.WriteAllText("log.txt", LogText);
        public static void Reset() => LogText = "";
    }
}
