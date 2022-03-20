using System;
using System.IO;
using System.Linq;

namespace lab1._3
{
    class Task
    {
        //Получить матрицу из консоли
        public double[,] GetMat()
        {
            string[] lines = File.ReadAllLines(@"C:\Users\Никита\Desktop\числаки\lab1.3\1.3.txt").Take(5).ToArray();
            double[,] matrix = new double[4, 5];
            for (int i = 0; i < 4; i++)
            {
                int[] row = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToArray();
                for (int j = 0; j < 5; j++)
                {
                    matrix[i, j] = row[j];
                }
            }

            return matrix;
        }

        //Вывести матрицу
        public void PrintMat(double[,] mat)
        {
            for (int i = 0; i < mat.GetLength(0); i++, Console.WriteLine())
                for (int j = 0; j < mat.GetLength(1); j++)
                    Console.Write("{0,8}", Math.Round(mat[i, j], 4));
        }

        //Вывод полинома
        public string PolToString(double[] a)
        {
            if (a.Length == 1)
            {
                return a[0].ToString();
            }

            return a.Reverse()
                    .Select((coeff, pow) => PolPartToString(coeff, pow))
                    .Where(x => x != null)
                    .Reverse()
                    .Aggregate((i, j) => i + "+" + j)
                    .Replace("+-", "-")
                    .Replace("++", "+")
                ;
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

        //Матрица на столбец
        public double[] MulVec(double[,] mat1, double[] vec)
        {
            double[] res = new double[mat1.GetLength(0)];
            for (int i = 0; i < mat1.GetLength(0); i++)
            {
                for (int k = 0; k < vec.Length; k++)
                {
                    res[i] += mat1[i, k] * vec[k];
                }
            }

            return res;
        }

        public double[,] ChangeLine(double[,] mat, int num2) // смена местами строк
        {
            double[] line = new double[mat.GetLength(1)];
            for (int i = 0; i < mat.GetLength(1); i++)
            {
                line[i] = mat[num2, i];
            }
            for (int i = 0; i < mat.GetLength(1); i++)
            {
                mat[num2, i] = mat[0, i];
            }
            for (int i = 0; i < mat.GetLength(1); i++)
            {
                mat[0, i] = line[i];
            }
            return mat;
        }

        public double[,] Alphas(double[,] mat)
        {
            double[,] alpha = new double[mat.GetLength(0), mat.GetLength(1) - 1];
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat.GetLength(1) - 1; j++)
                {
                    if (i == j)
                    {
                        alpha[i, j] = 0;
                    }
                    else
                    {
                        alpha[i, j] = -mat[i, j] / mat[i, i];
                    }
                }
            }
            Console.WriteLine("Alha:");
            PrintMat(alpha);
            return alpha;
        }

