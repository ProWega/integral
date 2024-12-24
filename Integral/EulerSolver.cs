using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integral
{
    public class EulerSolver : ISolver
    {
        public Solution Solve(ISystem system, double x0, double[] y0, double xEnd, double h)
        {
            var solution = new Solution();
            solution.AddPoint(x0, y0);

            double x = x0;
            var y = (double[])y0.Clone();

            while (x <= xEnd)
            {
                double[] derivatives = system.Compute(x, y);
                for (int i = 0; i < y.Length; i++)
                {
                    y[i] += h * derivatives[i];
                }

                x += h;
                solution.AddPoint(x, y);
            }

            return solution;
        }
    }
}
