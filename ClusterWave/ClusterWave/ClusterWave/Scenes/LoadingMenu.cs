using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ClusterWave.Scenes
{
    class LoadingMenu : Scene
    {
        String text;
        Thread thread;

        public LoadingMenu()
        {
            thread = new Thread(new ParameterizedThreadStart(game.LoadAllData));
            //thread.Start(this);
            game.LoadAllData(this);
        }

        public override void Update()
        {
            if (thread.ThreadState != ThreadState.Running)
            {
                game.SetScene(new ScenarioLoadingScene(Game1.game.client));
            }
        }

        public override void Draw()
        {
            batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Matrix.CreateTranslation(0, 0, 0));

            batch.DrawString(Game1.font, text, Vector2.Zero, Color.Red);
            
            batch.End();
        }

        public override void OnResize()
        {
            
        }

        public override void OnExit()
        {
            
        }

        public void SetText(string t)
        {
            text = t;
        }
    }
}
