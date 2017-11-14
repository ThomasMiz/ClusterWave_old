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
            Program.server.MoveLeft(player.GetId());
        }

        public void MoveRight()
        {
            spd.X += Constants.PlayerMovementSpeed;
            Program.server.MoveRight(player.GetId());
        }

        public void MoveUp()
        {
            spd.Y -= Constants.PlayerMovementSpeed;
            Program.server.MoveUp(player.GetId());
        }

        public void MoveDown()
        {
            spd.Y += Constants.PlayerMovementSpeed;
            Program.server.MoveDown(player.GetId());
        }

        public void Rotate(float rot)
        {
            rotation = rot;
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
