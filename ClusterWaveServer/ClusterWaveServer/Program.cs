using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClusterWaveServer.Network;
using ClusterWaveServer.Scenarios;

namespace ClusterWaveServer
{
    class Program
    {
        static Stopwatch watch;
        static Server server;

        static void Main(string[] args)
        {
            Console.WriteLine("Game made with help of \"EL BRUFE\" fuck you \"Dusto\"");
            watch = new Stopwatch();
            server = new Server();

            Thread thread = new Thread(serverthread);
            thread.Start();

            while (true)
            {
                String cmd = Console.ReadLine();
                Console.WriteLine("yes.");

                server.SendScenario(cmd);
            }
        }

        static void serverthread()
        {
            while (true)
            {
                const double fps = 1000.0 / 60.0;
                watch.Restart();
                server.UpdateServer();
                int slp = (int)(fps - watch.ElapsedMilliseconds);
                if (slp > 0)
                    Thread.Sleep(slp);
            }
        }
    }
}
