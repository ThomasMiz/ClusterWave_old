using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClusterWaveServer.Scenarios;
using ClusterWaveServer.Network;
using Lidgren.Network;

namespace ClusterWaveServer.Scenes
{
    class InGameScene : Scene
    {
        Scenario world;

        public InGameScene(Server server,Scenario world) : base(server)
        {
            this.world = world;
        }

        public override void Update()
        {
            
        }

        public override void OnPacket(Lidgren.Network.NetIncomingMessage msg)
        {
            byte index = msg.ReadByte();
            switch (index)
            {
                case MsgIndex.chat:
                    //el chat msg no lo handlees aca, seria copiar y pegar codigo en cada scene. Que el Server lo haga que es lo mismo siempre que hacer con el chat
                    break;
                case MsgIndex.statusUpdate:

                    break;
                case MsgIndex.disconnect:

                    break;
                case MsgIndex.error:

                    break;
                case MsgIndex.playerAct:

                    break;
                case MsgIndex.playerMove:
                    
                    break;

            }
        }

        void playerMoveUp()
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

        }
    }
}
