// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
namespace Integral
{

    class Program
    {
        static void Main(string[] args)
        {
            //var system = EquationSystemParser.ParseFromJson("SYSTEM.json");


            //Console.WriteLine("Добро пожаловать в программу численного интегрирования!");
            //Console.WriteLine("Введите путь к JSON-файлу с описанием системы уравнений:");
            //string filePath = Console.ReadLine();

            string filePath = "SYSTEM.json";

            // Загружаем систему уравнений из JSON
            ISystem system;
            int numberOfEquations;

            try
            {
                system = new EquationSystemStub();
                numberOfEquations = 2; // Получаем число уравнений
                Console.WriteLine($"Система уравнений успешно загружена. Число уравнений: {numberOfEquations}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке системы уравнений: {ex.Message}");
                return;
            }

            double[] result = system.Compute(0.1, new double[] { 1.0, 1.0 });
            Console.WriteLine(string.Join(", ", result));

            Console.WriteLine("\nВыберите метод решения:");
            Console.WriteLine("1 - Метод Эйлера");
            Console.WriteLine("2 - Метод Рунге-Кутта 4-го порядка");
            Console.WriteLine("3 - Адаптивный метод конечно-разностной формулы");
            Console.Write("Введите номер метода: ");
            int methodChoice;

            if (!int.TryParse(Console.ReadLine(), out methodChoice) || methodChoice < 1 || methodChoice > 3)
            {
                Console.WriteLine("Неверный выбор метода.");
                return;
            }

            ISolver solver = methodChoice switch
            {
                1 => new EulerSolver(),
                2 => new RungeKuttaSolver(),
                3 => new AdaptiveFiniteDifferenceSolver(tolerance: 1e-6),
                _ => throw new InvalidOperationException("Неизвестный метод решения.")
            };

            Console.WriteLine("\nВыберите способ задания параметров:");
            Console.WriteLine("1 - Ввести параметры вручную");
            Console.WriteLine("2 - Автоматический подбор параметров");
            Console.Write("Введите номер: ");
            
            if (!int.TryParse(Console.ReadLine(), out int parameterChoice) || parameterChoice < 1 || parameterChoice > 2)
            { 
                Console.WriteLine("Неверный выбор способа задания параметров.");
                return; 
            }
            double x0, xEnd, hInitial; double[] y0;

            if (parameterChoice == 1)
            {
                // Ввод параметров вручную
                Console.WriteLine("\nВведите параметры интегрирования:");
                Console.Write("Начальное значение X (x0): ");
                x0 = double.Parse(Console.ReadLine());

                Console.Write("Начальное значение Y (через пробел): ");
                y0 = Array.ConvertAll(Console.ReadLine().Split(' '), double.Parse);

                Console.Write("Конечное значение X (xEnd): ");
                xEnd = double.Parse(Console.ReadLine());

                Console.Write("Начальный шаг интегрирования (h): ");
                hInitial = double.Parse(Console.ReadLine());
            }
            else
            {
                // Автоматический подбор параметров
                Console.WriteLine("\nВыполняется автоматический подбор параметров...");
                x0 = 0.01;
                xEnd = 10.0;
                hInitial = 0.1;
                y0 = new double[numberOfEquations];
                for (int i = 0; i < numberOfEquations; i++)
                {
                    y0[i] = 1.0;
                }

                Console.WriteLine($"Автоматически установлены параметры: x0 = {x0}, xEnd = {xEnd}, h = {hInitial}");
                Console.WriteLine("Начальные значения Y: " + string.Join(", ", y0));
            }

            // Решаем систему уравнений
            Console.WriteLine("\nРешение системы...");
            Solution solution;

            try
            {
                solution = solver.Solve(system, x0, y0, xEnd, hInitial);
                Console.WriteLine("Решение завершено.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при решении системы: {ex.Message}");
                return;
            }

            // Вывод результатов
            Console.WriteLine("\nРезультаты решения:");
            for (int i = 0; i < solution.X.Count; i++)
            {
                Console.Write($"x = {solution.X[i]:F3}, ");
                for (int j = 0; j < solution.Y[i].Length; j++)
                {
                    Console.Write($"y{j + 1} = {solution.Y[i][j]:F6} ");
                }
                Console.WriteLine();
            }

            // Сохранение графика
            var plotter = new Plotter();
            string outputFilePath = "output.png";

            try
            {
                plotter.Plot(solution, outputFilePath);
                Console.WriteLine($"\nГрафик решения сохранен в файл: {outputFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении графика: {ex.Message}");
            }

            Console.WriteLine("\nРабота программы завершена.");
        }
    }


}