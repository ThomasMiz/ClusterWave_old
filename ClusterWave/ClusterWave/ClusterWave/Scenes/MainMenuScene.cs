using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ClusterWave.UI;
using Lidgren.Network;
using ClusterWave.Network;

namespace ClusterWave.Scenes
{
    class MainMenuScene : Scene
    {
        Client client;
        MainMenu m;

        public MainMenuScene(Client client)
        {
            m = new MainMenu();

            this.client = client;
            client.OnPacket += OnPacket;
        }

        public override void Update()
        {
            m.Update();
        }

        public override void Draw()
        {
            GraphicsDevice.Clear(Color.MediumAquamarine);
            m.Draw(batch, GraphicsDevice);
        }

        public override void OnResize()
        {
            m.Resize(new Vector2(Game1.ScreenWidth / 4f, Game1.ScreenHeight / 4f), new Vector2(Game1.HalfScreenWidth, Game1.HalfScreenHeight));
        }

        public override void OnExit()
        {
            client.OnPacket -= OnPacket;
        }

        void OnPacket(NetIncomingMessage msg)
        {
            byte index = msg.ReadByte();
            if (index == MsgIndex.statusUpdate){
                if(msg.ReadByte() == MsgIndex.subIndex.gameStarting)
                    game.SetScene(new ScenarioLoadingScene(client));
            }
            // sanda stuff here
        }
    }
}
