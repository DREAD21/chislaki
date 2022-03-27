using System;

namespace labc2._1
{
    class Decision : Exp
    {
        public static double x0 ;
        public static double x1 ;
        public static double q;
        public static void Iterations(out double answer)
        {
            q = get_q();
            x0 = 0.35;
            x1 = 0.35;
            do
            {
                x0 = x1;
                x1 = function_iter(x0);
            }
            while (q/(1-q)*Math.Abs(x1 - x0) > e);
            answer = x1; 
            
        }
        public static void Newton(out double answer)
        {
            x0 = 0.35;
            x1 = 0.35;
            do
            {
                x0 = x1;
                x1 = x0 - function(x0) / derivative(x1);
            } while (Math.Abs(x1 - x0) > e || x1 < 0);
            answer = x1;
        }
        public static double function(double arg)
        {
            return Math.Pow(2, arg) + Math.Pow(arg, 2) - 2;
        }
        public static double function_iter(double arg)
        {
            return Math.Pow((2-Math.Pow(2,arg)),0.5);
        }
        public static double derivative(double arg)
        {
            return Math.Pow(2, arg) * Math.Log(2) + 2 * arg;
        }
        public static double get_q()
        {
            return Math.Max(Math.Abs(function_iter(0)), Math.Abs(function_iter(0.7)));
        }
    }
    class Program
    {
        public static double answer;
        public static double answer2;
        static void Main(string[] args)
        {
            Exp.readFile();
            Decision.Newton(out answer);
            Decision.Iterations(out answer2);
            Console.WriteLine("Метод Ньютона:  " + answer);
            Console.WriteLine("Метод простых итераций:  " + answer2);
            Console.ReadKey();
        }
    }
}
