using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using ClusterWave.UI.Drawables;

namespace ClusterWave.UI.Elements
{
    delegate void OnTextBoxDone(TextBox sender);

    class TextBox
    {
        Vector2 pos, size;

        int maxLength;
        bool isTyping;
        public StringBuilder text;
        DrawMethod drawMethod, drawWritingMethod;

        SpriteFont font;
        float textScale;

        public StringBuilder TextBuilder { get { return text; } }
        public String Text { get { return text.ToString(); } }
        public bool enabled = true;

        public TextBox(int maxLength, DrawMethod drawMethod, DrawMethod drawWritingMethod, SpriteFont font)
        {
            this.maxLength = maxLength;
            this.drawMethod = drawMethod;
            if (drawWritingMethod == null)
                this.drawWritingMethod = drawMethod;
            else
                this.drawWritingMethod = drawWritingMethod;

            this.font = font;

            text = new StringBuilder(maxLength);
            isTyping = false;
        }

        public bool Contains(Vector2 point)
        {
            Vector2 m = point - pos;
            return (m.X > 0 && m.X < size.X) && (m.Y > 0 && m.Y < size.Y);
        }

        public void OnClick(Vector2 mousePos)
        {
            if (!isTyping && enabled)
                if (Contains(mousePos))
                {
                    isTyping = true;
                    EventInput.CharEntered += OnCharEnter;
                }
        }

        public void OnMouseDown(Vector2 mousePos)
        {
            if (!Contains(mousePos) && enabled)
            {
                EndWrite();
            }
        }

        public void EndWrite()
        {
            if (isTyping)
            {
                EventInput.CharEntered -= OnCharEnter;
                //onDone(this);
                isTyping = false;
            }
        }

        public void Draw(SpriteBatch batch, GraphicsDevice device, Vector2 mousePos)
        {
            (Contains(mousePos) ? drawWritingMethod : drawMethod).Draw(batch, device);


            if (isTyping && (int)Game1.Time % 2 != 0 && enabled)
            {
                batch.DrawString(font, "│", new Vector2(pos.X + font.MeasureString(Text).X * textScale - 5 * textScale, pos.Y), Color.Black, 0f, new Vector2(-2, 0), textScale, SpriteEffects.None, 0f);
            }
            batch.DrawString(font, Text, pos, Color.Black, 0f, new Vector2(-2, 0), textScale, SpriteEffects.None, 0f);
        }

        public void Resize(Vector2 pos, Vector2 size)
        {
            this.pos = pos;
            this.size = size;

            drawWritingMethod.Resize(pos, size);
            drawMethod.Resize(pos, size);


            //textScale = size.Y / font.MeasureString("8").Y;
            textScale = Math.Min(size.X / font.MeasureString(Text + ".").X, size.Y / font.MeasureString(Text + ".").Y);
        }

        void OnCharEnter(object sender, CharacterEventArgs args)
        {
            if (enabled)
            {
                char c = args.Character;
                if (c == 13)
                { //enter
                    EndWrite();
                }
                else if (c == 8)
                { //backspace
                    if (text.Length != 0)
                        text.Length--;
                }
                else if (c > 31 && c < 255)
                { //character entered
                    if (text.Length < maxLength)
                        text.Append(c);
                }
            }
        }
        public void setText(string t)
        {
            char[] c = t.ToCharArray();

            for (int i = 0; i < c.Length; i++)
            {
                text.Append(c[i]);
            }

        }
        //public event OnTextBoxDone onDone;
    }
}
