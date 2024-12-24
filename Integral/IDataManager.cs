using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integral
{
    public interface IDataManager
    {
        void SaveResults(string filePath, List<(double X, double[] Y)> results);
        (double x0, double xEnd, double h, double[] y0) LoadInput(string filePath);
    }
}
