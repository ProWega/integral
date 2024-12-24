using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integral
{
    public interface ISolver
    {
        Solution Solve(ISystem system, double x0, double[] y0, double xEnd, double h);
    }
}
