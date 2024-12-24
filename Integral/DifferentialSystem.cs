using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integral
{
    public class DifferentialSystem
    {
        public static double[] SystemEquations(double x, double[] y)
        {
            return new double[]
            {
                -4 * y[0] - 2 * y[1] + 2 / Math.Exp(x) - 1,  // y1'
                6 * y[0] + 3 * y[1] - 3 / Math.Exp(x) - 1   // y2'
            };
        }
    }
}
