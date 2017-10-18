using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Common;

namespace ClusterWave.Particles
{
    class TrianglePhysicsParticle : Particle
    {
        private const float Friction = 0f, Restitution = 1.05f;
        private const float ForceValue = 0.075f, ForceRandomDelta = 1.5f, ForceMinRandom = 0.25f;

        VertexBuffer[] buffers;
        Body[] bodies;
        float time;
        bool ending = false;
        World world;

        public TrianglePhysicsParticle(Vector2[] vertexStrip, World world, Vector2 offset)
        {
            for (int i = 0; i < vertexStrip.Length; i++)
                vertexStrip[i] += offset;
            this.world = world;
            time = Game1.Time;
            int count = vertexStrip.Length - 2;
            buffers = new VertexBuffer[count];
            bodies = new Body[count];
            
            int ind = 0;
            VertexPositionTexture[] arr = new VertexPositionTexture[3];
            Vector2[] centroids = new Vector2[count];
            for (int i = 0; i < count; i++)
            {
                buffers[ind] = new VertexBuffer(Game1.game.GraphicsDevice, typeof(VertexPositionTexture), 3, BufferUsage.WriteOnly);
                bodies[ind] = CreateBody(world, vertexStrip, i, ref arr, out centroids[ind]);

                buffers[ind++].SetData(arr);
            }

            for (int i = 0; i < count; i++)
            {
                float rotation = (float)Math.Atan2(centroids[i].Y - offset.Y, centroids[i].X - offset.X);
                Vector2 force = new Vector2((float)Math.Cos(rotation) * ForceValue * (Game1.Random(ForceRandomDelta) + ForceMinRandom),
                                            (float)Math.Sin(rotation) * ForceValue * (Game1.Random(ForceRandomDelta) + ForceMinRandom));

                bodies[i].ApplyForce(ref force);
            }
        }

        private static Body CreateBody(World w, Vector2[] vertices, int index, ref VertexPositionTexture[] arr, out Vector2 centroid)
        {
            Vector2 a = vertices[index], b = vertices[index + 1], c = vertices[index + 2];
            centroid = (a + b + c) / 3.0f;

            Body bo = new Body(w, centroid, 0f, null);
            bo.CollisionCategories = Constants.ParticleCategory;
            bo.CollidesWith = Constants.ParticleCollideWith;
            bo.BodyType = BodyType.Dynamic;
            bo.Restitution = Restitution;
            bo.Friction = Friction;
            bo.LinearDamping = 0f;
            
            Vertices vert = new Vertices(3);
            a -= centroid;
            b -= centroid;
            c -= centroid;
            vert.Add(a);
            vert.Add(b);
            vert.Add(c);
            arr[0] = new VertexPositionTexture(new Vector3(a, 0f), Vector2.Zero);
            arr[1] = new VertexPositionTexture(new Vector3(b, 0f), Vector2.Zero);
            arr[2] = new VertexPositionTexture(new Vector3(c, 0f), Vector2.Zero);

            PolygonShape pol = new PolygonShape(vert, 0.1f);
            Fixture f = bo.CreateFixture(pol);
            f.CollisionCategories = Constants.ParticleCategory;
            f.CollidesWith = Constants.ParticleCollideWith;
            f.Restitution = Restitution;
            f.Friction = Friction;

            return bo;
        }

        public override void Update()
        {
            if (ending)
            {
                time -= 0.01f;
                if (time < 0)
                    GetRekkt();
            }
            else if(Game1.Time - time > 7f)
            {
                ending = true;
                time = 1f;
            }
        }

        public override void Draw(SpriteBatch batch, GraphicsDevice device)
        {
            device.BlendState = BlendState.NonPremultiplied;
            device.SamplerStates[0] = SamplerState.LinearWrap;
            Effect fx = Scenes.InGameScene.colorFx;

            if (ending)
                fx.Parameters["alpha"].SetValue(time);
            else
                fx.Parameters["alpha"].SetValue(1f);

            for (int i = 0; i < buffers.Length; i++)
            {
                device.SetVertexBuffer(buffers[i]);

                Body b = bodies[i];

                fx.Parameters[0].SetValue(Matrix.CreateRotationZ(b.Rotation) * Matrix.CreateTranslation(new Vector3(b.Position, 0)));

                fx.CurrentTechnique.Passes[0].Apply();

                device.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);

            }
        }

        public override void Dispose()
        {
            for (int i = 0; i < buffers.Length; i++)
            {
                world.RemoveBody(bodies[i]);
                buffers[i].Dispose();
            }
        }
    }
}
