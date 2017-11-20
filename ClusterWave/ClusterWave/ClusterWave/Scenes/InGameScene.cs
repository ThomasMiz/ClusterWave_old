using ClusterWave.Network;
using ClusterWave.Particles;
using ClusterWave.Scenario;
using ClusterWave.Scenario.Backgrounds;
using ClusterWave.Scenario.Dynamic;
using FarseerPhysics.DebugView;
using FarseerPhysics.Dynamics;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace ClusterWave.Scenes
{
    class InGameScene : Scene
    {
        /// <summary>ColorFX is the shader responsible for the physics particle's color rendering and anything that renders alike</summary>
        public static Effect colorFx;
        /// <summary>Just a couple of colors in a 1D texture</summary>
        public static Texture2D colors;
        /// <summary>Loads shaders, textures, etc. Call once when the program is opened</summary>
        public static void Load(ContentManager Content)
        {
            colorFx = Content.Load<Effect>("Scenario/ColorFx");
            colors = Content.Load<Texture2D>("Scenario/colors");

            colorFx.Parameters["colors"].SetValue(colors);
        }

        Scenario.Scenario scenario;
        Vector2 mousePos;
        Background bg;
        Matrix PlayerDrawMatrix;

        ParticleList particles;
        ShieldList shields;
        BulletList bullets;

        Client client;
        LocalPlayer localPlayer;
        NetPlayer[] netPlayers;

        Powerup powerup;

        RenderTarget2D renderTarget;
        VertexBuffer vertexBuffer;

        public Scenario.Scenario Scenario { get { return scenario; } }
        public Client Client { get { return client; } }
        public ShieldList Shields { get { return shields; } }
        public ParticleList Particles { get { return particles; } }

        /// <summary>Debug Renderer for Farseer Physics</summary>
        DebugViewXNA debug;

        /// <summary>
        /// [deprecated] Creates an InGameScene that loads it's scenario from a file called "data.map"
        /// </summary>
        [Obsolete("NO USEN ESTE CONSTRUCTOR DE MIERDA, DEPRECATED AF (es para cargar sin server)", false)]
        public InGameScene(Client client)
            : this(client, Stuff.TryLoadScenario("data.map"))
        {

        }

        /// <summary>
        /// Creates an InGameScene with the loaded Scenario and Client. This should be done from the ScenarioLoadingScene
        /// </summary>
        /// <param name="client">The Client... do I really need to explain what this is for?</param>
        /// <param name="scenario">The Scenario object for the game round.</param>
        public InGameScene(Client client, Scenario.Scenario scenario)
        {
            this.client = client;
            this.scenario = scenario;

            client.OnPacket += OnPacket;

            bg = scenario.BackgroundObject;
            shields = new ShieldList();
            bullets = new BulletList();
            particles = new ParticleList();
            //shields.Add(new Shield(0, scenario.PhysicsWorld, particles, new Vector2(2.1f, 0.8f), 0f));

            float hw = scenario.HalfWidth, hh = scenario.HalfHeight;
            Matrix view = Matrix.CreateLookAt(new Vector3(hw, hh, 2), new Vector3(hw, hh, 1), Vector3.Up);
            bg.ShapeFillFx.Parameters["View"].SetValue(view);
            bg.ShapeLineFx.Parameters["View"].SetValue(view);
            bg.RayLightFx.Parameters["View"].SetValue(view);
            Shield.shieldFx.Parameters["View"].SetValue(view);
            bg.ShapeLineFx.Parameters["size"].SetValue(new Vector2(scenario.Width, scenario.Height));
            colorFx.Parameters[1].SetValue(view);
            bg.ScenarioSizeParameter.SetValue(new Vector2(scenario.Width, scenario.Height));

            debug = new DebugViewXNA(scenario.PhysicsWorld);
            debug.LoadContent(GraphicsDevice, game.Content);

            netPlayers = new NetPlayer[8];
            //localPlayer = new LocalPlayer(new Vector2(Game1.Random(scenario.Width), Game1.Random(scenario.Height)), this, null);

            powerup = new Powerup(scenario.PowerupPos, scenario.PhysicsWorld);

            vertexBuffer = new VertexBuffer(Game1.game.GraphicsDevice, typeof(VertexPositionTexture), 4, BufferUsage.WriteOnly);
            vertexBuffer.SetData(new VertexPositionTexture[]{
                new VertexPositionTexture(new Vector3(-1, -1, 0), new Vector2(0, 1)),
                new VertexPositionTexture(new Vector3(-1, 1, 0), new Vector2(0, 0)),
                new VertexPositionTexture(new Vector3(1, -1, 0), new Vector2(1, 1)),
                new VertexPositionTexture(new Vector3(1, 1, 0), new Vector2(1, 0)),
            });
        }

        public override void Update()
        {
            //mousePos.X = (Game1.ms.X - Game1.HalfScreenWidth) * scenario.ScreenToSizeRatio + scenario.HalfWidth;
            //mousePos.Y = (Game1.ms.Y - Game1.HalfScreenHeight) * scenario.ScreenToSizeRatio + scenario.HalfHeight;
            mousePos = scenario.TransformScreenToGame(new Vector2(Game1.ms.X, Game1.ms.Y));

            #region Weeeee Colorful Physics Particles
            /*if (Game1.ms.LeftButton == ButtonState.Pressed && Game1.oldms.LeftButton == ButtonState.Released && Game1.ms.X > 0 && Game1.ms.X < Game1.ScreenWidth && Game1.ms.Y > 0 && Game1.ms.Y < Game1.ScreenHeight)
            {
                System.Collections.Generic.List<Vector2> ekidelol = new System.Collections.Generic.List<Vector2>(60);
                for (int i = 0; i < 30; i++)
                {
                    ekidelol.Add(new Vector2(Game1.Random(-0.2f, 0.2f), Game1.Random(-0.2f, 0.2f)));
                    ekidelol.Add(new Vector2(Game1.Random(-0.2f, 0.2f), Game1.Random(-0.2f, 0.2f)));
                    ekidelol.Add(new Vector2(Game1.Random(-0.2f, 0.2f), Game1.Random(-0.2f, 0.2f)));
                }

                particles.Add(new TrianglePhysicsParticle(ekidelol.ToArray(), scenario.PhysicsWorld, mousePos));
                
            }*/
            #endregion

            //if (Game1.ms.LeftButton == ButtonState.Pressed) bullets.Add(new Bullet(0, scenario.PhysicsWorld, mousePos, Game1.Random(MathHelper.TwoPi), Game1.Random(15f, 20f) * 0.5f, 1000));

            powerup.Update();
            bullets.UpdateBullets();

            for (int i = 0; i < netPlayers.Length; i++)
                if (netPlayers[i] != null) netPlayers[i].UpdatePrePhysics();
            if (localPlayer != null) localPlayer.UpdatePrePhysics(mousePos);

            scenario.PhysicsStep(Game1.DeltaTime);

            for (int i = 0; i < netPlayers.Length; i++)
                if (netPlayers[i] != null) netPlayers[i].UpdatePostPhysics();
            if (localPlayer != null) localPlayer.UpdatePostPhysics();

            bg.Update();

            particles.UpdateParticles();
        }

        public override void Draw()
        {
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            GraphicsDevice.BlendState = BlendState.NonPremultiplied;
            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            GraphicsDevice.DepthStencilState = DepthStencilState.None;
            bg.SetTimeParameters(Game1.Time);

            client.chat.PreDraw(GraphicsDevice, batch);
            bg.PreDraw(GraphicsDevice, batch);

            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(bg.ClearColor);

            bg.Draw(GraphicsDevice, batch);

            float hw = scenario.HalfWidth, hh = scenario.HalfHeight;

            #region Batch: Bullets & Players
            batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, PlayerDrawMatrix);

            bullets.DrawBullets(batch);
            if (localPlayer != null) localPlayer.Draw(batch);
            for (int i = 0; i < netPlayers.Length; i++)
                if (netPlayers[i] != null) netPlayers[i].Draw(batch);

            batch.End();
            #endregion

            particles.DrawParticles(batch, GraphicsDevice);
            shields.DrawShields(GraphicsDevice);

            #region DrawFOVLight

            GraphicsDevice.BlendState = BlendState.Opaque;
            Viewport prevView = GraphicsDevice.Viewport;
            GraphicsDevice.Viewport = new Viewport(scenario.ScreenBounds);
            if (localPlayer != null)bg.LightPosParameter.SetValue(localPlayer.Position);
            bg.RayTimeParameter.SetValue(Game1.Time);

            scenario.DrawLightWalls(GraphicsDevice);
            GraphicsDevice.Viewport = prevView;
            GraphicsDevice.BlendState = BlendState.NonPremultiplied;

            #endregion

            scenario.DrawShapeFill(GraphicsDevice);
            scenario.DrawShapeLines(GraphicsDevice);

            #region ApplyPowerupEffect

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            Powerup.DistortTimeParam.SetValue(Game1.Time);
            Powerup.DistortTexParam.SetValue(renderTarget);
            Powerup.DistortEffect.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.SetVertexBuffer(vertexBuffer);
            GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);

            #endregion

            #region DrawPowerup

            batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, PlayerDrawMatrix);
            powerup.Draw(batch, GraphicsDevice);
            batch.End();

            #endregion

            //debug.RenderDebugData(scenario.CreateProjectionMatrix(), Matrix.CreateLookAt(new Vector3(hw, hh, 2), new Vector3(hw, hh, 1), Vector3.Up));

            client.chat.Draw(batch);
        }

        public override void OnResize()
        {
            if (scenario != null)
            {
                Matrix Projection = scenario.CreateProjectionMatrix();
                bg.RayLightFx.Parameters["Projection"].SetValue(Matrix.CreateOrthographicOffCenter(-scenario.HalfWidth, scenario.HalfWidth, scenario.HalfHeight, -scenario.HalfHeight, 0, 10));
                bg.ShapeFillFx.Parameters["Projection"].SetValue(Projection);
                bg.ShapeLineFx.Parameters["Projection"].SetValue(Projection);
                colorFx.Parameters["Projection"].SetValue(Projection);
                Shield.shieldFx.Parameters["Projection"].SetValue(Projection);
                Powerup.DistortEffect.Parameters["World"].SetValue(Matrix.CreateTranslation(1f / Game1.ScreenWidth, 1f / Game1.ScreenHeight, 0));

                Powerup.DistortEffect.Parameters["size"].SetValue(new Vector2(Game1.ScreenWidth, Game1.ScreenHeight));
                Powerup.DistortEffect.Parameters["pos"].SetValue(scenario.TransformGameToScreen(powerup.Position));
                Powerup.DistortEffect.Parameters["scale"].SetValue(scenario.ScreenToSizeRatio);

                bg.Resize();

                PlayerDrawMatrix = Matrix.CreateTranslation(-scenario.HalfWidth, -scenario.HalfHeight, 0) * Matrix.CreateScale(1f / scenario.ScreenToSizeRatio) * Matrix.CreateTranslation(Game1.HalfScreenWidth, Game1.HalfScreenHeight, 0);

                if (renderTarget != null)
                    renderTarget.Dispose();

                renderTarget = new RenderTarget2D(GraphicsDevice, Game1.ScreenWidth, Game1.ScreenHeight, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PlatformContents);
            }
        }

        public override void OnExit()
        {
            if (scenario != null)
                scenario.Dispose();
            if (debug != null)
                debug.Dispose();
            if (renderTarget != null)
                renderTarget.Dispose();
            if (vertexBuffer != null)
                vertexBuffer.Dispose();

            client.OnPacket -= OnPacket;
        }

        void OnPacket(NetIncomingMessage msg)
        {
            byte index = msg.ReadByte();
            byte id;
            switch (index)
            {
                case MsgIndex.chat:
                    string chatText = msg.ReadString();
                    //CHAT
                    break;
                case MsgIndex.statusUpdate:
                    byte subIndex = msg.ReadByte();
                    switch (subIndex)
                    {
                        case MsgIndex.subIndex.playerCreate:
                            id = msg.ReadByte();
                            Vector2 pos = new Vector2(msg.ReadFloat(), msg.ReadFloat());
                            client.chat.Add("Player connected with id " + id);
                            if (id != client.clientPlayer.Id)
                            {
                                netPlayers[id] = new NetPlayer(pos, this, client.players[id]);
                            }
                            else
                            {
                                localPlayer = new LocalPlayer(pos, this, client.clientPlayer);
                            }
                            break;
                    }
                    break;
                case MsgIndex.disconnect:

                    break;
                case MsgIndex.error:

                    break;
                case MsgIndex.playerAct:
                    byte action = msg.ReadByte();
                    id = msg.ReadByte();
                    byte bulletId = msg.ReadByte();
                    playerActed(id, bulletId , action);
                    break;
                case MsgIndex.playerMove:
                    byte dir = msg.ReadByte();
                    id = msg.ReadByte();
                    if (id != client.clientPlayer.Id) movePlayer(id, dir);
                    break;
                case MsgIndex.scenarioRecieve:

                    break;
                case MsgIndex.playerRot:
                    id = msg.ReadByte();
                    float rot = msg.ReadByte();
                    if (id != client.clientPlayer.Id) rotatePlayer(id, rot);
                    break;
            }
        }

        void rotatePlayer(int id, float rot)
        {
            netPlayers[id].Rotation = rot;
        }

        void playerActed(int id, int bulletId, byte action)
        {
            switch (action)
            {
                case MsgIndex.subIndex.smgShot:
                    SmgShot(id, bulletId);
                    break;
                case MsgIndex.subIndex.sniperShot:
                    SmgShot(id, bulletId);
                    break;
                case MsgIndex.subIndex.shotyShot:
                    SmgShot(id, bulletId);
                    break;
                case MsgIndex.subIndex.shieldPlaced:
                    SmgShot(id, bulletId);
                    break;
            }
        }

        void movePlayer(byte id, byte dir)
        {
            switch (dir)
            {
                case MsgIndex.subIndex.up:
                    netPlayers[id].MoveUp();
                    break;
                case MsgIndex.subIndex.down:
                    netPlayers[id].MoveDown();
                    break;
                case MsgIndex.subIndex.left:
                    netPlayers[id].MoveLeft();
                    break;
                case MsgIndex.subIndex.right:
                    netPlayers[id].MoveRight();
                    break;
            }
        }

        void ShotgunShot(int playerId, int bulletId)
        {
            Bullet tempBullet;
            if (playerId != client.clientPlayer.Id)
            {
                tempBullet = Bullet.CreateShotgun(scenario.PhysicsWorld, netPlayers[playerId], bulletId);
            }
            else
            {
                tempBullet = Bullet.CreateShotgun(scenario.PhysicsWorld, localPlayer, bulletId);
            }
            bullets.Add(tempBullet);
        }

        void SmgShot(int playerId, int bulletId)
        {
            Bullet tempBullet;
            if (playerId != client.clientPlayer.Id)
            {
                tempBullet = Bullet.CreateMachinegun(scenario.PhysicsWorld, netPlayers[playerId], bulletId);
            }
            else
            {
                tempBullet = Bullet.CreateMachinegun(scenario.PhysicsWorld, localPlayer, bulletId);
            }
            bullets.Add(tempBullet);
        }

        void SniperShot(int playerId, int bulletId)
        {
            Bullet tempBullet;
            if (playerId != client.clientPlayer.Id)
            {
                tempBullet = Bullet.CreateSniper(scenario.PhysicsWorld, netPlayers[playerId], bulletId);
            }
            else
            {
                tempBullet = Bullet.CreateSniper(scenario.PhysicsWorld, localPlayer, bulletId);
            }
            bullets.Add(tempBullet);
        }

        void ShieldPlaced()
        {

        }
    }
}
