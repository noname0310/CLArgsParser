﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CLArgsParser.Args
{
    public static class SliceParseExtension
    {
        public static string LazyParse(this in Slice slice, ArgsParser argsParser) => argsParser.LazyParse(slice);
    }

    public class ArgsParser
    {
        public string SpecifierPrefix { get; set; }
        public List<ConversionSpecifier> ConvSpecifiers { get; set; }
        public ParserOption<string> Literal { get; set; }

        private readonly List<Slice> _result;

        public ArgsParser()
        {
            SpecifierPrefix = string.Empty;
            ConvSpecifiers = new();
            Literal = new(false, string.Empty);
            _result = new();
        }

        public string[] SlowParse(string source)
        {
            List<string> result = new();
            Slice[] parsedSource = Parse(source);
            if (parsedSource == null)
                return Array.Empty<string>();
            foreach (var item in parsedSource)
            {
                string lazyParsedArg = LazyParse(item);
                result.Add(lazyParsedArg);
            }
            return result.ToArray();
        }

        public virtual Slice[] Parse(string source) => Parse((Slice)source);

        public virtual Slice[] Parse(in Slice source)
        {
            _result.Clear();

            if (Literal.Enable)
            {
                Slice specPrefixSlice = SpecifierPrefix.ToSlice();
                Slice literalSlice = Literal.Value.ToSlice();

                for (int i = 0; i < source.Length; i++)
                {
                    if (source[i] == ' ')
                    {
                        continue;
                    }
                    else if (specPrefixSlice.Length <= source.Length - i && source.SubSlice(i, specPrefixSlice.Length) == specPrefixSlice)
                    {
                        Slice curConvSpecifier = ConversionSpecifierParse(source.SubSlice(i));

                        if (_result.Count == 0)
                            _result.Add(curConvSpecifier);
                        else if (0 <= i - 1 && i - 1 < source.Length && source[i - 1] != ' ')
                            _result[^1] = _result[^1].ExpendEnd(curConvSpecifier.Length);
                        else
                            _result.Add(curConvSpecifier);

                        i += curConvSpecifier.Length - 1;
                    }
                    else if (literalSlice.Length <= source.Length - i && source.SubSlice(i, literalSlice.Length) == literalSlice)
                    {
                        Slice curLiteralParse = LiteralParse(source.SubSlice(i));

                        if (curLiteralParse.Length < literalSlice.Length * 2)
                        {
                            i += curLiteralParse.Length - 1;
                            continue;
                        }

                        if (_result.Count == 0)
                            _result.Add(curLiteralParse);
                        else if (0 <= i - 1 && i - 1 < source.Length && source[i - 1] != ' ')
                            _result[^1] = _result[^1].ExpendEnd(curLiteralParse.Length);
                        else
                            _result.Add(curLiteralParse);

                        i += curLiteralParse.Length - 1;
                    }
                    else
                    {
                        Slice curSpaceParse = SpaceParse(source.SubSlice(i));

                        if (_result.Count == 0)
                            _result.Add(curSpaceParse);
                        else if (0 <= i - 1 && i - 1 < source.Length && source[i - 1] != ' ')
                            _result[^1] = _result[^1].ExpendEnd(curSpaceParse.Length);
                        else
                            _result.Add(curSpaceParse);

                        i += curSpaceParse.Length - 1;
                    }
                }
            }
            else
            {
                Slice specPrefixSlice = SpecifierPrefix.ToSlice();

                for (int i = 0; i < source.Length; i++)
                {
                    if (source[i] == ' ')
                    {
                        continue;
                    }
                    else if (specPrefixSlice.Length <= source.Length - i && source.SubSlice(i, specPrefixSlice.Length) == specPrefixSlice)
                    {
                        Slice curConvSpecifier = ConversionSpecifierParse(source.SubSlice(i));

                        if (_result.Count == 0)
                            _result.Add(curConvSpecifier);
                        else if (0 <= i - 1 && i - 1 < source.Length && source[i - 1] != ' ')
                            _result[^1] = _result[^1].ExpendEnd(curConvSpecifier.Length);
                        else
                            _result.Add(curConvSpecifier);

                        i += curConvSpecifier.Length - 1;
                    }
                    else
                    {
                        Slice curSpaceParse = SpaceParse(source.SubSlice(i));

                        if (_result.Count == 0)
                            _result.Add(curSpaceParse);
                        else if (0 <= i - 1 && i - 1 < source.Length && source[i - 1] != ' ')
                            _result[^1] = _result[^1].ExpendEnd(curSpaceParse.Length);
                        else
                            _result.Add(curSpaceParse);

                        i += curSpaceParse.Length - 1;
                    }
                }
            }

            return _result.ToArray();
        }

        public string LazyParse(in Slice source)
        {
            StringBuilder stringBuilder = new();

            Slice specPrefixSlice = SpecifierPrefix.ToSlice();
            Slice literalSlice = Literal.Value.ToSlice();

            if (Literal.Enable)
            {
                for (int i = 0; i < source.Length; i++)
                {
                    if (specPrefixSlice.Length <= source.Length - i && source.SubSlice(i, specPrefixSlice.Length) == specPrefixSlice)
                    {
                        int curConvSpecifier = LazyConversionSpecifierParse(source.SubSlice(i), stringBuilder);
                        i += curConvSpecifier - 1;
                    }
                    else if (literalSlice.Length <= source.Length - i && source.SubSlice(i, literalSlice.Length) == literalSlice)
                    {
                        int curLiteralParse = LazyLiteralParse(source.SubSlice(i), stringBuilder);
                        i += curLiteralParse - 1;
                    }
                    else
                        stringBuilder.Append(source[i]);
                }
            }
            else
            {
                for (int i = 0; i < source.Length; i++)
                {
                    if (specPrefixSlice.Length <= source.Length - i && source.SubSlice(i, specPrefixSlice.Length) == specPrefixSlice)
                    {
                        int curConvSpecifier = LazyConversionSpecifierParse(source.SubSlice(i), stringBuilder);
                        i += curConvSpecifier - 1;
                    }
                    else
                        stringBuilder.Append(source[i]);
                }
            }

            return stringBuilder.ToString();
        }

        private Slice SpaceParse(in Slice source)
        {
            Slice specPrefixSlice = SpecifierPrefix.ToSlice();
            Slice literalSlice = Literal.Value.ToSlice();
            Slice trimedsource = source.TrimStart();

            for (int i = 0; i < trimedsource.Length; i++)
            {
                if (trimedsource[i] == ' ' || 
                    (specPrefixSlice.Length <= trimedsource.Length - i && trimedsource.SubSlice(i, specPrefixSlice.Length) == specPrefixSlice) ||
                    (literalSlice.Length <= trimedsource.Length - i && trimedsource.SubSlice(i, literalSlice.Length) == literalSlice))
                    return trimedsource.SubSlice(0, i);
            }

            return trimedsource;
        }

        private Slice LiteralParse(in Slice source)
        {
            Slice specPrefixSlice = SpecifierPrefix.ToSlice();
            Slice literalSlice = Literal.Value.ToSlice();

            for (int i = literalSlice.Length; i < source.Length; i++)
            {
                if (literalSlice.Length <= source.Length - i && source.SubSlice(i, literalSlice.Length) == literalSlice)
                    return source.SubSlice(0, i + literalSlice.Length);

                else if (specPrefixSlice.Length <= source.Length - i && source.SubSlice(i, specPrefixSlice.Length) == specPrefixSlice)
                {
                    Slice curConvSpecifier = ConversionSpecifierParse(source.SubSlice(i));
                    i += curConvSpecifier.Length - 1;
                }
            }

            return source;
        }

        private int LazyLiteralParse(in Slice source, StringBuilder stringBuilder)
        {
            Slice specPrefixSlice = SpecifierPrefix.ToSlice();
            Slice literalSlice = Literal.Value.ToSlice();

            for (int i = literalSlice.Length; i < source.Length; i++)
            {
                if (literalSlice.Length <= source.Length - i && source.SubSlice(i, literalSlice.Length) == literalSlice)
                    return i;

                if (specPrefixSlice.Length <= source.Length - i && source.SubSlice(i, specPrefixSlice.Length) == specPrefixSlice)
                {
                    int curConvSpecifier = LazyConversionSpecifierParse(source.SubSlice(i), stringBuilder);
                    i += curConvSpecifier - 1;
                }
                else
                    stringBuilder.Append(source[i]);
            }

            return source.Length;
        }

        private Slice ConversionSpecifierParse(in Slice source)
        {
            foreach (var item in ConvSpecifiers)
            {
                if (item.Key.Length <= source.Length - 1 && source.SubSlice(1, item.Key.Length) == item.Key)
                    return source.SubSlice(0, item.Key.Length + SpecifierPrefix.Length);
            }

            return source.SubSlice(0, SpecifierPrefix.Length);
        }

        private int LazyConversionSpecifierParse(in Slice source, StringBuilder stringBuilder)
        {
            foreach (var item in ConvSpecifiers)
            {
                if (item.Key.Length <= source.Length - 1 && source.SubSlice(1, item.Key.Length) == item.Key)
                {
                    stringBuilder.Append(item.Value);
                    return source.SubSlice(0, item.Key.Length + SpecifierPrefix.Length).Length;
                }
            }

            return SpecifierPrefix.Length;
        }
    }
}
