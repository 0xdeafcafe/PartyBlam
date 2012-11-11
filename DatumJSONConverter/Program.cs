using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;

namespace DatumJSONConverter
{
    class Program
    {
        public class Tag
        {
            public string MapName { get; set; }
            public Int32 MapID { get; set; }
            public IList<MapTags> Tags { get; set; }
        }
        #region MapTag Innards
        public class MapTags
        {
            public string TagClass { get; set; }
            public string TagPath { get; set; }
            public Int32 DatumIndex { get; set; }
        }
        #endregion

        static Stopwatch watch = new Stopwatch();
        static void Main(string[] args)
        {
            string[] inputSplit = File.ReadAllLines(@"C:/Users/Alex/Desktop/mapResources");
            
            List<Tag> tags = new List<Tag>();
            Tag currentTag = new Tag();

            Console.WriteLine("Press Enter to start this bitch up...");
            Console.ReadLine();
            watch.Start();

            Console.WriteLine("Beginning Process....");

            foreach (string line in inputSplit)
            {
                if (line.StartsWith("<?")) { }
                else if (line.StartsWith("<Map Map="))
                {
                    // Update Current Map
                    if (currentTag.MapName != null)
                        tags.Add(currentTag);

                    currentTag = new Tag();
                    string[] lineArgs = line.Split('\"');
                    
                    currentTag.MapName = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(lineArgs[1].ToLower());
                    currentTag.MapID = 0;
                    currentTag.Tags = new List<MapTags>();

                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("Processing Map Tags; MapName={0} MapID=\"lul\" TagCount={1}", currentTag.MapName, lineArgs[3]);
                }
                else if (line.StartsWith("<Tag Class="))
                {
                    // Add New Tag
                    string[] lineArgs = line.Split('\"');

                    MapTags thisTag = new MapTags();
                    thisTag.TagClass = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(lineArgs[1].ToLower());
                    thisTag.TagPath = lineArgs[3];
                    thisTag.DatumIndex = Int32.Parse(lineArgs[5]);
                    currentTag.Tags.Add(thisTag);

                    Console.WriteLine("Processed Tag Data; Class={0} Path{1} Datum{2}", thisTag.TagClass, thisTag.TagPath, thisTag.DatumIndex.ToString());
                }
            }

            tags.Add(currentTag);

            foreach (Tag tagg in tags)
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                jss.MaxJsonLength = Int32.MaxValue;
                string output = jss.Serialize(tagg);

                File.WriteAllText(@"C:/Users/Alex/Desktop/" + tagg.MapName + "_json.h3tagdb", output);
            }
            
            watch.Stop();
            Console.WriteLine("All Done - Time Taken = {0}ms - Needs more cowbell.", watch.ElapsedMilliseconds);
            Console.ReadLine();
        }
    }
}
