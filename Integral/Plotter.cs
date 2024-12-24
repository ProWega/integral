using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;


namespace Integral
{
    public class Plotter
    {
        public void Plot(Solution solution, string outputFilePath)
        {
            const int width = 800;  // Ширина изображения
            const int height = 600; // Высота изображения
            const int margin = 50;  // Отступ от краев

            using var bitmap = new SKBitmap(width, height);
            using var canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.White);

            // Определяем диапазоны
            double minX = solution.X[0];
            double maxX = solution.X[^1];
            double minY = double.MaxValue;
            double maxY = double.MinValue;

            foreach (var yValues in solution.Y)
            {
                foreach (var y in yValues)
                {
                    minY = Math.Min(minY, y);
                    maxY = Math.Max(maxY, y);
                }
            }

            // Рисуем клеточный фон
            DrawGrid(canvas, width, height, margin, minX, maxX, minY, maxY);

            // Рисуем оси
            DrawAxes(canvas, width, height, margin, minX, maxX, minY, maxY);

            // Рисуем график
            DrawGraph(canvas, width, height, margin, solution, minX, maxX, minY, maxY);

            // Сохраняем изображение в файл
            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            using var stream = File.Create(outputFilePath);
            data.SaveTo(stream);

            Console.WriteLine($"График сохранен в файл: {outputFilePath}");
        }

        private void DrawGrid(SKCanvas canvas, int width, int height, int margin, double minX, double maxX, double minY, double maxY)
        {
            var paint = new SKPaint
            {
                Color = SKColors.LightGray,
                StrokeWidth = 1
            };

            // Расстояние между линиями
            double xStep = (maxX - minX) / 10;
            double yStep = (maxY - minY) / 10;

            // Вертикальные линии
            for (double x = minX; x <= maxX; x += xStep)
            {
                float xCanvas = TransformX(x, width, margin, minX, maxX);
                canvas.DrawLine(xCanvas, margin, xCanvas, height - margin, paint);
            }

            // Горизонтальные линии
            for (double y = minY; y <= maxY; y += yStep)
            {
                float yCanvas = TransformY(y, height, margin, minY, maxY);
                canvas.DrawLine(margin, yCanvas, width - margin, yCanvas, paint);
            }
        }

        private void DrawAxes(SKCanvas canvas, int width, int height, int margin, double minX, double maxX, double minY, double maxY)
        {
            var axisPaint = new SKPaint
            {
                Color = SKColors.Black,
                StrokeWidth = 2
            };

            var textPaint = new SKPaint
            {
                Color = SKColors.Black,
                TextSize = 16
            };

            // Ось X
            float yAxisCanvas = TransformY(0, height, margin, minY, maxY);
            canvas.DrawLine(margin, yAxisCanvas, width - margin, yAxisCanvas, axisPaint);

            // Ось Y
            float xAxisCanvas = TransformX(0, width, margin, minX, maxX);
            canvas.DrawLine(xAxisCanvas, margin, xAxisCanvas, height - margin, axisPaint);

            // Метки на оси X
            double xStep = (maxX - minX) / 10;
            for (double x = minX; x <= maxX; x += xStep)
            {
                float xCanvas = TransformX(x, width, margin, minX, maxX);
                canvas.DrawText($"{x:F2}", xCanvas - 10, yAxisCanvas + 20, textPaint);
            }

            // Метки на оси Y
            double yStep = (maxY - minY) / 10;
            for (double y = minY; y <= maxY; y += yStep)
            {
                float yCanvas = TransformY(y, height, margin, minY, maxY);
                canvas.DrawText($"{y:F2}", xAxisCanvas - 30, yCanvas + 5, textPaint);
            }
        }

        private void DrawGraph(SKCanvas canvas, int width, int height, int margin, Solution solution, double minX, double maxX, double minY, double maxY)
        {
            var colors = new[] { SKColors.Blue, SKColors.Red, SKColors.Green, SKColors.Orange };
            var paints = new List<SKPaint>();

            for (int i = 0; i < solution.Y[0].Length; i++)
            {
                paints.Add(new SKPaint
                {
                    Color = colors[i % colors.Length],
                    StrokeWidth = 2,
                    IsAntialias = true
                });
            }

            for (int i = 0; i < solution.Y[0].Length; i++)
            {
                for (int j = 0; j < solution.X.Count - 1; j++)
                {
                    float x1 = TransformX(solution.X[j], width, margin, minX, maxX);
                    float y1 = TransformY(solution.Y[j][i], height, margin, minY, maxY);
                    float x2 = TransformX(solution.X[j + 1], width, margin, minX, maxX);
                    float y2 = TransformY(solution.Y[j + 1][i], height, margin, minY, maxY);

                    canvas.DrawLine(x1, y1, x2, y2, paints[i]);
                }
            }
        }

        private float TransformX(double x, int width, int margin, double minX, double maxX)
        {
            return margin + (float)((x - minX) / (maxX - minX) * (width - 2 * margin));
        }

        private float TransformY(double y, int height, int margin, double minY, double maxY)
        {
            return height - margin - (float)((y - minY) / (maxY - minY) * (height - 2 * margin));
        }
    }
}
