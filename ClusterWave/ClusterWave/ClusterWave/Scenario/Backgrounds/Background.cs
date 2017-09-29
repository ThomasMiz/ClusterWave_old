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
        }

        public abstract Texture2D ShapeTexture { get; }
        public abstract Effect ShapeFillFx { get; }
        public abstract Effect ShapeLineFx { get; }
        public abstract Effect RayLightFx { get; }

        public abstract void Update();
        public abstract void Draw(SpriteBatch batch, GraphicsDevice device);
        public abstract void Resize();
        public virtual void Dispose() { }
    }
}
