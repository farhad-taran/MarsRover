using System;
using System.Collections.Generic;
using System.Linq;
using MarsRover;

namespace Terminal
{
    class Program
    {
        private const string Header = @"
---------------------------------------------------------------------------------------------------------
Input your rover commands, when ready enter ""?"" to communicate with rovers or enter ""q"" to exit, eg:
---------------------------------------------------------------------------------------------------------

5 5
1 2 N
LMLMLMLMM
3 3 E
MMRMMRMRRM
?
";

        private const string Footer =
            "---------------------------------------------------------------------------------------------------------";

        static void Main(string[] args)
        {
            var mediator = new MessageParser();

            Console.WriteLine(Header);
            Console.WriteLine(Footer);

            var lines = new List<string>();
            string line = string.Empty;
            do
            {
                line = Console.ReadLine();

                if (line == "?")
                {
                    Console.WriteLine();
                    Console.WriteLine(mediator.HandleMessage(string.Join(Environment.NewLine, lines.Where(l=>!string.IsNullOrWhiteSpace(l)))));
                    Console.WriteLine(Footer);
                    lines = new List<string>();
                }
                else
                {
                    lines.Add(line);
                }

            } while (!string.IsNullOrWhiteSpace(line) || line?.ToLower() != "q");
        }
    }
}
