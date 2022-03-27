using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2._2c
{
    class Decision_1 : Exp
    {
        public static double[,] A1 = new double[2, 2];
        static double[,] A2 = new double[2, 2];
        static double[,] J = new double[2, 2];
        public static double x10, x20, x11, x21;
        public static void Newton(out double answer1, out double answer2)
        {
            x10 = 0.5;
            x20 = 0.5;
            x11 = 0.5;
            x21 = 0.5;
            do
            {
                x10 = x11;
                x20 = x21;
                A1 = _A1(x10, x20);
                A2 = _A2(x10, x20);
                J = _J(x10, x20);
                x11 = x10 - det(A1) / det(J);
                x21 = x20 - det(A2) / det(J);
            }
            while (Math.Max(Math.Abs(x11 - x10), Math.Abs(x21 - x20)) > e);
            //for (int i = 0; i < 2; i++)
            //{
            //    Console.WriteLine();
            //    for (int j = 0; j < 2; j++)
            //    {
            //        Console.Write(A2[i, j] + "   ");
            //    }
            //}
            answer1 = x11;
            answer2 = x21;
        }
        public static double f1(double arg1, double arg2)
        {
            return arg1 * arg1 + arg2 * arg2 - 4;
        }
        public static double f2(double arg1, double arg2)
        {
            return arg1 - Math.Exp(arg2) + 2;
        }
        public static double deriv_f1_d1(double arg1, double arg2)
        {
            return 2 * arg1;
        }
        public static double deriv_f1_d2(double arg1, double arg2)
        {
            return 2 * arg2;
        }
        public static double deriv_f2_d1(double arg1, double arg2)
        {
            return 1;
        }
        public static double deriv_f2_d2(double arg1, double arg2)
        {
            return -Math.Exp(arg2);
        }
        public static double[,] _A1(double x10, double x20)
        {
            A1[0, 0] = f1(x10, x20);
            A1[0, 1] = f2(x10, x20);
            A1[1, 0] = deriv_f1_d2(x10, x20);
            A1[1, 1] = deriv_f2_d2(x10, x20);
            return A1;
        }
        public static double[,] _A2(double x10, double x20)
        {
            A2[0, 0] = deriv_f1_d1(x10, x20);
            A2[0, 1] = deriv_f2_d1(x10, x20);
            A2[1, 0] = f1(x10, x20);
            A2[1, 1] = f2(x10, x20);
            return A2;
        }
        public static double[,] _J(double x10, double x20)
        {
            J[0, 0] = deriv_f1_d1(x10, x20);
            J[0, 1] = deriv_f2_d1(x10, x20);
            J[1, 0] = deriv_f1_d2(x10, x20);
            J[1, 1] = deriv_f2_d2(x10, x20);
            return J;
        }
        public static double det(double[,] mat)
        {
            return mat[0, 0] * mat[1, 1] - mat[1, 0] * mat[0, 0];
        }
    }
}
