using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integral
{
    public class Solution
    {
        public List<double> X { get; }
        public List<double[]> Y { get; }

        public Solution()
        {
            X = new List<double>();
            Y = new List<double[]>();
        }

        public void AddPoint(double x, double[] y)
        {
            X.Add(x);
            Y.Add((double[])y.Clone());
        }
    }
}
