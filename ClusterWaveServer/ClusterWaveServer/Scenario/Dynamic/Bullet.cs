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
        private int bouncesLeft, bounceCount;
        bool check = false;
        float spd, dmg;

        /// <summary>
        /// Creates a Bullet. Do not use this constructor, use Bullet.Create variations instead
        /// </summary>
        public Bullet(World physicsWorld, Vector2 pos, float rot, float speed, int bounces, float damage)
        {
            id = idCounter++;
            this.physicsWorld = physicsWorld;
            this.bouncesLeft = bounces;
            this.spd = speed;
            this.dmg = damage;
            Init(pos, new Vector2((float)Math.Cos(rot) * speed, (float)Math.Sin(rot) * speed), rot);
        }

        private void Init(Vector2 pos, Vector2 movement, float rot)
        {
            body = new Body(physicsWorld, pos, rot, this);
            body.BodyType = BodyType.Dynamic;
            body.IsBullet = true;
            body.LinearVelocity = movement;
            Fixture f = body.CreateFixture(new CircleShape(Constants.BulletRadius, Constants.BulletDensity), this);
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
            object userData = (fixtureA.Body == body ? fixtureB : fixtureA).UserData;
            if (userData != null)
            {
                #region CheckForDamage
                PlayerController player = userData as PlayerController;
                if (player != null)
                {
                    player.Damage(dmg * (bounceCount * 0.5f + 1));

                    GetRekkt();
                    return false;
                }

                Shield shield = userData as Shield;
                if (shield != null)
                {
                    shield.Damage(dmg * (bounceCount * 0.5f + 1));

                    GetRekkt();
                    return false;
                }
                #endregion
            }

            if (--bouncesLeft == -1)
            {
                GetRekkt();
                return false;
            }

            bounceCount++;
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

        /// <summary>Creates a Bullet of the specified gun type with the corresponding id, physics world, position and rotation to go to</summary>
        /// <param name="id">The Bullet's id</param>
        /// <param name="world">The physics world from the scenario</param>
        /// <param name="pos">The center position the bullet should be created at</param>
        /// <param name="rot">The rotation the bullet should be facing and going to</param>
        public static Bullet Create(int type, World world, Vector2 pos, float rot)
        {
            switch (type)
            {
                case Constants.ShotgunId:
                    return CreateShotgun(world, pos, rot);

                case Constants.SniperId:
                    return CreateSniper(world, pos, rot);

                case Constants.MachinegunId:
                    return CreateShotgun(world, pos, rot);

                default:
                    throw new SosUnPelotudoException();
            }
        }

        /// <summary>Creates a Shotgun bullet with the corresponding id, world, position and rotation to go to</summary>
        /// <param name="id">The Bullet's id</param>
        /// <param name="world">The physics world from the scenario</param>
        /// <param name="pos">The center position the bullet should be created at</param>
        /// <param name="rot">The rotation the bullet should be facing and going to</param>
        public static Bullet CreateShotgun(World world, Vector2 pos, float rot)
        {
            return new Bullet(world, pos, rot, Constants.ShotgunBulletSpeed, Constants.ShotgunBulletBounceCount, Constants.ShotgunBulletDamage);
        }

        /// <summary>Creates a Shotgun bullet with the corresponding id, world, position and rotation to go to</summary>
        /// <param name="id">The Bullet's id</param>
        /// <param name="world">The physics world from the scenario</param>
        /// <param name="pos">The center position the bullet should be created at</param>
        /// <param name="rot">The rotation the bullet should be facing and going to</param>
        public static Bullet CreateSniper(World world, Vector2 pos, float rot)
        {
            return new Bullet(world, pos, rot, Constants.SniperBulletSpeed, Constants.SniperBulletBounceCount, Constants.SniperBulletDamage);
        }

        /// <summary>Creates a Shotgun bullet with the corresponding id, world, position and rotation to go to</summary>
        /// <param name="id">The Bullet's id</param>
        /// <param name="world">The physics world from the scenario</param>
        /// <param name="pos">The center position the bullet should be created at</param>
        /// <param name="rot">The rotation the bullet should be facing and going to</param>
        public static Bullet CreateMachinegun(int id, World world, Vector2 pos, float rot)
        {
            return new Bullet(world, pos, rot, Constants.MachinegunBulletSpeed, Constants.MachinegunBulletBounceCount, Constants.MachinegunBulletDamage);
        }
    }
}
