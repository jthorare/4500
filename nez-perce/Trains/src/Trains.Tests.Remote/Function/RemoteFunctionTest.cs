using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trains.Remote;
using Trains.Remote.Function;

namespace Trains.Tests.Remote.Function;

[TestClass]
public class RemoteFunctionTest
{
    [TestMethod]
    public void RemoteFunctionTestConstruction()
    {
        // Construct the function
        var _ = new RemoteFunction
        {
            FunctionName = RemoteFunctionName.End,
            Arguments = new List<object> { true }
        };
    }
    
    [TestMethod]
    public void RemoteFunctionTestSendEndAsMessage()
    {
    }
}