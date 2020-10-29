using System;
using CLArgsParser.Command;

namespace CoreConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandParser commandParser = CommandParserBuilder.BuildDefault();

            Console.WriteLine("---------------------------------------");
            foreach (var item in commandParser.Parse(@"/""a b c"" d e"))       //    [a b c]      	    [d]       	    [e]
            {
                Console.Write($"[{item.Str}]    ");
            }
            Console.WriteLine();
            Console.WriteLine("---------------------------------------");
            foreach (var item in commandParser.Parse(@"/""ab\""c"" ""\\"" d")) //    [ab"c]       	    [\]       	    [d]
            {
                Console.Write($"[{item.Str}]    ");
            }
            Console.WriteLine();
            Console.WriteLine("---------------------------------------");
            foreach (var item in commandParser.Parse(@"/a\\\b d""e f""g h"))   //    [a\\\b] 	        [de fg]        	[h]
            {
                Console.Write($"[{item.Str}]    ");
            }
            Console.WriteLine();
            Console.WriteLine("---------------------------------------");
            foreach (var item in commandParser.Parse(@"/a\\\""b c d"))         //    [a\"b] 	        [c]       	    [d]
            {
                Console.Write($"[{item.Str}]    ");
            }
            Console.WriteLine();
            Console.WriteLine("---------------------------------------");
            foreach (var item in commandParser.Parse(@"/a\\\\""b c"" d e"))    //    [a\\b c] 	        [d] 	        [e]
            {
                Console.Write($"[{item.Str}]    ");
            }
            Console.ReadLine();
        }
    }
}
