using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integral
{
    public class AdaptiveFiniteDifferenceSolver : ISolver
    {
        private readonly double _tolerance;

        public AdaptiveFiniteDifferenceSolver(double tolerance)
        {
            _tolerance = tolerance;
        }

        public Solution Solve(ISystem system, double x0, double[] y0, double xEnd, double h)
        {
            var solution = new Solution();
            solution.AddPoint(x0, y0);

            var nodes = new List<(double X, double[] Y)>
            {
                (x0, y0)
            };

            // Выполняем начальные шаги с использованием EulerSolver
            var eulerSolver = new EulerSolver();
            var eulerSolution = eulerSolver.Solve(system, x0, y0, x0 + 3 * h, h);

            // Добавляем первые три точки из решения метода Эйлера
            for (int i = 1; i < eulerSolution.X.Count; i++)
            {
                nodes.Add((eulerSolution.X[i], eulerSolution.Y[i]));
                solution.AddPoint(eulerSolution.X[i], eulerSolution.Y[i]);
            }

            while (nodes[^1].X < xEnd)
            {
                // Оценка четвертой производной
                double[] fourthDerivative = EstimateFourthDerivative(nodes);

                // Адаптивный выбор шага
                double newH = SelectStep(fourthDerivative, h);

                // Решение с помощью метода конечных разностей
                var (nextX, nextY) = FiniteDifferenceStep(system, nodes, newH);
                nodes.Add((nextX, nextY));
                solution.AddPoint(nextX, nextY);

                h = newH;
            }

            return solution;
        }

        private double[] EstimateFourthDerivative(List<(double X, double[] Y)> nodes)
        {
            int n = nodes[0].Y.Length;
            var fourthDerivatives = new double[n];

            if (nodes.Count < 5)
            {
                throw new InvalidOperationException("Для вычисления четвертой производной требуется минимум 5 узлов.");
            }

            var h = nodes[^1].X - nodes[^2].X;
            var yMinus2h = nodes[^5].Y;
            var yMinusH = nodes[^4].Y;
            var y = nodes[^3].Y;
            var yPlusH = nodes[^2].Y;
            var yPlus2h = nodes[^1].Y;

            for (int i = 0; i < n; i++)
            {
                fourthDerivatives[i] = (yMinus2h[i] - 4 * yMinusH[i] + 6 * y[i] - 4 * yPlusH[i] + yPlus2h[i]) / Math.Pow(h, 4);
            }

            return fourthDerivatives;
        }

        private double SelectStep(double[] fourthDerivative, double h)
        {
            double maxFourthDerivative = 0.0;
            foreach (var value in fourthDerivative)
            {
                maxFourthDerivative = Math.Max(maxFourthDerivative, Math.Abs(value));
            }
            return h * Math.Pow(_tolerance / maxFourthDerivative, 0.25);
        }

        private (double X, double[] Y) FiniteDifferenceStep(ISystem system, List<(double X, double[] Y)> nodes, double h)
        {
            int n = nodes[0].Y.Length;

            // Формируем матрицу C и вектор c
            double[,] Ci = CalculateMatrixCi(system, nodes, h);
            double[] ci = CalculateVectorCi(system, nodes, h);

            // Решаем систему методом Гаусса
            double[] newY = GaussSolver.Solve(Ci, ci);

            // Вычисляем новое значение X
            double newX = nodes[^1].X + h;
            return (newX, newY);
        }

        private double[,] CalculateMatrixCi(ISystem system, List<(double X, double[] Y)> nodes, double h)
        {
            int n = nodes[0].Y.Length;
            var Ci = new double[n, n];

            // Формируем матрицу C_i для конечно-разностного уравнения
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Ci[i, j] = (i == j ? 1 : 0) - h * system.Compute(nodes[^1].X, nodes[^1].Y)[j];
                }
            }

            return Ci;
        }

        private double[] CalculateVectorCi(ISystem system, List<(double X, double[] Y)> nodes, double h)
        {
            int n = nodes[0].Y.Length;
            var ci = new double[n];
            var prevY = nodes[^1].Y;

            // Формируем вектор c_i
            for (int i = 0; i < n; i++)
            {
                ci[i] = prevY[i];
            }

            return ci;
        }
    }

}