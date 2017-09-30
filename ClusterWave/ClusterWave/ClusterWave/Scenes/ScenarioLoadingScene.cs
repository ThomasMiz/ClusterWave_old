using ClusterWave.Network;
using ClusterWave.Particles;
using ClusterWave.Scenario;
using ClusterWave.Scenario.Backgrounds;
using ClusterWave.Scenario.Dynamic;
using FarseerPhysics.DebugView;
using FarseerPhysics.Dynamics;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace ClusterWave.Scenes
{
    class ScenarioLoadingScene : Scene
    {
        Client client;
        Scenario.Scenario scenario;

        public ScenarioLoadingScene(Client client)
        {
            this.client = client;
            client.OnPacket += OnPacket;
            scenario = new Scenario.Scenario();
        }

        public override void Update()
        {
            //enable for client-side stuff testing. This loades "data.map" and just shows that.
            //game.SetScene(new InGameScene(client));
        }

        public override void Draw()
        {
            client.chat.PreDraw(GraphicsDevice, batch);
            GraphicsDevice.SetRenderTarget(null);

            Color back = Stuff.ColorFromHSV(Game1.Time * 0.2f, 1f, 0.2f);
            GraphicsDevice.Clear(back);

            batch.Begin();
            batch.DrawString(Game1.font, "Loading xddd", new Vector2(0, 0), new Color(255 - back.R, 255 - back.G, 255 - back.B, 255));
            batch.End();

            client.chat.Draw(batch);
        }

        public override void OnResize()
        {

        }

        public override void OnExit()
        {
            client.OnPacket -= OnPacket;
        }

        void OnPacket(NetIncomingMessage msg)
        {
            byte index = msg.ReadByte();
            switch (index)
            {
                case MsgIndex.chat:
                    string chatText = msg.ReadString();
                    //CHAT
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
                case MsgIndex.scenarioRecieve:
                    #region ScenarioRecieve
                    byte[] arr = new byte[msg.Data.Length - 1];
                    for (int i = 1; i != msg.Data.Length; i++)
                        arr[i - 1] = msg.Data[i];
                    scenario.CreatePacketArrive(arr);
                    if (scenario.DoneLoading)
                        game.SetScene(new InGameScene(client, scenario));
                    #endregion
                    break;
            }
        }
    }
}
