using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ClusterWave.UI.Drawables;

namespace ClusterWave.UI.Elements
{
    delegate void OnButtonClicked(Button sender);
    delegate void OnButtonDown(Button sender);

    class Button
    {
        public Text text;
        public Vector2 pos, size;
        public bool isMouseDown = false, enabled = true;
        bool downModeButton = false, upModeButton = true;


        Color textColr, textContainsColr, textClickColr;
        DrawMethod drawMethod, drawContainsMethod, drawClickMethod;
        /// <summary>
        /// UI Element: Button. Base for drawing all buttons
        /// </summary>
        /// <param name="drawMethod">The method to draw the button when inactive. i.e. DrawWin95</param>
        /// <param name="text">Button text, default none</param>
        /// <param name="drawContainsMethod">The method to draw the button when mouse is over. i.e DrawWin95Over</param>
        /// <param name="drawClickMethod">The method to draw the button when mouse is clicked down. i.e DrawWin95Press</param>
        public Button(DrawMethod drawMethod, Text text = null, DrawMethod drawContainsMethod = null, DrawMethod drawClickMethod = null)
        {
            this.drawMethod = drawMethod;
            if (text == null)
                this.text = new Text(new Drawables.DrawText(Color.Black), "", Vector2.Zero);
            else
            {
                this.text = text;
                textColr = text.drawMethod.col;
                textContainsColr = textColr;
                textClickColr = textColr;
            }

            if (drawContainsMethod == null)
                this.drawContainsMethod = drawMethod;
            else this.drawContainsMethod = drawContainsMethod;

            if (drawClickMethod == null)
                this.drawClickMethod = drawMethod;
            else this.drawClickMethod = drawClickMethod;

        }
        public Button(bool downMode, bool upMode, DrawMethod drawMethod, Text text = null, DrawMethod drawContainsMethod = null, DrawMethod drawClickMethod = null)
        {
            downModeButton = downMode;
            upModeButton = upMode;
            this.drawMethod = drawMethod;
            if (text == null)
                this.text = new Text(new Drawables.DrawText(Color.Black), "", Vector2.Zero);
            else
            {
                this.text = text;
                textColr = text.drawMethod.col;
                textContainsColr = textColr;
                textClickColr = textColr;
            }

            if (drawContainsMethod == null)
                this.drawContainsMethod = drawMethod;
            else this.drawContainsMethod = drawContainsMethod;

            if (drawClickMethod == null)
                this.drawClickMethod = drawMethod;
            else this.drawClickMethod = drawClickMethod;

        }

        public Button(DrawMethod drawMethod, Color contains, Color click, Text text, DrawMethod drawContainsMethod = null, DrawMethod drawClickMethod = null)
        {
            this.drawMethod = drawMethod;
            this.text = text;
            textColr = text.drawMethod.col;
            textContainsColr = contains;
            textClickColr = click;

            if (drawContainsMethod == null)
                this.drawContainsMethod = drawMethod;
            else this.drawContainsMethod = drawContainsMethod;

            if (drawClickMethod == null)
                this.drawClickMethod = drawMethod;
            else this.drawClickMethod = drawClickMethod;

        }
       
        public bool OnClick(Vector2 mousePos)
        {
            if (enabled)
            {
                isMouseDown = false;

                if (Contains(mousePos) && upModeButton)
                {
                    OnClicked(this);
                    return true;
                }
            }

            return false;
        }
        public bool OnDown(Vector2 mousePos)
        {
            if (enabled)
            {
                if (Contains(mousePos))
                {
                    DuringDown(this);
                    return true;
                }
            }
            return false;
        }
        public void OnMouseDown(Vector2 mousePos)
        {
            if (enabled)
            {
                isMouseDown = (Contains(mousePos));
                if (downModeButton && Contains(mousePos))
                {
                    DuringDown(this);
                }
            }
        }
        public bool Contains(Vector2 mousePos)
        {
            Vector2 m = mousePos - pos;
            return (m.X > 0 && m.Y > 0 && m.X < size.X && m.Y < size.Y);
        }

        public void Resize(Vector2 newPos, Vector2 newSize)
        {

            pos = newPos;
            size = newSize;

            drawMethod.Resize(newPos, newSize);
            drawContainsMethod.Resize(newPos, newSize);
            drawClickMethod.Resize(newPos, newSize);

            text.Resize(new Vector2(pos.X + size.X / 2f, pos.Y + size.Y / 2f), newSize);
        }

        public void Draw(SpriteBatch batch, GraphicsDevice device, Vector2 mousePos)
        {

            if (isMouseDown && enabled){
                text.drawMethod.col = textClickColr;
                text.drawMethod.colorChange = true;
                drawClickMethod.Draw(batch, device);
            }
            else if (Contains(mousePos) && enabled){

                text.drawMethod.col = textContainsColr;
                text.drawMethod.colorChange = true;
                drawContainsMethod.Draw(batch, device);
            }

            else
            {
                text.drawMethod.col = textColr;
                text.drawMethod.colorChange = false;
                drawMethod.Draw(batch, device);
            }
            text.Draw(batch, device);
        }

        public event OnButtonClicked OnClicked;
        public event OnButtonDown DuringDown;
    }
}
