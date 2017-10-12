using System;
using FarseerPhysics.Dynamics;

namespace ClusterWaveServer
{
    static class Constants
    {
        /// <summary>The physics Category for player collider bodies</summary>
        public const Category PlayerCategory = Category.Cat1;
        /// <summary>The physics Category for walls</summary>
        public const Category WallsCategory = Category.Cat2;
        /// <summary>The physics Category for bullet bodies</summary>
        public const Category BulletsCategory = Category.Cat3;
        /// <summary>The physics Category for particles</summary>
        public const Category ParticleCategory = Category.Cat4;
        /// <summary>The physics Category for shields</summary>
        public const Category ShieldCategory = Category.Cat5;
        /// <summary>The physics Category for powerup</summary>
        public const Category PowerupCategory = Category.Cat6;

        /// <summary>The Categories with which players can collide with</summary>
        public const Category PlayersCollideWith = PlayerCategory | WallsCategory | BulletsCategory;
        /// <summary>The Categories with which walls can collide with</summary>
        public const Category WallsCollideWith = Category.All;
        /// <summary>The Categories with which bullets can collide with</summary>
        public const Category BulletsCollideWith = BulletsCategory | PlayerCategory | WallsCategory;
        /// <summary>The Categories with which particles can collide with</summary>
        public const Category ParticleCollideWith = WallsCategory | ShieldCategory;
        /// <summary>The Categories with which shields can collide with</summary>
        public const Category ShieldCollideWith = PlayerCategory | BulletsCategory;
        /// <summary>The Categories with which powerups can collide with</summary>
        public const Category PowerupCollideWith = PlayerCategory;

        /// <summary>The Restitution value to be applied to player's physic bodies</summary>
        public const float PlayerRestitution = 0f;
        /// <summary>The Friction value to be applied to player's physic bodies</summary>
        public const float PlayerFriction = 0f;
        /// <summary>The Radius of the circle shape for player's collider</summary>
        public const float PlayerColliderSize = 1f / 6f;
        /// <summary>The maximum/starting amount of health a player gets</summary>
        public const float PlayerMaxHealth = 100f;

        /// <summary>The speed at which the players move</summary>
        public const float PlayerMovementSpeed = 1.3f;

        /// <summary>The Restitution value to be assigned to walls</summary>
        public const float WallsRestitution = 0f;
        /// <summary>The Friction value to be assigned to walls</summary>
        public const float WallsFriction = 0f;

        /// <summary>The Radious of the circle shape collider attached to bullet's physic bodies</summary>
        public const float BulletRadius = 1f / 20f;
        /// <summary>The Density of the circle shape collider attached to bullet's physic bodies</summary>
        public const float BulletDensity = 0.01f;
        /// <summary>The Restitution value to be assigned to bullets</summary>
        public const float BulletRestitution = 1f;
        /// <summary>The Friction value to be assigned to bullets</summary>
        public const float BulletFriction = 0f;

        /// <summary>The damage shotgun bullets inflict</summary>
        public const float ShotgunBulletDamage = 10f;
        /// <summary>The damage sniper bullets inflict</summary>
        public const float SniperBulletDamage = 75f;
        /// <summary>The damage machinegun bullets inflict</summary>
        public const float MachinegunBulletDamage = 5f;
        /// <summary>The maximum/starting amount of shotgun bullets a player can have</summary>
        public const int ShotgunMaxBullets = 8;
        /// <summary>The maximum/starting amount of sniper bullets a player can have</summary>
        public const int SniperMaxBullets = 4;
        /// <summary>the maximum/starting amount of machinegun bullets a player can have</summary>
        public const int MachinegunMaxBullets = 25;
        /// <summary>The time in seconds it takes after emptying a shotgun until it's reloaded</summary>
        public const float ShotgunReloadTimeSeconds = 5f;
        /// <summary>The time in seconds it takes after emptying a sniper until it's reloaded</summary>
        public const float SniperReloadTimeSeconds = 7.5f;
        /// <summary>The time in seconds it takes after emptying a machingun until it's reloaded</summary>
        public const float MachinegunReloadTimeSeconds = 5f;
        /// <summary>The time in seconds to wait between shotgun firing shots</summary>
        public const float ShotgunFireTimeSeconds = 10f / 6f;
        /// <summary>The time in seconds to wait between sniper firing shots</summary>
        public const float SniperFireTimeSeconds = 2f;
        /// <summary>The time in seconds to wait between machinegun firing shots</summary>
        public const float MachinegunFireTimeSeconds = 0.125f; //8 shots per seconds
        /// <summary>The amount of times Shotgun bullets should bounce</summary>
        public const int ShotgunBulletBounceCount = 0;
        /// <summary>The amount of times sniper bullets should bounce</summary>
        public const int SniperBulletBounceCount = 3;
        /// <summary>The amount of times a machinegun bullet should bounce</summary>
        public const int MachinegunBulletBounceCount = 0;

        /// <summary>The Restitution value to be assigned to shield physic bodies</summary>
        public const float ShieldRestitution = WallsRestitution;
        /// <summary>The Friction value to be assigned to shield physic bodies</summary>
        public const float ShieldFriction = WallsFriction;
        /// <summary>The amount of damage a shield can take before being destroyed</summary>
        public const float ShieldMaxHealth = 150;
        /// <summary>The time in seconds after using a shield until that player can place down another shield</summary>
        public const float ShieldReloadTimeSeconds = 20f;
        /// <summary>The time in seconds a shield lasts before committing suicide and sending wee wee nice particles everywhere</summary>
        public const float ShieldDurationSeconds = 15f;

        /// <summary>The radius of the circle shape attached to powerup's physic bodies</summary>
        public const float PowerupRadius = 1.6f;
        /// <summary>The time in seconds a powerup's effect lasts after it's been picked up</summary>
        public const float PowerupDurationSeconds = 15f;

        /// <summary>Whether we wrote enough "fuck you dusto"-s</summary>
        public const bool DidWeWriteEnoughFuckUDustos = false;
    }
}
