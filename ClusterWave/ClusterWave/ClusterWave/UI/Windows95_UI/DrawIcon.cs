using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ClusterWave.UI.Drawables
{
    class DrawIcon : DrawMethod
    {
        Color col;
        Texture2D tex;
        Vector2 pos;
        float size;
        public float yOffSet = 0;

        public DrawIcon(Texture2D tex, Color col)
        {
           this.tex = tex;
           this.col = col;
        }

        public override void Draw(SpriteBatch batch, GraphicsDevice device)
        {
            //batch.Draw(tex, Vector2.Zero, null, col, 0, Vector2.Zero, size, SpriteEffects.None, 0f);
            //batch.Draw(tex, new Vector2(pos.X, pos.Y - yOffSet), null, Color.White, 0f, new Vector2(tex.Width / 2f, tex.Height / 2f), size / 1f, SpriteEffects.None, 0f);

            batch.Draw(tex, pos, null, col, 0, Vector2.Zero, size, SpriteEffects.None, 0f);

        }

        public override void Resize(Vector2 newPos, Vector2 newSize)
        {

            pos = newPos;
            //size = new Vector2(newSize.X, newSize.X) / 100;
            size = newSize.X / tex.Width;
            
        }

    }
}
