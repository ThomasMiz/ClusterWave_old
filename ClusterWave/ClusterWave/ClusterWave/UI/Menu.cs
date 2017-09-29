using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace ClusterWave.UI
{
    abstract class Menu
    {
        private Vector2 pos, size;
        public Vector2 Pos { get { return pos; } set { pos = value; } }
        public Vector2 Size { get { return size; } set { size = value; Resize(pos, size); } }

        public virtual void Update()
        {
            if (Game1.ms.LeftButton == ButtonState.Pressed && Game1.oldms.LeftButton == ButtonState.Released)
                OnClick(new Vector2(Game1.ms.X - pos.X, Game1.ms.Y - pos.Y));
        }

        public abstract void Draw(SpriteBatch batch, GraphicsDevice device);

        public abstract void OnClick(Vector2 mousePos);

        public virtual void Resize(Vector2 pos, Vector2 size)
        {
            this.pos = pos;
            this.size = size;
        }
    }
}
