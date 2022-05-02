using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TDM
{
    public class EntryPoint
    {
        [STAThread]
        static void Main(string[] args)
        {
            LibLoader.Init();
            Application.Run(new Main());
        }
    }
}
