﻿using System;
using FarseerPhysics.Dynamics;

namespace ClusterWaveServer
{
    static class Constants
    {
        public const Category PlayerCategory = Category.Cat1;
        public const Category WallsCategory = Category.Cat2;
        public const Category BulletsCategory = Category.Cat3;
        public const Category ParticleCategory = Category.Cat4;
        public const Category ShieldCategory = Category.Cat5;

        public const Category PlayersCollideWith = PlayerCategory | WallsCategory | BulletsCategory;
        public const Category WallsCollideWith = Category.All;
        public const Category BulletsCollideWith = BulletsCategory | PlayerCategory | WallsCategory;
        public const Category ParticleCollideWith = WallsCategory | ShieldCategory;
        public const Category ShieldCollideWith = PlayerCategory | BulletsCategory;

        public const float PlayerRestitution = 0f;
        public const float PlayerFriction = 0f;
        public const float WallsRestitution = 0f;
        public const float WallsFriction = 0f;
        public const float BulletRestitution = 1f;
        public const float BulletFriction = 1f;
        public const float ShieldRestitution = WallsRestitution;
        public const float ShieldFriction = WallsFriction;
    }
}
