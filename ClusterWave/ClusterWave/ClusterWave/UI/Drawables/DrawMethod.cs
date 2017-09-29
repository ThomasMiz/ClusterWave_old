using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ClusterWave.UI.Drawables
{
    abstract class DrawMethod
    {

        public abstract void Draw(SpriteBatch batch, GraphicsDevice device);

        public abstract void Resize(Vector2 newPos, Vector2 newSize);
    }
}
