using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace labc2._1
{
    class Exp
    {
        public static double e;
        public static async void readFile()
        {
            string path = @"C:\Users\Никита\Desktop\числаки\labc2.1\2.1.txt";
            using (FileStream fstream = File.OpenRead(path))
            {
                byte[] buffer = new byte[fstream.Length];
                await fstream.ReadAsync(buffer, 0, buffer.Length);
                string textFromFile = Encoding.Default.GetString(buffer);
                Console.WriteLine($"Текст из файла: {textFromFile}");
                e = Convert.ToDouble(textFromFile, System.Globalization.CultureInfo.InvariantCulture);
            }
        }
    }
}
