using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ClusterWave.Network;

namespace ClusterWave.Scenes
{
    class Caca : Scene
    {
        public Caca()
        {
            InGameScene.scenario = new Scenario.Scenario();

            InGameScene.client = new Client();
            InGameScene.client.connect("127.0.0.1");
        }

        public override void Update()
        {
            InGameScene.client.Update();
            if (InGameScene.scenario.DoneLoading)
                game.SetScene(new InGameScene());
        }

        public override void Draw()
        {
            GraphicsDevice.Clear(Color.Red);
        }

        public override void OnExit()
        {
            
        }

        public override void OnResize()
        {
            
        }
    }
}
