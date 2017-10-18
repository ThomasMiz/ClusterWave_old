using System;
using System.Threading;

namespace ClusterWaveServer.DebugRenderer
{
    class DebugManager
    {
        Scenario.Scenario s;
        Thread thread;

        public DebugManager(Scenario.Scenario s)
        {
            this.s = s;
        }


        /// <summary>
        /// Returns this same object so you can do DebugManager debug = new DebugManager(s).Start();
        /// </summary>
        /// <returns></returns>
        public DebugManager Start()
        {
            if (thread != null)
                throw new SosUnPelotudoException("This DebugManager has already been used!");

            thread = new Thread(_run);
            thread.Start();

            return this;
        }

        private void _run()
        {
            using (DebugWindow window = new DebugWindow(s))
            {
                window.Run();
            }
        }

        public void Exit()
        {
            thread.Abort();
        }
    }
}
