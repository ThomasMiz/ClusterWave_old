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

        public static Texture2D BackgroundTexture, Noise128;
        public static Effect BackgroundFx;
        private static EffectParameter worldParameter, projParameter, timeParameter, sizeParameter, pointsParameter;
        public static EffectParameter lightPosParam, scenarioSizeParam, rayTimeParam, shapeTimeParam, linesTimeParam;
        public static void Load1(ContentManager Content)
        {
            shapeTexture = Content.Load<Texture2D>("Scenario/Backgrounds/1/shape");
            RayFx = Content.Load<Effect>("Scenario/Backgrounds/1/ray");
            LinesFx = Content.Load<Effect>("Scenario/Backgrounds/1/lines");
            TextureFx = Content.Load<Effect>("Scenario/Backgrounds/1/texturizer");
            Noise128 = Content.Load<Texture2D>("Scenario/Backgrounds/1/noise128");

            lightPosParam = RayFx.Parameters["lightPos"];
            scenarioSizeParam = RayFx.Parameters["size"];

            BackgroundTexture = Content.Load<Texture2D>("Scenario/Backgrounds/1/texture");
            BackgroundFx = Content.Load<Effect>("Scenario/Backgrounds/1/bgFx");
            worldParameter = BackgroundFx.Parameters["World"];
            projParameter = BackgroundFx.Parameters["Proj"];
            timeParameter = BackgroundFx.Parameters["time"];
            sizeParameter = BackgroundFx.Parameters["size"];
            pointsParameter = BackgroundFx.Parameters["points"];
            rayTimeParam = RayFx.Parameters["time"];
            shapeTimeParam = TextureFx.Parameters["time"];
            linesTimeParam = LinesFx.Parameters["time"];

            worldParameter.SetValue(Matrix.Identity);
            projParameter.SetValue(Matrix.CreateOrthographicOffCenter(-1, 1, 1, -1, 0f, 10f));
            BackgroundFx.Parameters["colors"].SetValue(BackgroundTexture);
        }

        VertexBuffer buffer;
        RenderTarget2D target;
        float resolutionMultiply = 1;

        public override Effect ShapeFillFx { get { return TextureFx; } }
        public override Effect ShapeLineFx { get { return LinesFx; } }
        public override Effect RayLightFx { get { return RayFx; } }
        public override EffectParameter LightPosParameter { get { return lightPosParam; } }
        public override EffectParameter ScenarioSizeParameter { get { return scenarioSizeParam; } }
        public override EffectParameter RayTimeParameter { get { return rayTimeParam; } }
        public override EffectParameter ShapeFillTimeParameter { get { return shapeTimeParam; } }
        public override EffectParameter ShapeLinesTimeParameter { get { return linesTimeParam; } }

        public BackgroundOne()
        {
            buffer = new VertexBuffer(Game1.game.GraphicsDevice, typeof(VertexPositionColorTexture), 4, BufferUsage.WriteOnly);
            buffer.SetData(new VertexPositionColorTexture[]{
                new VertexPositionColorTexture(new Vector3(-1, -1, -1), new Color(255, 0, 0), new Vector2(0, 1)),
                new VertexPositionColorTexture(new Vector3(1, -1, -1), new Color(0, 255, 0), new Vector2(1, 1)),
                new VertexPositionColorTexture(new Vector3(-1, 1, -1), new Color(0, 0, 255), new Vector2(0, 0)),
                new VertexPositionColorTexture(new Vector3(1, 1, -1), new Color(255, 255, 255), new Vector2(1, 0)),
            });
            TextureFx.Parameters["tex"].SetValue(shapeTexture);
            RayFx.Parameters["noise"].SetValue(Noise128);
        }

        public override void Update()
        {
            resolutionMultiply = ((float)Math.Sin(Game1.Time) * 0.5f + 0.5f) * 0.6f + 0.2f;
        }

        public override void PreDraw(GraphicsDevice device, SpriteBatch batch)
        {
            device.SetRenderTarget(target);
            device.Clear(Color.White);
            float time = Game1.Time;
            timeParameter.SetValue(time * 0.33f);
            time *= 0.064f; //0.2f;
            /*float hw = Game1.HalfScreenWidth, hh = Game1.HalfScreenHeight;
            Vector2[] values = new Vector2[]{
                new Vector2((float)Math.Sin(time*0.177)*hw+hw, (float)Math.Sin(time*1.92+9.0)*hh+hh),
                new Vector2((float)Math.Sin(time*0.316+1.0)*hw+hw, (float)Math.Sin(time*1.284+5.0)*hh+hh),
                new Vector2((float)Math.Sin(time*0.583+2.0)*hw+hw, (float)Math.Sin(time*0.195+6.0)*hh+hh),
                new Vector2((float)Math.Sin(time*0.815+3.0)*hw+hw, (float)Math.Sin(time*0.553+7.0)*hh+hh),
                new Vector2((float)Math.Sin(time*1.174+4.0)*hw+hw, (float)Math.Sin(time*0.817+8.0)*hh+hh),
            };*/
            Vector2[] values = new Vector2[]{
                new Vector2(Stuff.CheapWave(time*0.177f+0.0f)*Game1.ScreenWidth, Stuff.CheapWave(time*1.92f+9.0f)*Game1.ScreenHeight),
                new Vector2(Stuff.CheapWave(time*0.316f+1.0f)*Game1.ScreenWidth, Stuff.CheapWave(time*1.284f+5.0f)*Game1.ScreenHeight),
                new Vector2(Stuff.CheapWave(time*0.583f+2.0f)*Game1.ScreenWidth, Stuff.CheapWave(time*0.195f+6.0f)*Game1.ScreenHeight),
                new Vector2(Stuff.CheapWave(time*0.815f+3.0f)*Game1.ScreenWidth, Stuff.CheapWave(time*0.553f+7.0f)*Game1.ScreenHeight),
                new Vector2(Stuff.CheapWave(time*1.174f+4.0f)*Game1.ScreenWidth, Stuff.CheapWave(time*0.817f+8.0f)*Game1.ScreenHeight),
            };
            pointsParameter.SetValue(values);
            worldParameter.SetValue(Matrix.CreateTranslation(1, 1, 0) * Matrix.CreateScale(resolutionMultiply) * Matrix.CreateTranslation(-1, -1, 0));

            BackgroundFx.CurrentTechnique.Passes[0].Apply();
            device.SetVertexBuffer(buffer);
            device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
        }

        public override void Draw(GraphicsDevice device, SpriteBatch batch)
        {
            batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone);
            batch.Draw(target, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f/resolutionMultiply, SpriteEffects.None, 0f);
            batch.End();
        }

        public override void Resize()
        {
            sizeParameter.SetValue(new Vector2(Game1.ScreenWidth, Game1.ScreenHeight));
            if (target != null)
                target.Dispose();
            target = new RenderTarget2D(Game1.game.GraphicsDevice, Game1.ScreenWidth, Game1.ScreenHeight, false, SurfaceFormat.Color, DepthFormat.None);
        }

        public override void Dispose()
        {
            buffer.Dispose();
            target.Dispose();
        }
    }
}
