using System;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using ClusterWaveServer.Scenes;
using ClusterWaveServer.Network;

namespace ClusterWaveServer.Scenario
{
    class NetPlayer : PlayerController
    {
        public NetPlayer(Vector2 position, InGameScene scene, PlayerInfo player) : base(position, scene, player)
        {

        }

        public void UpdatePrePhysics()
        {

        }
        public void UpdatePostPhysics()
        {

        }

        public override void Damage(float amount)
        {
            health -= amount;
            if (health <= 0)
            {
                // implement: lack of existence.
            }
        }
    }
}
