using System;
using System.Net.Sockets;

namespace xtcp
{
    class Program
    {
        /// <summary>
        /// Entry point of xtcp
        /// </summary>
        /// <param name="args"> a string array that SHOULD only contain a single argument that is an int representing a port number</param>
        static void Main(string[] args)
        {
            Xtcp xtcp = new Xtcp();
            int port = xtcp.VerifyArgs(args); // get the port from the command line arguments
            if (port == -1) { return; } // something with the port argument was wrong

            TcpListener server = xtcp.ConnectListener(port);
            server.Start(); // start listening for client connections

            TcpClient client = xtcp.ConnectClient(server);
            if (client == null)
            {
                Console.WriteLine($"No client connected to server.");
                return;
            }

            string json = xtcp.ReadClientStream(client);
            string reversedJson = xtcp.Xjson(json); // call ../C/xjson

            // need to return the reversed xjson over the stream
            xtcp.WriteClientStream(reversedJson, client);

            client.Close();
            server.Stop();
        }

    }
}
