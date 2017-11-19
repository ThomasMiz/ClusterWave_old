using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using ClusterWave.UI.Drawables;

namespace ClusterWave.UI.Elements
{
    class DropDownList
    {
        public String selectedValue;
        public bool enabled = true;
        Vector2 size, pos;
        Button btn;
        bool click;
        public int value;
        DrawWin95TextBox bg = new DrawWin95TextBox();
        Button[] bgSelected;
        List<Button> buttons = new List<Button>(10);
        public String[] contents;
        SpriteFont font;
        DrawMethod drawMethod;

        /// <summary>
        /// UI Element: DropDownList (stored as strings), current selected element variable is named selectedValue
        /// </summary>
        /// <param name="drawMethod"> The drawing method of the list. i.e. DrawWin95</param>
        /// <param name="strs">text for diffrenet values</param>
        /// <param name="font">font to display values</param>
        /// <param name="menu">this</param>
        /// <param name="i">initial value, default 0</param>
        public DropDownList(DrawMethod drawMethod, String[] strs, SpriteFont font, int i = 0)
        {
            
            this.drawMethod = drawMethod;
            contents = strs;
            this.font = font;
            btn = new Button(new DrawWin95(), new Text(new DrawText(Color.Black), "▾", -5), null, new DrawWin95Press());
            
            btn.OnClicked += dropDownBtnClick;
            value = i;
            selectedValue = contents[value];

            bgSelected = new Button[contents.Length];
            for(int e = 0; e < bgSelected.Length; e++)
            {
                bgSelected[e] = new Button(new DrawSingleColor(Color.White), Color.White, Color.White, new Text(new DrawText(Color.Black), contents[e], -5, 0), new DrawSingleColor(new Color(0, 0, 127)),new DrawSingleColor(Color.LightBlue));
                buttons.Add(bgSelected[e]);
                bgSelected[e].OnClicked += dropDownSelect;
            }
            
            buttons.Add(btn);
        }
        public void Draw(SpriteBatch batch, GraphicsDevice device, Vector2 mousePos)
        {
            drawMethod.Draw(batch, device);
            
            if (click)
            {
                bg.Draw(batch, device);
                bg.Resize(pos, new Vector2(size.X, size.Y - 1 + (size.Y + 1) * contents.Length));

                for (int i = 0; i < contents.Length; i++)
                {
                    bgSelected[i].Resize(new Vector2(pos.X + 2, size.Y + pos.Y + i * size.Y + 2), new Vector2(size.X - 4, size.Y + 1));
                    bgSelected[i].Draw(batch, device, mousePos);
                    //batch.DrawString(font, contents[i], new Vector2(pos.X, size.Y + pos.Y + i * size.Y), Color.White, 0f, new Vector2(-2, 0), Math.Min(size.X / font.MeasureString(contents[i]).X, size.Y / font.MeasureString(contents[i]).Y), SpriteEffects.None, 1f);
                }
            }

            batch.DrawString(font, contents[value], pos, Color.Black, 0f, new Vector2(-2, 0), Math.Min(size.X / font.MeasureString(contents[value]).X, size.Y / font.MeasureString(contents[value]).Y), SpriteEffects.None, 0f);
            btn.Draw(batch, device, mousePos);
            btn.Resize(pos + new Vector2(size.X - 3 + 16 - 11 - size.Y, 2), new Vector2(size.Y - 4, size.Y - 4));
           
        }
        public void Resize(Vector2 newPos, Vector2 newSize)
        {
            pos = newPos;
            size = newSize;

            drawMethod.Resize(pos, size);
            bg.Resize(pos, new Vector2(size.X, size.Y + size.Y * contents.Length));
            btn.Resize(pos + new Vector2(size.X - 3 + 16 - 11 - size.Y, 2), new Vector2(size.Y - 4, size.Y - 4));
            for (int i = 0; i < contents.Length; i++)
                bgSelected[i].Resize(new Vector2(pos.X + 2, size.Y + pos.Y + i * size.Y + 2), new Vector2(size.X - 4, size.Y - 2));
            
        }
        
        void dropDownBtnClick(Button sender)
        {
            //if (enabled)
            {
                click = !click;
                if (click)
                    sender.text.txt = "▴";
                if (!click)
                    sender.text.txt = "▾";
            }
        }
        void dropDownSelect(Button sender)
        {
            if (click){
                //value = Convert.ToInt16((-sender.size.Y - sender.pos.Y - 2) / -sender.size.Y) - 5;
                value = (int)((pos.Y - sender.pos.Y + sender.size.Y) / -(sender.size.Y - 1));
                selectedValue = contents[value];
            }
        }

        public bool OnClick(Vector2 mousePos)
        {
            foreach (Button btnElement in buttons)
                btnElement.OnClick(mousePos);

            if (!btn.Contains(mousePos))
            {
                if (Contains(mousePos)  && !click)
                {
                    btn.text.txt = "▴";
                    click = true;
                    return true;
                }

                btn.text.txt = "▾";
                click = false;
            }



            return false;

        }
        public void OnMouseDown(Vector2 mousePos)
        {
            foreach (Button btnElement in buttons)
                btnElement.OnMouseDown(mousePos);
        }
        public bool Contains(Vector2 mousePos)
        {
            Vector2 m = mousePos - pos;
            return (m.X > 0 && m.Y > 0 && m.X < size.X && m.Y < size.Y);
        }

    }
}
