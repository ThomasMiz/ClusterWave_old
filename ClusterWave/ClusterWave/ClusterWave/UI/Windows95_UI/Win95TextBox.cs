using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ClusterWave.UI.Drawables
{
    class DrawWin95TextBox : DrawWin95
    {
        public DrawWin95TextBox()
        {
            cols[4] = cols[1];
            cols[1] = new DrawSingleColor(Color.White);
        }
        public override void Draw(SpriteBatch batch, GraphicsDevice device)
        {
            cols[1].Draw(batch, device);

            cols[2].Draw(batch, device);
            cols[3].Draw(batch, device);
            cols[0].Draw(batch, device);
            cols[4].Draw(batch, device);
            /*for (int i = 0; i < 5; i++)
            {
                cols[i].Draw(batch, device);
            }*/
        }

        public override void Resize(Vector2 newPos, Vector2 newSize)
        {
            cols[1].Resize(newPos, newSize);

            cols[2].Resize(newPos, newSize - pixelOffSet);
            cols[3].Resize(newPos + pixelOffSet, newSize - 2 * pixelOffSet);
            cols[0].Resize(newPos + pixelOffSet, newSize - 3 * pixelOffSet);
            cols[4].Resize(newPos + 2 * pixelOffSet, newSize - 4 * pixelOffSet);
        }
    }
}
