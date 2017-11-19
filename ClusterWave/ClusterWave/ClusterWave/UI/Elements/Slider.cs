using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ClusterWave.UI.Drawables;

namespace ClusterWave.UI.Elements
{
    class Slider
    {
        Vector2 pos, size;
        float minValue, maxValue;
        public float value;
        bool isMouseDown = false;
        DrawSingleColor[] cols = new DrawSingleColor[4];
        public DrawMethod sl;
        public bool enabled = true;

        public Slider(float minValue, float maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.value = minValue;

            //sl = new Button(new DrawWin95(),null, new DrawWin95Over(), new DrawWin95Press());
            sl = new DrawWin95();

            cols[0] = new DrawSingleColor(new Color(255, 255, 255));
            cols[2] = new DrawSingleColor(new Color(224, 224, 224));
            cols[1] = new DrawSingleColor(new Color(128, 128, 128));
            cols[3] = new DrawSingleColor(new Color(0, 0, 0));
        }

        public void Draw(SpriteBatch batch, GraphicsDevice device, Vector2 mousePos)
        {
            for (int i = 0; i < 4; i++)
            {
                cols[i].Draw(batch, device);
            }

            sl.Draw(batch, device);

            if (mousePos.X != Game1.ms.X - pos.X - size.X + 100)
                mousePos.X = new Vector2(Game1.ms.X, Game1.ms.Y).X - pos.X - size.X + 100;
            sl.Resize(new Vector2(pos.X + value / maxValue * size.X - size.X/32, pos.Y - size.X / 16 + 2), new Vector2(size.X / 16, size.X / 8));

        }
        public void Resize(Vector2 newPos, Vector2 newSize)
        {
            pos = newPos;
            size = newSize;
            for (int i = 0; i < 4; i++)
            {
                cols[i].Resize(newPos + (i / 2) * new Vector2(1,1), new Vector2 (newSize.X - i, 4 - i));
            }
            sl.Resize(new Vector2(pos.X + value / maxValue * size.X - size.X / 32, pos.Y - size.X / 16 + 2), new Vector2(size.X / 16, size.X / 8));
        }

        public void Update(Vector2 mousePos)
        {
            if (enabled&&Contains(mousePos))
                sldrFuncton(mousePos);
        }

        public void OnMouseDown(Vector2 mousePos)
        {
            if (Contains(mousePos)&&enabled)
                isMouseDown = true;
            else
                isMouseDown = false;
        }
        public void OnClick(Vector2 mousePos) 
        {
            isMouseDown = false;
        }
        public bool Contains(Vector2 mousePos)
        {
            Vector2 m = mousePos - pos;
            return (m.X > 0 && m.Y > - size.Y/2 && m.X < size.X && m.Y < size.Y/2);
        }
        public void sldrFuncton(Vector2 mousePos)
        {
            if (isMouseDown)
            {
                value = (mousePos.X - pos.X) / size.X * maxValue;
                if (value < minValue)
                    value = minValue;
                else if (value > maxValue)
                    value = maxValue;
            }
        }
    }
}
