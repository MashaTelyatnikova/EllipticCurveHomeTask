using System;
using System.IO;
using System.Numerics;
using HomeTask.EllipticCurve;

namespace HomeTask
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            StreamReader fileReader = fileReader = new StreamReader(args[0]);

            try
            {
                StreamWriter fileWriter = fileWriter = new StreamWriter(args[1]);
                
                try
                {
                    var tokenizer = new StreamTokenizer(fileReader);
                    var writer = fileWriter;

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
                        var tCopy = t;
                        var zeroX = curve.Zero.X;
                        if (t > 0)
                        {
                            var startPoint = new EllipticCurvePoint(
                                zeroX.Get(tokenizer.NextWord().ToBigInteger()),
                                zeroX.Get(tokenizer.NextWord().ToBigInteger()));
                            var flag = false;
                            while (t > 1)
                            {
                                var point = new EllipticCurvePoint(
                                    zeroX.Get(tokenizer.NextWord().ToBigInteger()),
                                    zeroX.Get(tokenizer.NextWord().ToBigInteger()));

                                if (t == tCopy && !curve.Contains(startPoint))
                                {
                                    writer.WriteLine($"Кривая не содержит точку {startPoint.X}, {startPoint.Y}");
                                    flag = true;
                                    break;
                                }
                                else if (!curve.Contains(point))
                                {
                                    writer.WriteLine($"Кривая не содержит точку {point.X}, {point.Y}");
                                    flag = true;
                                    break;
                                }
                                else
                                {
                                    startPoint = curve.Add(startPoint, point);
                                }
                                --t;
                            }
                            if (!flag)
                            {
                                writer.WriteLine($"Результат сложения точек = {startPoint}");
                            }
                            else
                            {
                                writer.WriteLine($"Результат сложения точек неизвестен");
                            }
                        }

                        var s = tokenizer.NextInt();
                        while (s > 0)
                        {
                            var n = tokenizer.NextInt();
                            var nCopy = n;
                            var zero = curve.Zero;
                            var point = new EllipticCurvePoint(
                                zero.X.Get(tokenizer.NextWord().ToBigInteger()),
                                zero.X.Get(tokenizer.NextWord().ToBigInteger()));
                            if (!curve.Contains(point))
                            {
                                writer.WriteLine($"Кривая не содержит точку {point}");
                            }
                            else
                            {
                                var start = point;
                                while (n > 1)
                                {
                                    start = curve.Add(start, point);
                                    --n;
                                }

                                writer.WriteLine($"Результат умножения точки {point} на {nCopy} = {start}");
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