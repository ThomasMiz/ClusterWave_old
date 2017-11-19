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
            batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Matrix.CreateTranslation(MainMenu.Cancer.X, MainMenu.Cancer.Y, 0));
            
        }

        public override void Draw(SpriteBatch batch, GraphicsDevice device, Vector2 mousePos)
        {
            Draw(batch, device);
        }

        public override void Resize(Vector2 center, Vector2 size)
        {
            chat.SetBounds(new Rectangle((int)center.X, (int)center.Y, (int)size.X, (int)size.Y));
        }
    }
}
