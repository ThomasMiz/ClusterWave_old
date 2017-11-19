using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ClusterWave.UI.Drawables
{
    class DrawSingleColor : DrawMethod
    {
        Texture2D white;
        Color col;
        Vector2 pos, size;

        public DrawSingleColor(Color color)
        {
            white = Game1.whiteSquare;
            col = color;
        }

        public override void Draw(SpriteBatch batch, GraphicsDevice device)
        {
            batch.Draw(white, pos, null, col, 0f, Vector2.Zero, size, SpriteEffects.None, 0f);
        }

        public override void Resize(Vector2 newPos, Vector2 newSize)
        {
            pos = newPos;
            size = newSize;
        }
    }
}
