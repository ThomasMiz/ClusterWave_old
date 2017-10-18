using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClusterWaveServer.Scenes
{
    class LoadingScene : Scene
    {

        public LoadingScene(string scenarioToLoad) : base()
        {
            Scenario.ScenarioLoader load = new Scenario.ScenarioLoader(scenarioToLoad);
            load.OnCreationPacket += Program.server.sendByteArray;
            Program.server.SetScene(new InGameScene(load.CreateScenario()));
            load.OnCreationPacket -= Program.server.sendByteArray;
        }

        public override void Update()
        {

        }


        public override void OnPacket(Lidgren.Network.NetIncomingMessage msg)
        {

        }
    }
}
