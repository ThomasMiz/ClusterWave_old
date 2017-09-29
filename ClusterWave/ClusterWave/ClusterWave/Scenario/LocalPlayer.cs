using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;

namespace ClusterWave.Scenario
{
    class LocalPlayer : PlayerController
    {
        public LocalPlayer(Vector2 position, World world, Texture2D texture)
            : base(position, world, texture)
        {

        }

        public override void UpdatePrePhysics()
        {
            
        }
        public override void UpdatePostPhysics()
        {
            
        }
    }
}
