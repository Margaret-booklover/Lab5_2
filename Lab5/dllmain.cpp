#include "pch.h"
#include <iostream>
#include <cstdio>
#include <ctime>
class Matrix
{
	int n = 0;  //размерность матрицы
	double* str;  //строка, которой задается матрица
public:
	Matrix(int n)  //конструктор для инициализации матрицы по умолчанию
	{
		this->n = n;
		str = new double[n];
		for (int i = 0; i < n; i++)
			str[i] = 2 * i + 1;
	}
	Matrix(int n, double* mas)  //конструктор для пользовательской инициализации матрицы
	{
		this->n = n;
		str = new double[n];
		for (int i = 0; i < n; i++)
			str[i] = mas[i];
	}
	Matrix(const Matrix& ob)  //конструктор копирования с прототипом
	{
		n = ob.n;
		str = new double[n];
		for (int i = 0; i < n; i++)
			str[i] = ob.str[i];
	}
	~Matrix()  //деструктор
	{
		delete str;
	}
	Matrix& operator=(const Matrix& temp)  //операция присваивания c прототипом
	{
		if (this == &temp) return *this;  //Если попытка присвоиться в себя, то сразу выходим, отдавая ссылку на самого себя
		delete[] str;  //зачищаем строку нынешнего объекта
		this->n = temp.n;  //узнаём длину строки
		str = new double[n];  //выделяем память
		for (int i = 0; i < n; i++)
			str[i] = temp.str[i];   //копируем в новорожденную строку значения из присваиваемой в неё строку
		return *this;  //отдаём назад ссылку на самого себя
	}
	double* Solve(double b[], double* res = nullptr)  //метод для решения системы линейных уравнений
	{
		double* x = new double[n];
		double* y = new double[n];
		x[0] = 1.0 / str[0];
		y[0] = x[0];
		for (int k = 1; k <n; k++)  //расчет х и у по рекуррентным формулам
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
			double* xk_1 = new double[k];
			double* yk_1 = new double[k];
			for (int i = 0; i < k; i++)
			{
				xk_1[i] = x[i];  //перекопируем данные из х и у во временные массивы
				yk_1[i] = y[i];
			}
			x[0] = xk_1[0] * rk + 0 * sk;
			y[0] = xk_1[0] * tk + 0 * rk;
			for (int i = 1; i < k; i++)
			{
				x[i] = xk_1[i] * rk + yk_1[i - 1] * sk; //чтобы потом перезаписать их по рекуррентным формулам
				y[i] = xk_1[i] * tk + yk_1[i - 1] * rk;
			}
			x[k] = 0 * rk + yk_1[k - 1] * sk;
			y[k] = 0 * tk + yk_1[k - 1] * rk;
			delete xk_1; delete yk_1;  //очищаем память выделенную на хранение временных массивов
		}
		double** matrix1 = new double* [n]; //создаем четыре части матрицы, из которых потом соберем обратную матрицу
		double** matrix2 = new double* [n];
		double** matrix3 = new double* [n];
		double** matrix4 = new double* [n];
		for (int i = 0; i < n; i++)
		{
			matrix1[i] = new double[n];
			matrix2[i] = new double[n];
			matrix3[i] = new double[n];
			matrix4[i] = new double[n];
			for (int j = 0; j < n; j++)
			{
				if (i >= j)  matrix1[i][j] = x[i - j]; //первые две матрицы, главные диагонали у них не нулевые
				else matrix1[i][j] = 0;
				if (j >= i) matrix2[i][j] = y[n - 1 - j + i];
				else matrix2[i][j] = 0;
				if (i > j)  //вторые две матрицы, главные диагонали у них нулевые
				{
					matrix3[i][j] = y[i - 1 - j];
					matrix4[i][j] = 0;
				}
				else
				{
					matrix3[i][j] = 0;
					matrix4[i][j] = x[n - 1 - j + 1 + i];
				}
			}
		}
		double** matrix12 = new double* [n]; //из этих четырех сначала сделаем две
		double** matrix34 = new double* [n];
		for (int i = 0; i < n; i++)
		{
			matrix12[i] = new double[n];  //выделили на них память
			matrix34[i] = new double[n];
			for (int j = 0; j < n; j++)
			{
				double sum12 = 0;
				double sum34 = 0;
				for (int k = 0; k < n; k++)
				{
					sum12 += matrix1[i][k] * matrix2[k][j];  //последовательно перемножаются и складываются элементы
					sum34 += matrix3[i][k] * matrix4[k][j];  //матриц 1 и 2, 3 и 4 соответственно
				}
				matrix12[i][j] = sum12;  //полученные значения записываются в соответствующие ячейки новых двух матриц
				matrix34[i][j] = sum34;
			}
		}
		double** invert = new double* [n]; //заготовка для обратной матрицы
		for (int i = 0; i < n; i++)
		{
			invert[i] = new double[n];
			for (int j = 0; j < n; j++)
				invert[i][j] = (1.0 / x[0]) * (matrix12[i][j] - matrix34[i][j]); 
		}
		if (res == nullptr)  res = new double[n]; //выделяем память под вектор с ответом
		for (int i = 0; i < n; i++)
		{
			res[i] = 0;
			for (int j = 0; j < n; j++)
				res[i] += invert[i][j] * b[j];//элемент строки вычисляется перемножением обратной матрицы с вектором правой части
		}
		delete[] matrix1;//освобождаем всю память, выделенную под вспомогательные матрицы
		delete[] matrix2;
		delete[] matrix3;
		delete[] matrix4;
		delete[] matrix12;
		delete[] matrix34;
		delete[] invert;
		return res;
	}
};

#include <iostream>
#include <cstdio>
#include <ctime>
using namespace std;

extern "C" __declspec(dllexport) double ForTime(int n, int repeat)  //замеры времени
{
	double time = 0;
	clock_t begin = clock();
	Matrix m = Matrix(n);  //создаем матрицу по умолчанию
	double* b = new double[n];  //создаем правую часть
	for (int i = 0; i < n; i++) 
		b[i] = 5 * i + 2;  //как-нибудь заполняем ее
	for (int i = 0; i < repeat; i++)
		m.Solve(b);  //решаем одну и ту же систему заданное число раз
	time = (clock() - begin) / (double)CLOCKS_PER_SEC;  //переводим тики в секунды
	delete b;
	return time;
}
extern "C" __declspec(dllexport) void ForSolve(int n, double a[], double b[], double* x)  //решение матричного уравнения
{
	Matrix matrix = Matrix(n, a);
	matrix.Solve(b, x);
}