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
            colorFx = Content.Load<Effect>("ColorFx");
            colors = Content.Load<Texture2D>("colors");

            colorFx.Parameters["colors"].SetValue(colors);
        }

        Scenario.Scenario scenario;
        Vector2 mousePos;
        Background bg;

        ParticleList particles;
        ShieldList shields;
        BulletList bullets;

        Client client;

        /// <summary>Debug Renderer for Farseer Physics</summary>
        DebugViewXNA debug;

        /// <summary>
        /// [deprecated] Creates an InGameScene that loads it's scenario from a file called "data.map"
        /// </summary>
        [Obsolete("NO USEN ESTE CONSTRUCTOR DE MIERDA, DEPRECATED AF (es para cargar sin server)", false)]
        public InGameScene(Client client)
        {
            this.client = client;
            if (!System.IO.File.Exists("data.map"))
            {
                Game1.game.Exit();
                return;
            }

            ByteStream b = new ByteStream(System.IO.File.ReadAllBytes("data.map"));
            if (!(b.HasNext(56) && b.ReadByte() == 222 && b.ReadByte() == 111 && b.ReadByte() == 41 && b.ReadByte() == 231 && b.ReadByte() == 60))
            {
                Game1.game.Exit();
                return;
            }

            scenario = new Scenario.Scenario(b);

            bg = scenario.BackgroundObject;
            shields = new ShieldList();
            bullets = new BulletList();
            particles = new ParticleList();
            shields.Add(new Shield(0, scenario.PhysicsWorld, particles, new Vector2(2.1f, 0.8f), 0f));

            float hw = scenario.HalfWidth, hh = scenario.HalfHeight;
            Matrix view = Matrix.CreateLookAt(new Vector3(hw, hh, 2), new Vector3(hw, hh, 1), Vector3.Up);
            bg.ShapeFillFx.Parameters["View"].SetValue(view);
            bg.ShapeLineFx.Parameters["View"].SetValue(view);
            bg.RayLightFx.Parameters["View"].SetValue(view);
            Shield.shieldFx.Parameters["View"].SetValue(view);
            bg.ShapeLineFx.Parameters["size"].SetValue(new Vector2(scenario.Width, scenario.Height));
            colorFx.Parameters[1].SetValue(view);

            debug = new DebugViewXNA(scenario.PhysicsWorld);
            debug.LoadContent(GraphicsDevice, game.Content);
        }

        /// <summary>
        /// Creates an InGameScene with the loaded Scenario and Client. This should be done from the ScenarioLoadingScene
        /// </summary>
        /// <param name="client"></param>
        public InGameScene(Client client, Scenario.Scenario scenario)
        {
            this.client = client;

            this.scenario = scenario;

            bg = scenario.BackgroundObject;
            shields = new ShieldList();
            bullets = new BulletList();
            particles = new ParticleList();
            shields.Add(new Shield(0, scenario.PhysicsWorld, particles, new Vector2(2.1f, 0.8f), 0f));

            float hw = scenario.HalfWidth, hh = scenario.HalfHeight;
            Matrix view = Matrix.CreateLookAt(new Vector3(hw, hh, 2), new Vector3(hw, hh, 1), Vector3.Up);
            bg.ShapeFillFx.Parameters["View"].SetValue(view);
            bg.ShapeLineFx.Parameters["View"].SetValue(view);
            bg.RayLightFx.Parameters["View"].SetValue(view);
            Shield.shieldFx.Parameters["View"].SetValue(view);
            bg.ShapeLineFx.Parameters["size"].SetValue(new Vector2(scenario.Width, scenario.Height));
            colorFx.Parameters[1].SetValue(view);

            debug = new DebugViewXNA(scenario.PhysicsWorld);
            debug.LoadContent(GraphicsDevice, game.Content);
        }

        public override void Update()
        {
            //client.Update();
            //game.SetScene(new Windows95());
            mousePos.X = (Game1.ms.X - Game1.HalfScreenWidth) * scenario.ScreenToSizeRatio + scenario.HalfWidth;
            mousePos.Y = (Game1.ms.Y - Game1.HalfScreenHeight) * scenario.ScreenToSizeRatio + scenario.HalfHeight;

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

            if (Game1.ms.LeftButton == ButtonState.Pressed) bullets.Add(new Bullet(0, scenario.PhysicsWorld, mousePos, Game1.Random(MathHelper.TwoPi), Game1.Random(15f, 20f)*0.5f, 1000));


            bullets.UpdateBullets();

            scenario.PhysicsStep();
            bg.Update();

            particles.UpdateParticles();
        }

        public override void Draw()
        {
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            GraphicsDevice.BlendState = BlendState.NonPremultiplied;
            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            GraphicsDevice.DepthStencilState = DepthStencilState.None;

            client.chat.PreDraw(GraphicsDevice, batch);
            bg.PreDraw(GraphicsDevice, batch);

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Gray);

            bg.Draw(GraphicsDevice, batch);

            float hw = scenario.HalfWidth, hh = scenario.HalfHeight;

            particles.DrawParticles(batch, GraphicsDevice);
            bullets.DrawBullets(batch, GraphicsDevice);
            shields.DrawShields(GraphicsDevice);

            bg.RayLightFx.Parameters["lightPos"].SetValue(mousePos);
            scenario.DrawLightWalls(GraphicsDevice);

            scenario.DrawShapeFill(GraphicsDevice);
            scenario.DrawShapeLines(GraphicsDevice);

            client.chat.Draw(batch);

            debug.RenderDebugData(scenario.CreateProjectionMatrix(), Matrix.CreateLookAt(new Vector3(hw, hh, 2), new Vector3(hw, hh, 1), Vector3.Up));
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;
        }

        public override void OnResize()
        {
            if (scenario != null)
            {
                Matrix proj = scenario.CreateProjectionMatrix();
                bg.RayLightFx.Parameters["Projection"].SetValue(proj);
                bg.ShapeFillFx.Parameters["Projection"].SetValue(proj);
                bg.ShapeLineFx.Parameters["Projection"].SetValue(proj);
                colorFx.Parameters["Projection"].SetValue(proj);
                Shield.shieldFx.Parameters["Projection"].SetValue(proj);

                bg.Resize();
            }
        }

        public override void OnExit()
        {
            if (scenario != null)
                scenario.Dispose();
            if (debug != null)
                debug.Dispose();
            if (bg != null)
                bg.Dispose();

            client.OnPacket -= OnPacket;
        }

        void OnPacket(NetIncomingMessage msg)
        {
            byte index = msg.ReadByte();
            switch (index)
            {
                case MsgIndex.chat:
                    string chatText = msg.ReadString();
                    //CHAT
                    break;
                case MsgIndex.statusUpdate:

                    break;
                case MsgIndex.disconnect:

                    break;
                case MsgIndex.error:

                    break;
                case MsgIndex.playerAct:

                    break;
                case MsgIndex.playerMove:

                    break;
                case MsgIndex.scenarioRecieve:

                    break;
            }
        }
    }
}
