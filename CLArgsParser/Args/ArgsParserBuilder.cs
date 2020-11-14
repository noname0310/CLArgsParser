using System;
using System.Collections.Generic;
using System.Linq;

namespace CLArgsParser.Args
{
    public class ArgsParserBuilder
    {
        private ArgsParser _argsParser;

        public ArgsParserBuilder()
        {
            _argsParser = new();
        }

        public static ArgsParser BuildDefault()
        {
            return new ArgsParserBuilder()
                .SetConvSpecifiers("\\",
                    new ConversionSpecifier[]
                    {
                        new("\""),
                        new("\\"),
                    }
                )
                .UseLiteral("\"")
                .Build();
        }

        public ArgsParserBuilder SetConvSpecifiers(string prefix, ConversionSpecifier[] convSpecifiers)
        {
            if (prefix == string.Empty || prefix == null)
                throw new ArgumentNullException(nameof(prefix), "\"prefix\" can not be string.Empty or null");
            if (prefix.IndexOf(' ') != -1)
                throw new ArgumentException("\"prefix\" can not contain space", nameof(prefix));
            _argsParser.SpecifierPrefix = prefix;
            List<ConversionSpecifier> srcSpecifiers = _argsParser.ConvSpecifiers;
            srcSpecifiers.AddRange(convSpecifiers);
            _argsParser.ConvSpecifiers = srcSpecifiers.Distinct().ToList();
            return this;
        }

        public ArgsParserBuilder UseLiteral(string literal)
        {
            if (literal == string.Empty || literal == null)
                throw new ArgumentNullException(nameof(literal), "\"literal\" can not be string.Empty or null");
            if (literal.IndexOf(' ') != -1)
                throw new ArgumentException("\"literal\" can not contain space", nameof(literal));
            _argsParser.Literal.Enable = true;
            _argsParser.Literal.Value = literal;
            return this;
        }

        public ArgsParser Build() => _argsParser;
    }
}
