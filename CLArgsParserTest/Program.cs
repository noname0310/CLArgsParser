﻿using System;
using System.Threading.Tasks;
using CLArgsParser;
using CLArgsParser.Args;
using CLArgsParser.Command;

namespace CoreConsoleApp2
{
    class Program
    {
        private static CommandParser commandParser;

        static void Main(string[] args)
        {
            commandParser = CommandParserBuilder.BuildDefault();

            string[] testCases = new string[]
            {
                @"/""a b c"" d e",        //    [a b c]      	    [d]       	    [e]
                @"/""ab\""c"" ""\\"" d",  //    [ab"c]       	    [\]       	    [d]
                @"/a\\\b d""e f""g h",    //    [a\\\b] 	        [de fg]        	[h]
                @"/a\\\""b c d",          //    [a\"b] 	            [c]       	    [d]
                @"/a\\\\""b c"" d e",     //    [a\\b c] 	        [d] 	        [e]
                @"/ze lbgf l \h"          //    [ze]                [lbgf]          [l]            [h]
            };
            foreach (var item in testCases)
                ParseTest(commandParser, item);

            string[] randomtest = new string[1000];
            for (int i = 0; i < randomtest.Length; i++)
            {
                randomtest[i] = "/" + RandomString(_random.Next(100));
            }
            GC.Collect();

            //Parallel.For(0, randomtest.Length, (int i, ParallelLoopState parallelLoopState) => {
            //    ParseTest(CommandParserBuilder.BuildDefault(), randomtest[i]);
            //});
            for (int i = 0; i < randomtest.Length; i++)
            {
                ParseTest(commandParser, randomtest[i]);
            }

            for (; ; ) ParseTestInput();
        }

        public static void ParseTestInput()
        {
            Console.WriteLine("---------------------------------------");
            string str = Console.ReadLine();
            Console.Write("Parse:         ");
            foreach (var item in commandParser.Parse(str) ?? Array.Empty<Slice>())
            {
                Console.Write($"[{item.LazyParse(commandParser)}]    ");
            }
            Console.WriteLine();
        }

        public static void ParseTest(CommandParser commandParser, string str)
        {
            Console.WriteLine("---------------------------------------");
            Console.WriteLine(str);
            Console.Write("Parse:         ");
            foreach (var item in commandParser.Parse(str) ?? Array.Empty<Slice>())
            {
                Console.Write($"[{item.LazyParse(commandParser)}]    ");
            }
            Console.WriteLine();
        }

        private static Random _random = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
        
        public static string RandomString(int _nLength = 12)
        {
            const string strPool = "abcdefghijklmnopqestuvwxyz0123456789                    \\\\\\\\\\\\\\ \"\"\"\"\"";
            char[] chRandom = new char[_nLength];
            for (int i = 0; i < _nLength; i++ ) 
            { 
                chRandom[i] = strPool[_random.Next(strPool.Length)]; 
            } 
            string strRet = new string(chRandom);
            return strRet; 
        }
    }
}