        public double[] Bettas(double[,] mat)
        {
            double[] betta = new double[mat.GetLength(0)];
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                betta[i] = mat[i, mat.GetLength(1) - 1] / mat[i, i];
            }
            Console.WriteLine("\n");
            Console.Write("Betta = (");
            PrintVec(betta);
            Console.Write(")");
            Console.WriteLine("\n");
            return betta;
        }

        public void Xs(double[,] alpha, double[] betta) // метод простых итераций
        {
            double e = 0.01;
            double[] x = betta;
            double[] xn = betta;
            double eps = 0;
            double temp = NormMat(alpha);
            do
            {
                x = xn;
                xn = MulVec(alpha, x);
                for (int i = 0; i < x.Length; i++)
                {
                    xn[i] = xn[i] + betta[i];
                }
                eps = Math.Round(Norms(x, xn) * temp / (1 - temp), 4);
            } while (eps > e);
            //Console.WriteLine("epsilon = {0}", eps);
            Console.WriteLine("\n");
            Console.WriteLine("Метод простых итераций:");
            Console.Write("x = ( ");
            PrintVec(xn);
            Console.Write(")");
            Console.WriteLine("\n");
        }

        public double Norms(double[] x, double[] xn)
        {
            /*double result = 0;
            for (int i = 0; i < x.Length; i++)
            {
                Console.WriteLine("xn-x: {0}-{1}", xn[i], x[i]);
                result = result + Math.Abs(xn[i] - x[i]);
            }
            return result;*/

            double[] result = new double[x.Length];
            for (int i = 0; i < x.Length; i++)
            {
                result[i] = Math.Abs(xn[i] - x[i]);
            }
            double max = result[0];
            for (int j = 1; j < x.Length; j++)
            {
                if (result[j] > max)
                {
                    max = result[j];
                }
            }
            return max;
        }

        public double NormMat(double[,] mat)
        {
            double[] line = new double[mat.GetLength(0)];
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                double summ = 0;
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    summ = summ + Math.Abs(mat[i, j]);
                }
                line[i] = summ;
            }
            double max = line[0];
            for (int k = 1; k < line.Length; k++)
            {
                if (line[k] > max)
                {
                    max = line[k];
                }
            }
            return max;
        }

        public void PrintVec(double[] vector)
        {
            for (int i = 0; i < vector.Length; i++)
            {
                Console.Write("{0} ", Math.Round(vector[i], 4));
            }
        }

        public double[,] GetC(double[,] mat)
        {
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    if (i > j)
                    {
                        mat[i, j] = 0;
                    }
                }
            }
            return mat;
        }

        public double[,] GetB(double[,] mat)
        {
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    if (i <= j)
                    {
                        mat[i, j] = 0;
                    }
                }
            }
            return mat;
        }

        public void ZeiX(double[,] alpha, double[] betta)
        {
            double[,] save = new double[alpha.GetLength(0), alpha.GetLength(1)];
            Array.Copy(alpha, save, alpha.Length);
            double[,] C = GetC(alpha);
            alpha = save;
            double[,] B = GetB(alpha);
            alpha = save;
            double[,] E = new double[alpha.GetLength(0), alpha.GetLength(1)];
            double e = 0.01;
            double[] x = betta;
            double[] xn = betta;
            double eps = 0;
            double[,] temp1 = new double[alpha.GetLength(0), alpha.GetLength(1)];
            double normC = NormMat(C);
            double normA = NormMat(alpha);
            for (int j = 0; j < alpha.GetLength(0); j++)
            {
                for (int k = 0; k < alpha.GetLength(1); k++)
                {
                    if (j == k)
                    {
                        E[j, k] = 1;
                    }
                }
            }
            for (int j = 0; j < alpha.GetLength(0); j++)
            {
                for (int k = 0; k < alpha.GetLength(1); k++)
                {
                    temp1[j, k] = E[j, k] - B[j, k];
                }
            }
            temp1 = Rev(temp1);
            double[] temp2 = MulVec(temp1, betta);
            temp1 = MulMat(temp1, C);
            do
            {
                x = xn;
                xn = MulVec(temp1, x);
                for (int i = 0; i < betta.Length; i++)
                {
                    xn[i] = xn[i] + temp2[i];
                }
                eps = Math.Round(Norms(x, xn) * normC / (1 - normA), 4);
                //Console.WriteLine("temp e = {0}", eps);
            } while (eps > e);

            Console.WriteLine("\n");
            Console.WriteLine("Метод Зейделя:");
            Console.Write("x = ( ");
            PrintVec(xn);
            Console.Write(")");
            Console.WriteLine("\n");
        }

        // ОБРАТНАЯ МАТРИЦА
        public double[,] Rev(double[,] matrixA)
        {
            int n = matrixA.GetLength(0); //кол-во строк
            int m = matrixA.GetLength(1); //кол-во столбцов
            int x, y, x1, y1;
            double[,] matrixS = new double[n, m];

            //разбиение на блоки и составление итоговой м-ы
            double[,] matrixS1 = new double[1, 1];
            matrixS1 = Copy(matrixA, 0, 0, 1, 1);

            double[,] matrixS2 = new double[2, 2];
            x = 0;
            y = 0;
            x1 = (int)Math.Ceiling((double)n / 2.0);
            y1 = (int)Math.Ceiling((double)m / 2.0);
            matrixS2 = Copy(matrixA, x, y, x1, y1);
            double[,] matrixS2_ = new double[2, 2];
            matrixS2_ = Div(matrixS2, matrixS1, x1, y1);

            double[,] matrixS3 = new double[3, 3];
            x = 0;
            y = 0;
            x1 = n - 1;
            y1 = m - 1;
            matrixS3 = Copy(matrixA, x, y, x1, y1);
            double[,] matrixS3_ = new double[3, 3];
            matrixS3_ = Div(matrixS3, matrixS2_, x1, y1);

            double[,] matrixS4 = new double[4, 4];
            x = 0;
            y = 0;
            x1 = n;
            y1 = m;
            matrixS4 = Copy(matrixA, x, y, x1, y1);
            double[,] matrixS4_ = new double[4, 4];
            matrixS4_ = Div(matrixS4, matrixS3_, x1, y1);
            return matrixS4_;
        }

        public double[,] Copy(double[,] matrixA, int x, int y, int x1, int y1) //копирование элементов 
        {
            var matrixA1 = new double[x1 - x, y1 - y];
            for (int i = x, i1 = 0; i < x1; i++, i1++)
            {
                for (int j = y, j1 = 0; j < y1; j++, j1++)
                {
                    matrixA1[i1, j1] = matrixA[i, j];
                }
            }
            return matrixA1;
        }

        public double[,] Div(double[,] matrixA, double[,] matrixS, int n, int m)//деление на блоки
        {
            int x, y, x1, y1;
            x = 0;
            y = 0;
            x1 = matrixS.GetLength(0);
            y1 = matrixS.GetLength(1);
            double[,] matrixA1 = new double[x1, y1];
            matrixA1 = Copy(matrixA, x, y, x1, y1);

            x = 0;
            y = matrixS.GetLength(1);
            x1 = matrixS.GetLength(0);
            y1 = m;
            double[,] matrixA2 = new double[x1, y1 - y];
            matrixA2 = Copy(matrixA, x, y, x1, y1);

            x = matrixS.GetLength(0);
            y = 0;
            x1 = n;
            y1 = matrixS.GetLength(1);
            double[,] matrixA3 = new double[x1 - x, y1];
            matrixA3 = Copy(matrixA, x, y, x1, y1);

            x = matrixS.GetLength(0);
            y = matrixS.GetLength(1);
            x1 = n;
            y1 = m;
            double[,] matrixA4 = new double[x1 - x, y1 - y];
            matrixA4 = Copy(matrixA, x, y, x1, y1);

            double[,] X = Multiply(matrixS, matrixA2);
            double[,] Y = Multiply(matrixA3, matrixS);
            double[,] O = Sum(matrixA4, Multiply(MultiplyCount(Y, -1), matrixA2));
            double[,] O_ = new double[O.GetLength(0), O.GetLength(1)];
            for (int i = 0; i < O.GetLength(0); i++)
            {
                for (int j = 0; j < O.GetLength(1); j++)
                {
                    O_[i, j] = 1 / O[i, j];
                }
            }
            matrixA = Creation(matrixA, Sum(matrixS, Multiply(Multiply(X, O_), Y)), 0, 0);
            matrixA = Creation(matrixA, Multiply(MultiplyCount(X, -1), O_), 0, m - matrixA2.GetLength(1));
            matrixA = Creation(matrixA, Multiply(MultiplyCount(O_, -1), Y), n - matrixA3.GetLength(0), 0);
            matrixA = Creation(matrixA, O_, n - matrixA4.GetLength(0), m - matrixA4.GetLength(1));

            return matrixA;
        }

        public static double[,] Creation(double[,] matrixC, double[,] matrixC1, int x, int y) //занесение блоков в общую м-у
        {
            for (int i = 0, i1 = x; i < matrixC1.GetLength(0); i++, i1++)
            {
                for (int j = 0, j1 = y; j < matrixC1.GetLength(1); j++, j1++)
                {
                    matrixC[i1, j1] = matrixC1[i, j];
                }
            }
            return matrixC;
        }

        public double[,] Multiply(double[,] matrixA, double[,] matrixB) //произведение м-ц
        {
            var matrixD = new double[matrixA.GetLength(0), matrixB.GetLength(1)];
            for (int i = 0; i < matrixA.GetLength(0); i++)
            {
                for (int j = 0; j < matrixB.GetLength(1); j++)
                {
                    for (var k = 0; k < matrixA.GetLength(1); k++)
                    {
                        matrixD[i, j] += matrixA[i, k] * matrixB[k, j];
                    }
                }
            }
            return matrixD;
        }

        public double[,] MultiplyCount(double[,] matrixA, double a) //произведение м-ц на число
        {
            var matrixD = new double[matrixA.GetLength(0), matrixA.GetLength(1)];
            for (int i = 0; i < matrixA.GetLength(0); i++)
            {
                for (int j = 0; j < matrixA.GetLength(1); j++)
                {
                    matrixD[i, j] = matrixA[i, j] * a;
                }
            }
            return matrixD;
        }

        public double[,] Sum(double[,] matrixA, double[,] matrixB) //сумма м-ц
        {
            var matrixS = new double[matrixA.GetLength(0), matrixB.GetLength(1)];
            for (int i = 0; i < matrixA.GetLength(0); i++)
            {
                for (int j = 0; j < matrixB.GetLength(1); j++)
                {
                    matrixS[i, j] = matrixA[i, j] + matrixB[i, j];
                }
            }
            return matrixS;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Task task = new Task();
            var matrix = task.GetMat();
            Console.WriteLine("Входная матрица:");
            task.PrintMat(matrix);

            double[,] alpha = task.Alphas(matrix);
            if (task.NormMat(alpha) >= 1)
            {
                Console.WriteLine("Условие сходимости не выполнено!");
            }
            else
            {
                double[] betta = task.Bettas(matrix);
                task.Xs(alpha, betta);
                task.ZeiX(alpha, betta);
            }
            Console.ReadKey();
        }
    }
}
