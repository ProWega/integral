using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integral
{
    public static class GaussSolver
    {
        public static double[] Solve(double[,] matrix, double[] rhs)
        {
            int n = rhs.Length;
            var augmentedMatrix = new double[n, n + 1];

            // Формируем расширенную матрицу
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    augmentedMatrix[i, j] = matrix[i, j];
                }
                augmentedMatrix[i, n] = rhs[i];
            }

            // Прямой ход метода Гаусса
            for (int k = 0; k < n; k++)
            {
                // Поиск строки с максимальным элементом
                int maxRow = k;
                for (int i = k + 1; i < n; i++)
                {
                    if (Math.Abs(augmentedMatrix[i, k]) > Math.Abs(augmentedMatrix[maxRow, k]))
                    {
                        maxRow = i;
                    }
                }

                // Перестановка строк
                for (int j = k; j < n + 1; j++)
                {
                    var temp = augmentedMatrix[maxRow, j];
                    augmentedMatrix[maxRow, j] = augmentedMatrix[k, j];
                    augmentedMatrix[k, j] = temp;
                }

                // Приведение к треугольному виду
                for (int i = k + 1; i < n; i++)
                {
                    double factor = augmentedMatrix[i, k] / augmentedMatrix[k, k];
                    for (int j = k; j < n + 1; j++)
                    {
                        augmentedMatrix[i, j] -= factor * augmentedMatrix[k, j];
                    }
                }
            }

            // Обратный ход метода Гаусса
            var solution = new double[n];
            for (int i = n - 1; i >= 0; i--)
            {
                solution[i] = augmentedMatrix[i, n] / augmentedMatrix[i, i];
                for (int j = i - 1; j >= 0; j--)
                {
                    augmentedMatrix[j, n] -= augmentedMatrix[j, i] * solution[i];
                }
            }

            return solution;
        }
    }
}
