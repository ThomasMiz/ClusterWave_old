using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ClusterWave.UI
{
    class TestMenu : Menu
    {
        Button btn;
        Drawables.DrawSingleColor d;

        public TestMenu()
        {
            btn = new Button(new Drawables.DrawSingleColor(Color.IndianRed));
            d = new Drawables.DrawSingleColor(Color.MediumOrchid);
            btn.OnClicked += btnClick;
            btn.OnClicked += otherClc;
        }

        public override void Update()
        {
            base.Update();
        }

        void btnClick(Button sender)
        {
            Game1.game.Window.Title = "Clicked at " + Game1.Time;
        }

        void otherClc(Button sender)
        {
            Game1.game.Window.Title += " Lolazo";
        }

        public override void OnClick(Vector2 mousePos)
        {
            btn.OnClick(mousePos);
        }

        public override void Draw(SpriteBatch batch, GraphicsDevice GraphicsDevice)
        {
            batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Matrix.CreateTranslation(Pos.X, Pos.Y, 0));

            d.Draw(batch, GraphicsDevice);

            btn.Draw(batch, GraphicsDevice);

            batch.End();
        }

        public override void Resize(Vector2 pos, Vector2 size)
        {
            base.Resize(pos, size);
            btn.Resize(new Vector2(10, 10), size / 2f);
            d.Resize(Vector2.Zero, size);
        }
    }
}
