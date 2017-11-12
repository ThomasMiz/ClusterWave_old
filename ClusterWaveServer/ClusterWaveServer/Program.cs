using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClusterWaveServer.Network;
using ClusterWaveServer.Scenario;

namespace ClusterWaveServer
{
    class Program
    {
        /// <summary>
        /// The Update rate in milliseconds. Set to update the game 60 times per second.
        /// </summary>
        private const float UpdateRate = 1000f / 60f;

        /// <summary>
        /// The elapsed game time in seconds
        /// </summary>
        public static float DeltaTime = 0;
        
        /// <summary>
        /// THE SERVER :O
        /// </summary>
        public static Server server;

        static void Main(string[] args)
        {
            Console.WriteLine("Game made with help of \"EL BRUFE\" fuck you \"Dusto\"");
            server = new Server();

            Thread thread = new Thread(ServerThreadFunction);
            thread.Start();

            while (true)
            {
                String cmd = Console.ReadLine();
                if (cmd == "scenario")
                {
                    string scenario = Console.ReadLine();
                    server.SetScenario(scenario);
                    Console.WriteLine("Scenario Set to : " + scenario);
                }
                if (cmd == "start") server.SendScenario();

                if (cmd == "startM") server.startMatch();
                 //aca hay q poner para que pruebe hacer comandos (que llame una funcion que lo haga en otro lado?)
            }
        }

        static void ServerThreadFunction()
        {
            Stopwatch watch = new Stopwatch();
            while (true)
            {
                DeltaTime = (float)watch.Elapsed.TotalSeconds;
                watch.Restart();
                server.UpdateServer();
                int slp = (int)(UpdateRate - watch.ElapsedMilliseconds);
                
                if (slp > 0)
                    Thread.Sleep(slp);
            }
        }
    }
}
