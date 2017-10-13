using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Common;

namespace ClusterWaveServer.Scenario.Dynamic
{
    class Bullet
    {
        private int idCounter = 0;

        private LinkedList<Bullet> list;
        private LinkedListNode<Bullet> listNode;

        public readonly int id;
        private Body body;
        private World physicsWorld;
        private int bouncesLeft;
        bool check = false;
        float spd;

        public Bullet(World physicsWorld, Vector2 pos, float rot, float speed, int bounces)
        {
            id = idCounter++;
            this.physicsWorld = physicsWorld;
            this.bouncesLeft = bounces;
            this.spd = speed;
            Init(pos, new Vector2((float)Math.Cos(rot) * speed, (float)Math.Sin(rot) * speed), rot);
        }

        private void Init(Vector2 pos, Vector2 movement, float rot)
        {
            body = new Body(physicsWorld, pos, rot, this);
            body.BodyType = BodyType.Dynamic;
            body.LinearVelocity = movement;
            Fixture f = body.CreateFixture(new CircleShape(Constants.BulletRadius, Constants.BulletDensity));
            f.Friction = Constants.BulletFriction;
            f.Restitution = Constants.BulletRestitution;
            body.Friction = Constants.BulletFriction;
            body.Restitution = Constants.BulletRestitution;
            body.AngularDamping = 0f;
            body.LinearDamping = 0f;
            body.CollidesWith = Constants.BulletsCollideWith;
            body.CollisionCategories = Constants.BulletsCategory;
            body.FixedRotation = true;
            body.OnCollision += body_OnCollision;
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (--bouncesLeft == -1)
            {
                GetRekkt();
                return false;
            }
            contact.Restitution = 1;
            check = true;
            return true;
        }

        public void SetList(LinkedList<Bullet> list, LinkedListNode<Bullet> node)
        {
            this.list = list;
            this.listNode = node;
        }

        public void Update()
        {
            if (check)
            {
                Vector2 lv = body.LinearVelocity;
                if (lv.X != 0 && lv.Y != 0)
                    body.Rotation = (float)Math.Atan2(lv.Y, lv.X);
                body.LinearVelocity = new Vector2((float)Math.Cos(body.Rotation) * spd, (float)Math.Sin(body.Rotation) * spd);
                check = false;
            }
        }

        public void GetRekkt()
        {
            list.Remove(listNode);
            physicsWorld.RemoveBody(body);
        }
    }
}
