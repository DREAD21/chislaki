using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ex2_lab8
{
    class Task
    {
        public double[,] GetMat()
        {
            string[] lines = File.ReadAllLines(@"C:\Users\Никита\Desktop\числаки\lab1.2\1.2.txt").Take(5).ToArray();
            double[,] matrix = new double[5, 6];
            for (int i = 0; i < 5; i++)
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
        public double[] CoefCounter(double[,] mat)
        {
            double[] Ps = new double[mat.GetLength(0)];
            double[] Qs = new double[mat.GetLength(0)];
            double[] x = new double[mat.GetLength(0)];
            Ps[0] = -mat[0, 1] / mat[0, 0];
            Qs[0] = mat[0, mat.GetLength(1) - 1] / mat[0, 0];
            for (int i = 1; i < mat.GetLength(0); i++)
            {
                Ps[i] = -mat[i, 1 + i] / (mat[i, i] + mat[i, i - 1] * Ps[i - 1]);
                Qs[i] = (mat[i, mat.GetLength(1) - 1] - mat[i, i - 1] * Qs[i - 1]) / (mat[i, i] + mat[i, i - 1] * Ps[i - 1]);
            }

            // обратный ход
            x[mat.GetLength(0) - 1] = Qs[mat.GetLength(0) - 1];
            for (int j = mat.GetLength(0) - 2; j >= 0; j--)
            {
                x[j] = Ps[j] * x[j + 1] + Qs[j];
            }
            for (int m = 0; m < mat.GetLength(0); m++)
            {
                Console.WriteLine("x{0}={1}", m + 1, Math.Round(x[m], 4));
            }

            return x;

        }
        public double[,] Geta(double[,] mat)
        {
            double[,] mat1 = new double[mat.GetLength(0), mat.GetLength(0)];
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat.GetLength(1) - 1; j++)
                {
                    mat1[i, j] = mat[i, j];
                }
            }
            PrintMat(mat1);
            return mat1;
        }
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
            PrintMat(res);

            return res;
        }
        public double[] Stolb(double[,] A, double[] B)
        {
            double[] res = new double[B.GetLength(0)];
            for (int row = 0; row < A.GetLength(0); row++)
            {
                for (int col = 0; col < A.GetLength(1); col++)
                {
                    res[col] += A[col,row] * B[row];
                }
            }
           for(int i = 0; i < res.Length; i++)
           {
                Console.WriteLine(res[i]);
           }
            return res;
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
            double[] c = task.CoefCounter(matrix);
            double[,] watch = task.Geta(matrix);
            Console.WriteLine("Проверка");
            task.Stolb(watch, c);
            Console.ReadLine();
        }
    }
}
