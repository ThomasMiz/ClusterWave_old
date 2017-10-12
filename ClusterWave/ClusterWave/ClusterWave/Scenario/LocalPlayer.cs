using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Input;
using ClusterWave.Scenes;

namespace ClusterWave.Scenario
{
    class LocalPlayer : PlayerController
    {
        public LocalPlayer(Vector2 position, InGameScene scene, Player player)
            : base(position, scene, player)
        {

        }

        public void UpdatePrePhysics(Vector2 mousePos)
        {
            Vector2 spd = Vector2.Zero;
            if (Game1.ks.IsKeyDown(Keys.Left))
            {
                spd.X = -Constants.PlayerMovementSpeed;
            }
            else if (Game1.ks.IsKeyDown(Keys.Right))
            {
                spd.X = Constants.PlayerMovementSpeed;
            }

            if (Game1.ks.IsKeyDown(Keys.Up))
            {
                spd.Y = -Constants.PlayerMovementSpeed;
            }
            else if (Game1.ks.IsKeyDown(Keys.Down))
            {
                spd.Y = Constants.PlayerMovementSpeed;
            }
            body.LinearVelocity = spd;

            rotation = (float)Math.Atan2(mousePos.Y - body.Position.Y, mousePos.X - body.Position.X);
        }

        public void UpdatePostPhysics()
        {
            
        }
    }
}
