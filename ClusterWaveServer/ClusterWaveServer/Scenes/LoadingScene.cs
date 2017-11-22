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
            /*
            Scenario.ScenarioLoader load = new Scenario.ScenarioLoader(scenarioToLoad);
            load.OnCreationPacket += Program.server.sendByteArray;
            Program.server.SetScene(new InGameScene(load.CreateScenario()));
            load.OnCreationPacket -= Program.server.sendByteArray;
             */
            SendScenario(scenarioToLoad);
        }

        public override void Update()
        {

        }


        public override void OnPacket(Lidgren.Network.NetBuffer msg)
        {

        }

        public override void OnExit()
        {

        }

        public void SendScenario(string scenario)
        {
            //para el juego terminado, agregar checkeo de que se haya cargado bien? (load.IsOk)
            Scenario.ScenarioLoader load = new Scenario.ScenarioLoader(scenario);
            load.OnCreationPacket += Program.server.sendByteArray;
            Program.server.SetScene(new InGameScene(load.CreateScenario()));
            load.OnCreationPacket -= Program.server.sendByteArray;
        }
    }
}
