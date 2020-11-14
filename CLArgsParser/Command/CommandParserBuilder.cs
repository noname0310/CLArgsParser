using System;
using CLArgsParser.Args;

namespace CLArgsParser.Command
{
    public class CommandParserBuilder
    {
        private CommandParser _commandParser;

        public CommandParserBuilder(ArgsParser argsParser)
        {
            _commandParser = new(argsParser);
        }

        public static CommandParser BuildDefault()
        {
            return new CommandParserBuilder(ArgsParserBuilder.BuildDefault())
                .UseCommandPrefix("/")
                .Build();
        }

        public CommandParserBuilder UseCommandPrefix(string prefix)
        {
            if (prefix == string.Empty || prefix == null)
                throw new ArgumentNullException(nameof(prefix), "\"prefix\" can not be string.Empty or null");
            if (prefix.IndexOf(' ') != -1)
                throw new ArgumentException("\"prefix\" can not contain space", nameof(prefix));
            _commandParser.CommandPrefix.Enable = true;
            _commandParser.CommandPrefix.Value = prefix;
            return this;
        }

        public CommandParser Build() => _commandParser;
    }
}
