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
            netPlayers = new NetPlayer[8];
        }

        public override void Update()
        {
            scenario.PhysicsStep(Program.DeltaTime);
        }

        public override void OnPacket(Lidgren.Network.NetBuffer msg)
        {
            byte index = msg.ReadByte();
            switch (index)
            {
                case MsgIndex.statusUpdate:
                    if (msg.ReadByte() == MsgIndex.subIndex.playerCreate)
                    {;
                        byte id = msg.ReadByte();
                        netPlayers[id] = new NetPlayer(scenario.PlayersPos[id], this, Program.server.players[id]);
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
