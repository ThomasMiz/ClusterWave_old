using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ClusterWave.UI.Drawables;

namespace ClusterWave.UI.InGame
{
    class healthBar
    {
        int healthValue, max, barValue;
        Rectangle rect;
        Texture2D[] bars;

        DrawWin95TextBox back;
        DrawWin95Over front;

        public healthBar(int max, int barValue, Texture2D bar)
        {

            this.max = max;
            this.barValue = barValue;
            int barAmount = max / barValue;
            healthValue = max;
            bars = new Texture2D[barAmount];
            for(int i=0; i<barAmount; i++){
                bars[i] = bar;
            }

            back = new DrawWin95TextBox();
            front = new DrawWin95Over();

        }

        public void Draw(SpriteBatch batch, GraphicsDevice device)
        {
            //foreach (Texture2D b in bars){}
            /*BlendState blend = new BlendState();
            blend.ColorBlendFunction = BlendFunction.Add;
            blend.ColorSourceBlend = Blend.DestinationColor;
            blend.AlphaSourceBlend = Blend.DestinationAlpha;
            blend.ColorDestinationBlend = Blend.Zero;*/

            /*batch.Begin(SpriteSortMode.BackToFront, blend); //ADD COLOR DODGE

            batch.Draw(Game1.whiteSquare, new Rectangle(rect.X, rect.Y, bars.Length * rect.Width - (int)Game1.Time, rect.Height), Color.Black);
            for (int i = 0; i < bars.Length; i++)
            {
                batch.Draw(bars[i], new Rectangle(i * rect.Width + rect.X, rect.Y, rect.Width + 10, rect.Height), new Color(0, 255, 0, 50));
            }
            //batch.Draw(Game1.whiteSquare, new Rectangle(rect.X, rect.Y, bars.Length * rect.Width + 10, rect.Height), Color.Green);

            batch.End();
            */

            batch.Begin();

            batch.DrawString(Game1.font, "" + healthValue, new Vector2(rect.X * 1.0f, rect.Y * 1.0f - rect.Height * 1.0f), Color.White, 0, new Vector2(0, rect.Height / 15f), rect.Height / 15f, SpriteEffects.None, 0);
            batch.DrawString(Game1.font, "/", new Vector2(rect.X + Game1.font.MeasureString("" + max).X *0.1f * rect.Width, rect.Y * 1.0f - rect.Height * 1.0f), Color.White, 0, new Vector2(0, rect.Height / 15f), rect.Height / 15f, SpriteEffects.None, 0);
            batch.DrawString(Game1.font, "" + max, new Vector2(rect.X + Game1.font.MeasureString("" + max + 1).X * 0.1f * rect.Width, rect.Y * 1.0f - rect.Height * 1.0f), Color.White, 0, new Vector2(0, rect.Height / 20f), rect.Height / 20f, SpriteEffects.None, 0);

            //batch.Draw(Game1.whiteSquare, new Rectangle(rect.X, rect.Y, bars.Length * rect.Width, rect.Height), new Color(0,0,0,50));
            back.Draw(batch, device);
            front.Resize(new Vector2(rect.X, rect.Y), new Vector2(bars.Length * rect.Width - (int)(((max - healthValue * 1.0f) / max * 1.0f) * (bars.Length * rect.Width)), rect.Height));
            front.Draw(batch, device);
            //batch.Draw(Game1.whiteSquare, new Rectangle(rect.X, rect.Y, bars.Length * rect.Width - (int)(((max - healthValue * 1.0f) / max * 1.0f) * (bars.Length * rect.Width)), rect.Height), Color.White);

            batch.End();
        }

        public void Resize(Vector2 pos, Vector2 size)
        {
            back.Resize(pos, new Vector2(bars.Length * size.X, size.Y));

            rect.Width = (int)size.X;
            rect.Height = (int)size.Y;
            rect.X = (int)pos.X;
            rect.Y = (int)pos.Y;
        }

        public void damage(int d)
        {
            changeValue(-d);
        }

        public void heal(int h) 
        {
            changeValue(h);
        }

        void changeValue(int v)
        {
            if ((healthValue += v) > max)
                healthValue = max;
            else if ((healthValue += v) < 0)
                healthValue = 0;
            else
                healthValue += v;
        }
    }
}
