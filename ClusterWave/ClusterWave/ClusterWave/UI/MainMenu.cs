using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using ClusterWave.UI.Drawables;
using ClusterWave.UI.Elements;
using ClusterWave.UI.MenuManager;

namespace ClusterWave.UI
{
    class MainMenu : Menu
    {
        public static Vector2 Cancer;

        Rectangle destinationRectangle;
        Button playBtn, optionsBtn, hostBtn, exitBtn, plJoin, plCreate, plGame, exYes, exNo, opButton, opNameGen, opFullScreen;
        TextBox txbPlIP, txbOp;
        DropDownList ddlOp;
        Slider musicOp, soundOp;
        DrawMethod bar;
        Window pl, plIP, wConn, wOpt, wEx, plHost, wLobby, wFail;
        Text plTxt, tConn, tEx, fText;
        Color[] playTextCols, failTextCols;
        List<Player> playerList = new List<Player>(10);
        bool connecting, windowed = true;
        Vector2 bounds;
        string[] firstNames, lastNames;
        int genericPlayerName = 1;

        public MainMenu()
        {
            playBtn = new Button(new DrawIcon(playTex, Color.White), Color.LightBlue, Color.Blue, new Text(new DrawChainText(), "Play.exe", -1, -0.6f), new DrawIcon(playTex, Color.LightBlue), new DrawIcon(playTex, Color.Blue));
            optionsBtn = new Button(new DrawIcon(optionsTex, Color.White), Color.LightBlue, Color.Blue, new Text(new DrawChainText(), "Options", -1, -0.6f), new DrawIcon(optionsTex, Color.LightBlue), new DrawIcon(optionsTex, Color.Blue));
            hostBtn = new Button(new DrawIcon(hostTex, Color.White), Color.LightBlue, Color.Blue, new Text(new DrawChainText(), "Host", -1, -0.6f), new DrawIcon(optionsTex, Color.LightBlue), new DrawIcon(optionsTex, Color.Blue));
            exitBtn = new Button(new DrawIcon(exitTex, Color.White), Color.LightBlue, Color.Blue, new Text(new DrawChainText(), "    Exit    ", -1, -0.6f), new DrawIcon(exitTex, Color.LightBlue), new DrawIcon(exitTex, Color.Blue));
            bar = new DrawWin95();

            Add(playBtn);
            Add(optionsBtn);
            Add(exitBtn);


            playBtn.OnClicked += playClick;
            exitBtn.OnClicked += exitClick;
            optionsBtn.OnClicked += optionsClick;
            #region playWindow
            playTextCols = new Color[10];
            for (int i = 0; i < 5; i++)
            {
                playTextCols[i] = Color.Lerp(new Color(192, 192, 192), Color.Gray, i / 4.9f);
            }
            for (int i = 5; i < 10; i++)
            {
                playTextCols[i] = Color.Lerp(playTextCols[4], Color.Black, (i - 4) / 4.9f);
            }
            playTextCols[9] = Color.Black;

            plTxt = new Text(new DrawChainText(playTextCols, new Vector2(-0.6f,0f)), "Enter the Wave", Vector2.Zero);

            plJoin = new Button(new DrawIcon(gameTex, Color.White), Color.LightBlue, Color.Blue, new Text(new DrawChainText(), "  Join Game  ", -5, -0.6f), new DrawIcon(gameTex, Color.LightBlue), new DrawIcon(gameTex, Color.Blue));
            plCreate = new Button(new DrawIcon(hostTex, Color.White), Color.LightBlue, Color.Blue, new Text(new DrawChainText(), "Create Game", -5, -0.6f), new DrawIcon(hostTex, Color.LightBlue), new DrawIcon(hostTex, Color.Blue));

            plJoin.OnClicked += gameClick;
            plCreate.OnClicked += hostClick;

            #endregion
            #region gameWindow
            plGame = new Button(new DrawWin95(), new Text(new DrawText(Color.Black), "JOIN", -1),new DrawWin95Over(), new DrawWin95Press());
            txbPlIP = new TextBox(15, new DrawWin95TextBox(), new DrawWin95TextBox(), Game1.font);

            plGame.OnClicked += plJoinClick;
            #endregion
            #region optionWindow
            musicOp = new Slider(0, 100);
            musicOp.value = 100;
            soundOp = new Slider(0, 100);
            soundOp.value = 100;
            txbOp = new TextBox(15, new DrawWin95TextBox(), new DrawWin95TextBox(), Game1.font);
            opFullScreen = new Button(new DrawWin95(), new Text(new DrawText(Color.Black), "Fullscreen", 1), new DrawWin95Over(), new DrawWin95Press());
            opNameGen = new Button(new DrawWin95(), new Text(new DrawText(Color.Black), "Random", 1), new DrawWin95Over(), new DrawWin95Press());
            ddlOp = new DropDownList(new DrawWin95TextBox(), new String[] { "800x600", "1280x720", "1600x900", "3200x1800",}, Game1.font);
            opButton = new Button(new DrawWin95(), new Text(new DrawText(Color.Black), "Apply", 25), new DrawWin95Over(), new DrawWin95Press());
            firstNames = new string[] { "McMuffin", "Hacker", "Gl3K", "Lagger", "frannie", "nostalgia", "dude", "username", "The boy", "that", "the", "Melanchonic", "cluster", "synth", "retro", "digital", "Boaty Mc", "Taz Dingo", "ye", "yeeee"};
            lastNames = new string[] { "man", "dude", "XI", "nagger", "capitalist", "communist", "dude", "socialist", "boy", "boi", "", "wave", "xd", "Glick", "Killer49", "boatface", "boiii"};
            opButton.OnClicked += settingsClick;
            opNameGen.OnClicked += randomNameGen;
            opFullScreen.OnClicked += fullscreenClick;
            #endregion
            #region add segal
            /*segalCols = new Color[100];
            for (int i = 0; i <= 49; i++)
                segalCols[i] = Color.Lerp(Color.OrangeRed, Color.Fuchsia, i / 49f);
            for (int i = 50; i < 100; i++)
                segalCols[i] = Color.Lerp(segalCols[49], Color.CornflowerBlue, (i - 49) / 49f);

            segalText = new Text(new DrawChainText(segalCols, new Vector2(0.5f, 1)), "hooOOOOooola gleEEEEEeek",Vector2.Zero, 25);
            Add(segalText);*/
            #endregion

            bounds = new Vector2(400, 240);

            failTextCols = new Color[] {Color.LimeGreen};
            fText = new Text(new DrawChainText(failTextCols, new Vector2(-1,-1)), "CONNECTION FAILED\nYou may have been hacked by the Russia\nIt was the Trump people.", Vector2.Zero, -100);
        }

