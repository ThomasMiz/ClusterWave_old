using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using ClusterWave.UI.Drawables;
using ClusterWave.UI.Elements;
using ClusterWave.UI.MenuManager;


namespace ClusterWave.UI
{
    class TestMenu : Menu
    {
        Vector2 mousePos;
        int textStuff = 0, timer = 0, txIndex = 0;
        string[] shittyTextForTheWindowWhenYouTryToCloseIt = new string[]{
            "Did you just try to close me?","Nope","I am unstopabble","Stop it","Please I have a wife and kids",
        "Just kidding I am a piece of code","Seriously","I will crash the program","I mean it"
        };
        string[] windowTextStory = new string[]{
            "But you would kill all my family! you monster!", "Seriously, stop it. Don't make me stop you", 
            "Don't do something you'll regret doing", "Okay, that's enough! Don't make me mad!", 
            "That's it! I'M ANGRYYYYYYYYYYYY", "WITNESS MY POWERRRRRR!!!!!!!"
        };

        Window w,w1,w2,w3, textifier, WindowText;

        Button win95;
        Button btn,btn1,ico,ico1, txBtn;
        Text txt;
        Drawables.Draw95Window d;
        Text ct, sliderNum, coolText, coolerText, txText;
        TextBox tbx;

        bool open = false;
        bool chainWindowDraw = false;
        Slider sl, txSlR, txSlG, txSlB, txVal;
        Draw95Window[] windows;
        DropDownList ddl, txDdl;
        Tooltip tt;
        Color[] coolCols, coolerCols;

        public TestMenu()
        {
            //Color.Orchid, Color.OrangeRed, Color.Maroon, Color.LimeGreen, Color.MediumVioletRed ,Color.White

            
            tbx = new TextBox(19, new DrawWin95TextBox(), new DrawWin95TextBox(), Game1.font);
            btn = new Button(new Drawables.DrawWin95(), new Text(new Drawables.DrawText(Color.Black), "I'm a Windows Button"), new Drawables.DrawWin95Over(), new Drawables.DrawWin95Press());
            btn1 = new Button(new Drawables.DrawWin95(), new Text(new Drawables.DrawText(Color.Black), "yes",-5), new Drawables.DrawWin95Over(), new Drawables.DrawWin95Press());
            ico = new Button(new Drawables.DrawIcon(Game1.game.Content.Load<Texture2D>("Desktop"), Color.White), Color.LightBlue, Color.Blue, new Text(new Drawables.DrawChainText(), "Clusterwave.exe", -50, -0.6f), new Drawables.DrawIcon(Game1.game.Content.Load<Texture2D>("Desktop"), Color.LightBlue), new Drawables.DrawIcon(Game1.game.Content.Load<Texture2D>("Desktop"), Color.Blue));

            ico1 = new Button(new Drawables.DrawIcon(Game1.game.Content.Load<Texture2D>("Desktop"), Color.White), Color.LightBlue, Color.Blue, new Text(new Drawables.DrawChainText(), "textifier_Ver1.hex", -50, -0.6f), new Drawables.DrawIcon(Game1.game.Content.Load<Texture2D>("Desktop"), Color.LightBlue), new Drawables.DrawIcon(Game1.game.Content.Load<Texture2D>("Desktop"), Color.Blue));

            win95 = new Button(new Drawables.DrawWin95(), new Text(new Drawables.DrawText(Color.Black),"I'm a Windows Button"),new Drawables.DrawWin95Over(), new Drawables.DrawWin95Press());

            txt = new Text(new Drawables.DrawText(Color.Black),"Are you sure you want to close this window?", Vector2.Zero, -100);
            d = new Drawables.Draw95Window(new Text(new Drawables.DrawText(Color.White),"This is a window title"),this);

            //ct = new Drawables.DrawChainText("CHAIN TEXT", new Color[] {Color.Black, Color.White}, new Vector2(1, 1), 25f);
            //ct = new Text(new Drawables.DrawChainText(new Color[] { Color.Black, Color.White }, new Vector2(1, 1)), "This is CHAIN TEXT");
            //ct = new Text(new Drawables.DrawChainText(new Color[] { Color.Orchid, Color.OrangeRed, Color.Maroon, Color.LimeGreen, Color.MediumVioletRed, Color.White }, new Vector2(2, 5)), "Feel the Aesthetic");
            sl = new Slider(0, 100);
            sliderNum = new Text(new DrawText(Color.Black), Convert.ToString(sl.value), -1);
            ddl = new DropDownList(new DrawWin95TextBox(), new String[] { "banana", "manzana", "durazno", "Zapallo", "Pera", "Frutilla"}, Game1.font);
            //new Color[] {Color.White, Color.Orchid, Color.OrangeRed, Color.Maroon, Color.LimeGreen, Color.MediumVioletRed, Color.PaleVioletRed, Color.Magenta 
            tt = new Tooltip(new DrawSingleColor(Color.LightGoldenrodYellow), new Text(new DrawText(Color.Black), "This is some tooltip text for showing detailed information on the selected item :)"), ico);
            
            
            btn.OnClicked += btnClick;

            btn1.OnClicked += otherClc;
            d.btn.OnClicked += Title;
            win95.OnClicked += btnClick;
            ico.OnClicked += iconClick;
            //sl.sl.OnClicked += sl.sliderFunction;
            ico1.OnClicked += ico1Clc;

            if (chainWindowDraw)
            {
                for (int i = 0; i < windows.Length; i++)
                {
                    windows[i].btn.OnClicked += remove;
                }
            }

            Add(ico);
            Add(ico1);
            //Add(tbx);
            //Add(win95);
            //Add(sl);
            //Add(ddl);
            //Add(tt);

            w = new Window("a", this, new Vector2(40, 80));
            w1 = new Window("b", this, Vector2.Zero);

            

            //w1.Add(ico, Vector2.Zero, new Vector2(2, 2));
            w.Add(win95, Vector2.Zero, new Vector2(1,1));
            

            coolCols = new Color[100];
            for (int i = 0; i < 50; i++)
            {
                coolCols[i] = Color.Lerp(Color.OrangeRed, Color.Fuchsia, i / 49f);
            }
            for (int i = 50; i < 99; i++)
            {
                coolCols[i] = Color.Lerp(coolCols[49], Color.CornflowerBlue, (i-49) / 49f);
            }
            coolCols[99] = Color.White;
            coolText = new Text(new DrawChainText(coolCols, new Vector2(1, 1)), "cool text");

            coolerCols = new Color[100];
            for (int i = 0; i < 16; i++)
            {
                coolerCols[i] = Color.Lerp(Color.Red, Color.Orange, i / 15f);
            }
            for (int i = 16; i < 32; i++)
            {
                coolerCols[i] = Color.Lerp(coolerCols[15], Color.Yellow, (i-15)/15f);
            }
            for (int i = 32; i < 48; i++)
            {
                coolerCols[i] = Color.Lerp(coolerCols[31], Color.Lime, (i-31)/15f);
            }
            for (int i = 48; i < 74; i++)
            {
                coolerCols[i] = Color.Lerp(coolerCols[47], Color.CornflowerBlue,(i-47)/15f);
            }
            for (int i = 74; i < 99; i++)
            {
                coolerCols[i] = Color.Lerp(coolerCols[73], Color.Fuchsia,(i-73)/15f);
            }
            coolerCols[99] = Color.White;

            coolerText = new Text(new DrawChainText(coolerCols, new Vector2(-0.8f, 0.5f)), "nice text");

            Add(coolText);
            Add(coolerText);




        }

