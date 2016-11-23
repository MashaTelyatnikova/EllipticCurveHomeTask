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
            commandLineParser.Setup(options => options.InputFile)
                .As("if")
                .WithDescription("Параметр input file");
            commandLineParser.Setup(options => options.OutputFile)
                .As("of")
                .WithDescription("Параметр output file");

            commandLineParser
                .SetupHelp("h", "help")
                .WithHeader(
                    $"{AppDomain.CurrentDomain.FriendlyName} [-a a] [-b b] [-c c] [-p p] [-if input file] [-of output file]")
                .Callback(text => Console.WriteLine(text));
            if (commandLineParser.Parse(args).HelpCalled)
            {
                return;
            }

            BigInteger p1 = BigInteger.Parse("6277101735386680763835789423207666416083908700390324961279");
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
                    var characteristic = tokenizer.NextInt();
                    var modular = BigInteger.Parse(tokenizer.NextWord());
                    var a = BigInteger.Parse(tokenizer.NextWord());
                    var b = BigInteger.Parse(tokenizer.NextWord());
                    if (characteristic == 0)
                    {
                        curve = new OrdinaryEllipticCurve(a, b, modular);
                    }
                    else if (characteristic == 1)
                    {
                        var c = BigInteger.Parse(tokenizer.NextWord());
                        var type = tokenizer.NextInt();
                        switch (type)
                        {
                            case 0:
                            {
                                curve = new SuperSingularEllipticCurve(a, b, c, modular);
                                break;
                            }
                            case 1:
                            {
                                
                                break;
                            }
                            default:
                            {
                                Console.WriteLine(
                                    "Тип кривой для поля с характеристикой 2 можеть быть либо 0 (суперсингулярная) либо 1(несуперсингулярная)");
                                Environment.Exit(0);
                                break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Значение характеристики кода может быть либо 0 (!= 2 и !=3) или 1(==2)");
                        Environment.Exit(0);
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