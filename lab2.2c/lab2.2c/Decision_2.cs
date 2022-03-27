using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2._2c
{
    class Decision_2 : Exp
    {
        public static double x10, x20, x11, x21;
        public static double q;
        public static void Iteration(out double answer1, out double answer2)
        {
            q = get_q();
            x10 = 0.5;
            x20 = 0.5;
            x11 = 0.5;
            x21 = 0.5;
            do
            {
                x10 = x11;
                x20 = x21;
                x11 = phi1(x10, x20);
                x21 = phi2(x10, x20);
            } while ((q / (1 - q)) * (Math.Max(Math.Abs(x11 - x10), Math.Abs(x21 - x20))) > e);

            answer1 = x11;
            answer2 = x21;
        }
        public static double phi1(double arg1, double arg2)
        {
            return Math.Pow((4 - arg2 * arg2), 0.5);
        }
        public static double phi2(double arg1, double arg2)
        {
            return Math.Log(arg1 + 2);
        }
        public static double deriv_phi1_d1(double arg1, double arg2)
        {
            return 0;
        }
        public static double deriv_phi1_d2(double arg1, double arg2)
        {
            return arg2 / Math.Pow((4 - arg2 * arg2), 0.5);
        }
        public static double deriv_phi2_d1(double arg1, double arg2)
        {
            return 1 / (arg1 + 2);
        }
        public static double deriv_phi2_d2(double arg1, double arg2)
        {
            return 0;
        }
        public static double get_q()
        {
            return Math.Max(Math.Abs(deriv_phi1_d1(0.5, 0.5) + deriv_phi1_d2(0.5, 0.5)), Math.Abs(deriv_phi2_d1(0.5, 0.5) + deriv_phi2_d2(0.5, 0.5)));
        }
    }
}
