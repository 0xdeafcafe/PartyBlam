using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;

namespace TagDBCompression
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start compressing this whore;");
            Console.ReadLine();

            string dbFolder = @"C:\Users\Alex\Desktop\";

            DirectoryInfo di = new DirectoryInfo(dbFolder);
            FileInfo[] rgFiles = di.GetFiles("*.h3tagdb");
            foreach (FileInfo fi in rgFiles)
            {
                string[] derp = File.ReadAllText(fi.FullName).Split('\"');
                string derp1 = derp[6].Replace(":", "").Replace(",", "");
                CompressStringToFile(@"C:\Users\Alex\Desktop\compression\" + derp1 + ".h3tagDB_compression", File.ReadAllText(fi.FullName));

                Console.WriteLine("Compressed "+fi.Name+"!");
            }

            Console.WriteLine("All done");
            Console.ReadLine();
        }

        public static void CompressStringToFile(string fileName, string value)
        {
            string temp = Path.GetTempFileName();
            File.WriteAllText(temp, value);

            byte[] b;
            using (FileStream f = new FileStream(temp, FileMode.Open))
            {
                b = new byte[f.Length];
                f.Read(b, 0, (int)f.Length);
            }

            using (FileStream f2 = new FileStream(fileName, FileMode.Create))
            using (GZipStream gz = new GZipStream(f2, CompressionMode.Compress, false))
            {
                gz.Write(b, 0, b.Length);
            }
        }
    }
}
