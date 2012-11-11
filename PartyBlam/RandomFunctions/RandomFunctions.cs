using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PartyBlam
{
    public class RandomFunctions
    {
        /// <summary>
        /// Generates a percentage, formatted with "places" decimal places.
        /// </summary>
        /// <param name="value">Value for which a percentage is needed</param>
        /// <param name="total">Total from which to generate a percentage</param>
        /// <returns>string with the percentage value</returns>
        public static int GetPercentage(Int32 value, Int32 total)
        {
            Int32 percent = 0;
            String retval = string.Empty;

            if (value == 0 || total == 0)
            {
                percent = 0;
            }

            else
            {
                percent = (Int32)Decimal.Divide(value, total) * 100;
            }

            return percent;
        }

        /// <summary>
        /// Read a resource from the application embedded resources
        /// </summary>
        /// <param name="resourceName">The name of the resource...</param>
        /// <returns>A byte array of the resource</returns>
        public static byte[] ReadResource(string resourceName)
        {
            using (Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                byte[] buffer = new byte[1024];
                using (MemoryStream ms = new MemoryStream())
                {
                    while (true)
                    {
                        int read = s.Read(buffer, 0, buffer.Length);
                        if (read <= 0)
                            return ms.ToArray();
                        ms.Write(buffer, 0, read);
                    }
                }
            }
        }

        public enum EncodingType
        {
            ASCII,
            Unicode,
            UTF7,
            UTF8
        }
        /// <summary> 
        /// Converts a byte array to a string using specified encoding. 
        /// </summary> 
        /// <param name="bytes">Array of bytes to be converted.</param> 
        /// <param name="encodingType">EncodingType enum.</param> 
        public static string ByteArrayToString(byte[] bytes, EncodingType encodingType)
        {
            System.Text.Encoding encoding = null;
            switch (encodingType)
            {
                case EncodingType.ASCII:
                    encoding = new System.Text.ASCIIEncoding();
                    break;
                case EncodingType.Unicode:
                    encoding = new System.Text.UnicodeEncoding();
                    break;
                case EncodingType.UTF7:
                    encoding = new System.Text.UTF7Encoding();
                    break;
                case EncodingType.UTF8:
                    encoding = new System.Text.UTF8Encoding();
                    break;
            }
            return encoding.GetString(bytes);
        } 

        public class GZip
        {
            public static byte[] Decompress(byte[] input)
            {
                using (GZipStream stream = new GZipStream(new MemoryStream(input), CompressionMode.Decompress))
                {
                    const int size = 4096;
                    byte[] buffer = new byte[size];
                    using (MemoryStream memory = new MemoryStream())
                    {
                        int count = 0;
                        do
                        {
                            count = stream.Read(buffer, 0, size);
                            if (count > 0)
                            {
                                memory.Write(buffer, 0, count);
                            }
                        }
                        while (count > 0);
                        return memory.ToArray();
                    }
                }

            }

            public static byte[] Compress(byte[] input)
            {
                byte[] b;
                using (Stream f = new MemoryStream(input))
                {
                    b = new byte[f.Length];
                    f.Read(b, 0, (int)f.Length);
                }

                using (Stream f2 = new MemoryStream(input))
                using (GZipStream gz = new GZipStream(f2, CompressionMode.Compress, false))
                {
                    gz.Write(b, 0, b.Length);
                }

                return b;
            }
        }

        public class Images
        {
            public static Image byteArrayToImage(byte[] byteArrayIn)
            {
                MemoryStream ms = new MemoryStream(byteArrayIn);
                Image returnImage = Image.FromStream(ms);
                return returnImage;
            }

            public byte[] imageToByteArray(Image imageIn)
            {
                MemoryStream ms = new MemoryStream();
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                return ms.ToArray();
            }
        }
    }
}
