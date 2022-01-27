using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Trains.Tests.Admin;
using Trains.Tests.Common;
using Trains.Tests.Common.Map;
using Trains.Tests.Common.Map.Json;
using Trains.Tests.Player.Strategy;
using Trains.Visual.Controls.Tests;

namespace xtest
{
    class Program
    {
        static void Main(string[] args)
        {
            HashSet<object> tests = new();
            // Trains.Tests.Admin
            tests.Add(new RefereeTest());
            tests.Add(new ManagerTests());

            //Trains.Tests.Common
            tests.Add(new PlayerStateJsonTest());

            //Trains.Tests.Common.Map
            tests.Add(new ConnectionTest());
            tests.Add(new LocationTest());
            tests.Add(new MapBuilderTest());
            tests.Add(new TrainsMapTest());

            //Trains.Tests.Common.Map.Json
            tests.Add(new CityJsonConverterTest());
            tests.Add(new CityJsonTest());
            tests.Add(new MapJsonTest());
            tests.Add(new TrainsMapJsonConverterTest());

            //Trains.Tests.Player.Strategy
            tests.Add(new DynamicStrategyTest());
            tests.Add(new StrategyTest());
            tests.Add(new ConnectionsControlTests());

            //Trains.Tests.Player.Strategy
            tests.Add(new ConnectionsControlTests());

            foreach (object test in tests)
            {
                InvokeMethods(test);
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
            var initialize = methods.Where(method => method.Name.Contains("Initialize", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            methods = CleanMethods(methods);

            foreach (MethodInfo method in methods)
            {
                if (method == initialize) { continue; }
                if (initialize != null) { initialize.Invoke(obj, new object[0]); }
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
