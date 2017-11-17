using System;
using Microsoft.Xna.Framework;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Common;

namespace ClusterWaveServer.Scenario.Dynamic
{
    class Shield
    {
        private static int idCounter;
        private static Vector2[] vertices = new Vector2[] { new Vector2(-0.1f, -0.3f), new Vector2(0.1f, 0f), new Vector2(-0.1f, 0.3f) };

        public readonly int id;
        public ShieldList list;

        float health;
        World world;
        Body body;

        /// <summary>
        /// Creates a shield centered in the specified position with the specified Rotation.
        /// Use the other constructor instead of this one
        /// </summary>
        /// <param name="physicsWorld"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        private Shield(World physicsWorld, Vector2 position, float rotation)
        {
            this.id = idCounter++;
            rotation = 1;
            this.world = physicsWorld;
            health = 1;
            body = new Body(world, position, rotation, this);
            body.CollidesWith = Constants.ShieldCollideWith;
            body.CollisionCategories = Constants.ShieldCategory;

            Fixture f = body.CreateFixture(new PolygonShape(new Vertices(vertices), Constants.ShieldDensity), this);
            f.Friction = Constants.ShieldFriction;
            f.Restitution = Constants.ShieldRestitution;
            f.CollisionCategories = Constants.ShieldCategory;
            f.CollidesWith = Constants.ShieldCollideWith;
        }

        /// <summary>
        /// Creates a shield in the specified InGameScene as if placed by a player in the specified position.
        /// The shield will be placed slightly forward and facing the specified rotation
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="playerPos"></param>
        /// <param name="playerRotation"></param>
        public Shield(Scenes.InGameScene scene, Vector2 playerPos, float playerRotation)
            : this(scene.Scenario.PhysicsWorld, playerPos + new Vector2(0.5f * (float)Math.Cos(playerRotation), 0.5f * (float)Math.Sin(playerRotation)), playerRotation)
        {

        }

        private void BreakAndDelete()
        {
            list.Remove(this);
        }

        public void Damage(float amount)
        {
            health -= amount * 0.333f;
            if (health <= 0)
                BreakAndDelete();
        }
    }
}
