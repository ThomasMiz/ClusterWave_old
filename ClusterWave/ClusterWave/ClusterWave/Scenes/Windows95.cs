using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using ClusterWave.UI;

namespace ClusterWave.Scenes
{
    class Windows95 : Scene
    {
        public static void Load(ContentManager Content)
        {

        }

        TestMenu m;

        public Windows95()
        {
            m = new TestMenu();
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
            
        }
    }
}
