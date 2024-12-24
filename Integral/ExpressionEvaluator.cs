using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NCalc;

namespace Integral
{
    public class ExpressionEvaluator
    {
        public double Evaluate(string expression, Dictionary<string, double> variables)
        {
            try
            {
                // Создаем выражение
                var exp = new Expression(expression);

                // Добавляем переменные
                foreach (var variable in variables)
                {
                    exp.Parameters[variable.Key] = variable.Value;
                }

                // Регистрация математических функций
                exp.EvaluateFunction += (name, args) =>
                {
                    double arg = Convert.ToDouble(args.Parameters[0]); // Преобразуем аргумент в double

                    args.Result = name switch
                    {
                        "Exp" => Math.Exp(arg),
                        "Sin" => Math.Sin(arg),
                        "Cos" => Math.Cos(arg),
                        "Log" => Math.Log(arg),
                        "Sqrt" => Math.Sqrt(arg),
                        _ => throw new InvalidOperationException($"Неизвестная функция: {name}")
                    };
                };

                // Вычисляем выражение
                var result = exp.Evaluate();

                // Проверяем корректность результата
                if (result is double res && (double.IsNaN(res) || double.IsInfinity(res)))
                {
                    throw new InvalidOperationException($"Результат вычисления выражения '{expression}' некорректен: {result}");
                }

                return Convert.ToDouble(result);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка при вычислении выражения '{expression}': {ex.Message}");
            }
        }
    }

}
