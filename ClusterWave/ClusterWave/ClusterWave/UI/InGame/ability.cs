using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ClusterWave.UI.Drawables;

namespace ClusterWave.UI.InGame
{
    class ability
    {
        float cd, ct, time;
        public bool isCooldown = false, isCasting = false;
        Texture2D tex;
        Rectangle rect;

        DrawChainText text;
        DrawWin95Press drawPress;
        DrawWin95 drawCooldown;
        DrawWin95Over drawIdle;
        /// <summary>
        /// Base class for displaying abilities (dash/shield)
        /// </summary>
        /// <param name="cooldown">Seconds the ability takes to reload</param>
        /// <param name="castTime">Seconds the ability takes to perform</param>
        /// <param name="tex">Texture for display</param>
        public ability(float cooldown, float castTime, Texture2D tex)
        {
            cd = cooldown;
            ct = castTime;
            this.tex = tex;
            time = cd;

            text = new DrawChainText(new Color[]{Color.Black, Color.White}, new Vector2(1,1));
            drawPress = new DrawWin95Press();
            drawCooldown = new DrawWin95();
            drawIdle = new DrawWin95Over();
        }
        public void Draw(SpriteBatch batch, GraphicsDevice device)
        {
            batch.Begin();

            if(isCasting)
            {
                drawPress.Draw(batch, device);
                //batch.Draw(Game1.whiteSquare, rect, Color.Orange);
            }
            else
            {
                if (isCooldown)
                {
                    
                    //batch.Draw(Game1.whiteSquare, new Rectangle(rect.X, rect.Y, rect.Width, (int)(time / cd * 1.0f * rect.Height)), Color.White);
                    drawCooldown.Draw(batch, device);
                    batch.Draw(Game1.whiteSquare, new Rectangle(rect.X, (rect.Y + rect.Height) - (int)(time / cd * 1.0f * rect.Height), rect.Width, (int)(time / cd * 1.0f * rect.Height)), new Color(0, 0, 0, 10));
                    text.Draw(batch, device, String.Format("{0:0.0}", time), Vector2.Zero);
                }
                else
                {
                    drawIdle.Draw(batch, device);
                    batch.Draw(tex, new Rectangle(rect.X, rect.Y, rect.Width, rect.Height), Color.White);
                }

            }



            batch.End();
        }
        public void Resize(Vector2 pos, Vector2 size)
        {
            text.Resize(pos + new Vector2(size.X/5, size.X/4), size.X/25);
            drawPress.Resize(pos, size);
            drawCooldown.Resize(pos, size);
            drawIdle.Resize(pos, size);

            rect.Width = (int)size.X;
            rect.Height = (int)size.Y;
            rect.X = (int)pos.X;
            rect.Y = (int)pos.Y;
        }
        public void Update()
        {
            if (isCasting){
                if (time >= ct){
                    isCasting = false;
                    isCooldown = true;
                    time = 0;
                }
                else
                    time += 1 * Game1.DeltaTime;
            }
            if (isCooldown){
                if (time >= cd){
                    isCooldown = false;
                }
                else
                    time += 1 * Game1.DeltaTime;
            }
        }
        public void Use()
        {
            if (!isCooldown)
            {
                time = 0;
                isCasting = true;
            }

        }
    }
}
