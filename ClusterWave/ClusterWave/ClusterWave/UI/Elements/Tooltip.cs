using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using ClusterWave.UI.Drawables;

namespace ClusterWave.UI.Elements
{
    class Tooltip
    {
        Vector2 pos, size;
        Vector2 mousePos;

        DrawMethod drawMethod;
        Text text;
        Button btn;
        float maxTime;
        float time;

        public Tooltip(DrawMethod drawMethod, Text text, Button btn, float maxTime = 1)
        {
            this.drawMethod = drawMethod;
            this.text = text;
            this.btn = btn;
            this.maxTime = maxTime;

            this.text.origin = new Vector2(-2, 0);

        }

        public void Update(Vector2 mousePos)
        {
            if(time <= maxTime && btn.Contains(mousePos)&&btn.enabled)
                this.mousePos = new Vector2(mousePos.X, mousePos.Y + 21);
            if (btn.Contains(mousePos) && !btn.isMouseDown)
            {
                    time += 1 * Game1.DeltaTime;
            }
            else
                time = 0;

           
        }
        public void Draw(SpriteBatch batch, GraphicsDevice device)
        {
            if (time >= maxTime)
            {

                drawMethod.Resize(mousePos, size);
                
                text.Resize(mousePos, new Vector2(Game1.ScreenWidth, Game1.ScreenHeight));
                drawMethod.Draw(batch, device);
                //text.Draw(batch, device);
                text.DrawWraped(batch, device, size);

                size.Y = Game1.font.MeasureString(text.parseText(text.txt, size.X)).Y * text.scale;
            }
        }
        public void Resize(Vector2 pos, Vector2 size)
        {
            this.pos = pos;
            this.size = size;

            drawMethod.Resize(mousePos, size);
            text.Resize(mousePos, new Vector2(Game1.ScreenWidth, Game1.ScreenHeight));
        }

        
    }
}
