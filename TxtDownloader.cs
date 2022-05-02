using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TDM
{

    public class NekoJsonFormat
    {
        public string hash { get; set; } = "";
        public string name { get; set; } = "";
        public string path { get; set; } = "";
        public long size { get; set; } = 0;
        public string url { get; set; } = "";
        public int type { get; set; } = 0;
    }

    public class TxtDownloader
    {
        public class LinkData
        {
            public string Name { get; set; }
            public string Link { get; set; }
            public long Size { get; set; }
            public string TimeSpan { get; set; } 
            public bool IsValidate { get; set; } = false;
        }

        public string Path { get; private set; } = "";
        public List<LinkData> Links { get; private set; } = new List<LinkData>();

        public static LinkData GetDataFromLink(string link, bool isTld = false)
        {
            try
            {
                if (!isTld)
                {
                    LinkData data = new LinkData();
                    if (link.Contains("?"))
                    {
                        string[] tokens = link.Split('?');
                        data.Link = tokens[0];
                        data.Name = tokens[1];
                        return data;
                    }
                    else if (link.Contains("{") && link.Contains("}"))
                    {
                        NekoJsonFormat jsonData = JsonConvert.Deserialize<NekoJsonFormat>(link);
                        data.Link = jsonData.url;
                        data.Name = jsonData.name;
                        return data;
                    }
                    else
                    {
                        string line = link.Replace(" ", "").Replace("\t", "?").Replace("=", ".");
                        string[] tokens = line.Split('?');
                        data.Link = tokens[0];
                        data.Name = tokens[1];
                        return data;
                    }
                }
                else
                {
                    LinkData data = new LinkData();
                    string parse = link.Replace("[", "").Replace("]","");
                    string[] parts = parse.Split(',');
                    string name = parts[0].Split('=')[1];
                    string url =  parts[1].Split('=')[1];
                    long size =   long.Parse(parts[2].Split('=')[1]);
                    string timespan = parts[3].Split('=')[1];
                    data.Name = name;
                    data.Link = url;
                    data.Size = size;
                    data.TimeSpan = timespan;
                    return data;
                }
            }
            catch (Exception ex)
            {
                TDM.ErrorLog.CreateError( ErrorLogType.TxTLoad, ex.Message);
                return null;
            }
            return null;
        }

        public bool IsTLD = false;

        public TxtDownloader(string path, bool isTld = false)
        {
            IsTLD = isTld;
            Path = path;
            LoadLinks();
        }
        private void LoadLinks()
        {
                string[] lines = File.ReadAllLines(Path);
            foreach (var l in lines)
            {
                    Application.DoEvents();
                    LinkData data = GetDataFromLink(l,IsTLD);
                    if (data != null)
                        Links.Add(data);
            }
        }
        public void Foreach(Action<LinkData> action)
        {
            foreach (var item in Links)
            {
                action(item);
            }
        }
    }
}
