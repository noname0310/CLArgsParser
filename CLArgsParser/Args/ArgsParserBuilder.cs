using System.Collections.Generic;
using System.Linq;

namespace CLArgsParser.Args
{
    class ArgsParserBuilder
    {
        private ArgsParser _argsParser;

        public ArgsParserBuilder()
        {
            _argsParser = new ArgsParser();
        }

        public static ArgsParser BuildDefault()
        {
            return new ArgsParserBuilder()
                .UseSpecifierPrefix("\\")
                .SetConvSpecifiers(
                    new ConversionSpecifier[]
                    {
                        new ConversionSpecifier("\""),
                        new ConversionSpecifier("\\"),
                    }
                )
                .UseLiteral("\"")
                .Build();
        }

        public ArgsParserBuilder UseSpecifierPrefix(string prefix)
        {
            _argsParser.SpecifierPrefix = prefix;
            return this;
        }

        public ArgsParserBuilder SetConvSpecifiers(ConversionSpecifier[] convSpecifiers)
        {
            List<ConversionSpecifier> srcSpecifiers = _argsParser.ConvSpecifiers;
            srcSpecifiers.AddRange(convSpecifiers);
            _argsParser.ConvSpecifiers = srcSpecifiers.Distinct().ToList();
            return this;
        }

        public ArgsParserBuilder UseLiteral(string literal)
        {
            _argsParser.Literal.Enable = true;
            _argsParser.Literal.Value = literal;
            return this;
        }

        public ArgsParser Build() => _argsParser;
    }
}
