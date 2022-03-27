using System;
using System.IO;
using System.Text;

namespace lab2._2c
{
   
    class Program
    {
        public static double x11,x21;
        public static double _x11, _x21;
        static void Main(string[] args)
        {
            Exp.readFile();
            Decision_1.Newton(out x11, out x21);
            Decision_2.Iteration(out _x11, out _x21);
            Console.WriteLine(x11.ToString());
            Console.WriteLine(x21.ToString());
            Console.WriteLine("******************");
            Console.WriteLine(_x11.ToString());
            Console.WriteLine(_x21.ToString());
        }
    }
}
