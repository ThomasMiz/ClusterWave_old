using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ClusterWave.UI.Drawables
{
    class DrawWin95Over : DrawWin95
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
            cols[0].Resize(newPos, newSize);
            cols[1].Resize(newPos + pixelOffSet, newSize - 3 * pixelOffSet);
            cols[2].Resize(newPos + 2 * pixelOffSet, newSize - 4 * pixelOffSet);
            cols[3].Resize(newPos + 2 * pixelOffSet, newSize - 5 * pixelOffSet);
            cols[4].Resize(newPos + 3 * pixelOffSet, newSize - 6 * pixelOffSet);

            /*for (int i = 0; i < 5; i++)
            {
                cols[i].Resize(newPos + i * pixelOffSet, newSize - i * 3 * pixelOffSet);
            }*/
        }
    }
}
