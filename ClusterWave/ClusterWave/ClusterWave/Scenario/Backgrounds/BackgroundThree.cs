using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ClusterWave.Scenario.Backgrounds
{
    class BackgroundThree : Background
    {
        private static Effect RayFx, LinesFx, TextureFx;
        private static Effect BackgroundFx, InitFx, BlurFx;
        private static Texture2D colors;

        private static EffectParameter lightPosParam, scenarioSizeParam, rayTimeParam, shapeTimeParam, linesTimeParam;
        private static EffectParameter backgroundTimeParam, backgroundPrevParam, blurTexParam;
        public static void Load1(ContentManager Content)
        {
            RayFx = Content.Load<Effect>("Scenario/Backgrounds/3/ray");
            LinesFx = Content.Load<Effect>("Scenario/Backgrounds/3/lines");
            TextureFx = Content.Load<Effect>("Scenario/Backgrounds/3/texturizer");
            BackgroundFx = Content.Load<Effect>("Scenario/Backgrounds/3/bgFx");
            InitFx = Content.Load<Effect>("Scenario/Backgrounds/3/initFx");
            BlurFx = Content.Load<Effect>("Scenario/Backgrounds/3/blur");

            blurTexParam = BlurFx.Parameters["tex"];
            InitFx.Parameters["Proj"].SetValue(Matrix.CreateOrthographicOffCenter(0, 1, 1, 0, 0, 1));
            BlurFx.Parameters["Proj"].SetValue(Matrix.CreateOrthographicOffCenter(0, 1, 1, 0, 0, 1));

            colors = Content.Load<Texture2D>("Scenario/Backgrounds/3/colors");

            BackgroundFx.Parameters["Proj"].SetValue(Matrix.CreateOrthographicOffCenter(0, 1, 1, 0, 0, 1));
            TextureFx.Parameters["colors"].SetValue(colors);

            lightPosParam = RayFx.Parameters["lightPos"];
            scenarioSizeParam = RayFx.Parameters["size"];

            rayTimeParam = RayFx.Parameters["time"];
            shapeTimeParam = TextureFx.Parameters["time"];
            linesTimeParam = LinesFx.Parameters["time"];
            backgroundTimeParam = BackgroundFx.Parameters["time"];
            backgroundPrevParam = BackgroundFx.Parameters["prev"];
        }

        int wid, hei;

        VertexBuffer buffer;
        RenderTarget2D prev, newer;

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
            backgroundTimeParam.SetValue(time * 2f);
        }

        public override void Update()
        {

        }

        public override void PreDraw(GraphicsDevice device, SpriteBatch batch)
        {
            device.RasterizerState = RasterizerState.CullNone;
            device.BlendState = BlendState.Opaque;
            device.DepthStencilState = DepthStencilState.None;

            SimStep(device);
        }

        private void SimStep(GraphicsDevice device)
        {
            device.SamplerStates[0] = SamplerState.PointWrap;

            RenderTarget2D tmp = newer;
            newer = prev;
            prev = tmp;

            device.SetRenderTarget(newer);
            device.SetVertexBuffer(buffer);
            backgroundPrevParam.SetValue(prev);
            BackgroundFx.CurrentTechnique.Passes[0].Apply();
            device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
        }

        public override void Draw(GraphicsDevice device, SpriteBatch batch)
        {
            device.SetVertexBuffer(buffer);
            blurTexParam.SetValue(newer);
            BlurFx.CurrentTechnique.Passes[0].Apply();
            device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);

            //batch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone);
            //batch.Draw(newer, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, new Vector2((float)Game1.ScreenWidth / wid, (float)Game1.ScreenHeight / hei), SpriteEffects.None, 0f);
            //batch.End();
        }

        public override void Resize()
        {
            wid = Game1.ScreenWidth / 2;
            hei = Game1.ScreenHeight / 2;

            Vector2 pix = new Vector2(wid, hei);
            TextureFx.Parameters["size"].SetValue(pix);
            pix.X = 1f / pix.X;
            pix.Y = 1f / pix.Y;
            BackgroundFx.Parameters["pix"].SetValue(pix);
            BlurFx.Parameters["pix"].SetValue(pix);

            if (prev != null)
                prev.Dispose();
            if (newer != null)
                newer.Dispose();

            GraphicsDevice device = Game1.game.GraphicsDevice;
            prev = new RenderTarget2D(device, wid, hei, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
            newer = new RenderTarget2D(device, wid, hei, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);

            device.RasterizerState = RasterizerState.CullNone;
            device.BlendState = BlendState.Opaque;
            device.DepthStencilState = DepthStencilState.None;
            device.SetRenderTarget(newer);
            device.SetVertexBuffer(buffer);
            InitFx.CurrentTechnique.Passes[0].Apply();
            device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);

            device.RasterizerState = RasterizerState.CullNone;
            device.BlendState = BlendState.Opaque;
            device.DepthStencilState = DepthStencilState.None;
            device.SetRenderTarget(newer);
            device.SetVertexBuffer(buffer);
            InitFx.CurrentTechnique.Passes[0].Apply();
            device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            for (int i = 0; i < 100; i++)
                SimStep(device);
        }

        public override void Dispose()
        {
            buffer.Dispose();
            prev.Dispose();
            newer.Dispose();
        }
    }
}
