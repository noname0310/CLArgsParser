using System;
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
                "/ze lbgf l \\h"
            };
            foreach (var item in testCases)
                ParseTest(commandParser, item);

            string[] randomtest = new string[1000000000];
            string[] randomtest2 = new string[1000000000];
            string[] randomtest3 = new string[1000000000];
            string[] randomtest4 = new string[1000000000];
            string[] randomtest5 = new string[1000000000];
            string[] randocszfvsdgbdfmtest2 = new string[1000000000];
            string[] rando3mtest2 = new string[1000000000];
            string[] rand3omtest4 = new string[1000000000];
            string[] random5test5 = new string[1000000000];
            string[] random6test2 = new string[1000000000];
            string[] randomtfest3 = new string[1000000000];
            string[] randomtedst4 = new string[1000000000];
            string[] randomtescct5 = new string[1000000000];
            string[] randomtesvt2 = new string[1000000000];
            string[] randomtefbmtest4 = new string[1000000000];
            string[] rangvbdomtcvest5 = new string[1000000000];
            string[] randomgcvbtest2 = new string[1000000000];
            string[] randomvvtest3 = new string[1000000000];
            string[] randomtdfdfest4 = new string[1000000000];
            string[] randovvvzmtest5 = new string[1000000000];
            string[] randvvobdomtest3 = new string[1000000000];
            string[] randombntest4 = new string[1000000000];
            string[] ranvvvvvvvvvvvvvvvdomtest5 = new string[1000000000];
            string[] randovmtest2 = new string[1000000000];
            string[] randovmtest3 = new string[1000000000];
            string[] randvomtest4 = new string[1000000000];
            string[] randvomtest5 = new string[1000000000];
            string[] randvvvvomtest2 = new string[1000000000];
            string[] randvomtest3 = new string[1000000000];
            string[] randvvomtest4 = new string[1000000000];
            string[] randzzzzomtest5 = new string[1000000000];
            string[] randzzomtest2 = new string[1000000000];
            string[] randomtezzst3 = new string[1000000000];
            string[] rzzandomtest4 = new string[1000000000];
            string[] randomzztest5 = new string[1000000000];
            string[] ranzzdomtest2 = new string[1000000000];
            string[] randomtzzest3 = new string[1000000000];
            string[] ranzzdomtest4 = new string[1000000000];
            string[] randzzomtest5 = new string[1000000000];
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
            foreach (var item in commandParser.Parse(str) ?? new Slice[0])
            {
                Console.Write($"[{item.Parse()}]    ");
            }
            Console.WriteLine();
        }

        public static void ParseTest(CommandParser commandParser, string str)
        {
            Console.WriteLine("---------------------------------------");
            Console.WriteLine(str);
            foreach (var item in commandParser.Parse(str) ?? new Slice[0])
            {
                Console.Write($"[{item.Parse()}]    ");
            }
            Console.WriteLine();
        }

        private static Random _random = new Random((int)DateTime.Now.Ticks & 0x0000FFFF); //랜덤 시드값
        
        public static string RandomString(int _nLength = 12)
        {
            const string strPool = "abcdefghijklmnopqestuvwxyz0123456789                    \\\\\\\\\\\\\\ \"\"\"\"\""; //문자 생성 풀
            char[] chRandom = new char[_nLength];
            for (int i = 0; i < _nLength; i++ ) 
            { 
                chRandom[i] = strPool[_random.Next(strPool.Length)]; 
            } 
            string strRet = new string(chRandom); // char to string
            return strRet; 
        }

    }
}
