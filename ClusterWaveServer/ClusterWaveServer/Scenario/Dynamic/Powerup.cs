using System;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;

namespace ClusterWaveServer.Scenario.Dynamic
{
    delegate void OnPowerupPickedUp(PlayerController by);

    class Powerup
    {
        private Vector2 pos;
        private Body body;

        public Vector2 Position { get { return pos; } }
        public bool IsActive { get { return body.Enabled; } }

        public Powerup(Vector2 pos, World physicsWorld)
        {
            this.pos = pos;
            body = new Body(physicsWorld, pos, 0f, this);
            body.BodyType = BodyType.Static;
            body.IsSensor = true;
            body.CollisionCategories = Constants.PowerupCategory;
            body.CollidesWith = Constants.PowerupCollideWith;
            Fixture f = body.CreateFixture(new CircleShape(Constants.PowerupRadius, 1f), this);
            f.CollisionCategories = Constants.PowerupCategory;
            f.CollidesWith = Constants.PowerupCollideWith;
            body.OnCollision += body_OnCollision;
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            PlayerController player = (fixtureA.Body == body ? fixtureB : fixtureA).UserData as PlayerController;

            if (player == null)
                return false;

            if (OnPickedUp != null)
                OnPickedUp(player);

            return false;
        }

        public void SetActive()
        {
            body.Enabled = true;
        }

        public void SetInactive()
        {
            body.Enabled = false;
        }

        public event OnPowerupPickedUp OnPickedUp;
    }
}
