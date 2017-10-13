using System;
using Microsoft.Xna.Framework;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;

namespace ClusterWaveServer.Scenario.Dynamic
{
    class Shield
    {
        private static int idCounter;
        private static Vector2[] vertices;

        public readonly int id;
        public ShieldList list;

        float health;
        World world;
        Body body;

        public Shield(int id, World physicsWorld, Vector2 position, float rotation)
        {
            this.id = idCounter++;
            rotation = 1;
            this.world = physicsWorld;
            health = 1;
            body = new Body(world, position, rotation, this);
            body.CollidesWith = Constants.ShieldCollideWith;
            body.CollisionCategories = Constants.ShieldCategory;

            body.OnCollision += OnCollision;
        }

        bool OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            
            return true;
        }

        private void BreakAndDelete()
        {
            list.Remove(this);
        }

        public void DecreseHealth(float amount)
        {
            health -= amount;
        }

        public void OnPacketArrive(Lidgren.Network.NetIncomingMessage msg)
        {

        }
    }
}
