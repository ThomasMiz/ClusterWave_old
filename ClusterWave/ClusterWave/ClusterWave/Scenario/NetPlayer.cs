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

        Vector2 spd = Vector2.Zero;

        public void UpdatePrePhysics()
        {
            body.LinearVelocity = spd;
        }

        public void UpdatePostPhysics()
        {
            spd = Vector2.Zero;
        }

        public void MoveLeft()
        {
            spd.X -= Constants.PlayerMovementSpeed;
        }

        public void MoveRight()
        {
            spd.X += Constants.PlayerMovementSpeed;
        }

        public void MoveUp()
        {
            spd.Y -= Constants.PlayerMovementSpeed;
        }

        public void MoveDown()
        {
            spd.Y += Constants.PlayerMovementSpeed;
        }

        public void OnPacket(NetIncomingMessage message)
        {

        }
    }
}
