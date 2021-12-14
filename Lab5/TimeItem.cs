using System;

namespace Lab5
{
    [Serializable]
    public class TimeItem
    {
        public int n { get; set; } //порядок матрицы
        public int repeat { get; set; } // число повторов
        public double sharp { get; set; }  //время выполнения цикла в коде на C#
        public double si { get; set; }  // время выполнения цикла в коде на C++
        public double coef { get { return sharp / si; } } //коэффициент, равный отношению времени выполнения кода на C# и кода на C++
        public override string ToString()
        {
            return n + "\t         " + repeat + "\t        " + sharp + "\t        " + si + "\t        " + coef;
        }
    }
}