using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ClusterWave.UI.Drawables
{
    class DrawWin95 : DrawMethod
    {
        public Vector2 pixelOffSet = new Vector2(1,1);
        public DrawSingleColor[] cols = new DrawSingleColor [5];

        public DrawWin95()
        {
            cols[0] = new DrawSingleColor(new Color(0, 0, 0));
            cols[1] = new DrawSingleColor(new Color(255, 255, 255));
            cols[2] = new DrawSingleColor(new Color(128, 128, 128));
            cols[3] = new DrawSingleColor(new Color(224, 224, 224));
            cols[4] = new DrawSingleColor(new Color(192, 192, 192));
        }
 
        public override void Draw(SpriteBatch batch, GraphicsDevice device)
        {
            for (int i = 0; i < 5; i++)
            {
                cols[i].Draw(batch, device);
            }
        }

        public override void Resize(Vector2 newPos, Vector2 newSize)
        {
            for (int i = 0; i < 5; i++ )
            {
                cols[i].Resize(newPos + (i/2) * pixelOffSet, newSize - i * pixelOffSet);
            }

            /*cols[0].Resize(newPos, newSize);
            cols[1].Resize(newPos, newSize - pixelOffSet);
            cols[2].Resize(newPos + pixelOffSet, newSize - 2 * pixelOffSet);
            cols[3].Resize(newPos + pixelOffSet, newSize - 3 * pixelOffSet);
            cols[4].Resize(newPos + 2 * pixelOffSet, newSize - 4 * pixelOffSet);*/
        }
    }
}
