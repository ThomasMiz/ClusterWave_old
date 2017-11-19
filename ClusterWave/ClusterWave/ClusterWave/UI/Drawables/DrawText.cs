using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace ClusterWave.UI.Drawables
{
    class DrawText : DrawTextMethod
    {
        public Vector2 pos;
        public float scale;

        public DrawText(Color col)
        {
            this.col = col;
        }

        public override void Draw(SpriteBatch batch, GraphicsDevice dev, String txt, Vector2 origin)
        {
            batch.DrawString(Game1.font, txt, pos, col, 0f, origin, scale, SpriteEffects.None, 0f);
        }
        public override void Resize(Vector2 newPos, float newScale)
        {
            pos = newPos;
            scale = newScale / 1.1f;
        }
    }
}
