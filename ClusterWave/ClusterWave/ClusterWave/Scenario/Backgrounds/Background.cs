using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ClusterWave.Scenario.Backgrounds
{
    abstract class Background : IDisposable
    {
        public static void Load(ContentManager Content)
        {
            BackgroundOne.Load1(Content);
            BackgroundTwo.Load1(Content);
        }

        public abstract Effect ShapeFillFx { get; }
        public abstract Effect ShapeLineFx { get; }
        public abstract Effect RayLightFx { get; }
        public abstract EffectParameter LightPosParameter { get; }
        public abstract EffectParameter ScenarioSizeParameter { get; }
        public abstract EffectParameter RayTimeParameter { get; }
        public abstract EffectParameter ShapeFillTimeParameter { get; }
        public abstract EffectParameter ShapeLinesTimeParameter { get; }
        public virtual Color ClearColor { get { return Color.Black; } }

        public void SetTimeParameters(float time)
        {
            RayTimeParameter.SetValue(time);
            ShapeFillTimeParameter.SetValue(time);
            ShapeLinesTimeParameter.SetValue(time);
        }

        public abstract void Update();
        public abstract void Draw(GraphicsDevice device, SpriteBatch batch);
        public abstract void PreDraw(GraphicsDevice device, SpriteBatch batch);
        public abstract void Resize();
        public virtual void Dispose() { }
    }
}
