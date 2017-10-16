using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ClusterWave.Scenario.Backgrounds.BackgroundTwoStuff;
using System.Collections.Generic;

namespace ClusterWave.Scenario.Backgrounds
{
    class BackgroundTwo : Background
    {
        private const int CircleVertex = 12;
        public static VertexPositionColor[] circle;
        private static Texture2D noiseTexture;
        private static Effect RayFx, LinesFx, TextureFx;
        private static EffectParameter lightPosParam, scenarioSizeParam, rayTimeParam, shapeTimeParam, linesTimeParam;
        public static void Load1(ContentManager Content)
        {
            noiseTexture = BackgroundOne.Noise128;
            RayFx = Content.Load<Effect>("Scenario/Backgrounds/2/ray");
            LinesFx = Content.Load<Effect>("Scenario/Backgrounds/2/lines");
            TextureFx = Content.Load<Effect>("Scenario/Backgrounds/2/texturizer");

            lightPosParam = RayFx.Parameters["lightPos"];
            scenarioSizeParam = RayFx.Parameters["size"];
            rayTimeParam = RayFx.Parameters["time"];
            shapeTimeParam = TextureFx.Parameters["time"];
            linesTimeParam = LinesFx.Parameters["time"];
        }

        public override Effect ShapeFillFx { get { return TextureFx; } }
        public override Effect ShapeLineFx { get { return LinesFx; } }
        public override Effect RayLightFx { get { return RayFx; } }
        public override EffectParameter LightPosParameter { get { return lightPosParam; } }
        public override EffectParameter ScenarioSizeParameter { get { return scenarioSizeParam; } }
        public override EffectParameter RayTimeParameter { get { return rayTimeParam; } }
        public override EffectParameter ShapeFillTimeParameter { get { return shapeTimeParam; } }
        public override EffectParameter ShapeLinesTimeParameter { get { return linesTimeParam; } }

        BasicEffect fx;
        VertexColorBatch pBatch;
        List<Line> lines;
        LinkedList<Particle> particles;

        public BackgroundTwo()
        {
            pBatch = new VertexColorBatch();
            fx = new BasicEffect(Game1.game.GraphicsDevice);
            fx.VertexColorEnabled = true;
            TextureFx.Parameters["tex"].SetValue(noiseTexture);

            circle = new VertexPositionColor[CircleVertex+1];
            for(int i=0; i<CircleVertex; i++)
            {
                float rot = i * MathHelper.TwoPi / CircleVertex;
                circle[i] = new VertexPositionColor(new Vector3((float)Math.Cos(rot), (float)Math.Sin(rot), 0), Color.White);
            }
            circle[CircleVertex] = circle[0];
            lines = new List<Line>();
            particles = new LinkedList<Particle>();
        }

        public override void Update()
        {

        }

        public override void PreDraw(GraphicsDevice device, SpriteBatch batch)
        {

        }

        public override void Draw(GraphicsDevice device, SpriteBatch batch)
        {
            fx.CurrentTechnique.Passes[0].Apply();

            for (int i = 0; i < lines.Count; i++)
                lines[i].Draw(pBatch);

            pBatch.FlushAllLineFirst(device);

            LinkedListNode<Particle> p = particles.First;
            while (p != null)
            {
                Particle v = p.Value;
                p = p.Next;
                v.Draw(pBatch);
            }

            pBatch.FlushAllLineFirst(device);
        }

        public void AddParticle(Particle p)
        {
            p.node = particles.AddLast(p);
        }

        public override void Resize()
        {
            fx.Projection = Matrix.CreateOrthographicOffCenter(0, Game1.ScreenWidth, Game1.ScreenHeight, 0, 0, 10f);
            lines.Clear();
            lines.Capacity = (Game1.ScreenWidth * Game1.ScreenHeight) / 15000;
            for (int i = 0; i < lines.Capacity; i++)
            {
                lines.Add(new Line(new Vector2(Game1.Random(Game1.ScreenWidth), Game1.Random(Game1.ScreenHeight)), this));
            }
        }

        public override void Dispose()
        {
            pBatch.Dispose();
            fx.Dispose();
            circle = null;
        }
    }
}
