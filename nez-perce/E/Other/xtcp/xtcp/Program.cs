using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;

namespace xtcp
{
    internal static class Program
    {
        /// <summary>
        /// Start a program that allows a single TCP client to connect. It consumes a series of JSON values from
        /// the input side of this TCP connection and delivers JSON to the output side of a TCP connection. Once the
        /// connection is closed, the server shuts down.
        /// </summary>
        private static async Task<int> Main(string[] args)
        {

            var cmd = new RootCommand("A program that allows a single TCP client to connect. It consumes a series of JSON values from the input side of this TCP connection and delivers JSON to the output side of a TCP connection. Once the connection is closed, the server shuts down.")
            {
                new Argument<ushort?>("port", "Your name.")
            };
            cmd.Handler = CommandHandler.Create<ushort>(HandleXtcp);
            return await cmd.InvokeAsync(args);
        }

        /// <summary>
        /// Handle the logic for xtcp given the required parameters
        /// </summary>
        /// <param name="port">The port the run xtcp on.</param>
        private static void HandleXtcp(ushort port=45678)
        {
            XTcpServer server = new(port);
            Task.Run(() =>
            {
                Thread.Sleep(3000);
                server.Shutdown();
            });
            server.Run();
        }
    }
}