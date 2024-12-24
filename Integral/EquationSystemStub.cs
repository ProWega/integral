using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integral
{
    public class EquationSystemStub : ISystem
    {
        /// <summary>
        /// Вычисляет значения уравнений системы для заданных параметров.
        /// </summary>
        /// <param name="x">Значение независимой переменной.</param>
        /// <param name="y">Массив значений зависимых переменных.</param>
        /// <returns>Результаты системы уравнений.</returns>
        public double[] Compute(double x, double[] y)
        {
            if (y.Length != 2)
            {
                throw new InvalidOperationException("Ожидается массив из 2 переменных: y1 и y2.");
            }

            // Предопределённые уравнения (замена на заглушку):
            // Уравнение 1: -4 * y1 - 2 * y2 + 2 / (Exp(-x) - 1)
            // Уравнение 2: 6 * y1 + 3 * y2 - 3 / (Sin(x) + 1)

            var results = new double[2];
            results[0] = -4 * y[0] - 2 * y[1] + 2 / (Math.Exp(-x) - 1);
            results[1] = 6 * y[0] + 3 * y[1] - 3 / (Math.Sin(x) + 1);

            return results;
        }
    }

}
