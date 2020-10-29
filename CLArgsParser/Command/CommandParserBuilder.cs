using CLArgsParser.Args;

namespace CLArgsParser.Command
{
    public class CommandParserBuilder
    {
        private CommandParser _commandParser;

        public CommandParserBuilder(ArgsParser argsParser)
        {
            _commandParser = new CommandParser(argsParser);
        }

        public static CommandParser BuildDefault()
        {
            return new CommandParserBuilder(ArgsParserBuilder.BuildDefault())
                .UseCommandPrefix("/")
                .Build();
        }

        public CommandParserBuilder UseCommandPrefix(string prefix)
        {
            _commandParser.CommandPrefix.Enable = true;
            _commandParser.CommandPrefix.Value = prefix;
            return this;
        }

        public CommandParser Build() => _commandParser;
    }
}
