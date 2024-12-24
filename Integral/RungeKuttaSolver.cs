using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integral
{
    public class RungeKuttaSolver : ISolver
    {
        public Solution Solve(ISystem system, double x0, double[] y0, double xEnd, double h)
        {
            var solution = new Solution();
            solution.AddPoint(x0, y0);

            double x = x0;
            var y = (double[])y0.Clone();

            while (x <= xEnd)
            {
                var k1 = system.Compute(x, y);
                var k2 = system.Compute(x + h / 2, AddVectors(y, MultiplyVector(k1, h / 2)));
                var k3 = system.Compute(x + h / 2, AddVectors(y, MultiplyVector(k2, h / 2)));
                var k4 = system.Compute(x + h, AddVectors(y, MultiplyVector(k3, h)));

                for (int i = 0; i < y.Length; i++)
                {
                    y[i] += h / 6 * (k1[i] + 2 * k2[i] + 2 * k3[i] + k4[i]);
                }

                x += h;
                solution.AddPoint(x, y);
            }

            return solution;
        }

        private double[] AddVectors(double[] v1, double[] v2)
        {
            var result = new double[v1.Length];
            for (int i = 0; i < v1.Length; i++)
            {
                result[i] = v1[i] + v2[i];
            }
            return result;
        }

        private double[] MultiplyVector(double[] v, double scalar)
        {
            var result = new double[v.Length];
            for (int i = 0; i < v.Length; i++)
            {
                result[i] = v[i] * scalar;
            }
            return result;
        }
    }
}
