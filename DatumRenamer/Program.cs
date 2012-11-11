using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using PartyBlam.IO;

namespace DatumRenamer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Let's do this shit, go chris brown on the return key");
            Console.ReadLine();

            string mapInfoFolder = @"C:\Users\Alex\Documents\My Received Files\info\info\";

            DirectoryInfo di = new DirectoryInfo(mapInfoFolder);
            FileInfo[] rgFiles = di.GetFiles("*.mapinfo");
            foreach (FileInfo fi in rgFiles)
            {
                string fileName = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(fi.Name.ToLower());

                EndianStream stream = new EndianStream(new FileStream(fi.FullName, FileMode.OpenOrCreate), Endian.BigEndian);
                stream.SeekTo(0x3C);
                Int32 mapID = stream.ReadInt32();

                string jsonDB = File.ReadAllText(@"C:/Users/Alex/Desktop/" + fi.Name.Replace(fi.Extension, "") + "_json.h3tagdb");
                jsonDB = jsonDB.Replace("\"MapID\":0", "\"MapID\":" + mapID.ToString());
                File.WriteAllText(@"C:/Users/Alex/Desktop/" + fi.Name.Replace(fi.Extension, "") + "_json.h3tagdb", jsonDB);

                Console.WriteLine("Grabbed ID of {0}", fi.Name.Replace(fi.Extension, ""));
            }

            Console.WriteLine("All done");
            Console.ReadLine();
        }
    }
}