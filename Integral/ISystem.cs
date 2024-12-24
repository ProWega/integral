using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integral
{
    public interface ISystem
    {
        // Метод для вычисления правой части системы уравнений
        double[] Compute(double x, double[] y);
    }
}
