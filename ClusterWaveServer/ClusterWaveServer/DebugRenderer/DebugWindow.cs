using ClusterWaveServer.Scenario;
using ClusterWaveServer.Scenario.Dynamic;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Diagnostics;
using System.IO;

namespace ClusterWaveServer.DebugRenderer
{
    class DebugWindow : GameWindow
    {
        public static Random r = new Random();

        float time = 0, screenScale = 1;
        Stopwatch watch;
        Scenario.Scenario scenario;
        float next;

        DebugViewTK debug;

        BulletList bl;
        

        public DebugWindow(Scenario.Scenario scenario)
        {
            this.scenario = scenario;
            bl = new BulletList();
        }

        private float rand(float min, float max)
        {
            return min + (float)r.NextDouble() * (max - min);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            time += (float)e.Time;
            MouseState ms = Mouse.GetState();

            if (ms.LeftButton == ButtonState.Pressed && time > next)
            {
                Microsoft.Xna.Framework.Vector2 pos = new Microsoft.Xna.Framework.Vector2(
                    (Mouse.X - Width / 2) * screenScale + scenario.Width*0.5f,
                    (Mouse.Y - Height / 2) * screenScale + scenario.Height*0.5f
                );

                #region Shieeet Y'all
                /*next += 0.1f;

                Body b = new Body(scenario.PhysicsWorld, pos, 0f, null);
                Vertices vert = new Vertices((int)rand(3, 5));
                float[] rots = new float[vert.Capacity];
                for (int i = 0; i < rots.Length; i++)
                    rots[i] = rand(0, MathHelper.TwoPi);
                Array.Sort(rots);
                for (int i = 0; i < rots.Length; i++)
                {
                    vert.Add(new Microsoft.Xna.Framework.Vector2(0.1f * (float)Math.Cos(rots[i]), 0.1f * (float)Math.Sin(rots[i])));
                }
                b.CreateFixture(new PolygonShape(vert, 0.5f));
                b.BodyType = BodyType.Dynamic;
                b.Friction = 0.1f;
                b.Restitution = 0.975f;
                b.LinearDamping = 0f;
                b.AngularDamping = 0.2f;
                b.LinearVelocity = new Microsoft.Xna.Framework.Vector2(rand(-5, 5), rand(-5, 5));
                b.IsBullet = true;
                */
                #endregion
            }

            watch.Restart();

            GL.ClearColor(Color4.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            debug.RenderDebug();

            SwapBuffers();
            System.Threading.Thread.Sleep(Math.Max(0, (int)(16 - watch.ElapsedMilliseconds)));
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.ColorArray);
            GL.EnableClientState(ArrayCap.TextureCoordArray);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);

            debug = new DebugViewTK(scenario.PhysicsWorld);
            debug.View = Matrix4.CreateTranslation(scenario.Width * -0.5f, scenario.Height * -0.5f, 0);

            watch = new Stopwatch();
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            debug.Projection = CreateProjectionMatrix();
        }

        /*protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
            {
                next = time;
                Microsoft.Xna.Framework.Vector2 pos = new Microsoft.Xna.Framework.Vector2(
                    (e.X - Width / 2) * screenScale + scenario.Width*0.5f,
                    (e.Y - Height / 2) * screenScale + scenario.Height*0.5f
                );

                Body b = new Body(scenario.PhysicsWorld, pos, 0f, null);
                Vertices vert = new Vertices((int)rand(3, 5));
                float[] rots = new float[vert.Capacity];
                for (int i = 0; i < rots.Length; i++)
                    rots[i] = rand(0, MathHelper.TwoPi);
                Array.Sort(rots);
                for (int i = 0; i < rots.Length; i++)
                {
                    vert.Add(new Microsoft.Xna.Framework.Vector2(0.3f * (float)Math.Cos(rots[i]), 0.3f * (float)Math.Sin(rots[i])));
                }
                b.CreateFixture(new PolygonShape(vert, 0.5f));
                b.BodyType = BodyType.Dynamic;
                b.Friction = 0.1f;
                b.Restitution = 0.975f;
                b.LinearDamping = 0f;
                b.AngularDamping = 0.2f;
                b.LinearVelocity = new Microsoft.Xna.Framework.Vector2(rand(-5, 5), rand(-5, 5));
            }
        }*/

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            debug.Dispose();
        }

        private Matrix4 CreateProjectionMatrix()
        {
            Vector2 size;
            if ((float)Width / (float)Height < (scenario.Width / scenario.Height))
            { //should try to adjust based on width
                size = new Vector2(scenario.Width, Height * scenario.Width / Width);
                screenScale = scenario.Width / Width;
            }
            else
            { //should try to adjust based on height
                size = new Vector2(Width * scenario.Height / Height, scenario.Height);
                screenScale = scenario.Height / Height;
            }

            size *= 1.05f;
            screenScale *= 1.05f;

            /*float _halfw = scenario.Width * 0.5f, _halfh = scenario.Height * 0.5f;
            screenBounds.X = (int)(-_halfw / screenScale + 0.5*Width);
            screenBounds.Y = (int)(-_halfh / screenScale + 0.5*Height);
            screenBounds.Width = (int)(scenario.Width / screenScale);
            screenBounds.Height = (int)(scenario.Width / screenScale);*/

            return Matrix4.CreateOrthographic(size.X, -size.Y, 0f, 10f);
        }
    }
}
