using IntegrationTests.IntegrationObjects;
using System;

namespace IntegrationTests
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                throw new ArgumentException("Only one argument allowed");
            }

            switch (args[0])
            {
                case "xlegal":
                    new Xlegal().Run();
                    break;
                case "xstrategy":
                    new Xstrategy().Run();
                    break;
                case "xmap":
                    new Xmap().Run();
                    break;
                case "xvisualize":
                    new Xvisualize().Run();
                    break;
                default:
                    throw new ArgumentException($"Invalid integration test harness chosen: {args[0]}");
            }
        }
    }
}
