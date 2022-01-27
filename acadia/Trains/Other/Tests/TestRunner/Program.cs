using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Tests;
namespace TestRunner
{
    /// <summary>
    /// Class representing our Test Harness
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            HashSet<object> objects = new HashSet<object>();
            objects.Add(new TrainsModelsGamePiecesTests());
            objects.Add(new TrainsUtilAvaloniaConverterTests());
            objects.Add(new TrainsUtilJsonTests());
            objects.Add(new TrainsUtilUtilitiesTests());
            objects.Add(new TrainsModelsGameStatesTests());
            objects.Add(new TrainsModelsStrategiesTests());
            objects.Add(new TrainsModelsGameEntitiesTests());
            foreach (object obj in objects)
            {
                InvokeMethods(obj);
            }
        }

        /// <summary>
        /// Runs all non-default object related methods for the given object.
        /// </summary>
        /// <param name="obj">The Test class instance whose tests should be run and recorded</param>
        private static void InvokeMethods(object obj)
        {
            int pass = 0;
            int fail = 0;
            var methods = obj.GetType().GetMethods();
            methods = CleanMethods(methods);

            foreach (MethodInfo method in methods)
            {
                try
                {
                    method.Invoke(obj, new object[0]);
                    pass++;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    fail++;
                }
            }

            Console.WriteLine($"Test suite: {obj}");
            Console.WriteLine($"Passed tests: {pass}");
            Console.WriteLine($"Failed tests: {fail}");
        }

        /// <summary>
        /// Cleans the MethodInfo[] array to not include ToString, GetHashCode, Equals, and ...
        /// </summary>
        /// <param name="methods">The MethodInfo[] to clean from</param>
        /// <returns>A MethodInfo[] with the above methods removed</returns>
        private static MethodInfo[] CleanMethods(MethodInfo[] methods)
        {
            MethodInfo[] cleanMethods = new MethodInfo[methods.Count() - 4];
            Array.Copy(methods, cleanMethods, methods.Count() - 4);
            return cleanMethods;
        }
    }
}
