using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarseerPhysics.Dynamics;
using ClusterWaveServer.Scenario;
using ClusterWaveServer.Network;
using Lidgren.Network;

namespace ClusterWaveServer.Scenes
{
    abstract class Scene
    {
        protected Server server;

        public Scene()
        {
            this.server = Program.server;
        }

        public abstract void Update();
        public abstract void OnPacket(NetIncomingMessage msg);
    }
}
