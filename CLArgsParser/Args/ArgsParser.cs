using System.Collections.Generic;
using System.Text;

namespace CLArgsParser.Args
{
    public static class SliceParseExtension
    {
        public static string LazyParse(this Slice slice, ArgsParser argsParser) => argsParser.LazyParse(slice);
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
            ConvSpecifiers = new List<ConversionSpecifier>();
            Literal = new ParserOption<string>(false, string.Empty);
            _result = new List<Slice>();
        }

        public virtual Slice[] Parse(string source) => Parse(source.Slice(0, source.Length));

        public virtual Slice[] Parse(Slice source)
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
                    else if (specPrefixSlice.Length != 0 && specPrefixSlice.Length <= source.Length - i && source.SubSlice(i, specPrefixSlice.Length) == specPrefixSlice)
                    {
                        Slice curConvSpecifier = ConversionSpecifierParse(source.SubSlice(i));
                        
                        if (curConvSpecifier.Length < specPrefixSlice.Length)
                        {
                            i += curConvSpecifier.Length - 1;
                            continue;
                        }

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
                    else if (specPrefixSlice.Length != 0 && specPrefixSlice.Length <= source.Length - i && source.SubSlice(i, specPrefixSlice.Length) == specPrefixSlice)
                    {
                        Slice curConvSpecifier = ConversionSpecifierParse(source.SubSlice(i));

                        if (curConvSpecifier.Length < specPrefixSlice.Length)
                        {
                            i += curConvSpecifier.Length - 1;
                            continue;
                        }

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

        public string LazyParse(Slice source)
        {
            StringBuilder stringBuilder = new StringBuilder();

            Slice specPrefixSlice = SpecifierPrefix.ToSlice();
            Slice literalSlice = Literal.Value.ToSlice();

            if (Literal.Enable)
            {
                for (int i = 0; i < source.Length; i++)
                {
                    if (specPrefixSlice.Length != 0 && specPrefixSlice.Length <= source.Length - i && source.SubSlice(i, specPrefixSlice.Length) == specPrefixSlice)
                    {
                        Slice curConvSpecifier = ConversionSpecifierParse(source.SubSlice(i));
                        if (curConvSpecifier != specPrefixSlice)
                            stringBuilder.Append(curConvSpecifier.SubSlice(specPrefixSlice.Length));

                        i += curConvSpecifier.Length - 1;
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
                    if (specPrefixSlice.Length != 0 && specPrefixSlice.Length <= source.Length - i && source.SubSlice(i, specPrefixSlice.Length) == specPrefixSlice)
                    {
                        Slice curConvSpecifier = ConversionSpecifierParse(source.SubSlice(i));
                        if (curConvSpecifier != specPrefixSlice)
                            stringBuilder.Append(curConvSpecifier.SubSlice(specPrefixSlice.Length));

                        i += curConvSpecifier.Length - 1;
                    }
                    else
                        stringBuilder.Append(source[i]);
                }
            }

            return stringBuilder.ToString();
        }

        private Slice SpaceParse(Slice source)
        {
            Slice specPrefixSlice = SpecifierPrefix.ToSlice();
            Slice literalSlice = Literal.Value.ToSlice();
            source = source.TrimStart();

            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] == ' ' || 
                    (specPrefixSlice.Length != 0 && specPrefixSlice.Length <= source.Length - i && source.SubSlice(i, specPrefixSlice.Length) == specPrefixSlice) ||
                    (literalSlice.Length <= source.Length - i && source.SubSlice(i, literalSlice.Length) == literalSlice))
                    return source.SubSlice(0, i);
            }

            return source;
        }

        private Slice LiteralParse(Slice source)
        {
            Slice specPrefixSlice = SpecifierPrefix.ToSlice();
            Slice literalSlice = Literal.Value.ToSlice();

            for (int i = literalSlice.Length; i < source.Length; i++)
            {
                if (literalSlice.Length <= source.Length - i && source.SubSlice(i, literalSlice.Length) == literalSlice)
                    return source.SubSlice(0, i + literalSlice.Length);

                else if (specPrefixSlice.Length != 0 && specPrefixSlice.Length <= source.Length - i && source.SubSlice(i, specPrefixSlice.Length) == specPrefixSlice)
                {
                    Slice curConvSpecifier = ConversionSpecifierParse(source.SubSlice(i));
                    i += curConvSpecifier.Length - 1;
                }
            }

            return source;
        }

        private int LazyLiteralParse(Slice source, StringBuilder stringBuilder)
        {
            Slice specPrefixSlice = SpecifierPrefix.ToSlice();
            Slice literalSlice = Literal.Value.ToSlice();

            for (int i = literalSlice.Length; i < source.Length; i++)
            {
                if (literalSlice.Length <= source.Length - i && source.SubSlice(i, literalSlice.Length) == literalSlice)
                    return i;

                if (specPrefixSlice.Length != 0 && specPrefixSlice.Length <= source.Length - i && source.SubSlice(i, specPrefixSlice.Length) == specPrefixSlice)
                {
                    Slice curConvSpecifier = ConversionSpecifierParse(source.SubSlice(i));
                    if (curConvSpecifier != specPrefixSlice)
                        stringBuilder.Append(curConvSpecifier.SubSlice(specPrefixSlice.Length));
                    i += curConvSpecifier.Length - 1;
                }
                else
                    stringBuilder.Append(source[i]);
            }

            return source.Length;
        }

        private Slice ConversionSpecifierParse(Slice source)
        {
            foreach (var item in ConvSpecifiers)
            {
                if (item.Key.Length <= source.Length - 1 && source.SubSlice(1, item.Key.Length) == item.Key)
                    return source.SubSlice(0, item.Key.Length + SpecifierPrefix.Length);
            }

            return source.SubSlice(0, SpecifierPrefix.Length);
        }
    }
}
