﻿using CLArgsParser.Args;

namespace CLArgsParser.Command
{
    public class CommandParser : ArgsParser
    {
        public ParserOption<string> CommandPrefix { get; set; }

        public CommandParser() : base()
        {
            CommandPrefix = new ParserOption<string>(false, string.Empty);
        }

        public CommandParser(ArgsParser argsParser) : this()
        {
            SpecifierPrefix = argsParser.SpecifierPrefix;
            ConvSpecifiers = argsParser.ConvSpecifiers;
            Literal = argsParser.Literal;
        }

        public override Slice[] Parse(string source) => Parse(source.Slice(0, source.Length));

        public override Slice[] Parse(Slice source)
        {
            if (!CommandPrefix.Enable)
                return base.Parse(source);

            Slice TrimedSlice = source.TrimStart();

            if (TrimedSlice.Length < CommandPrefix.Value.Length)
                return null;

            if (TrimedSlice.SubSlice(0, CommandPrefix.Value.Length) != CommandPrefix.Value)
                return null;

            return base.Parse(TrimedSlice.SubSlice(CommandPrefix.Value.Length));
        }
    }
}
