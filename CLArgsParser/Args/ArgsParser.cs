using System.Collections.Generic;
using CLArgsParser;

namespace CLArgsParser.Args
{
    public class ArgsParser
    {
        public string SpecifierPrefix { get; set; }
        public List<ConversionSpecifier> ConvSpecifiers { get; set; }
        public ParserOption<string> Literal { get; set; }

        public ArgsParser()
        {
            SpecifierPrefix = string.Empty;
            ConvSpecifiers = new List<ConversionSpecifier>();
            Literal = new ParserOption<string>(false, string.Empty);
        }

        public virtual Slice[] Parse(string source) => Parse(source.Slice(0, source.Length));

        public virtual Slice[] Parse(Slice source)
        {
            List<Slice> result = new List<Slice>();
            bool ParsingLiteral = false;
            bool ParsingSpecifierPrefix = false;



            if (Literal.Enable)
            {
                Slice SpecPrefixSlice = SpecifierPrefix.ToSlice();
                Slice LiteralSlice = Literal.Value.ToSlice();

                for (int i = 0; i < source.Length; i++)
                {
                    if (source[i] == ' ')
                    {
                        continue;
                    }
                    else if (SpecPrefixSlice.Length < source.Length - i && source.SubSlice(i, SpecPrefixSlice.Length) == SpecPrefixSlice)
                    {
                        SpecifierItem CurConvSpecifier = ConversionSpecifierParse(source.SubSlice(i));
                        Slice CurConvSpecifierSlice = CurConvSpecifier.Value.ToSlice();
                        if (result.Count == 0)
                            result.Add(CurConvSpecifierSlice);
                        else if (0 <= i - 1 && i - 1 < source.Length && source[i - 1] != ' ')
                            result[^1] = (result[^1].Str + CurConvSpecifier.Value).ToSlice();
                        else
                            result.Add(CurConvSpecifierSlice);

                        i += CurConvSpecifier.Length - 1;
                    }
                    else if (LiteralSlice.Length < source.Length - i && source.SubSlice(i, LiteralSlice.Length) == LiteralSlice)
                    {
                        Slice CurLiteralParse = LiteralParse(source.SubSlice(i));

                        if (CurLiteralParse.Length < 2)
                        {
                            i += CurLiteralParse.Length - 1;
                            continue;
                        }

                        if (result.Count == 0)
                            result.Add(CurLiteralParse.SliceFromSlice(1, CurLiteralParse.Length - 1));
                        else if (0 <= i - 1 && i - 1 < source.Length && source[i - 1] != ' ')
                            result[^1] = result[^1].ExpendEnd(CurLiteralParse.Length);
                        else
                            result.Add(CurLiteralParse.SliceFromSlice(1, CurLiteralParse.Length - 1));

                        i += CurLiteralParse.Length - 1;
                    }
                    else
                    {
                        Slice CurSpaceParse = SpaceParse(source.SubSlice(i));

                        if (result.Count == 0)
                            result.Add(CurSpaceParse);
                        else if (0 <= i - 1 && i - 1 < source.Length && source[i - 1] != ' ')
                            result[^1] = result[^1].ExpendEnd(CurSpaceParse.Length);
                        else
                            result.Add(CurSpaceParse);

                        i += CurSpaceParse.Length - 1;
                    }
                }
            }
            else
            {
                Slice SpecPrefixSlice = SpecifierPrefix.ToSlice();

                for (int i = 0; i < source.Length; i++)
                {
                    if (source[i] == ' ')
                    {
                        continue;
                    }
                    else if (SpecPrefixSlice.Length < source.Length - i && source.SubSlice(i, SpecPrefixSlice.Length) == SpecPrefixSlice)
                    {
                        SpecifierItem CurConvSpecifier = ConversionSpecifierParse(source);
                        Slice CurConvSpecifierSlice = CurConvSpecifier.Value.ToSlice();
                        if (result.Count == 0)
                            result.Add(CurConvSpecifierSlice);
                        else if (0 <= i - 1 && i - 1 < source.Length && source[i - 1] != ' ')
                            result[^1] = (result[^1].Str + CurConvSpecifier.Value).ToSlice();
                        else
                            result.Add(CurConvSpecifierSlice);

                        i += CurConvSpecifier.Length;
                    }
                    else
                    {
                        Slice CurSpaceParse = SpaceParse(source);

                        if (result.Count == 0)
                            result.Add(CurSpaceParse);
                        else if (0 <= i - 1 && i - 1 < source.Length && source[i - 1] != ' ')
                            result[^1] = result[^1].ExpendEnd(CurSpaceParse.Length);
                        else
                            result.Add(CurSpaceParse);

                        i += CurSpaceParse.Length - 1;
                    }
                }
            }

            return result.ToArray();
        }

        //public Slice SpaceParse(Slice source)
        //{
        //    source = source.TrimStart();

        //    for (int i = 0; i < source.Length; i++)
        //    {
        //        if (source[i] == ' ')
        //            return source.SubSlice(0, i);
        //    }

        //    return source;
        //}

        //public Slice LiteralParse(Slice source)
        //{
        //    Slice LiteralSlice = Literal.Value.ToSlice();

        //    for (int i = 1; i < source.Length; i++)
        //    {
        //        if (LiteralSlice.Length < source.Length - i && source.SubSlice(i, LiteralSlice.Length) == LiteralSlice)
        //            return source.SubSlice(0, i + 1);
        //    }

        //    return source;
        //}

        //public SpecifierItem ConversionSpecifierParse(Slice source)
        //{
        //    source = source.SubSlice(1);

        //    foreach (var item in ConvSpecifiers)
        //    {
        //        if (item.Key.Length < source.Length && source.SubSlice(0, item.Key.Length) == item.Key)
        //            return new SpecifierItem(item.Key.Length + 1, item.Value);
        //    }

        //    return new SpecifierItem(1, string.Empty);
        //}
    }

    //struct SpecifierItem
    //{
    //    public readonly int Length;
    //    public readonly string Value;

    //    public SpecifierItem (int length, string value)
    //    {
    //        Length = length;
    //        Value = value;
    //    }
    //}

    public struct ConversionSpecifier
    {
        public readonly string Key;
        public readonly string Value;

        public ConversionSpecifier(string value)
        {
            Key = value;
            Value = value;
        }

        public ConversionSpecifier(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
