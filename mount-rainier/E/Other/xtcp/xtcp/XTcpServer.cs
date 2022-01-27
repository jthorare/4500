using System;
using System.Net;
using System.Net.Sockets;
using xtcp.xjson;

namespace xtcp
{
    /// <summary>
    /// Represents a TCP Server that takes input as JSON and reverses it using xjson.
    /// </summary>
    public class XTcpServer
    {
        /// <summary>
        /// The TcpListener that accepts connections.
        /// </summary>
        private readonly TcpListener _listener;

        /// <summary>
        /// Whether the current server is active. Setting this to false breaks the Run loop.
        /// </summary>
        private bool _isActive;

        /// <summary>
        /// Initialize an xtcp server with the given port.
        /// </summary>
        /// <param name="port">The port to listen on.</param>
        /// <exception cref="ArgumentException">Thrown if the port is not in the valid range of ports.</exception>
        public XTcpServer(ushort port)
        {
            if (port is < 2046 or > 65535)
            {
                throw new ArgumentException("Port must be between [2046, 65535]");
            }

            IPAddress localAddress = IPAddress.Parse("127.0.0.1");
            _listener = new TcpListener(localAddress, port);
            _isActive = true;
            _listener.Start();
        }

        ~XTcpServer()
        {
            Shutdown();
            _listener.Stop();
        }

        /// <summary>
        /// Shutdown the server.
        /// </summary>
        public void Shutdown()
        {
            _isActive = false;
        }

        /// <summary>
        /// Start the server and listen for incoming connections. If a connection is pending, accept it. Stop looking
        /// for connections if the server is shutdown.
        /// </summary>
        public void Run()
        {
            while (_isActive)
            {
                if (!_listener.Pending()) continue;
                AcceptConnection();
                break;
            }
        }

        /// <summary>
        /// Accept an incoming connection and send a response.
        /// See https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.tcplistener?view=net-5.0
        /// </summary>
        private void AcceptConnection()
        {
            // Data Buffer
            Byte[] bytes = new Byte[256];
            String data = null!;

            // Perform a blocking call to accept requests.
            TcpClient client = _listener.AcceptTcpClient();// Get a stream object for reading
            NetworkStream stream = client.GetStream();
            string output = null!;
            // Loop to receive all the data sent by the client.
            int i;
            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                // Translate data bytes to a ASCII string.
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                Console.Write("Received: {0}", data);

                // Process the data sent by the client.
                output += data;


            }

            output = ProcessData(output);

            byte[] msg = System.Text.Encoding.ASCII.GetBytes(output);
            stream.Write(msg, 0, msg.Length);
            Console.WriteLine("Sent: {0}", output);
            // Shutdown and end connection
            client.Close();
        }

        /// <summary>
        /// Process the data and return the message that should be sent back.
        /// </summary>
        /// <param name="data">The data in string format.</param>
        /// <returns>The processed data to be sent back.</returns>
        private static string ProcessData(string data)
        {
            return Xjson.XjsonMain(data);
        }

    }
}