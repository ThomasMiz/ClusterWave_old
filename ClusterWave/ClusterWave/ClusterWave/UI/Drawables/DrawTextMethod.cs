using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ClusterWave.UI.Drawables
{
    abstract class DrawTextMethod
    {
        public bool colorChange;
        public Color col;

        public abstract void Draw(SpriteBatch batch, GraphicsDevice device, String text, Vector2 origin);

        public abstract void Resize(Vector2 newPos, float newScale);
    }
}
