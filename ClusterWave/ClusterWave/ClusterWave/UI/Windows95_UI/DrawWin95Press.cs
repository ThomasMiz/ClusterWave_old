using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ClusterWave.UI.Drawables
{
    class DrawWin95Press : DrawWin95
    {
        public override void Draw(SpriteBatch batch, GraphicsDevice device)
        {
            for (int i = 0; i < 5; i++)
            {
                cols[i].Draw(batch, device);
            }
        }

        public override void Resize(Vector2 newPos, Vector2 newSize)
        {
            for (int i = 0; i < 5; i += 2)
            {
                cols[i].Resize(newPos + (i / 2) * pixelOffSet, newSize - i * pixelOffSet);
            }
        }
    }
}
