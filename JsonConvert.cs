using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TDM
{
    public static class JsonConvert
    {
        public static T Deserialize<T>(string value)
        {
            Type type = typeof(T);
            object obj = type.Assembly.CreateInstance(type.FullName,false);
            Dictionary<string, object> queue = GetJsonQueue(value);
            foreach(var p in obj.GetType().GetProperties())
            {
                foreach(var q in queue)
                {
                    if (p.Name == q.Key)
                    {
                        p.SetValue(obj, q.Value);
                    }
                }
            }
            return (T)obj;
        }

        public static Dictionary<string,object> GetJsonQueue(string value)
        {
            Dictionary<string, object> queue = new Dictionary<string, object>();
            string json = value.Replace("{","").Replace("}","");
            string[] tokens = json.Split(',');
            foreach(var k in tokens)
            {
                string[] parts = k.Split(':');
                string key = parts[0].Replace("\"","");
                string val = parts[1].Replace("\"", "");
                if (parts.Length > 2)
                    val += ":" + parts[2].Replace("\"","").Replace(@"\", "");
                queue.Add(key, GetObjValue(val));
            }
            return queue;
        }
        public static object GetObjValue(string str)
        {
            object obj = null;
            if (str == "")
                obj = string.Empty;
            else if (str.Contains("."))
            try { obj = float.Parse(str); } catch { obj = str; }
            else                
            try { obj = int.Parse(str); } catch { obj = str; }
            return obj;
        }
    }
}
