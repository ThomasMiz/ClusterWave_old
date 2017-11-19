using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace ClusterWave.UI.Drawables
{
    class DrawChainText : DrawTextMethod
    {

        
        public Vector2 offSet;
        public Color[] ColrArr;
        DrawText[] texts;

        public DrawChainText()
        {
            texts = new DrawText[2];
            texts[0] = new DrawText(Color.Black);
            texts[1] = new DrawText(Color.White);
            offSet = new Vector2(1, 1);
            this.ColrArr = new Color[] { Color.Black, Color.White};
        }
        public DrawChainText(Color[] colsArr, Vector2 pixelOffSet)
        {
            this.ColrArr = colsArr;
            texts = new DrawText[colsArr.Length];
            for (int i = 0; i < texts.Length; i++)
            {
                texts[i] = new DrawText(colsArr[i]);
            }
            //texts[texts.Length - 1] = new DrawText(col);
            offSet = pixelOffSet;

        }

        public override void Draw(SpriteBatch batch, GraphicsDevice device, string text, Vector2 origin)
        {
            if (colorChange)
                texts[texts.Length - 1].col = col;
            else
                texts[texts.Length - 1].col = ColrArr[texts.Length - 1];

            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].Draw(batch, device, text, origin);
            }
           
        }

        public override void Resize(Vector2 newPos, float newScale)
        {
            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].Resize(newPos - i * offSet, newScale);
            }
           
        }
    }
}
