using System;

namespace Lab5
{
    public class Matrix
    {
        double[] str;  //строка, определяющая матрицу
        public Matrix(int n)  //конструктор для инициализации матрицы по умолчанию
        {
            str = new double[n];
            for (int i = 0; i < n; i++)
                str[i] = 2 * i + 1;
        }
        public Matrix(int n, double[] temp)  //конструктор для пользовательской инициализации матрицы
        {
            str = new double[n];
            for (int i = 0; i < n; i++)
                str[i] = temp[i];
        }
        public double[] Solve(double[] b)  //метод для решения системы линейных уравнений
        {
            int n = str.Length;
            double[] x = new double[n];
            double[] y = new double[n];
            x[0] = 1.0 / str[0];
            y[0] = x[0];
            for (int k = 1; k < n; k++)
            {
                double Fk = 0;
                double Gk = 0;
                for (int i = 0; i < k; i++)
                {
                    Fk += str[k - i] * x[i];
                    Gk += str[i + 1] * y[i];
                }
                double rk = 1.0 / (1.0 - Fk * Gk);
                double sk = -rk * Fk;
                double tk = -rk * Gk;
                double[] xk_1 = new double[k];
                double[] yk_1 = new double[k];
                for (int i = 0; i < k; i++)
                {
                    xk_1[i] = x[i];
                    yk_1[i] = y[i];
                }
                x[0] = xk_1[0] * rk + 0 * sk;
                y[0] = xk_1[0] * tk + 0 * rk;
                for (int i = 1; i < k; i++)
                {
                    x[i] = xk_1[i] * rk + yk_1[i - 1] * sk;
                    y[i] = xk_1[i] * tk + yk_1[i - 1] * rk;
                }
                x[k] = 0 * rk + yk_1[k - 1] * sk;
                y[k] = 0 * tk + yk_1[k - 1] * rk;
            }
            double[,] matrix1 = new double[n, n];
            double[,] matrix2 = new double[n, n];
            double[,] matrix3 = new double[n, n];
            double[,] matrix4 = new double[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    matrix1[i, j] = (i >= j) ? x[i - j] : 0;
                    matrix2[i, j] = (j >= i) ? y[n - 1 - j + i] : 0;
                    matrix3[i, j] = (i > j) ? y[i - 1 - j] : 0;
                    matrix4[i, j] = (j > i) ? x[n - 1 - j + 1 + i] : 0;
                }
            double[,] matrix12 = new double[n, n];
            double[,] matrix34 = new double[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    double sum_ab = 0;
                    double sum_cd = 0;
                    for (int k = 0; k < n; k++)
                    {
                        sum_ab += matrix1[i, k] * matrix2[k, j];
                        sum_cd += matrix3[i, k] * matrix4[k, j];
                    }
                    matrix12[i, j] = sum_ab;
                    matrix34[i, j] = sum_cd;
                }
            double[,] invert = new double[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    invert[i, j] = (1.0 / x[0]) * (matrix12[i, j] - matrix34[i, j]);
            double[] res = new double[n];
            for (int i = 0; i < n; i++)
            {
                res[i] = 0;
                for (int j = 0; j < n; j++)
                    res[i] += invert[i, j] * b[j];
            }
            return res;
        }
        public override string ToString()//перегруженная версия виртуального метода ToString
        {
            string res = "";
            int n = str.Length;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++) res += ("\t" + str[Math.Abs(i - j)]);
                res += "\n ";
            }
            return res;
        }
    }
}