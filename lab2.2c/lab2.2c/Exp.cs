using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2._2c
{
    class Exp
    {
        public static double e;
        public static async void readFile()
        {
            string path = @"C:\Users\Никита\Desktop\числаки\lab2.2c\2.2.txt";
            using (FileStream fstream = File.OpenRead(path))
            {
                // выделяем массив для считывания данных из файла
                byte[] buffer = new byte[fstream.Length];
                // считываем данные
                await fstream.ReadAsync(buffer, 0, buffer.Length);
                // декодируем байты в строку
                string textFromFile = Encoding.Default.GetString(buffer);
                Console.WriteLine($"Текст из файла: {textFromFile}");
                e = Convert.ToDouble(textFromFile, System.Globalization.CultureInfo.InvariantCulture);
            }
        }
    }
}
