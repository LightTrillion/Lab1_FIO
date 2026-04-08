using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laba_6_080426
{
    internal class ProcessTrinagle
    {
        internal static (string type, List<(int, int)> coords) PT(double sa, double sb, double sc, ILogger logger)
        {
            DateTime now = DateTime.Now;
            string inputParams = $"A:{sa}, B:{sb}, C:{sc}";

            try
            {
                // 1. Валидация входных данных
                if (sa <= 0 || sb <= 0 || sc <= 0)
                {
                    var res = ("", new List<(int, int)> { (-2, -2), (-2, -2), (-2, -2) });
                    logger.Write(FormatError(now, inputParams, "Невалидные (нечисловые или отрицательные) данные", ""));
                    return res;
                }

                // 2. Проверка на существование треугольника
                if (sa + sb <= sc || sa + sc <= sb || sb + sc <= sa)
                {
                    var res = ("не треугольник", new List<(int, int)> { (-1, -1), (-1, -1), (-1, -1) });
                    logger.Write(FormatError(now, inputParams, res.Item1, ""));
                    return res;
                }

                // 3. Определение типа
                string type = "разносторонний";
                if (sa == sb && sb == sc) type = "равносторонний";
                else if (sa == sb || sb == sc || sa == sc) type = "равнобедренный";

                // 4. Вычисление координат (вершина A в 0,0, B на оси X)
                double x1 = 0, y1 = 0;
                double x2 = sa, y2 = 0;
                double x3 = (sa * sa + sb * sb - sc * sc) / (2 * sa);
                double y3 = Math.Sqrt(Math.Max(0, sb * sb - x3 * x3));

                // 5. Масштабирование под 100x100
                var rawCoords = new[] { (x1, y1), (x2, y2), (x3, y3) };
                double maxX = rawCoords.Max(p => p.Item1);
                double maxY = rawCoords.Max(p => p.Item2);
                double scale = 100.0 / Math.Max(maxX, maxY);

                var scaledCoords = rawCoords
                    .Select(p => ((int)Math.Round(p.Item1 * scale), (int)Math.Round(p.Item2 * scale)))
                    .ToList();

                logger.Write(FormatSuccess(now, inputParams, type, scaledCoords));
                return (type, scaledCoords);
            }
            catch (Exception ex)
            {
                logger.Write(FormatError(now, inputParams, "Внутренняя ошибка", ex.ToString()));
                return ("", new List<(int, int)> { (-2, -2), (-2, -2), (-2, -2) });
            }
        }

        private static string FormatSuccess(DateTime dt, string par, string type, List<(int, int)> coords)
        {
            return $"[SUCCESS] {dt} | Params: {par} | Result: {type}, Coords: {string.Join(" ", coords)}";
        }

        private static string FormatError(DateTime dt, string par, string result, string trace)
        {
            return $"[ERROR] {dt} | Params: {par} | Result: {result} | Trace: {trace}";
        }
    }

    public interface ILogger
    {
        void Write(string message);
    }

    public class FileLogger : ILogger
    {
        private readonly string _filePath;
        public FileLogger(string filePath) => _filePath = filePath;

        public void Write(string message)
        {
            File.AppendAllText(_filePath, message + Environment.NewLine);
        }
    }

    public class ConsoleLogger : ILogger
    {
        public void Write(string msg)
        {
            Console.WriteLine(msg);
        }
    }

    public class AggregateLogger : ILogger
    {
        private readonly IEnumerable<ILogger> _loggers;
        public AggregateLogger(params ILogger[] loggers) => _loggers = loggers;
        public void Write(string msg)
        {
            foreach (var l in _loggers)
            {
                l.Write(msg);
            }
        }
    }
}