        void fullscreenClick(Button sender)
        {
            windowed = !windowed;
            if (windowed)
                opFullScreen.text.txt = "Fullscreen";
            else
            {
                opFullScreen.text.txt = "Windowed";
                string[] resolutionValue = ddlOp.selectedValue.Split('x');
                //fullscreenResolution(resolutionValue[0], resolutionValue[1])
            }

        }
        void randomNameGen(Button sender)
        {
            txbOp.text.Clear();
            Random r = new Random();
            txbOp.setText(firstNames[r.Next(firstNames.Length)]);
            txbOp.setText(" " + lastNames[r.Next(lastNames.Length)]);
        }
        void settingsClick(Button sender)
        {
            //game1.MusicVolume(music.OpValue);
            //game1.SoundVolume(sound.OpValue);
            //thisPlayer.name = txbOp.txt;
            //game1.resolution = ddlOp.selectedValue;
        }
        void playClick(Button sender)
        {
            if (pl == null || !pl.exists)
            {
                pl = new Window("PLAY :^^)", this, Vector2.Zero);
                pl.Resize(bounds);
                WindowForward(pl);
                pl.Add(plTxt, Vector2.Zero, new Vector2(1, 1));
                pl.Add(new DrawWin95TextBox(), new Vector2(0, bounds.Y / 4), new Vector2(1, 1.5f));
                pl.Add(plJoin, new Vector2(pl.size.X / 8, pl.size.Y / 3), new Vector2(4, 2));
                pl.Add(plCreate, new Vector2(pl.size.X - hostTex.Width * 2 - pl.size.X / 8, pl.size.Y / 3), new Vector2(4, 2));
            }
            else
                WindowForward(pl);
        }
        void gameClick(Button sender)
        {
            if (plIP == null || !plIP.exists)
            {
                plIP = new Window("Join Game", this, pl.pos + new Vector2(5, 5));
                plIP.Resize(bounds / 2);
                WindowForward(plIP);

                plIP.Add(plGame, new Vector2(plIP.size.X / 4, plIP.size.Y / 2 - 10), new Vector2(2, 2));
                plIP.Add(txbPlIP, new Vector2(plIP.size.X / 8, 10), new Vector2(1.3f, 2.2f));
            }
            else
                WindowForward(plIP);
        }
        void hostClick(Button sender)
        {
            if (plHost == null || !plHost.exists)
            {
                plHost = new Window("Create game", this, Vector2.Zero + new Vector2(5,5));
                plHost.Resize(bounds);
                WindowForward(plHost);
            }
            else
                WindowForward(plHost);

        }
        void exitClick(Button sender)
        {
            if (wEx == null || !wEx.exists)
            {
                wEx = new Window("Don't do it man", this, Vector2.Zero);
                wEx.Resize(bounds / 2);
                WindowForward(wEx);

                tEx = new Text(new DrawText(Color.Black), "Are you sure you want to exit without saving?\nSome nostalgia may be lost", Vector2.Zero, -100);
                exYes = new Button(new DrawWin95(), new Text(new DrawText(Color.Black), "Yes", 10), new DrawWin95Over(), new DrawWin95Press());
                exNo = new Button(new DrawWin95(), new Text(new DrawText(Color.Black), "No", 10), new DrawWin95Over(), new DrawWin95Press());

                wEx.Add(tEx, new Vector2(1, 1), new Vector2(1, 1));
                wEx.Add(exYes, new Vector2(0, wEx.size.Y / 2), new Vector2(2, 2));
                wEx.Add(exNo, new Vector2(wEx.size.X / 2, wEx.size.Y / 2), new Vector2(2, 2));

                exYes.OnClicked += exitClickYes;
                exNo.OnClicked += exitClickNo;
            }
            else
                WindowForward(wEx);
            //Game1.game.Exit();
        }
        void exitClickNo(Button sender)
        {
            WindowRemove(wEx);
        }
        void exitClickYes(Button sender)
        {
            Game1.game.Exit();
        }
        void optionsClick(Button sender)
        {
            if (wOpt == null || !wOpt.exists)
            {
                wOpt = new Window("Options", this, new Vector2(0, -100));
                wOpt.Resize(new Vector2(bounds.X - bounds.X / 10, bounds.Y + bounds.Y / 1.5f));
                wOpt.Add(new Text(new DrawText(Color.Black), " Music ", Vector2.Zero, 5), new Vector2(wOpt.size.X/4, 110), new Vector2(5.5f, 3));
                wOpt.Add(musicOp, new Vector2(wOpt.size.X/4, 100), new Vector2(2, 6));
                wOpt.Add(new Text(new DrawText(Color.Black), "Effects", Vector2.Zero, 5), new Vector2(wOpt.size.X / 4, 60), new Vector2(5.5f, 3));
                wOpt.Add(soundOp, new Vector2(wOpt.size.X / 4, 50), new Vector2(2, 6));

                wOpt.Add(new Text(new DrawText(Color.Black), "Name:", Vector2.Zero, 5), new Vector2(wOpt.size.X / 8, 150), new Vector2(5.5f, 3));
                wOpt.Add(txbOp, new Vector2(wOpt.size.X / 8 + wOpt.size.X / 5.5f, 150), new Vector2(2.5f, 8));
                wOpt.Add(opNameGen, new Vector2(wOpt.size.X / 2 + wOpt.size.X / 5.0f, 150), new Vector2(3.5f, 8));

                wOpt.Add(new Text(new DrawText(Color.Black), "Screen", Vector2.Zero, 5), new Vector2(wOpt.size.X / 8, 200), new Vector2(5.5f, 3));
                wOpt.Add(ddlOp, new Vector2(wOpt.size.X / 8 + wOpt.size.X / 5.5f, 200), new Vector2(2.5f, 8));
                wOpt.Add(opFullScreen, new Vector2(wOpt.size.X / 2 + wOpt.size.X / 5.0f, 200), new Vector2(3.5f, 8));

                wOpt.Add(opButton, new Vector2(wOpt.size.X/8, 300), new Vector2(2, 5));
                WindowForward(wOpt);
            }
            else
                WindowForward(wOpt);

        }
        void plJoinClick(Button sender)
        {
            wConn = new Window(@"C:\Windows\system32\cmd.exe", this, -bounds / 2);
            wConn.Resize(bounds * 2);
            wConn.Add(new DrawSingleColor(Color.Black), Vector2.Zero, new Vector2(1, 1));
            WindowForward(wConn);

            tConn = new Text(new DrawText(Color.White), "Connecting to " + txbPlIP.Text + "...\nRetrying...\nRetrying...\nError 404: Server is unreachable\n\nMaybe Fran has something to do with it... :)\n\nExecuting not_virus.exe from developer: softonic\n\nWINDOWS ERROR: j1i2r0rh2108h1208381u2801h82013\n\nDeleting system32", Vector2.Zero, -10);
            wConn.Add(tConn, new Vector2(1,1), new Vector2(2,2));
            connecting = true;

        }
        void ConnectedWindow()
        {
            wLobby = new Window("Lobby", this, new Vector2(-50,-100));
            wLobby.Resize(bounds * 1.5f);
            wLobby.Add(new DrawWin95TextBox(), new Vector2(5, wLobby.size.Y - (wLobby.size.Y / 1.2f) - (wLobby.size.Y - (wLobby.size.Y / 1.2f))/2), new Vector2(1.5f, 1.2f));
            wLobby.chat = new ChatTextBox(Game1.game.client.chat);
            WindowForward(wLobby);

            for(int i = 1; i <= 9; i++){
                wLobby.Add(new DrawWin95Press(), new Vector2(5 + wLobby.size.X / 1.5f, i * (wLobby.size.Y - (wLobby.size.Y / 1.2f) - (wLobby.size.Y - (wLobby.size.Y / 1.2f)) / 2)), new Vector2(4, 8/1.3f));
                wLobby.Add(new Text(new DrawText(Color.Black), "", new Vector2(-1, 1), -5), new Vector2(5 + wLobby.size.X / 1.5f, i * (wLobby.size.Y - (wLobby.size.Y / 1.2f) - (wLobby.size.Y - (wLobby.size.Y / 1.2f)) / 2)), new Vector2(4, 8 / 1.3f));
            }
        }
        void FailedConnectWindow()
        {
            wFail = new Window("Connection Failed", this, new Vector2(-50, -100));
            wFail.Resize(bounds * 0.5f);
            wFail.Add(new DrawSingleColor(Color.Black), Vector2.Zero, new Vector2(1, 1));
            wFail.Add(fText, Vector2.Zero, new Vector2(1, 1));
            WindowForward(wFail);
        }
        public void ConnectedPlayers(List<Player> p)
        {
            for (int i = 0; i < p.Count; i++)
            {
                wLobby.texts[i].txt = p[i].Name;
            }
        }
        
