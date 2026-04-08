namespace laba_6_080426
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Входныe данных
            Console.WriteLine("Введите стороны A, B и C через Enter:");
            double sA = Convert.ToDouble(Console.ReadLine());
            double sB = Convert.ToDouble(Console.ReadLine());
            double sC = Convert.ToDouble(Console.ReadLine());
            var fileLog = new FileLogger(@"G:\тестирование\laba_6_080426\log.txt");
            var consoleLog = new ConsoleLogger();

            var logger = new AggregateLogger(fileLog, consoleLog);

            var (type, coords) = ProcessTrinagle.PT(sA, sB, sC, logger);

            Console.WriteLine($"Тип: {type}");
            Console.WriteLine($"Координаты: {string.Join(", ", coords)}");
        }
    }
}
