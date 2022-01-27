using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace xtcp
{
    public class Xtcp
    {
        /// <summary>
        /// Calls the ../C/xjson program with the supplied JSON
        /// </summary>
        /// <param name="json">The JSON to be reversed by xjson</param>
        /// <returns>returns the supplied JSON after reversing it</returns>
        public string Xjson(string json)
        {
            // get the path for xjson
            string currDir = Environment.CurrentDirectory; // get the absolute path for this executable
            string xjsonPath = Path.GetFullPath(Path.Combine(currDir, "../../../C/xjson")); // xjson is in the greatgrandparent directory/C
            // Use xjson with the TCP I/O
            ProcessStartInfo startInfo = new ProcessStartInfo(xjsonPath); // relative path to the xjson executable
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false; // must be set to redirect std i/o/e
            var process = new Process();
            process.StartInfo = startInfo;
            process.Start();
            StreamWriter sw = process.StandardInput;
            sw.Flush();
            sw.WriteLine(json); // write the json to be reversed to xjson's stdin
            sw.Close(); // Signifies EOF for xjson to return
            process.WaitForExit(); // wait for exit before reading
            StreamReader sr = process.StandardOutput;
            string reversed = sr.ReadToEnd(); // read the stdout from xjson
            return reversed;
        }

        /// <summary>
        /// Returns the port given through a command line argument, or the default port if one is not provided.
        /// Verifies only valid arguments were supplied to the program. Prints to console error messages that describe what was incorrect.
        /// </summary>
        /// <param name="args">The command-line arguments from calling this program</param>
        /// <returns>The port number supplied. port = -1 signifies an error.</returns>
        public int VerifyArgs(string[] args)
        {
            int port = 45678; // default port value in case none is supplied
            // Parse the client's command line arguments
            if (args.Length == 0) { return port; } // no port number supplied means use default port number from assignment spec
            if (args.Length > 1) // exit with a helpful message if more than one arg
            {
                Console.WriteLine("Too many arguments given; one argument expected at most.");
                return -1;
            }
            else if (args.Length == 1) // expected arg length
            {
                bool parsed = Int32.TryParse(args[0], out int rv); // returns false if parsing fails ONLY WORKS ON INTs
                if (!parsed || rv < 2048 || rv > 65535) // Assignment supplied bounds; exit with a helpful message if invalid arg given
                {
                    Console.WriteLine("Invalid argument given; numeric value within [2048, 65535] expected.");
                    return -1;
                }
                port = rv; // store client provided port value if valid arg
            }
            return port;
        }


        /// <summary>
        /// Creates a TcpListener to listen for client connections
        /// </summary>
        /// <param name="port">The port number to use for the TcpListener</param>
        /// <returns>A TcpListener that is using the supplied TcpListener</returns>
        public TcpListener ConnectListener(int port)
        {
            // Connect TCP client (3s timeout w error msg)
            // https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.tcplistener?view=net-5.0

            IPAddress localhost = IPAddress.Parse("127.0.0.1"); // use local host ip
            TcpListener server = new TcpListener(localhost, port); // create tcplistener
            return server;
        }

        /// <summary>
        /// Creates a TcpClient connection. Times out if no client is connected within 3 seconds
        /// </summary>
        /// <param name="server">The TcpListener that is listening for client connections</param>
        /// <returns>The TcpClient that was connected to the given TcpListener. Returning null signifies the connection timed out.</returns>
        public TcpClient ConnectClient(TcpListener server)
        {
            DateTime endTime = DateTime.Now.AddSeconds(3);
            TcpClient client = null;

            while (!(DateTime.Now > endTime) && client == null) // only try to connect for 3 seconds and while we do not have an existing connection
            {
                if (server.Pending()) // is there a client waiting to connect
                {
                    client = server.AcceptTcpClient(); // connect
                }
            }
            return client;
        }

        /// <summary>
        /// Read the TcpClient's stream into a single string.
        /// </summary>
        /// <param name="client">The TcpClient whose stream to read from</param>
        /// <returns>The TcpClient's stream read into a single string</returns>
        public string ReadClientStream(TcpClient client)
        {
            // buffer to read into
            Byte[] bytes = new Byte[4096]; // arbitrary array size. 4kb seems good for our purposes
            string json = "";

            //TcpClient client = server.AcceptTcpClient();
            NetworkStream stream = client.GetStream();
            int readBytes; // represents the bytes read from the stream in each NetworkStream.Read() call

            while ((readBytes = stream.Read(bytes, 0, bytes.Length)) != 0) // read until no data is sent
            {
                json += System.Text.Encoding.UTF8.GetString(bytes, 0, readBytes);
            }
            return json;
        }

        /// <summary>
        /// Writes the given string as JSON to the given TcpClient
        /// </summary>
        /// <param name="reversedJson"></param>
        /// <param name="client"></param>
        public void WriteClientStream(string reversedJson, TcpClient client)
        {
            NetworkStream stream = client.GetStream();

            byte[] data = Encoding.UTF8.GetBytes(reversedJson);
            stream.Flush();
            stream.Write(data, 0, data.Length);
        }
    }
}
