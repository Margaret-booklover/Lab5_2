using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Lab5
{
    class Program
    {
        const string DllPath = "C:/Users/Fujitsu/Desktop/студент/2 курс/c#/Lab5/Debug/DLL1.dll";
        [DllImport(DllPath)]
        static extern double ForTime(int n, int repeat);
        [DllImport(DllPath)]
        static unsafe extern void ForSolve(int n, double[] a, double[] b, double* x);
        unsafe static void Main(string[] args)
        {
            //программно задать матрицу 3-го порядка и правую часть
            const int n = 3;
            double[] a = new double[n] { 3, 1, 2 };
            double[] b = new double[n] { 8, -4, 6 };
            double[] x = new double[n];
            try
            {
                //Решить систему линейных уравнений  
                Matrix ob = new Matrix(n, a);
                //вывести матрицу, правую часть и решение, полученное в коде C#
                double[] ans = ob.Solve(b);
                Console.WriteLine("Матрица");
                Console.WriteLine(ob);
                Console.WriteLine("Правая часть");
                foreach (double i in b)
                    Console.WriteLine(i);
                Console.WriteLine("Решение, полученное с помощью C#");
                foreach (double i in ans)
                    Console.WriteLine(i);
                //_______________________________________________________________________________________
                //Передать в код C++ через параметры глобальной экспортируемой функции данные, которые определяют матрицу и правую часть
                //Решить систему линейных уравнений, решение передать в код C# и вывести полученное решение
                fixed (double* result = x) ForSolve(3, a, b, result);
                Console.WriteLine("Решение, полученное с помощью C++");
                for (int i = 0; i < x.Length; i++)
                    Console.WriteLine(x[i]);
                //___________________________________________________________________________________________
                //Создать один объект типа TimesList и предложить пользователю ввести имя файла
                TimeList tl = new TimeList();
                Console.WriteLine("Введите имя файла (используйте расширение .dat)");
                string filename = Console.ReadLine();
                if (File.Exists(filename))
                {
                    Console.WriteLine("Файл обнаружен, началась загрузка данных...");
                    tl.Load(filename);
                    Console.WriteLine(tl);
                }
                else
                {
                    Console.WriteLine("Не было файла с таким именем, создание нового...");
                    File.Create(filename);
                }
                //___________________________________________________________________________________________
                //приглашение ввести порядок матрицы и число повторов или завершить работу приложения
                while (true)
                {
                    Console.WriteLine("Завершить работу приложения или продолжить работу? (1 - продолжить, 0 - завершить)");
                    if (Console.ReadLine() != "1") break;
                    TimeItem item = new TimeItem();
                    Console.WriteLine("Введите порядок матрицы (натуральное число)");
                    item.n = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Введите требуемое число повторов");
                    item.repeat = Convert.ToInt32(Console.ReadLine());
                    //_____________________________________________________________________
                    //вычисления на C#
                    item.sharp = ForTiming(item.n, item.repeat);
                    //вызов экспортируемой функции из DLL-библиотеки С++
                    item.si = ForTime(item.n, item.repeat);
                    //добавляет объект TimeItem в список List<TimeItem> объекта  TimeList
                    tl.Add(item);
                }
                //_______________________________________________________________________________________
                //выводится вся коллекция из объекта TimesList
                Console.WriteLine(tl);
                //коллекция из объекта TimeList сохраняется c использованием сериализации
                //в файле с именем, которое пользователь ввел в начале работы приложения
                tl.Save(filename);
            }
            catch
            {
                Console.WriteLine("В процессе выполнения программы возникла ошибка");
            }
        }
        static double ForTiming(int n, int k)
        {
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            Matrix matrix = new Matrix(n);
            double[] right = new double[n];
            for (int i = 0; i < n; i++)
                right[i] = (i + 1) * 10;
            for (int i = 0; i < k; i++)
                matrix.Solve(right);
            sw.Stop();
            return sw.Elapsed.TotalSeconds;
        }
    }
}