using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ex2_lab8
{
    class Task
    {
        //Получить матрицу из консоли
        public double[,] GetMat()
        {
            string[] lines = File.ReadAllLines(@"C:\Users\Никита\Desktop\числаки\1.2.txt").Take(5).ToArray();
            double[,] matrix = new double[5, 6];
            for(int i = 0; i < 5; i++)
            {
                int[] row = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToArray();
                for (int j = 0; j < 6; j++)
                {
                    matrix[i, j] = row[j];
                }
            }

            return matrix;
        }

        //Вывести матрицу
        public void PrintMat(double[,] mat)
        {
            Console.WriteLine("Входная матрица:");
            for (int i = 0; i < mat.GetLength(0); i++, Console.WriteLine())
                for (int j = 0; j < mat.GetLength(1); j++)
                    Console.Write("{0,8}", mat[i, j]);
        }


        public string PolPartToString(double coeff, int pow)
        {
            if (coeff == 0) return null;
            if (pow == 0) return coeff.ToString();
            var cf = coeff.ToString();
            if (coeff == 1) cf = "";
            if (coeff == -1) cf = "-";
            if (pow == 1) return cf + "x";
            return cf + "x^" + pow.ToString();
        }

        //Умножение матриц
        public double[,] MulMat(double[,] mat1, double[,] mat2)
        {
            double[,] res = new double[mat1.GetLength(0), mat2.GetLength(1)];
            for (int i = 0; i < mat1.GetLength(0); i++)
            {
                for (int j = 0; j < mat2.GetLength(1); j++)
                {
                    for (int k = 0; k < mat2.GetLength(0); k++)
                    {
                        res[i, j] += mat1[i, k] * mat2[k, j];
                    }
                }
            }

            return res;
        }
        public void CoefCounter(double[,] mat)
        {
            double[] Ps = new double[mat.GetLength(0)];
            double[] Qs = new double[mat.GetLength(0)];
            double[] x = new double[mat.GetLength(0)];
            Ps[0] = -mat[0, 1] / mat[0, 0];
            Qs[0] = mat[0, mat.GetLength(1) - 1] / mat[0, 0];
            for (int i=1; i < mat.GetLength(0); i++)
            {
                Ps[i] = -mat[i,1+i]/(mat[i,i]+mat[i,i-1]*Ps[i-1]);
                Qs[i] = (mat[i, mat.GetLength(1) - 1] - mat[i, i - 1] * Qs[i - 1]) / (mat[i,i]+ mat[i, i - 1]*Ps[i-1]);
            }

            // обратный ход
            x[mat.GetLength(0) - 1] = Qs[mat.GetLength(0)-1];
            for (int j = mat.GetLength(0)-2; j >= 0; j--)
            {
                x[j] = Ps[j] * x[j + 1] + Qs[j];
            }
            for (int m = 0; m < mat.GetLength(0); m++)
            {
                Console.WriteLine("x{0}={1}", m+1, Math.Round(x[m],4));
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Task task = new Task();
            //Console.WriteLine("Введите матрицу:");
            //string[] input = Console.ReadLine().Split('|');
            var matrix = task.GetMat();
            task.PrintMat(matrix);
            task.CoefCounter(matrix);
        }
    }
}