        public override void Update()
        {
            base.Update();

            if (Game1.ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter) && Game1.oldks.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Enter))
                OnConnectSuccessful();

            /*if(connecting)
            {
                time += 1 * Game1.DeltaTime;
            }

            if(boolFunctionForDudeConnect()){
                WindowRemove(wConn);
                connecting = false;
                time = 0;
                ConnectedWindow(); 
             }
            if (time >= maxWaitTime)
            {
                //FailedConnectWindow();
                WindowRemove(wConn);
                connecting = false;
                time = 0;
                ConnectedWindow();
            }*/
        }

        public void Draw(SpriteBatch batch, GraphicsDevice device)
        {
            mousePos = new Vector2(Game1.ms.X - Pos.X, Game1.ms.Y - Pos.Y);

            batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Matrix.CreateTranslation(Pos.X, Pos.Y,  0));
            batch.Draw(mainBG, destinationRectangle, Color.White);
            bar.Draw(batch, device);
            MenuDraw(batch, device, mousePos);

            batch.End();
        }

        public override void Resize(Vector2 pos, Vector2 size)
        {
            base.Resize(pos, size);
            Cancer = Pos;

            playBtn.Resize(-Pos + new Vector2(0, 10), new Vector2(90, 90));
            optionsBtn.Resize(-Pos + new Vector2(0, 10 + Size.Y / 2), new Vector2(90, 90));
            exitBtn.Resize(-Pos + new Vector2(0, Size.Y + 10), new Vector2(90));
            bar.Resize(-Pos + new Vector2(0, Game1.ScreenHeight - 40), new Vector2(Game1.ScreenWidth, 40));
            destinationRectangle = new Rectangle(-(int)Pos.X, -(int)Pos.Y, Game1.ScreenWidth, Game1.ScreenHeight);
            /*if (pl != null)
                pl.Resize(Size);
            if(plIP != null)
                plIP.Resize(Size / 2);
            if(wConn != null)
                wConn.Resize(Size * 2);
            if(wOpt != null)
                wOpt.Resize(new Vector2(Size.X - Size.X / 10, Size.Y + Size.Y / 20));*/
        }

        public void OnConnectSuccessful()
        {
            WindowRemove(wConn);
            ConnectedWindow();
        }

        public void OnConnectFailed()
        {
            WindowRemove(wConn);
            FailedConnectWindow();
        }
    }

}
