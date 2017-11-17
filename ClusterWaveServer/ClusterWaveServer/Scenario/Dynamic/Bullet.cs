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
        private static int idCounter = 0;

        private LinkedList<Bullet> list;
        private LinkedListNode<Bullet> listNode;

        public readonly int id;
        private Body body;
        private World physicsWorld;
        private int bouncesLeft, bounceCount;
        bool check = false;
        float spd, dmg;
        PlayerController ignore;

        /// <summary>
        /// Creates a Bullet. Do not use this constructor, use Bullet.Create variations instead
        /// </summary>
        public Bullet(World physicsWorld, Vector2 pos, float rot, float speed, int bounces, float damage, PlayerController ignoreFirst)
        {
            id = idCounter++;
            this.physicsWorld = physicsWorld;
            this.bouncesLeft = bounces;
            this.spd = speed;
            this.dmg = damage;
            this.ignore = ignoreFirst;
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
                    if (player == ignore)
                        return false;

                    player.Damage(dmg * (bounceCount * 0.5f + 1));

                    GetRekkt();
                    return false;
                }

                Shield shield = userData as Shield;
                if (shield != null)
                {
                    shield.Damage(dmg * (bounceCount * 0.5f + 1));
                }
                #endregion
            }

            if (--bouncesLeft == -1)
            {
                GetRekkt();
                return false;
            }

            ignore = null;
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
            body.OnCollision -= body_OnCollision;
        }

        /// <summary>Creates a Bullet of the specified gun type with the specified physics world, position and rotation to go to</summary>
        /// <param name="world">The physics world from the scenario</param>
        /// <param name="pos">The center position the bullet should be created at</param>
        /// <param name="rot">The rotation the bullet should be facing and going to</param>
        /// <param name="ignore">The PlayerController the bullet will ignore until it's first bounce</param>
        public static Bullet Create(int type, World world, Vector2 pos, float rot, PlayerController ignore)
        {
            switch (type)
            {
                case Constants.ShotgunId:
                    return CreateShotgun(world, pos, rot, ignore);

                case Constants.SniperId:
                    return CreateSniper(world, pos, rot, ignore);

                case Constants.MachinegunId:
                    return CreateShotgun(world, pos, rot, ignore);

                default:
                    throw new SosUnPelotudoException();
            }
        }

        /// <summary>Creates a Shotgun bullet with the specified world, position and rotation to go to</summary>
        /// <param name="world">The physics world from the scenario</param>
        /// <param name="pos">The center position the bullet should be created at</param>
        /// <param name="rot">The rotation the bullet should be facing and going to</param>
        /// <param name="ignore">The PlayerController the bullet will ignore until it's first bounce</param>
        public static Bullet CreateShotgun(World world, Vector2 pos, float rot, PlayerController ignore)
        {
            return new Bullet(world, pos, rot, Constants.ShotgunBulletSpeed, Constants.ShotgunBulletBounceCount, Constants.ShotgunBulletDamage, ignore);
        }

        /// <summary>Creates a Shotgun bullet with the specified world, position and rotation to go to</summary>
        /// <param name="world">The physics world from the scenario</param>
        /// <param name="pos">The center position the bullet should be created at</param>
        /// <param name="rot">The rotation the bullet should be facing and going to</param>
        /// <param name="ignore">The PlayerController the bullet will ignore until it's first bounce</param>
        public static Bullet CreateSniper(World world, Vector2 pos, float rot, PlayerController ignore)
        {
            return new Bullet(world, pos, rot, Constants.SniperBulletSpeed, Constants.SniperBulletBounceCount, Constants.SniperBulletDamage, ignore);
        }

        /// <summary>Creates a Shotgun bullet with the corresponding id, world, position and rotation to go to</summary>
        /// <param name="world">The physics world from the scenario</param>
        /// <param name="pos">The center position the bullet should be created at</param>
        /// <param name="rot">The rotation the bullet should be facing and going to</param>
        /// <param name="ignore">The PlayerController the bullet will ignore until it's first bounce</param>
        public static Bullet CreateMachinegun(World world, Vector2 pos, float rot, PlayerController ignore)
        {
            return new Bullet(world, pos, rot, Constants.MachinegunBulletSpeed, Constants.MachinegunBulletBounceCount, Constants.MachinegunBulletDamage, ignore);
        }


        /// <summary>Creates a Bullet of the specified gun type with the specified physics world and PlayerController that shoot it</summary>
        /// <param name="world">The physics world from the scenario</param>
        /// <param name="pos">The center position the bullet should be created at</param>
        /// <param name="rot">The rotation the bullet should be facing and going to</param>
        /// <param name="ignore">The PlayerController the bullet will ignore until it's first bounce</param>
        public static Bullet Create(int type, World world, PlayerController player)
        {
            switch (type)
            {
                case Constants.ShotgunId:
                    return CreateShotgun(world, player);

                case Constants.SniperId:
                    return CreateSniper(world, player);

                case Constants.MachinegunId:
                    return CreateShotgun(world, player);

                default:
                    throw new SosUnPelotudoException();
            }
        }

        /// <summary>Creates a Shotgun bullet with the specified physics world and player that shoot it</summary>
        /// <param name="world">The physics world from the scenario</param>
        /// <param name="player">The PlayerController that fired the bullet, for calculating bullet position offset</param>
        public static Bullet CreateShotgun(World world, PlayerController player)
        {
            Vector2 rot = new Vector2(0.18f, -0.28f);
            rot.RotateVector(player.Rotation);
            return CreateShotgun(world, player.Position + rot, player.Rotation, player);
        }

        /// <summary>Creates a Shotgun bullet with the specified physics world and player that shoot it</summary>
        /// <param name="id">The Bullet's id</param>
        /// <param name="world">The physics world from the scenario</param>
        /// <param name="pos">The center position the bullet should be created at</param>
        /// <param name="rot">The rotation the bullet should be facing and going to</param>
        public static Bullet CreateSniper(World world, PlayerController player)
        {
            Vector2 rot = new Vector2(0.18f, -0.28f);
            rot.RotateVector(player.Rotation);
            return CreateSniper(world, player.Position + rot, player.Rotation, player);
        }

        /// <summary>Creates a Shotgun bullet with the specified physics world and player that shoot it</summary>
        /// <param name="id">The Bullet's id</param>
        /// <param name="world">The physics world from the scenario</param>
        /// <param name="pos">The center position the bullet should be created at</param>
        /// <param name="rot">The rotation the bullet should be facing and going to</param>
        public static Bullet CreateMachinegun(World world, PlayerController player)
        {
            Vector2 rot = new Vector2(0.18f, -0.28f);
            rot.RotateVector(player.Rotation);
            return CreateMachinegun(world, player.Position + rot, player.Rotation, player);
        }
    }
}
