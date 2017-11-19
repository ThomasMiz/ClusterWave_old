using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ClusterWave.UI.Elements
{
    abstract class Element
    {
        public abstract void Draw(SpriteBatch batch, GraphicsDevice device);
        public abstract void Draw(SpriteBatch batch, GraphicsDevice device, Vector2 mousePos);

        public abstract void Resize(Vector2 newPos, Vector2 newSize);
    }
}
