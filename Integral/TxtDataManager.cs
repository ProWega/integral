using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integral
{
    public class TxtDataManager : IDataManager
    {
        public (double x0, double xEnd, double h, double[] y0) LoadInput(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            double x0 = 0, xEnd = 0, h = 0;
            double[] y0 = null;

            foreach (var line in lines)
            {
                if (line.StartsWith("x0="))
                    x0 = double.Parse(line.Split('=')[1], CultureInfo.InvariantCulture);
                else if (line.StartsWith("x_end="))
                    xEnd = double.Parse(line.Split('=')[1], CultureInfo.InvariantCulture);
                else if (line.StartsWith("h="))
                    h = double.Parse(line.Split('=')[1], CultureInfo.InvariantCulture);
                else if (line.StartsWith("y0="))
                    y0 = line.Split('=')[1]
                            .Split(',')
                            .Select(value => double.Parse(value, CultureInfo.InvariantCulture))
                            .ToArray();
            }

            return (x0, xEnd, h, y0);
        }

        public void SaveResults(string filePath, List<(double X, double[] Y)> results)
        {
            using var writer = new StreamWriter(filePath);
            writer.WriteLine("x,y1,y2");
            foreach (var result in results)
            {
                writer.WriteLine($"{result.X.ToString(CultureInfo.InvariantCulture)},{string.Join(",", result.Y.Select(y => y.ToString(CultureInfo.InvariantCulture)))}");
            }
        }
    }
}
