using src;
using System;
using System.IO;

namespace src
{
    /// <summary>
    /// Class representing an implementation of the Head Linux command
    /// </summary>
    public class XHead
    {
        /// <summary>
        /// Writes the given number of lines from STDIN to STOUT
        /// </summary>
        /// <param name="lineCount">Number of lines to read</param>
        /// dsgbfsb
        public void Output(int lineCount, StreamReader input, StreamWriter output)
        {
            string line; // the current line to direct from input to the output
            while ((lineCount > 0) && ((line = input.ReadLine()) != null)) // while there is still a line to read
            {
                try // try to write to the given StreamWriter
                {
                    output.WriteLine(line);
                    lineCount--; // decrease number of lines to redirect
                }
                catch (Exception)
                {
                    CheckCLA(true, output); // display an error message and exit
                    return;
                }
            }
        }

        /// <summary>
        /// Returns whether the given condition signifies an error and displays an error message appropriately.
        /// </summary>
        /// <param name="cond">Given condition indicating presence of an error</param>
        public bool CheckCLA(bool cond, StreamWriter output)
        {
            // if cond indicates there is an error (true)
            if (cond)
            {
                // display error message
                output.WriteLine("error");
            }
            // return the error status regardless of whether there is one or not
            return cond;
        }
    }
    /// <summary>
    /// Class containing the main method to run an implementation of the Head Linux Command
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Re-directs a command-line-specified number of lines from STDIN to STDOUT
        /// </summary>
        /// <param name="args">Command line arguments supplied</param>
        public static void Main(string[] args) // need to catch all errors related to Console
        {
            XHead xhead = new XHead();
            StreamReader input = new StreamReader(Console.OpenStandardInput());
            Console.SetIn(input);

            StreamWriter output = new StreamWriter(Console.OpenStandardOutput());
            output.AutoFlush = true;
            Console.SetOut(output);

            // check that the number of arguments is exactly 2; 
            if (xhead.CheckCLA(args.Length != 1, output))
            {
                return;
            }

            string arg = args[0]; // read the argument

            if (arg.Length > 1 && xhead.CheckCLA(arg[0] != '-', output)) // check that the number argument is preceded by a hyphen and is more than a hyphen
            {
                return;
            }

            int lineCount;
            // short circuit if argument is not a number OR if the number is not greater than 0
            if (xhead.CheckCLA(!int.TryParse(arg.Substring(1, arg.Length - 1), out lineCount) || lineCount < 0, output))
            {
                return;
            }

            xhead.Output(lineCount, input, output);
            input.Close();
            output.Close();
        }
    }
}
