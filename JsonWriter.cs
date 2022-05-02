using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDM
{
    public class JsonWriter : IDisposable
    {
        public string Writer { get; private set; }

        public void Begin() => Writer = "{";

        public void Write(string name, string value) => Writer += $"\"{name}\":\"{value}\",";

        public void Write(string name, int value) => Writer += $"\"{name}\":{value},";

        public void Write(string name, long value) => Writer += $"\"{name}\":{value},";

        public void Write(string name, double value) => Writer += $"\"{name}\":{value},";

        public void Write(string name, short value) => Writer += $"\"{name}\":{value},";

        public void Write(string name, float value) => Writer += $"\"{name}\":{value},";

        public void End() => Writer = Writer.Remove(Writer.Length-1) + "}";


        public void Dispose()
        {

        }
    }
}
