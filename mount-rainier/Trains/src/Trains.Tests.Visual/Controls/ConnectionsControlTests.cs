using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trains.Visual.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trains.Visual.Controls.Tests
{
    [TestClass()]
    public class ConnectionsControlTests
    {
        [TestMethod()]
        public void calculateIndexTest()
        {
            Assert.AreEqual(ConnectionsControl.calculateIndex(0), 0);
            Assert.AreEqual(ConnectionsControl.calculateIndex(1), 1);
            Assert.AreEqual(ConnectionsControl.calculateIndex(2), -1);
            Assert.AreEqual(ConnectionsControl.calculateIndex(3), 2);
            Assert.AreEqual(ConnectionsControl.calculateIndex(4), -2);
            Assert.AreEqual(ConnectionsControl.calculateIndex(5), 3);
            Assert.AreEqual(ConnectionsControl.calculateIndex(6), -3);
        }
    }
}