using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;

namespace ClusterWave.Scenario
{
    class LocalPlayer : PlayerController
    {
        public LocalPlayer(Vector2 position, World world, Player player)
            : base(position, world, player)
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
