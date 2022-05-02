using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pack
{
    public class Package
    {
        public static void Extract(byte[] bytes,string directory)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            using (Stream stream = new MemoryStream(bytes))
            {
                using(BinaryReader reader = new BinaryReader(stream))
                {
                    while (stream.Position<stream.Length)
                    {
                        string name = reader.ReadString();
                        int length = reader.ReadInt32();
                        byte[] readBytes = reader.ReadBytes(length);
                        File.WriteAllBytes($"{directory}/{name}",readBytes);
                    }
                }
            }
        }
        public static void CreateFrom(string directory,string saveFile)
        {
            using (Stream stream = File.Create(saveFile))
            {
                using(BinaryWriter writer = new BinaryWriter(stream))
                {
                    WriteDirectory(new DirectoryInfo(directory), writer);
                }
            }
        }
       
        public static void WriteDirectory(DirectoryInfo di,BinaryWriter writer)
        {
            foreach(var fi in di.GetFiles())
            {
                WriteFile(fi, writer);
            }
        }
        public static void WriteFile(FileInfo fi, BinaryWriter writer)
        {
            writer.Write(fi.Name);
            writer.Write((int)fi.Length);
            writer.Write(File.ReadAllBytes(fi.FullName));
        }
    }
}
