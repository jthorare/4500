using System;

namespace xhead
{
    internal static class Program
    {
        internal static void Main(string[] arguments)
        {
            // Should be exactly two args: command (xhead) and number of lines to read from stdin (-x).
            if (Environment.GetCommandLineArgs().Length != 2)
            {
                // Error: wrong number of arguments
                Console.WriteLine("error");
                return;
            }

            // Get first argument ignoring command (xhead).
            var arg = Environment.GetCommandLineArgs()[1];
            
            // Try to parse out number of lines needed to read.
            if (!ParseLines(arg, out var numLines))
            {
                // Error: Argument malformed. Cannot determine number of lines to return.
                Console.WriteLine("error");
                return;
            }
            
            PrintLines(numLines);
        }
        
        /**
         * Try to parse out number of lines needed to read. Return false if the line cannot be parsed.
         */
        private static bool ParseLines(string arg, out uint numLines)
        {
            numLines = 0;
            return arg.Split('-').Length == 2 &&
                   uint.TryParse(arg.Split('-')[1], out numLines);
        }

        /**
         * Read the specified number of lines and output them if they exist.
         */
        private static void PrintLines(uint numLines)
        {
            // Read the specified number of lines (if they exist).
            var output = "";
            uint count = 0;
            while (count < numLines)
            {
                string line;
                if ((line = Console.ReadLine()) != null)
                {
                    output += line + "\n";
                }
                count++;
            }
            
            Console.Write(output);
        }
    }
}
