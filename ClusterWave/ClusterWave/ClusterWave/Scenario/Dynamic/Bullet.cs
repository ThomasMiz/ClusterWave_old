using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Common;

namespace ClusterWave.Scenario.Dynamic
{
    class Bullet
    {
        public static Texture2D texture;
        public static Vector2 origin;
        public static float drawScale;
        public static void Load(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("Scenario/Bullet/texture");
            origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            drawScale = Constants.BulletRadius / origin.X;
        }

        private LinkedList<Bullet> list;
        private LinkedListNode<Bullet> listNode;

        public readonly int id;
        private Body body;
        private World physicsWorld;
        private int bouncesLeft;
        bool check = false;
        float spd;

        public Bullet(int id, World physicsWorld, Vector2 pos, float rot, float speed, int bounces)
        {
            this.id = id;
            this.physicsWorld = physicsWorld;
            this.bouncesLeft = bounces;
            this.spd = speed;
            Init(pos, new Vector2((float)Math.Cos(rot) * speed, (float)Math.Sin(rot) * speed), rot);
        }

        private void Init(Vector2 pos, Vector2 movement, float rot)
        {
            body = new Body(physicsWorld, pos, rot, null);
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
                physicsWorld.RemoveBody(body);
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
                body.Rotation = (float)Math.Atan2(body.LinearVelocity.Y, body.LinearVelocity.X);
                body.LinearVelocity = new Vector2((float)Math.Cos(body.Rotation) * spd, (float)Math.Sin(body.Rotation) * spd);
                check = false;
            }
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, body.Position, null, Stuff.ColorFromHue(Game1.Time+body.Position.X), body.Rotation, origin, drawScale, SpriteEffects.None, 0f);
        }

        public void PacketArrive(Lidgren.Network.NetIncomingMessage msg)
        {

        }

        public void GetRekkt()
        {
            list.Remove(listNode);
        }
    }
}
