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
    }
}
