﻿using System.Collections.Generic;

namespace CLArgsParser.Args
{
    public static class SliceParseExtension
    {
        public static string Parse(this Slice slice)
        {

        }
    }

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
                        Slice CurConvSpecifier = ConversionSpecifierParse(source.SubSlice(i));
                        
                        if (CurConvSpecifier.Length < SpecPrefixSlice.Length)
                        {
                            i += CurConvSpecifier.Length - 1;
                            continue;
                        }

                        if (result.Count == 0)
                            result.Add(CurConvSpecifier);
                        else if (0 <= i - 1 && i - 1 < source.Length && source[i - 1] != ' ')
                            result[^1] = result[^1].ExpendEnd(CurConvSpecifier.Length);
                        else
                            result.Add(CurConvSpecifier);

                        i += CurConvSpecifier.Length - 1;
                    }
                    else if (LiteralSlice.Length < source.Length - i && source.SubSlice(i, LiteralSlice.Length) == LiteralSlice)
                    {
                        Slice CurLiteralParse = LiteralParse(source.SubSlice(i));

                        if (CurLiteralParse.Length < LiteralSlice.Length * 2)
                        {
                            i += CurLiteralParse.Length - 1;
                            continue;
                        }

                        if (result.Count == 0)
                            result.Add(CurLiteralParse);
                        else if (0 <= i - 1 && i - 1 < source.Length && source[i - 1] != ' ')
                            result[^1] = result[^1].ExpendEnd(CurLiteralParse.Length);
                        else
                            result.Add(CurLiteralParse);

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
                        Slice CurConvSpecifier = ConversionSpecifierParse(source.SubSlice(i));

                        if (CurConvSpecifier.Length < SpecPrefixSlice.Length)
                        {
                            i += CurConvSpecifier.Length - 1;
                            continue;
                        }

                        if (result.Count == 0)
                            result.Add(CurConvSpecifier.SliceFromSlice(1, CurConvSpecifier.Length - 1));
                        else if (0 <= i - 1 && i - 1 < source.Length && source[i - 1] != ' ')
                            result[^1] = result[^1].ExpendEnd(CurConvSpecifier.Length);
                        else
                            result.Add(CurConvSpecifier.SliceFromSlice(1, CurConvSpecifier.Length - 1));

                        i += CurConvSpecifier.Length - 1;
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

            return result.ToArray();
        }

        public Slice SpaceParse(Slice source)
        {
            Slice SpecPrefixSlice = SpecifierPrefix.ToSlice();
            Slice LiteralSlice = Literal.Value.ToSlice();
            source = source.TrimStart();

            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] == ' ' || 
                    (SpecPrefixSlice.Length < source.Length - i && source.SubSlice(i, SpecPrefixSlice.Length) == SpecPrefixSlice) ||
                    (LiteralSlice.Length < source.Length - i && source.SubSlice(i, LiteralSlice.Length) == LiteralSlice))
                    return source.SubSlice(0, i);
            }

            return source;
        }

        public Slice LiteralParse(Slice source)
        {
            Slice SpecPrefixSlice = SpecifierPrefix.ToSlice();
            Slice LiteralSlice = Literal.Value.ToSlice();

            for (int i = 1; i < source.Length; i++)
            {
                if (LiteralSlice.Length < source.Length - i && source.SubSlice(i, LiteralSlice.Length) == LiteralSlice)
                    return source.SubSlice(0, i + LiteralSlice.Length);

                else if (SpecPrefixSlice.Length < source.Length - i && source.SubSlice(i, SpecPrefixSlice.Length) == SpecPrefixSlice)
                {
                    Slice CurConvSpecifier = ConversionSpecifierParse(source.SubSlice(i));
                    i += CurConvSpecifier.Length - 1;
                }
            }

            return source;
        }

        public Slice ConversionSpecifierParse(Slice source)
        {
            source = source.SubSlice(1);

            foreach (var item in ConvSpecifiers)
            {
                if (item.Key.Length < source.Length && source.SubSlice(0, item.Key.Length) == item.Key)
                    return source.SubSlice(0, item.Key.Length + SpecifierPrefix.Length);
            }

            return source.SubSlice(0, SpecifierPrefix.Length);
        }
    }
}
