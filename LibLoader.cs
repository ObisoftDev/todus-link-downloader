using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TDM
{
    public class LibLoader
    {
        public static void Init()
        {
            AppDomain.CurrentDomain.AssemblyResolve += OnResolveAssembly;
        }

        private static Assembly OnResolveAssembly(object sender, ResolveEventArgs args)
        {
            string require = args.Name.Split(',')[0];
            using(Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"TDM.Libs.{require}.dll"))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                    return Assembly.Load(reader.ReadBytes((int)stream.Length));
            }
        }
    }
}
