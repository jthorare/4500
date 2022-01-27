using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trains.Player.Strategy;

namespace Trains.Tests.Player.Strategy
{
    [TestClass]
    public class DynamicStrategyTest
    {
        [TestMethod]
        public void DynamicStrategyTestLoadManualStrategy()
        {
            string file = "../../../../Trains/Player/Strategy/Hold10Strategy.cs";
            string assemblyQualifiedName = "Trains.Player.Strategy.Hold10Strategy";
            DynamicStrategy strategy = new(file, assemblyQualifiedName);
        }
    }
}