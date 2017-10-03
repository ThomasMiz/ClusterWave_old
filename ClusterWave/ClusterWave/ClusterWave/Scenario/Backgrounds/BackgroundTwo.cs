using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ClusterWave.Scenario.Backgrounds
{
    class BackgroundTwo : Background
    {
        public static Texture2D shapeTexture;
        public static Effect RayFx, LinesFx, TextureFx;
        public static EffectParameter lightPosParam, scenarioSizeParam, rayTimeParam;
        public static void Load1(ContentManager Content)
        {
            shapeTexture = BackgroundOne.shapeTexture;
            RayFx = BackgroundOne.RayFx;
            LinesFx = BackgroundOne.LinesFx;
            TextureFx = BackgroundOne.TextureFx;
            lightPosParam = RayFx.Parameters["lightPos"];
            scenarioSizeParam = RayFx.Parameters["size"];
            rayTimeParam = RayFx.Parameters["time"];
        }

        DynamicVertexBuffer buffer;

        public BackgroundTwo()
        {
            buffer = new DynamicVertexBuffer(Game1.game.GraphicsDevice, typeof(VertexPositionColor), 420, BufferUsage.WriteOnly);
        }

        public override Texture2D ShapeTexture { get { return shapeTexture; } }
        public override Effect ShapeFillFx { get { return TextureFx; } }
        public override Effect ShapeLineFx { get { return LinesFx; } }
        public override Effect RayLightFx { get { return RayFx; } }
        public override EffectParameter LightPosParameter { get { return lightPosParam; } }
        public override EffectParameter ScenarioSizeParameter { get { return scenarioSizeParam; } }
        public override EffectParameter RayTimeParameter { get { return rayTimeParam; } }

        public override void Update()
        {

        }

        public override void PreDraw(GraphicsDevice device, SpriteBatch batch)
        {

        }

        public override void Draw(GraphicsDevice device, SpriteBatch batch)
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