        public override void Update()
        {
            timer = 0;
            base.Update();
        }

        void ico1Clc(Button sender)
        {
            textifier = new Window("Textifier", this, Vector2.Zero);
            textifier.Resize(Size);
            WindowBringFront(textifier);

            txVal = new Slider(0, 5);
            textifier.Add(txVal, new Vector2(Size.X / 32, 40), new Vector2(2, 2));

            txBtn = new Button(new Drawables.DrawWin95(), new Text(new Drawables.DrawText(Color.Black), "OK", -5), new Drawables.DrawWin95Over(), new Drawables.DrawWin95Press());
            txBtn.OnClicked += txBtnClc;
            textifier.Add(txBtn, new Vector2(Size.X / 32, 60), new Vector2(3, 4.5f));

            //textifier.Add(txDdl, new Vector2(Size.X / 32, 70), new Vector2(2, 4.5f));
            
        }
        void txBtnClc(Button sender)
        {
            if (txIndex == 0)
            {
                textifier.buttonsResize[0, 0] = new Vector2(Size.X/ 1.5f, 60);
                textifier.slidersResize[0, 1] = Vector2.Zero;
                textifier.Add(tbx, new Vector2(Size.X / 32, 40), new Vector2(2, 4.5f));
                String[] str = new String[(int)textifier.sliders[0].value + 1];
                for (int i = 0; i < str.Length; i++)
                {
                    str[i] = "" + i;
                }
                txDdl = new DropDownList(new DrawWin95TextBox(), str, Game1.font);
                textifier.Add(txDdl, new Vector2(Size.X / 32, 70), new Vector2(2, 4.5f));

                txSlR = new Slider(0, 255);
                txSlG = new Slider(0, 255);
                txSlB = new Slider(0, 255);
                textifier.Add(txSlR, new Vector2(Size.X / 32, 110), new Vector2(2, 2));
                textifier.Add(txSlG, new Vector2(Size.X / 32, 130), new Vector2(2, 2));
                textifier.Add(txSlB, new Vector2(Size.X / 32, 150), new Vector2(2, 2));
                txIndex++;
            }
            if (txIndex > 1)
            {
                Color[] cols = new Color[100];
                for (int i = 0; i < 100; i++)
                {
                    cols[i] = Color.Lerp(new Color((int)textifier.sliders[1].value, (int)textifier.sliders[2].value, (int)textifier.sliders[3].value), Color.LightGoldenrodYellow, i / 100f);
                }
                cols[99] = Color.Black;

                txText = new Text(new DrawChainText(cols, new Vector2(1, 1)), textifier.textboxes[0].Text, Vector2.Zero);

                WindowText = new Window("Your textified text", this, new Vector2(10, 10));
                WindowText.Resize(Size);
                WindowForward(WindowText);
                WindowBringFront(WindowText);

                WindowText.Add(txText, Size / 2, new Vector2(2, 2));
            }
            else
                txIndex++;
        }

