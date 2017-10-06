using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using ClusterWave.Network;

namespace ClusterWave
{
    class Game1 : Microsoft.Xna.Framework.Game
    {
        public static Game1 game;
        public static int ScreenWidth, ScreenHeight, HalfScreenWidth, HalfScreenHeight;
        public static float Time = 0, DeltaTime = 0;
        public static KeyboardState ks, oldks;
        public static MouseState ms, oldms;
        public static SpriteFont font;
        public static Random r = new Random(DateTime.Now.Millisecond);
        public static float Random() { return (float)r.NextDouble(); }
        public static float Random(float max) { return (float)r.NextDouble() * max; }
        public static float Random(float min, float max) { return min + (float)r.NextDouble() * (max - min); }
        public static int RandomInt(int max) { return (int)(r.NextDouble() * max); }
        public static bool RandomBool() { return r.NextDouble() > 0.5; }
        public static Color RandomColor()
        {
            return new Color((byte)r.Next(256), (byte)r.Next(256), (byte)r.Next(256));
        }
        public static Texture2D whiteSquare;

        GraphicsDeviceManager graphics;
        public SpriteBatch batch;

        Scenes.Scene scene;
        public Client client;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this) { SynchronizeWithVerticalRetrace = false };
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            game = this;
            Window.ClientSizeChanged += Window_ClientSizeChanged;
            EventInput.Initialize(Window);
            base.Initialize(); //note: LoadContent() is called during Initialize()
        }

        protected override void LoadContent()
        {
            batch = new SpriteBatch(GraphicsDevice);

            Chat.LoadFonts(Content);

            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            InactiveSleepTime = TargetElapsedTime;
            IsFixedTimeStep = true;

            font = Content.Load<SpriteFont>("Font");
            if (whiteSquare != null) whiteSquare.Dispose();
            whiteSquare = new Texture2D(GraphicsDevice, 1, 1);
            whiteSquare.SetData(new Color[] { Color.White });

            Scenes.Windows95.Load(Content);

            Scenes.InGameScene.Load(Content);

            ClusterWave.Scenario.Dynamic.Bullet.Load(Content);
            ClusterWave.Scenario.Backgrounds.Background.Load(Content);
            ClusterWave.Scenario.Dynamic.Shield.Load(Content);
            ClusterWave.Scenario.PlayerController.Load(Content);

            if (client == null)
                client = new Client();
            if (scene == null)
                scene = new Scenes.InGameScene(client);// new Scenes.ScenarioLoadingScene(client);

            Window_ClientSizeChanged(null, null);
        }

        protected override void UnloadContent()
        {
            ClusterWave.Scenario.Dynamic.Shield.DisposeContent();
        }

        protected override void Update(GameTime gameTime)
        {
            oldks = ks;
            oldms = ms;
            ms = Mouse.GetState();
            ks = Keyboard.GetState();
            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Time += DeltaTime;

            scene.Update();
            client.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            scene.Draw();

            //batch.Begin();
            //batch.DrawString(font, (((int)(100.0 / gameTime.ElapsedGameTime.TotalSeconds)) / 100.0).ToString(), Vector2.Zero, Color.Black);
            //batch.End();

            GraphicsDevice.SetVertexBuffer(null);
            base.Draw(gameTime);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            scene.OnExit();
            
            whiteSquare.Dispose();
            ClusterWave.Scenario.Dynamic.Shield.DisposeContent();
        }

        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            if (ScreenWidth != Window.ClientBounds.Width || ScreenHeight != Window.ClientBounds.Height)
            {
                ScreenWidth = Math.Max(Window.ClientBounds.Width, 1);
                ScreenHeight = Math.Max(Window.ClientBounds.Height, 1);
                
                int chatHeight = Math.Min(ScreenHeight * 3 / 4, 900);
                client.chat.SetBounds(new Rectangle(10, ScreenHeight - chatHeight - 20, Math.Min(800, ScreenWidth), chatHeight));

                if (ScreenWidth == 0) ScreenWidth = 1;
                else if (ScreenWidth > 2048) ScreenWidth = 2048;
                if (ScreenHeight == 0) ScreenHeight = 1;
                else if (ScreenHeight > 2048) ScreenHeight = 2048;
                HalfScreenHeight = ScreenHeight / 2;
                HalfScreenWidth = ScreenWidth / 2;

                graphics.PreferredBackBufferWidth = ScreenWidth;
                graphics.PreferredBackBufferHeight = ScreenHeight;
                //graphics.ApplyChanges();

                scene.OnResize();

            }
        }

        public void SetScene(Scenes.Scene s)
        {
            scene.OnExit();
            scene = s;
            s.OnResize();
        }
    }
}
