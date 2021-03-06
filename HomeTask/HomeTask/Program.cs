﻿using System;
using System.IO;
using System.Numerics;
using Fclp;
using HomeTask.EllipticCurve;

namespace HomeTask
{
    public static class Program
    {
        private class RunOptions
        {
            public string InputFile { get; set; }

            public string OutputFile { get; set; }
        }

        private static readonly FluentCommandLineParser<RunOptions> Parser;

        static Program()
        {
            Parser = new FluentCommandLineParser<RunOptions>();
            Parser.Setup(options => options.InputFile)
                .As("if")
                .WithDescription("Параметр input file");
            Parser.Setup(options => options.OutputFile)
                .As("of")
                .WithDescription("Параметр output file");
            Parser
                .SetupHelp("h", "help")
                .WithHeader(
                    $"{AppDomain.CurrentDomain.FriendlyName} [-if input file] [-of output file]")
                .Callback(text => Console.WriteLine(text));
        }

        public static void Main(string[] args)
        {
            var arguments = Parser.Parse(args);
            if (arguments.HelpCalled)
            {
                return;
            }

            if (arguments.HasErrors)
            {
                Parser.HelpOption.ShowHelp(Parser.Options);
                return;
            }

            StreamReader fileReader = null;
            if (Parser.Object.InputFile != null)
            {
                fileReader = new StreamReader(Parser.Object.InputFile);
            }

            try
            {
                StreamWriter fileWriter = null;
                if (Parser.Object.OutputFile != null)
                {
                    fileWriter = new StreamWriter(Parser.Object.OutputFile);
                }

                try
                {
                    var tokenizer = new StreamTokenizer(fileReader ?? Console.In);
                    var writer = fileWriter ?? Console.Out;

                    EllipticCurve.EllipticCurve curve = null;
                    var characteristic = tokenizer.NextInt();
                    var modular = tokenizer.NextWord().ToBigInteger();

                    var a = tokenizer.NextWord().ToBigInteger();
                    var b = tokenizer.NextWord().ToBigInteger();
                    if (characteristic == 0)
                    {
                        curve = new OrdinaryEllipticCurve(new MyBigInteger(a, modular), new MyBigInteger(b, modular),
                            new MyBigInteger(modular, modular));
                    }
                    else if (characteristic == 1)
                    {
                        var c = tokenizer.NextWord().ToBigInteger();
                        var type = tokenizer.NextInt();
                        var m = tokenizer.NextWord().ToBigInteger();
                        var modular1 = BigInteger.Pow(2, (int) modular - 1);
                        switch (type)
                        {
                            case 0:
                            {
                                curve = new SuperSingularEllipticCurve(new GaluaBigInteger(a, m, modular1),
                                    new GaluaBigInteger(b, m, modular1), new GaluaBigInteger(c, m, modular1),
                                    new GaluaBigInteger(m, m, modular1));
                                break;
                            }
                            case 1:
                            {
                                curve = new NonSupersSingularEllipticCurve(new GaluaBigInteger(a, m, modular1),
                                    new GaluaBigInteger(b, m, modular1), new GaluaBigInteger(c, m, modular1),
                                    new GaluaBigInteger(modular1, m, modular1));
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

                    if ((curve is OrdinaryEllipticCurve) && !curve.IsNonSpecial())
                    {
                        writer.WriteLine("Кривая особая");
                    }
                    else
                    {
                        var t = tokenizer.NextInt();
                        var startPoint = curve.Zero;
                        while (t > 0)
                        {
                            var point = new EllipticCurvePoint(
                                startPoint.X.Get(tokenizer.NextWord().ToBigInteger()),
                                startPoint.X.Get(tokenizer.NextWord().ToBigInteger()));
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
                            var zero = curve.Zero;
                            var point = new EllipticCurvePoint(
                                zero.X.Get(tokenizer.NextWord().ToBigInteger()),
                                zero.X.Get(tokenizer.NextWord().ToBigInteger()));
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