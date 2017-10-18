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

        NetPlayer[] netPlayers;

        public InGameScene(Scenario.Scenario scenario) : base()
        {
            this.scenario = scenario;
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

                    break;
                case MsgIndex.error:

                    break;
                case MsgIndex.playerAct:

                    break;
                case MsgIndex.playerMove:
                    
                    break;

            }
        }

        void CreatePlayer(Vector2 pos,int id,PlayerInfo player)
        {
            new NetPlayer(pos, this, player);
        }

        /*void playerMoveUp() //que son estas? no se, no las quiero ver a menos que tengas una buena explicacion.
        {
            
        }

        void playerMoveDown()
        {

        }

        void playerMoveLeft()
        {

        }

        void playerMoveRight()
        {

        }*/
    }
}
