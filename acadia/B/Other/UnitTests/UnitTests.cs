using NUnit.Framework;
using src;
using System;
using System.IO;
using System.Linq;

namespace UnitTests
{
    public class UnitTests
    {
        XHead head;
        [SetUp]
        public void Setup()
        {
            head = new XHead();
        }

        [TestCase(-10)]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(224)]
        [TestCase(24124)]
        public void Test_Output(int lineCount)
        {
            string dir = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(dir).Parent.Parent.FullName;
            StreamReader testInput = new StreamReader($"{projectDirectory}\\input.txt");
            StreamWriter testOutput = new StreamWriter($"{projectDirectory}\\output.txt");

            head.Output(lineCount, testInput, testOutput); // write result to the file
            testInput.Close();
            testOutput.Close();

            var lineCountLines = string.Join("\r\n", File.ReadLines($"{projectDirectory}\\input.txt").Take(lineCount));
            if (lineCountLines.Length > 0) lineCountLines += "\r\n";
            Assert.AreEqual(lineCountLines, File.ReadAllText($"{projectDirectory}\\output.txt"));
        }
    }
}