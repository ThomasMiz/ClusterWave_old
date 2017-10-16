using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ClusterWave.Scenario.Backgrounds
{
    class BackgroundThree : Background
    {
        private static Effect RayFx, LinesFx, TextureFx;
        private static Effect BackgroundFx;
        private static Texture2D colors;

        private static EffectParameter lightPosParam, scenarioSizeParam, rayTimeParam, shapeTimeParam, linesTimeParam;
        private static EffectParameter backgroundTimeParam;
        public static void Load1(ContentManager Content)
        {
            RayFx = Content.Load<Effect>("Scenario/Backgrounds/3/ray");
            LinesFx = Content.Load<Effect>("Scenario/Backgrounds/3/lines");
            TextureFx = Content.Load<Effect>("Scenario/Backgrounds/3/texturizer");
            BackgroundFx = Content.Load<Effect>("Scenario/Backgrounds/3/bgFx");

            colors = Content.Load<Texture2D>("Scenario/Backgrounds/3/colors");

            BackgroundFx.Parameters["Proj"].SetValue(Matrix.CreateOrthographicOffCenter(0, 1, 1, 0, 0, 1));
            TextureFx.Parameters["colors"].SetValue(colors);

            lightPosParam = RayFx.Parameters["lightPos"];
            scenarioSizeParam = RayFx.Parameters["size"];

            rayTimeParam = RayFx.Parameters["time"];
            shapeTimeParam = TextureFx.Parameters["time"];
            linesTimeParam = LinesFx.Parameters["time"];
            backgroundTimeParam = BackgroundFx.Parameters["time"];
        }

        int wid, hei;

        VertexBuffer buffer;
        RenderTarget2D target;

        public override Effect ShapeFillFx { get { return TextureFx; } }
        public override Effect ShapeLineFx { get { return LinesFx; } }
        public override Effect RayLightFx { get { return RayFx; } }
        public override EffectParameter LightPosParameter { get { return lightPosParam; } }
        public override EffectParameter ScenarioSizeParameter { get { return scenarioSizeParam; } }
        public override EffectParameter RayTimeParameter { get { return rayTimeParam; } }
        public override EffectParameter ShapeFillTimeParameter { get { return shapeTimeParam; } }
        public override EffectParameter ShapeLinesTimeParameter { get { return linesTimeParam; } }

        public BackgroundThree()
        {
            buffer = new VertexBuffer(Game1.game.GraphicsDevice, typeof(VertexPositionTexture), 4, BufferUsage.WriteOnly);
            buffer.SetData(new VertexPositionTexture[]{
                new VertexPositionTexture(new Vector3(0, 0, 0), new Vector2(0, 0)),
                new VertexPositionTexture(new Vector3(1, 0, 0), new Vector2(1, 0)),
                new VertexPositionTexture(new Vector3(0, 1, 0), new Vector2(0, 1)),
                new VertexPositionTexture(new Vector3(1, 1, 0), new Vector2(1, 1))
            });
        }

        public override void SetTimeParameters(float time)
        {
            RayTimeParameter.SetValue(time);
            ShapeFillTimeParameter.SetValue(time);
            ShapeLinesTimeParameter.SetValue((float)Math.Floor(time * 2));
        }

        public override void Update()
        {

        }

        public override void PreDraw(GraphicsDevice device, SpriteBatch batch)
        {
            device.RasterizerState = RasterizerState.CullNone;
            device.BlendState = BlendState.Opaque;
            device.DepthStencilState = DepthStencilState.None;
            device.SamplerStates[0] = SamplerState.LinearWrap;

            device.SetRenderTarget(target);
            device.SetVertexBuffer(buffer);
            BackgroundFx.Parameters["time"].SetValue(Game1.Time * 0.02f);
            BackgroundFx.CurrentTechnique.Passes[0].Apply();
            device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
        }

        public override void Draw(GraphicsDevice device, SpriteBatch batch)
        {
            batch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);
            batch.Draw(target, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, new Vector2((float)Game1.ScreenWidth / wid, (float)Game1.ScreenHeight / hei), SpriteEffects.None, 0f);
            batch.End();
        }

        public override void Resize()
        {
            wid = Game1.ScreenWidth / 4;
            hei = Game1.ScreenHeight / 4;

            TextureFx.Parameters["size"].SetValue(new Vector2(wid, hei));

            if (target != null)
                target.Dispose();
            target = new RenderTarget2D(Game1.game.GraphicsDevice, wid, hei);
        }

        public override void Dispose()
        {

        }
    }
}
