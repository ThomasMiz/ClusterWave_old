using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using ClusterWave.UI.Elements;
using ClusterWave.UI.MenuManager;


namespace ClusterWave.UI
{
    abstract class Menu
    {
        private Vector2 pos, size;
        public Vector2 Pos { get { return pos; } set { pos = value; } }
        public Vector2 Size { get { return size; } set { size = value; Resize(pos, size); } }

        public bool isMouseWindowed;

        List<Button> buttons;
        List<TextBox> textboxes;
        List<Slider> sliders;
        List<DropDownList> droplists;
        List<Tooltip> tooltips;
        List<Text> texts;
        public static Texture2D playTex, optionsTex, hostTex, exitTex, mainBG, gameTex, inGameShield, inGameDash;
        public static Texture2D[] inGameWeapons = new Texture2D[1];
        public List<Window> windows;
        //List<List<List<List<Exception>>>> list_of_lists_of_lists_of_lists_lelggxdxdxd = new List<List<List<List<Exception>>>>(5000);

        protected Vector2 mousePos;

        public static void LoadContent(ContentManager content)
        {
            playTex = content.Load<Texture2D>("UI_Content/icon_play");
            optionsTex = content.Load<Texture2D>("UI_Content/icon_options");
            hostTex = content.Load<Texture2D>("UI_Content/icon_host");
            exitTex = content.Load<Texture2D>("UI_Content/icon_exit");
            mainBG = content.Load<Texture2D>("UI_Content/UI_Background");
            gameTex = content.Load<Texture2D>("UI_Content/icon_play");

            inGameShield = content.Load<Texture2D>("UI_Content/Barrier");
            inGameDash = content.Load<Texture2D>("UI_Content/Dash");
            for (int i = 0; i < inGameWeapons.Length; i++) {
                inGameWeapons[i] = content.Load<Texture2D>("UI_Content/Weapon" + i);
            }
        }

        public Menu()
        {
            texts = new List<Text>(100);
            buttons = new List<Button>(100);
            textboxes = new List<TextBox>(100);
            sliders = new List<Slider>(100);
            droplists = new List<DropDownList>(100);
            tooltips = new List<Tooltip>(100);
            windows = new List<Window>(50);
            //windowManager = new WindowManager();
        }

        public virtual void Update()
        {
            mousePos = new Vector2(Game1.ms.X - pos.X, Game1.ms.Y - pos.Y);

            if (Game1.ms.LeftButton == ButtonState.Pressed && Game1.oldms.LeftButton == ButtonState.Released)
                OnMouseDown(mousePos);
            else if (Game1.ms.LeftButton == ButtonState.Released && Game1.oldms.LeftButton == ButtonState.Pressed)
            {
                OnClick(mousePos);
                if (windows.Count > 0)
                    windows[0].isMouseDown = false;
            }


            foreach (Slider sl in sliders)
                sl.Update(mousePos);
            foreach (Tooltip tt in tooltips)
                tt.Update(mousePos);
            foreach (Window w in windows)
                w.Update(mousePos);

            if(windows.Count>0)
                isMouseWindowed = windows[0].d.b.Contains(mousePos);

        }

        public void MenuDraw(SpriteBatch batch, GraphicsDevice device, Vector2 mousePos)
        {
            
            //windowManager.Draw(batch, device, mousePos);

            foreach (Button b in buttons)
                b.Draw(batch, device, mousePos);
            foreach (TextBox t in textboxes)
                t.Draw(batch, device, mousePos);
            foreach (Slider s in sliders)
                s.Draw(batch, device, mousePos);
            foreach (DropDownList d in droplists)
                d.Draw(batch, device, mousePos);
            foreach (Tooltip t in tooltips)
                t.Draw(batch, device);
            foreach (Text t in texts)
                t.Draw(batch, device);

            for (int i = windows.Count - 1; i >= 0; i--)
            {
                if (windows[i] != null)
                    windows[i].Draw(batch, device, mousePos);
            }
        }

        public void OnClick(Vector2 mousePos)
        {
            if (!isMouseWindowed || windows.Count == 0)
            {
                foreach (Button btnElement in buttons)
                    btnElement.OnClick(mousePos);
                foreach (TextBox txtBxElement in textboxes)
                    txtBxElement.OnClick(mousePos);
                foreach (Slider slrElement in sliders)
                    slrElement.OnClick(mousePos);
                foreach (DropDownList listElement in droplists)
                    listElement.OnClick(mousePos);

            }
            for (int i = 0; i < windows.Count; i++)
                windows[i].OnClick(mousePos);
           
        }
        public void OnMouseDown(Vector2 mousePos)
        {
            if (!isMouseWindowed || windows.Count == 0)
            {
                foreach (Button btnElement in buttons)
                    btnElement.OnMouseDown(mousePos);
                foreach (TextBox txtBxElement in textboxes)
                    txtBxElement.OnMouseDown(mousePos);
                foreach (Slider slrElement in sliders)
                    slrElement.OnMouseDown(mousePos);
                foreach (DropDownList d in droplists)
                    d.OnMouseDown(mousePos);

            }
            for (int i = 0; i < windows.Count; i++)
                windows[i].OnMouseDown(mousePos);
        }

        #region ADD REGION
        public void Add(Button b)
        {
            buttons.Add(b);
        }
        protected void Add(TextBox b)
        {
            textboxes.Add(b);
        }
        protected void Add(Slider b)
        {
            sliders.Add(b);
        }
        protected void Add(DropDownList b)
        {
            droplists.Add(b);
        }
        protected void Add(Tooltip b)
        {
            tooltips.Add(b);
        }
        protected void Add(Text t)
        {
            texts.Add(t);
        }
        public void Add(Window w)
        {
            windows.Add(w);
            //WindowBringFront(w);
            /*Add(w.d.b);
            Add(w.d.t);
            Add(w.d.btn);*/
            if (windows.Count == 1)
            {
                w.Enable(true);
            }
        }
        #endregion
        public virtual void Resize(Vector2 pos, Vector2 size)
        {
            this.pos = pos;
            this.size = size;

            foreach (Window w in windows)
            {
                //w.Resize(size);
            }
        }

        public void WindowBringFront(Window w)
        {
            if (!isMouseWindowed && windows[0] != w)
            {
                windows[0].isFocused = false;
                windows[0].Enable(false);

                windows.Remove(w);
                windows.Insert(0, w);
                w.Enable(true);
                w.isFocused = true;

            }
        }
        public void WindowRemove(Window w)
        {
            w.exists = false;
            windows.Remove(w);
            if (windows.Count > 0)
            {
                Window temp = windows[0];
                windows.Remove(windows[0]);
                windows.Insert(0, temp);
                windows[0].isFocused = true;
                windows[0].Enable(true);
            }
        }
        public void WindowForward(Window w)
        {
            windows[0].isFocused = false;
            windows[0].Enable(false);

            windows.Remove(w);
            windows.Insert(0, w);
            w.Enable(true);
            w.isFocused = true;
        }
    }
}
