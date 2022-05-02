using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TDM
{
    public class TodusUtil
    {
        [DllImport("user32.dll")]
        private extern static int ShowWindow(IntPtr hWnd, IntPtr nCmdShow);

        public static void ExtractDB(string paquete = "cu.todus.android")
        {
            Process.Start($"adb.exe", "shell cp /data/data/" + paquete + "/databases/internal.db-wal /storage/self/primary/internal.db-wal");
            Process.Start("cmd.exe", " /C mkdir temp");
            Process.Start($"adb.exe", "pull /storage/self/primary/internal.db-wal ./temp/");
            Process.Start($"adb.exe", "rm /storage/self/primary/internal.db-wal");
        }

        public static void DeleteTemp()
        {
            Process.Start("cmd.exe", "/C rd /q /s temp");
        }

        public static string GetToken(string path)
        {
            string token = "";
            string file = $"{path}/internal.db-wal";
            if (File.Exists(file))
            {
                string[] lines = File.ReadAllLines(file);
                foreach (var line in lines)
                {
                    if (line.Contains("\"token\":"))
                    {
                        int pos = line.LastIndexOf("\"token\":") + 8;
                        string[] tokens = line.Split(',');
                        token = tokens[2].Replace("\"", "").Replace(":", "").Replace("token", "");
                    }
                }
            }
            return token;
        }
        public static string GetPhone(string path)
        {
            string token = "";
            string file = $"{path}/internal.db-wal";
            if (File.Exists(file))
            {
                string[] lines = File.ReadAllLines(file);
                foreach (var line in lines)
                {
                    if (line.Contains("\"username\":"))
                    {
                        int pos = line.LastIndexOf("\"username\":") + 8;
                        string[] tokens = line.Split(',');
                        token = tokens[4].Replace("\"", "").Replace(":", "").Replace("username", "").Split('}')[0];
                    }
                }
            }
            return token;
        }
    }
}