        void btnClick(Button sender)
        {
            Window[] w3 = new Window[(int)w2.sliders[0].value];
            mousePos = Vector2.Zero;
            for (int i = 0; i < w3.Length; i++)
            {
                w3[i] = new Window(w2.droplists[0].selectedValue, this, new Vector2(5 * i, 5 * i) + w2.pos);
                w3[i].Resize(new Vector2(200, 100));
                WindowForward(w3[i]);
                w3[i].Enable(true);
                w3[i].Add(btn1, w3[i].size/2,new Vector2(2,2));
                w3[i].Add(txt, Vector2.Zero, new Vector2(1,1));
            }
            /*if ((int)w2.sliders[0].value > 0)
            {
                Color[] cols = new Color[(int)w2.sliders[0].value];
                for (int i = 0; i < cols.Length; i++)
                    cols[i] = Color.Lerp(Color.Red, Color.Plum, (i / (float)cols.Length));
                ct = new Text(new Drawables.DrawChainText(cols, new Vector2(1, 1)), w2.droplists[0].selectedValue);
                w2.Add(ct, Vector2.Zero, new Vector2(1, 1));
            }*/
            /*if (chainWindowDraw || sl.value == 0)
            {
                sl.value = 0;
                chainWindowDraw = false;
                
            }
            else
            {
                chainWindowDraw = true;
            }
            //Game1.game.Window.Title = "Clicked at " + Game1.Time + " " + tbx.Text;
            windows = new Draw95Window[(int)sl.value];
            for (int i = 0; i < windows.Length; i++)
            {
                windows[i] = new Draw95Window(new Text(new DrawText(Color.White), ddl.selectedValue));

            }

            chainWindowResize(base.Pos, base.Size);*/

        }

        void remove(Button sender)
        {
            sl.value = 0;
            btnClick(sender);
        }

        void Title(Button sender)
        {
            
            d.text.txt = shittyTextForTheWindowWhenYouTryToCloseIt[textStuff];
            textStuff++;
        }

        void otherClc(Button sender)
        {
            if (timer == 0)
            {
                if (textStuff < windowTextStory.Length)
                {
                    txt.txt = windowTextStory[textStuff];
                    textStuff++;
                }
                else
                {
                    Random r = new Random();
                    
                    btn1.text.txt = "no";
                    Window[] w3 = new Window[100];
                    mousePos = Vector2.Zero;
                    for (int i = 0; i < w3.Length; i++)
                    {
                        w3[i] = new Window(w2.droplists[0].selectedValue, this, new Vector2(r.Next(-100,(int)Size.X), r.Next(-100,(int)Size.Y)));
                        w3[i].Resize(new Vector2(200, 100));
                        WindowForward(w3[i]);
                        w3[i].Enable(true);
                        w3[i].Add(btn1, w3[i].size / 2, new Vector2(2, 2));
                        w3[i].Add(txt, Vector2.Zero, new Vector2(1, 1));
                    }
                }
                timer = 1;
            }

        }
        void iconClick(Button sender)
        {
            w2 = new Window("c", this, new Vector2(-50, 40));
            WindowBringFront(w2);
            w2.Add(sl, new Vector2(Size.X/4,40), new Vector2(2,2));
            w2.Add(ddl, new Vector2(Size.X / 4, 60), new Vector2(2, 4.5f));
            w2.Add(btn, new Vector2(Size.X / 4, 90), new Vector2(2, 2));
            w2.Resize(Size);

        }
        public void Draw(SpriteBatch batch, GraphicsDevice GraphicsDevice)
        {
           
            sliderNum.txt = Convert.ToString(sl.value);
            mousePos = new Vector2(Game1.ms.X - Pos.X, Game1.ms.Y - Pos.Y);

            batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Matrix.CreateTranslation(Pos.X, Pos.Y, 0));

