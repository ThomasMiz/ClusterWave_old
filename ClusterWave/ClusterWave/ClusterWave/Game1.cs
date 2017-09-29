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
        public static RenderTarget2D DefaultRenderTarget;
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
            base.Initialize();
        }

        protected override void LoadContent()
        {
            game = this;
            batch = new SpriteBatch(GraphicsDevice);
            Window.ClientSizeChanged += Window_ClientSizeChanged;

            EventInput.Initialize(Window);

            Chat.LoadFonts(Content);
            client = new Client();

            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            InactiveSleepTime = TargetElapsedTime;
            IsFixedTimeStep = true;

            scene = new Scenes.LoadingMenu();

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
            GraphicsDevice.SetRenderTarget(DefaultRenderTarget);
            scene.Draw();

            batch.Begin();
            //batch.DrawString(font, (((int)(100.0 / gameTime.ElapsedGameTime.TotalSeconds)) / 100.0).ToString(), Vector2.Zero, Color.Black);
            batch.End();

            //chat.Draw(batch);
            GraphicsDevice.SetRenderTarget(null);
            batch.Begin();
            batch.Draw(DefaultRenderTarget, Vector2.Zero, Color.White);
            batch.End();
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
                ScreenWidth = Window.ClientBounds.Width;
                ScreenHeight = Math.Max(Window.ClientBounds.Height, 1);
                
                int chatHeight = Math.Min(ScreenHeight * 3 / 4, 900);
                client.chat.SetBounds(new Rectangle(10, ScreenHeight-chatHeight-20, 800, chatHeight));

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

                if (DefaultRenderTarget != null) DefaultRenderTarget.Dispose();
                DefaultRenderTarget = new RenderTarget2D(GraphicsDevice, ScreenWidth, ScreenHeight, false, SurfaceFormat.Color, DepthFormat.Depth24);
            }
        }

        public void SetScene(Scenes.Scene s)
        {
            scene.OnExit();
            scene = s;
            s.OnResize();
        }

        public void LoadAllData(object m)
        {
            Scenes.LoadingMenu menu = (Scenes.LoadingMenu)m;

            font = Content.Load<SpriteFont>("Font");
            whiteSquare = new Texture2D(GraphicsDevice, 1, 1);
            whiteSquare.SetData(new Color[] { Color.White });

            menu.SetText("Loading Windows 95...");
            Scenes.Windows95.Load(Content);

            menu.SetText("Loading Data...");
            Scenes.InGameScene.Load(Content);


            ClusterWave.Scenario.Dynamic.Bullet.Load(Content);
            ClusterWave.Scenario.Backgrounds.Background.Load(Content);
            ClusterWave.Scenario.Dynamic.Shield.Load(Content);
        }
    }
}
