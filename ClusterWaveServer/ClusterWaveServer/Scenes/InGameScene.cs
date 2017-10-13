using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClusterWaveServer.Scenario;
using ClusterWaveServer.Network;
using Lidgren.Network;

namespace ClusterWaveServer.Scenes
{
    class InGameScene : Scene
    {
        Scenario.Scenario scenario;

        public Scenario.Scenario Scenario { get { return scenario; } }

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
