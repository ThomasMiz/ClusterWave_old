using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ClusterWave.Scenario.Backgrounds
{
    class BackgroundOne : Background
    {
        public static Texture2D shapeTexture;
        public static Effect RayFx, LinesFx, TextureFx;
        public static void Load1(ContentManager Content)
        {
            shapeTexture = Content.Load<Texture2D>("Scenario/Backgrounds/1/shape");
            RayFx = Content.Load<Effect>("Scenario/Backgrounds/1/ray");
            LinesFx = Content.Load<Effect>("Scenario/Backgrounds/1/lines");
            TextureFx = Content.Load<Effect>("Scenario/Backgrounds/1/texturizer");
        }

        public BackgroundOne()
        {

        }

        public override Texture2D ShapeTexture { get { return shapeTexture; } }
        public override Effect ShapeFillFx { get { return TextureFx; } }
        public override Effect ShapeLineFx { get { return LinesFx; } }
        public override Effect RayLightFx { get { return RayFx; } }

        public override void Update()
        {
            
        }

        public override void Draw(SpriteBatch batch, GraphicsDevice device)
        {

        }

        public override void Resize()
        {

        }

        public override void Dispose()
        {

        }
    }
}
