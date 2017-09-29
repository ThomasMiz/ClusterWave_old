using System;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace ClusterWaveServer.Scenarios
{
    abstract class PlayerController
    {
        const float PlayerSize = 0.2f;

        protected Body body;
        protected float rotation;

        Vector2 origin;
        float scale;


        public PlayerController(Vector2 position, World world)
        {
            rotation = 0;
            body = new Body(world, position, 0f, null);
            body.CreateFixture(new FarseerPhysics.Collision.Shapes.CircleShape(0.2f, 1f), null);
            body.BodyType = BodyType.Dynamic;
            body.FixedRotation = true;
            body.Friction = 0;
            body.Restitution = 0;

            /*origin = new Vector2(texture.Width/2f, texture.Height/2f);
            scale = PlayerSize / texture.Width;*/
        }

        public abstract void UpdatePrePhysics();
        public abstract void UpdatePostPhysics();
    }
}
