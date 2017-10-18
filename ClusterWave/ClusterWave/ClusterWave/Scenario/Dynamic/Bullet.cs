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

        /// <summary>
        /// Creates a Bullet. Do not use this constructor, use Bullet.Create variations instead
        /// </summary>
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
                GetRekkt();
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

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, body.Position, null, Stuff.ColorFromHue(Game1.Time+body.Position.X), body.Rotation, origin, drawScale, SpriteEffects.None, 0f);
        }

        public void PacketArrive(Lidgren.Network.NetIncomingMessage msg)
        {

        }

        /// <summary>
        /// Kills the bullet, removing it from the update list and removing it's physics body from the simulation
        /// </summary>
        public void GetRekkt()
        {
            list.Remove(listNode);
            physicsWorld.RemoveBody(body);
        }

        /// <summary>Creates a Bullet of the specified gun type with the corresponding id, physics world, position and rotation to go to</summary>
        /// <param name="id">The Bullet's id</param>
        /// <param name="world">The physics world from the scenario</param>
        /// <param name="pos">The center position the bullet should be created at</param>
        /// <param name="rot">The rotation the bullet should be facing and going to</param>
        public static Bullet Create(int type, int id, World world, Vector2 pos, float rot)
        {
            switch(type)
            {
                case Constants.ShotgunId:
                    return CreateShotgun(id, world, pos, rot);

                case Constants.SniperId:
                    return CreateSniper(id, world, pos, rot);

                case Constants.MachinegunId:
                    return CreateShotgun(id, world, pos, rot);

                default:
                    throw new SosUnPelotudoException();
            }
        }

        /// <summary>Creates a Shotgun bullet with the corresponding id, world, position and rotation to go to</summary>
        /// <param name="id">The Bullet's id</param>
        /// <param name="world">The physics world from the scenario</param>
        /// <param name="pos">The center position the bullet should be created at</param>
        /// <param name="rot">The rotation the bullet should be facing and going to</param>
        public static Bullet CreateShotgun(int id, World world, Vector2 pos, float rot)
        {
            return new Bullet(id, world, pos, rot, Constants.ShotgunBulletSpeed, Constants.MachinegunBulletBounceCount);
        }

        /// <summary>Creates a Shotgun bullet with the corresponding id, world, position and rotation to go to</summary>
        /// <param name="id">The Bullet's id</param>
        /// <param name="world">The physics world from the scenario</param>
        /// <param name="pos">The center position the bullet should be created at</param>
        /// <param name="rot">The rotation the bullet should be facing and going to</param>
        public static Bullet CreateSniper(int id, World world, Vector2 pos, float rot)
        {
            return new Bullet(id, world, pos, rot, Constants.SniperBulletSpeed, Constants.SniperBulletBounceCount);
        }

        /// <summary>Creates a Shotgun bullet with the corresponding id, world, position and rotation to go to</summary>
        /// <param name="id">The Bullet's id</param>
        /// <param name="world">The physics world from the scenario</param>
        /// <param name="pos">The center position the bullet should be created at</param>
        /// <param name="rot">The rotation the bullet should be facing and going to</param>
        public static Bullet CreateMachinegun(int id, World world, Vector2 pos, float rot)
        {
            return new Bullet(id, world, pos, rot, Constants.MachinegunBulletSpeed, Constants.MachinegunBulletBounceCount); 
        }
    }
}
