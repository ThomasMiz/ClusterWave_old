using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ClusterWave.UI.Elements;

namespace ClusterWave.UI.Drawables
{
    class Draw95Window
    {
        Vector2 pos;
        public Text text;
        DrawWin95 b;
        DrawSingleColor t;
        public Button btn;

        public Draw95Window(Text text, Menu menu)
        {
            text.origin = Vector2.Zero;
            text.pixelsIn = 0;
            btn = new Button(new Drawables.DrawWin95(), new Text(new Drawables.DrawText(Color.Black), "X", 40), null, new Drawables.DrawWin95Press());
            b = new DrawWin95();
            t = new DrawSingleColor(new Color(0, 0, 127));
            this.text = text;
            menu.Add(btn);
        }

        public Draw95Window(Text text)
        {
            text.origin = Vector2.Zero;
            text.pixelsIn = 0;
            btn = new Button(new Drawables.DrawWin95(), new Text(new Drawables.DrawText(Color.Black), "X", 40), null, new Drawables.DrawWin95Press());
            b = new DrawWin95();
            t = new DrawSingleColor(new Color(0, 0, 127));
            this.text = text;
        }

        public void Draw(SpriteBatch batch, GraphicsDevice device, Vector2 mousePos)
        {
            b.Draw(batch, device);
            t.Draw(batch, device);
            text.Draw(batch, device);
            btn.Draw(batch, device, mousePos);

        }
        public void Resize(Vector2 newPos, Vector2 newSize)
        {
            pos = newPos;
            b.Resize(newPos, newSize);
            t.Resize(newPos + new Vector2(3,3), new Vector2(newSize.X - 6, 23));
            text.Resize(newPos + new Vector2(5, 4), new Vector2(newSize.X - 6, 23));
            btn.Resize(newPos + new Vector2(newSize.X - 3 - 16 - 2 , 6), new Vector2(16, 16));

            btn.text.Resize(newPos + new Vector2(newSize.X - 3 - 16 - 2 + 8, 6 + 7), new Vector2(newSize.X - 6, 23));
        }

    }
}
