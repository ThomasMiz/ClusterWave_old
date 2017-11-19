using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ClusterWave.UI.Drawables;

namespace ClusterWave.UI.Elements
{
    class Text
    {
        public Vector2 origin;
        public float scale, pixelsIn;
        public String txt;
        float yOffSet = 0;

        public DrawTextMethod drawMethod;

        public Text(DrawTextMethod drawMethod, String text, Vector2 origin, float pixelsIn = 50, float yOffSet = 0)
        {
            this.yOffSet = yOffSet;
            this.pixelsIn = pixelsIn;
            this.origin = origin;
            this.drawMethod = drawMethod;
            txt = text;
        }

        public Text(DrawTextMethod drawMethod, String text, float pixelsIn = 50, float yOffSet = 0)
        {
            this.yOffSet = yOffSet;
            this.pixelsIn = pixelsIn;
            this.origin = Game1.font.MeasureString(text) / 2f;
            this.drawMethod = drawMethod;
            txt = text;
        }
        public void Draw(SpriteBatch batch, GraphicsDevice dev)
        {
            drawMethod.Draw(batch, dev, txt, origin);
        }
        public void Resize(Vector2 center, Vector2 size)
        {
           // size = Game1.ScreenSize;
            center.Y -= size.X * yOffSet;
            size.X -= pixelsIn;
            size.Y -= pixelsIn;
            scale = Math.Min(size.X / Game1.font.MeasureString(txt).X, size.Y / Game1.font.MeasureString(txt).Y);
            drawMethod.Resize(center, scale);
            
        }
        public void DrawWraped(SpriteBatch batch, GraphicsDevice device, Vector2 bounds)
        {
            drawMethod.Draw(batch, device, parseText(txt, bounds.X), origin);
        }
        public String parseText(String text, float bound)
        {
            String line = String.Empty;
            String returnString = String.Empty;
            String[] wordArray = text.Split(' ');
 
            foreach (String word in wordArray)
            {
                if (Game1.font.MeasureString(line + word).X * scale > bound)
                {
                    returnString = returnString + line + '\n';
                    line = String.Empty;
                }
         
                line = line + word + ' ';
            }

            return returnString + line;
        }

    }
}