            //btn.Draw(batch, GraphicsDevice, new Vector2(Game1.ms.X - Pos.X, Game1.ms.Y-Pos.Y));
           // btn1.Draw(batch, GraphicsDevice, new Vector2(Game1.ms.X-Pos.X, Game1.ms.Y-Pos.Y));
            //txt.Draw(batch, GraphicsDevice);
            /*coolText.Draw(batch, GraphicsDevice);
            coolerText.Draw(batch, GraphicsDevice);*/
            MenuDraw(batch, GraphicsDevice, mousePos);
            
            /*
                                                  * ct.Draw(batch, GraphicsDevice);
            if (open)
            {
                d.Draw(batch, GraphicsDevice, mousePos);
                win95.Draw(batch, GraphicsDevice, mousePos);
                tbx.Draw(batch, GraphicsDevice, mousePos);
                sl.Draw(batch, GraphicsDevice, mousePos);
                sliderNum.Draw(batch, GraphicsDevice);
                ddl.Draw(batch, GraphicsDevice, mousePos);
                
            }
            if (chainWindowDraw)
            {
                for (int i = 0; i < windows.Length; i++)
                {
                    windows[i].Draw(batch, GraphicsDevice, mousePos);
                }
            }
            

            //ico1.Draw(batch, GraphicsDevice, mousePos);
            ico.Draw(batch, GraphicsDevice, mousePos);


            tt.Draw(batch, GraphicsDevice);*/
            batch.End();
        }

        public override void Resize(Vector2 pos, Vector2 size)
        {
            base.Resize(pos, size);
            w.Resize(new Vector2(300,300));
            w1.Resize(new Vector2(300,500));
            if(w2 != null)
                w2.Resize(size);

            if(w3!=null)
                w3.Resize(size);

            if (textifier != null)
                textifier.Resize(size);

            if (WindowText != null)
                WindowText.Resize(size);
            coolText.Resize(new Vector2(Game1.HalfScreenWidth, Game1.HalfScreenHeight), size);
            coolerText.Resize(Vector2.Zero, size);
            /*ddl.Resize(new Vector2(size.X / 4f, -size.Y / 4f), new Vector2(size.X / 2f, size.Y / 10));
            sl.Resize(new Vector2(size.X / 4f, size.Y / 6f), new Vector2(size.X / 2f, size.Y / 10));
            tbx.Resize(new Vector2(size.X/4f, size.Y/4f), new Vector2(size.X/2f, size.Y/10));*/
            //txt.Resize(new Vector2(size.X/2f, -Game1.font.MeasureString(txt.txt).Y), size / 2f);
            //sliderNum.Resize(new Vector2(size.X / 8f, size.Y / 4f), new Vector2(size.X / 10f, size.Y / 10));
            //txt.Resize(new Vector2(size.X/2f, -Game1.font.MeasureString(txt.txt).Y), size / 2f);
            //btn1.Resize(new Vector2(size.X / 4f, size.Y / 2f + 15), size / 2f);
            //btn.Resize(new Vector2(size.X / 4f, 10), size / 2f);
            //d.Resize(Vector2.Zero, new Vector2(size.X,size.Y + 20));
            ico.Resize(new Vector2(-size.X * 0.4f, size.Y * 0.3f), new Vector2(size.X,size.X) / 4f);
            ico1.Resize(new Vector2(-size.X * 0.4f, size.Y * 0.9f), new Vector2(size.X, size.X) / 4f);
            //ico1.Resize(new Vector2(size.X, -100), new Vector2(size.X,size.X) / 4f);

            //win95.Resize(new Vector2(size.X / 4f, size.Y/2), size / 2f);
            //ct.Resize(new Vector2(pos.X/2,-30), size);
            //tt.Resize(pos, new Vector2(size.X/2, size.Y/2));

            if (chainWindowDraw)
            {
                chainWindowResize(pos, size);
            }


        }
        private void chainWindowResize(Vector2 pos, Vector2 size)
        {
            for (int i = 0; i < windows.Length; i++)
            {
                windows[i].Resize(new Vector2(i * 5, i * 5), new Vector2(size.X / 2, size.Y / 4));
            }
        }

        //Class window < UI element
    }
}
