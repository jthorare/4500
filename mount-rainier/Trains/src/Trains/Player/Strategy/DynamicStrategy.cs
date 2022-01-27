using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Trains.Common.GameState;
using Trains.Common.Map;

namespace Trains.Player.Strategy
{
    /// <summary>
    /// A <see cref="IStrategy"/> that allows for dynamic loading assuming that the namespace for the strategy pointed to by a filepath
    /// is Trains.Player.Strategy.
    /// </summary>
    // This partial fragment implements IStrategy
    public partial class DynamicStrategy : IStrategy
    {
        /// <summary>
        /// The instance of <see cref="IStrategy"/> being dynamically loaded
        /// </summary>
        private readonly IStrategy _strategy;

        /// <inheritdoc/>
        public IImmutableSet<Destination> ChooseDestinations(IImmutableSet<Destination> destinations)
        {
            return _strategy.ChooseDestinations(destinations);
        }

        /// <inheritdoc/>
        public PlayerResponse PlayTurn(PlayerGameState gameState)
        {
            return _strategy.PlayTurn(gameState);
        }
    }
    
    // This partial fragment handles construction of the strategy by compiling and setting up reflective late binding of
    // source code that implements IStrategy. Implemented with inspiration from
    // https://www.tallan.com/blog/2019/09/12/compiling-and-running-a-dll-in-net-core-2-using-roslyn-and-reflection/
    public partial class DynamicStrategy
    {
        /// <summary>
        /// Construct a <see cref="DynamicStrategy"/> with a path to a file that implements a <see cref="IStrategy"/>
        /// along with the Assembly qualified Name of the class.
        /// </summary>
        /// <param name="filename">The file path to a source-code implementation of <see cref="IStrategy"/>.</param>
        /// <param name="assemblyQualifiedName">The Assembly qualified Name of the class.</param>
        /// <exception cref="InvalidOperationException">
        /// If the Assembly qualified Name does not match any class in the Assembly or the class does not implement
        /// <see cref="IStrategy"/>
        /// </exception>
        public DynamicStrategy(string filename, string assemblyQualifiedName)
        {
            // Get source code from file
            string sourceCode = ReadFromFile(filename);
            
            // Generate the compiler to compile the source code
            Compilation compilation = GenerateCompilation(sourceCode, assemblyQualifiedName);
            
            // Compile the file into an assembly
            Assembly assembly = GenerateAssembly(compilation);

            // Get the Type of the compiled strategy
            Type dynamicStrategyClassType = assembly.GetType(assemblyQualifiedName) ??
                                            throw new InvalidOperationException(
                                                "Class does not exist in compiled assembly.");
            
            // Use reflection to create an instance of the strategy
            _strategy = (IStrategy)Activator.CreateInstance(dynamicStrategyClassType)! ??
                        throw new InvalidOperationException("Failed to instantiate Dynamic Strategy.");
        }

        /// <summary>
        /// Read from a file and return a string of its contents.
        /// </summary>
        /// <param name="filename">The file path/name of the file.</param>
        /// <returns>The contents of the file as a string.</returns>
        private static string ReadFromFile(string filename)
        {
            // Set up reading of file
            FileStream fileStream = File.Open(filename, FileMode.Open);
            StreamReader fileReader = new(fileStream);
            
            // Set up building of string
            StringBuilder stringBuilder = new();
            StringWriter stringWriter = new(stringBuilder);
            
            // Copy each line to the StringBuilder
            string? line;
            while ((line = fileReader.ReadLine()) != null)
            {
                stringWriter.WriteLine(line);
            }

            // Build the string
            return stringWriter.ToString();
        }

        /// <summary>
        /// Generate a <see cref="Compilation"/> that will be used to compile the given source code with a class that
        /// matches the Assembly qualified Name.
        /// </summary>
        /// <param name="sourceCode">The source code to be compiled.</param>
        /// <param name="assemblyQualifiedName">
        /// The Assembly qualified Name of the class. Used to name the dynamically linked library.
        /// </param>
        /// <returns>A <see cref="Compilation"/> that compiles the source code.</returns>
        private static Compilation GenerateCompilation(string sourceCode, string assemblyQualifiedName)
        {
            // Tell the compiler how to interpret the source code
            var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);

            // Construct a list of references the generated assembly should be able to use. In this case it is all 
            // references this project knows about.
            List<PortableExecutableReference> references = new();
            foreach (var reference in ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(
                Path.PathSeparator))
            {
                references.Add(MetadataReference.CreateFromFile(reference));
            }

            // Create the Compilation object that will generate a unique .dll for the class.
            return CSharpCompilation.Create($"{assemblyQualifiedName}_DynamicStrategy.dll",
                new[] { syntaxTree },
                references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        }

        /// <summary>
        /// Generate a <see cref="Assembly"/> from the given <see cref="Compilation"/>.
        /// </summary>
        /// <param name="compilation">
        /// The <see cref="Compilation"/> used to compile into a <see cref="Assembly"/>
        /// </param>
        /// <returns>The compiled <see cref="Assembly"/></returns>
        /// <exception cref="InvalidOperationException">If the <see cref="Assembly"/> fails to compile</exception>
        private static Assembly GenerateAssembly(Compilation compilation)
        {
            // A Stream of memory to hold the compiled assembly.
            var memoryStream = new MemoryStream();
            
            // Get the result of the compilation and store it in the memory stream.
            EmitResult result = compilation.Emit(memoryStream);
            
            // If the compilation failed, get error messages and throw an exception with details about how it failed.
            if (!result.Success)
            {
                throw new InvalidOperationException("Failed to compile dynamic strategy");
            }
            
            // Compilation succeeded so copy the bytes of the assembly and load them.
            memoryStream.Seek(0, SeekOrigin.Begin);
            byte[] byteAssembly = memoryStream.ToArray();
            return Assembly.Load(byteAssembly);
        }
    }
}