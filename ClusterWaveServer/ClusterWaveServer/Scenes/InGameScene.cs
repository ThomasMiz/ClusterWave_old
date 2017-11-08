using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ClusterWaveServer.Scenario;
using ClusterWaveServer.Network;
using Lidgren.Network;

namespace ClusterWaveServer.Scenes
{
    class InGameScene : Scene
    {
        Scenario.Scenario scenario;

        public Scenario.Scenario Scenario { get { return scenario; } }
        DebugRenderer.DebugManager debugManager;

        NetPlayer[] netPlayers;

        public InGameScene(Scenario.Scenario scenario) : base()
        {
            this.scenario = scenario;
            debugManager = new DebugRenderer.DebugManager(scenario).Start();
        }

        public override void Update()
        {
            scenario.PhysicsStep(Program.DeltaTime);
        }

        public override void OnPacket(Lidgren.Network.NetIncomingMessage msg)
        {
            byte index = msg.ReadByte();
            switch (index)
            {
                case MsgIndex.statusUpdate:
                    if (msg.ReadByte() == MsgIndex.subIndex.playerCreate)
                    {
                        Random r = new Random();
                        byte id = msg.ReadByte();
                        netPlayers[id] = new NetPlayer(new Vector2(r.Next( 0 , (int)scenario.Width), r.Next(0 , (int)scenario.Height)), this, null);
                    }
                    break;
                case MsgIndex.error:

                    break;
                case MsgIndex.playerAct:

                    break;
                case MsgIndex.playerMove:
                    
                    break;

            }
        }

        public override void OnExit()
        {
            debugManager.Exit();
        }

        void CreatePlayer(Vector2 pos,int id,PlayerInfo player)
        {
            new NetPlayer(pos, this, player);
        }

    }
}
