using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using ClusterWave.UI.Elements;
using ClusterWave.UI.Drawables;

namespace ClusterWave.UI.MenuManager
{
    class Window
    {
        public Vector2 pos, size, initialSize;
        Vector2 mouseDownPos;
        public WindowDrawMethod d;
        public bool isFocused, isMouseDown, isMoving, exists = false;
        public List<DrawMethod> dm = new List<DrawMethod>(10);
        public List<Button> buttons = new List<Button>(10);
        public List<TextBox> textboxes = new List<TextBox>(10);
        public List<Slider> sliders = new List<Slider>(10);
        public List<DropDownList> droplists = new List<DropDownList>(10);
        public List<Tooltip> tooltips = new List<Tooltip>(10);
        public List<Text> texts = new List<Text>(10);
        public Vector2[,] buttonsResize = new Vector2[10, 10], textboxesResize = new Vector2[10, 10], slidersResize = new Vector2[10, 10], droplistsResize = new Vector2[10, 10], tooltipsResize = new Vector2[10, 10], textsResize = new Vector2[10, 10], dmResize = new Vector2[10, 10];
        public Vector2[,] chatResize = new Vector2[2, 2];
        Menu menu;
        public ChatTextBox chat;

        public Window(String windowTitle, Menu m, Vector2 pos) 
        {
            this.pos = pos;
            exists = true;
            initialSize = size;

            d = new WindowDrawMethod(new Text(new DrawText(Color.White), windowTitle));
            d.t.DuringDown += topDown;
            d.t.OnClicked += topClick;
            d.b.DuringDown += bodyClick;
            d.btn.OnClicked += closeClick;

            menu = m;
            menu.Add(this);

            //buttonsResize = new Vector2[50, 50];
        }
        public void Draw(SpriteBatch batch, GraphicsDevice device, Vector2 mousePos)
        {
            d.Draw(batch, device, mousePos);
            Resize(size);
            foreach (DrawMethod m in dm)
                m.Draw(batch, device);
            foreach (Button b in buttons)
                b.Draw(batch, device, mousePos);
            foreach (TextBox t in textboxes)
                t.Draw(batch, device, mousePos);
            foreach (Slider s in sliders)
                s.Draw(batch, device, mousePos);
            foreach (DropDownList b in droplists)
                b.Draw(batch, device, mousePos);
            foreach (Tooltip t in tooltips)
                t.Draw(batch, device);
            foreach (Text t in texts)
                t.DrawWraped(batch, device, size);

            if (chat != null)
                chat.Draw(batch, device);

        }
        public void Resize(Vector2 newSize)
        {
            //pos = newPos;
            size = newSize;
            d.Resize(pos, size);
            for (int i = 0; i < buttons.Count; i++) 
                buttons[i].Resize(buttonsResize[i, 0] + pos + new Vector2(3, 27), size / buttonsResize[i, 1] - new Vector2(6, 30));
            for (int i = 0; i < textboxes.Count; i++)
                textboxes[i].Resize(textboxesResize[i, 0] + pos + new Vector2(3, 27), size / textboxesResize[i, 1] - new Vector2(6, 30));
            for (int i = 0; i < sliders.Count; i++)
                sliders[i].Resize(slidersResize[i, 0] + pos + new Vector2(3, 27), size / slidersResize[i, 1] - new Vector2(6, 30));
            for (int i = 0; i < texts.Count; i++)
                texts[i].Resize(textsResize[i, 0] + pos + new Vector2(3, 27), size / textsResize[i, 1] - new Vector2(6, 30));
            for (int i = 0; i < droplists.Count; i++)
                droplists[i].Resize(droplistsResize[i, 0] + pos + new Vector2(3, 27), size / droplistsResize[i, 1] - new Vector2(6, 30));
            for (int i = 0; i < tooltips.Count; i++)
                tooltips[i].Resize(tooltipsResize[i, 0] + pos + new Vector2(3, 27), size / tooltipsResize[i, 1] - new Vector2(6, 30));
            for (int i = 0; i < dm.Count; i++)
                dm[i].Resize(dmResize[i, 0] + pos + new Vector2(3, 27), size / dmResize[i, 1] - new Vector2(6, 30));
                //dm[i].Resize(dmResize[i, 0] + pos + new Vector2(3, 27), size / dmResize[i, 1] - new Vector2(6, 30));//basta mizreaaaaasafuhasufahfafa
            if (chat != null)
            {
                chat.Resize(pos, size);
            }


            
        }
        public void Update(Vector2 mousePos)
        {
            if (isMouseDown && menu.windows[0] == this)
            {
                if (!isMoving)
                {
                    mouseDownPos = mousePos - pos;
                    isMoving = true;
                }
                else
                    pos = mousePos - mouseDownPos;

            }

            foreach (Slider sl in sliders)
                sl.Update(mousePos);
            foreach (Tooltip tt in tooltips)
                tt.Update(mousePos);
        }
        public void Add(Button b, Vector2 pos, Vector2 size)
        {
            buttons.Add(b);
            buttonsResize[buttons.IndexOf(b), 0] = pos;
            buttonsResize[buttons.IndexOf(b), 1] = size;
        }
        public void Add(TextBox b, Vector2 pos, Vector2 size)
        {
            textboxes.Add(b);
            textboxesResize[textboxes.IndexOf(b), 0] = pos;
            textboxesResize[textboxes.IndexOf(b), 1] = size;
        }
        public void Add(DropDownList ddl, Vector2 pos, Vector2 size)
        {
            droplists.Add(ddl);
            droplistsResize[droplists.IndexOf(ddl), 0] = pos;
            droplistsResize[droplists.IndexOf(ddl), 1] = size;
        }
        public void Add(Slider b, Vector2 pos, Vector2 size)
        {
            sliders.Add(b);
            slidersResize[sliders.IndexOf(b), 0] = pos;
            slidersResize[sliders.IndexOf(b), 1] = size;
        }
        public void Add(Text b, Vector2 pos, Vector2 size)
        {
            texts.Add(b);
            textsResize[texts.IndexOf(b), 0] = pos;
            textsResize[texts.IndexOf(b), 1] = size;
        }
        public void Add(Tooltip b, Vector2 pos, Vector2 size)
        {
            tooltips.Add(b);
            tooltipsResize[tooltips.IndexOf(b), 0] = pos;
            tooltipsResize[tooltips.IndexOf(b), 1] = size;
        }
        public void Add(DrawMethod m, Vector2 pos, Vector2 size)
        {
            dm.Add(m);
            dmResize[dm.IndexOf(m), 0] = pos;
            dmResize[dm.IndexOf(m), 1] = size;
        }
        public void Enable(bool val)
        {
            foreach (Button b in buttons)
                b.enabled = val;
            foreach (TextBox t in textboxes)
                t.enabled = val;
            foreach (Slider s in sliders)
                s.enabled = val;
        }
        void topDown(Button s)
        {

            bodyClick(s);
            if (menu.windows[0] == this)
            {
                isMoving = false;
                isMouseDown = true;
            }
        }
        void topClick(Button s)
        {
            //isMoving = false;
            //isMouseDown = false;

        }
        void bodyClick(Button s)
        {
            if (!isFocused)
            {
                menu.WindowBringFront(this);
            }

        }
        void closeClick(Button s)
        {
            menu.WindowRemove(this);
        }
        public void OnMouseDown(Vector2 mousePos)
        {

                d.t.OnMouseDown(mousePos);
                d.b.OnMouseDown(mousePos);
                d.btn.OnMouseDown(mousePos);
                if (menu.windows[0] == this)
                {
                    foreach (Button btnElement in buttons)
                        btnElement.OnMouseDown(mousePos);
                    foreach (TextBox txtBxElement in textboxes)
                        txtBxElement.OnMouseDown(mousePos);
                    foreach (Slider slrElement in sliders)
                        slrElement.OnMouseDown(mousePos);
                }
        }
        public void OnClick(Vector2 mousePos)
        {
            if (menu.windows[0] == this)
            {
                d.t.OnClick(mousePos);
                d.b.OnClick(mousePos);
                d.btn.OnClick(mousePos);

                foreach (Button btnElement in buttons)
                    btnElement.OnClick(mousePos);
                foreach (TextBox txtBxElement in textboxes)
                    txtBxElement.OnClick(mousePos);
                foreach (Slider slrElement in sliders)
                    slrElement.OnClick(mousePos);
                foreach (DropDownList listElement in droplists)
                    listElement.OnClick(mousePos);
            }
        }
    }
}
