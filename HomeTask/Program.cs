using System;
using System.Drawing;
using System.IO;
using System.Numerics;
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

            public int? C { get; set; }
            public int? P { get; set; }

            public string InputFile { get; set; }

            public string OutputFile { get; set; }
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
            commandLineParser
                .Setup(options => options.P)
                .As('p')
                .Required()
                .WithDescription("Параметр p");
            commandLineParser
                .Setup(options => options.C)
                .As('c')
                .Required()
                .WithDescription("Параметр c");
            commandLineParser.Setup(options => options.InputFile)
                .As("if")
                .WithDescription("Параметр input file");
            commandLineParser.Setup(options => options.OutputFile)
                .As("of")
                .WithDescription("Параметр output file");

            commandLineParser
                .SetupHelp("h", "help")
                .WithHeader(
                    $"{AppDomain.CurrentDomain.FriendlyName} [-a a] [-b b] [-p p] [-if input file] [-of output file]")
                .Callback(text => Console.WriteLine(text));
            if (commandLineParser.Parse(args).HelpCalled)
            {
                return;
            }

            if (commandLineParser.Object.A == null || commandLineParser.Object.B == null ||
                commandLineParser.Object.P == null)
            {
                Console.WriteLine("Неправильные параметры");
                Environment.Exit(0);
            }

            StreamReader fileReader = null;
            if (commandLineParser.Object.InputFile != null)
            {
                fileReader = new StreamReader(commandLineParser.Object.InputFile);
            }

            try
            {
                StreamWriter fileWriter = null;
                if (commandLineParser.Object.OutputFile != null)
                {
                    fileWriter = new StreamWriter(commandLineParser.Object.OutputFile);
                }

                try
                {
                    var tokenizer = new StreamTokenizer(fileReader ?? Console.In);
                    var writer = fileWriter ?? Console.Out;

                    EllipticCurve curve = null;
                    var p = new BigInteger(commandLineParser.Object.P.Value);
                    if (p.IsPowerOfTwo)
                    {
                        if (commandLineParser.Object.C == null)
                        {
                            Console.WriteLine("Надо указать параметр c");
                            Environment.Exit(0);
                        }
                        var m = 0;
                        while (p != 1)
                        {
                            p /= 2;
                            ++m;
                        }
                        curve = new SuperSingularEllipticCurve(commandLineParser.Object.A.Value,
                            commandLineParser.Object.B.Value, (int) p, commandLineParser.Object.C.Value);
                    }
                    else
                    {
                        curve = new EllipticCurve(
                            commandLineParser.Object.A.Value,
                            commandLineParser.Object.B.Value,
                            commandLineParser.Object.P.Value
                        );
                    }

                    if (!curve.IsNonSpecial())
                    {
                        writer.WriteLine("Кривая особая");
                    }
                    else
                    {
                        var t = tokenizer.NextInt();
                        var startPoint = new Point(0, 0);
                        while (t > 0)
                        {
                            var point = new Point(tokenizer.NextInt(), tokenizer.NextInt());
                            if (!curve.Contains(point))
                            {
                                writer.WriteLine($"Кривая не содержит точку {point.X}, {point.Y}");
                            }
                            else
                            {
                                startPoint = curve.Add(startPoint, point);
                            }
                            --t;
                        }
                        writer.WriteLine($"Результат сложения точек = ({startPoint.X}, {startPoint.Y})");

                        var s = tokenizer.NextInt();
                        while (s > 0)
                        {
                            var n = tokenizer.NextInt();
                            var point = new Point(tokenizer.NextInt(), tokenizer.NextInt());
                            if (!curve.Contains(point))
                            {
                                writer.WriteLine($"Кривая не содержит точку {point.X}, {point.Y}");
                            }
                            else
                            {
                                var start = point;
                                while (n > 1)
                                {
                                    start = curve.Add(start, point);
                                    --n;
                                }

                                writer.WriteLine($"Результат = ({start.X}, {start.Y})");
                            }
                            --s;
                        }
                    }
                }
                finally
                {
                    fileWriter?.Close();
                }
            }
            finally
            {
                fileReader?.Close();
            }
        }
    }
}