using System;
using CLArgsParser;
using CLArgsParser.Args;
using CLArgsParser.Command;

class Program
{
    static void Main(string[] args)
    {
        CommandParser commandParser = CommandParserBuilder.BuildDefault();
        Slice[] result = commandParser.Parse(@"/""a b c"" d \\\""e");//    a b c, "d, \"e
        foreach (var item in result)
            Console.WriteLine(item.LazyParse(commandParser));
    }
}
