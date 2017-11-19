using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Text;

namespace ClusterWave.UI.Elements
{
    class ChatTextBox : Element
    {
        Chat chat;

        public ChatTextBox(Chat chat)
        {
            this.chat = chat;
        }

        public override void Draw(SpriteBatch batch, GraphicsDevice dev)
        {
            batch.End();
            chat.Draw(batch);
            batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Matrix.CreateTranslation(MainMenu.chatPos.X, MainMenu.chatPos.Y, 0));
        }

        public void PreDraw(SpriteBatch batch, GraphicsDevice dev)
        {
            chat.PreDraw(dev, batch);
        }

        public override void Draw(SpriteBatch batch, GraphicsDevice device, Vector2 mousePos)
        {
            Draw(batch, device);
        }

        public override void Resize(Vector2 center, Vector2 size)
        {
            Vector2 bounds = new Vector2(400, 240);

            //chat.SetBounds(new Rectangle((int)center.X, (int)center.Y, (int)size.X, (int)size.Y));
            bounds *= 1.5f;
            chat.SetBounds(new Rectangle((int)center.X + (int)(MainMenu.chatPos.X/2) + 10, (int)center.Y + (int)(MainMenu.chatPos.Y/2) + (int)(size.Y - (size.Y / 1.2f) - (size.Y - (size.Y / 1.2f)) / 2) + 27, (int)(size.X / 1.5f) - 9, (int)(size.Y / 1.2f) - 40));
        }
    }
}
