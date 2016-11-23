using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using EllipticCurveUtils;
using Fclp;

namespace HomeTask
{
    public static class Program
    {
        private class RunOptions
        {
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
                    var modular = tokenizer.NextWord().ToBigInteger();
                    var a = tokenizer.NextWord().ToBigInteger();
                    var b = tokenizer.NextWord().ToBigInteger();
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
                        var startPoint = new EllipticCurvePoint(0, 0);
                        while (t > 0)
                        {
                            var point = new EllipticCurvePoint(tokenizer.NextWord().ToBigInteger(),
                                tokenizer.NextWord().ToBigInteger());
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
                            var point = new EllipticCurvePoint(tokenizer.NextWord().ToBigInteger(),
                                tokenizer.NextWord().ToBigInteger());
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