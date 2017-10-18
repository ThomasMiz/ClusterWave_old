using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ClusterWave.Particles;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Common;

namespace ClusterWave.Scenario.Dynamic
{
    class Shield
    {
        private static Vector2[] vertices;
        private static VertexBuffer buffer;
        public static Texture2D texture;
        public static Effect shieldFx;
        private static int primitiveCount;
        public static void Load(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("Scenario/shield");
            shieldFx = Content.Load<Effect>("Scenario/shieldFx");
            shieldFx.Parameters["shield"].SetValue(texture);
            shieldFx.Parameters["colors"].SetValue(Scenes.InGameScene.colors);

            vertices = new Vector2[]{
                new Vector2(-0.1386795f, -0.3004721f),
                new Vector2(-0.02311324f, -0.2773589f),
                new Vector2(-0.06933995f, -0.2080199f),
                new Vector2(0.0462266f, -0.1849065f),
                new Vector2(-0.04622662f, -0.1155666f),
                new Vector2(0.06933991f, -0.09245324f),
                new Vector2(-0.02311333f, -0.09245333f),
                new Vector2(0.09245325f, -0.06933992f),
                new Vector2(0f, 0f),
                new Vector2(0.1155666f, 0),
                new Vector2(-0.02311333f, 0.09245333f),
                new Vector2(0.09245323f, 0.06933993f),
                new Vector2(-0.04622665f, 0.1155665f),
                new Vector2(0.06933992f, 0.09245324f),
                new Vector2(-0.06933995f, 0.2080199f),
                new Vector2(0.04622664f, 0.1849065f),
                new Vector2(-0.1386799f, 0.3004728f),
                new Vector2(-0.02311324f, 0.2773589f),
            };
            float minX = -0.1386795f, maxX = 0.1155666f;
            float minY = -0.3f, maxY = 0.3f;
            float height = maxY - minY;
            float width = maxX - minX;

            primitiveCount = vertices.Length - 2;

            buffer = new VertexBuffer(Game1.game.GraphicsDevice, typeof(VertexPositionTexture), vertices.Length, BufferUsage.WriteOnly);

            VertexPositionTexture[] data = new VertexPositionTexture[vertices.Length];
            for (int i = 0; i < data.Length; i++)
                data[i] = new VertexPositionTexture(new Vector3(vertices[i], 0), new Vector2((vertices[i].X + minX) / width, (vertices[i].Y + minY) / height));

            buffer.SetData(data);
        }
        public static void DisposeContent()
        {
            buffer.Dispose();
        }

        public readonly int id;
        public ShieldList list;

        float health;
        Particles.ParticleList particles;
        World world;
        Body body;

        public Shield(int id, World physicsWorld, Particles.ParticleList particleList, Vector2 position, float rotation)
        {
            this.id = id;
            rotation = 1;
            this.world = physicsWorld;
            particles = particleList;
            health = 1;
            body = new Body(world, position, rotation, this);
            body.BodyType = BodyType.Kinematic;
            body.CollidesWith = Constants.ShieldCollideWith;
            body.CollisionCategories = Constants.ShieldCategory;
            Fixture f = body.CreateFixture(new PolygonShape(new Vertices(new Vector2[]{ new Vector2(-0.1f, -0.3f), new Vector2(0.1f, 0f), new Vector2(-0.1f, 0.3f) }), Constants.ShieldDensity));
            f.Friction = Constants.ShieldFriction;
            f.Restitution = Constants.ShieldRestitution;
            f.CollisionCategories = Constants.ShieldCategory;
            f.CollidesWith = Constants.ShieldCollideWith;

            body.OnCollision += OnCollision;
        }

        bool OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            
            return true;
        }

        public void Draw(GraphicsDevice device)
        {
            if (Game1.ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
                health -= 0.01f;
            if (Game1.ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
                health += 0.01f;
            //Game1.game.Window.Title = health.ToString();
            body.Rotation += Game1.DeltaTime;
            if (Game1.ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.B))
            {
                if (Game1.oldks.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.B))
                    BreakAndDelete();
            }
            else
            {
                device.SetVertexBuffer(buffer);
                shieldFx.Parameters["World"].SetValue(Matrix.CreateRotationZ(body.Rotation) * Matrix.CreateTranslation(body.Position.X, body.Position.Y, 0));
                //shieldFx.Parameters["World"].SetValue(Matrix.CreateRotationZ(-MathHelper.PiOver2) * Matrix.CreateScale(15f) * Matrix.CreateTranslation(5, 3, 0));
                //this commented line draws the shield very big onto the screen or something
                shieldFx.Parameters["value"].SetValue(health);
                shieldFx.CurrentTechnique.Passes[0].Apply();

                device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, primitiveCount);
            }
        }

        private void BreakAndDelete()
        {
            Vector2[] vert = new Vector2[vertices.Length];
            for (int i = 0; i < vert.Length; i++)
            {
                Vector2 v = vertices[i];
                float rot = (float)Math.Atan2(v.Y, v.X) + body.Rotation;
                float dist = (float)Math.Sqrt(v.Y * v.Y + v.X * v.X);
                vert[i] = new Vector2((float)Math.Cos(rot) * dist, (float)Math.Sin(rot) * dist);
            }
            particles.Add(new TrianglePhysicsParticle(vert, world, body.Position));
            list.Remove(this);
            world.RemoveBody(body);
        }

        public void DecreseHealth(float amount)
        {
            health -= amount;
            if (health <= 0)
                BreakAndDelete();
        }

        public void OnPacketArrive(Lidgren.Network.NetIncomingMessage msg)
        {

        }
    }
}
