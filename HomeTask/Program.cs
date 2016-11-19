using System;
using System.Drawing;
using System.Linq;
using EllipticCurveUtils;
using Fclp;

namespace HomeTask
{
    public static class Program
    {
        private class RunOptions
        {
            public int? A { get; set; }

            public int? B { get; set; }

            public int? P { get; set; }
        }

        static void Main(string[] args)
        {
            var commandLineParser = new FluentCommandLineParser<RunOptions>();
            commandLineParser
                .Setup(options => options.A)
                .As('a')
                .Required()
                .WithDescription("Параметр a");
            commandLineParser
                .Setup(options => options.B)
                .As('b')
                .Required()
                .WithDescription("Параметр b");
            commandLineParser.Setup(options => options.P)
                .As('p')
                .Required()
                .WithDescription("Параметр p");

            commandLineParser
                .SetupHelp("h", "help")
                .WithHeader($"{AppDomain.CurrentDomain.FriendlyName} [-a a] [-b b] [-p p]")
                .Callback(text => Console.WriteLine(text));
            if (commandLineParser.Parse(args).HelpCalled)
            {
                return;
            }

            if (commandLineParser.Object.A == null || commandLineParser.Object.B == null ||
                commandLineParser.Object.P == null)
            {
                Console.WriteLine("Неправильные параметры");
            }

            var curve = new EllipticCurve(
                commandLineParser.Object.A.Value,
                commandLineParser.Object.B.Value,
                commandLineParser.Object.P.Value
            );

            while (true)
            {
                try
                {
                    Console.WriteLine("Enter x1 y1");
                    var first = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();

                    Console.WriteLine("Enter x2 y2");

                    var second = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();

                    var point1 = new Point(first[0], first[1]);
                    var point2 = new Point(second[0], second[1]);
                    var result = curve.Add(point1, point2);
                    Console.WriteLine($"({result.X}, {result.Y})");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Вы ввели неправильные данные");
                }
            }
        }
    }
}