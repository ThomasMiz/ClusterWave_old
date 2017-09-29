using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ClusterWave.UI.Drawables;

namespace ClusterWave.UI
{
    delegate void OnButtonClicked(Button sender);

    class Button
    {
        Vector2 pos, size;
        DrawMethod drawMethod;

        public Button(DrawMethod drawMethod)
        {
            this.drawMethod = drawMethod;
        }

        public bool OnClick(Vector2 mousePos)
        {
            if (Contains(mousePos))
            {
                OnClicked(this);
                return true;
            }
            return false;
        }

        public bool Contains(Vector2 mousePos)
        {
            Vector2 m = mousePos - pos;
            return (m.X > 0 && m.Y > 0 && m.X < size.X && m.Y < size.Y);
        }

        public void Resize(Vector2 newPos, Vector2 newSize)
        {
            drawMethod.Resize(newPos, newSize);
            pos = newPos;
            size = newSize;
        }

        public void Draw(SpriteBatch batch, GraphicsDevice device)
        {
            drawMethod.Draw(batch, device);
        }

        public event OnButtonClicked OnClicked;
    }
}
