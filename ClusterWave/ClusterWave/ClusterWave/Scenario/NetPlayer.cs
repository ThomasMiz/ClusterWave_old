using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using Lidgren.Network;
using ClusterWave.Network;
using ClusterWave.Scenes;

namespace ClusterWave.Scenario
{
    class NetPlayer : PlayerController
    {
        Client client;

        public NetPlayer(Vector2 position, InGameScene scene, Player player)
            : base(position, scene, player)
        {
            
        }

        public void UpdatePrePhysics()
        {

        }
        public void UpdatePostPhysics()
        {

        }

        public void OnPacket(NetIncomingMessage message)
        {

        }
    }
}
