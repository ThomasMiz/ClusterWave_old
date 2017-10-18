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
            if (Game1.ks.IsKeyDown(Keys.A))
            {
                spd.X = -Constants.PlayerMovementSpeed;
                scene.Client.MoveLeft();
            }
            else if (Game1.ks.IsKeyDown(Keys.D))
            {
                spd.X = Constants.PlayerMovementSpeed;
                scene.Client.MoveRight();
            }

            if (Game1.ks.IsKeyDown(Keys.W))
            {
                spd.Y = -Constants.PlayerMovementSpeed;
                scene.Client.MoveUp();
            }
            else if (Game1.ks.IsKeyDown(Keys.S))
            {
                spd.Y = Constants.PlayerMovementSpeed;
                scene.Client.MoveDown();
            }
            body.LinearVelocity = spd;

            rotation = (float)Math.Atan2(mousePos.Y - body.Position.Y, mousePos.X - body.Position.X);
        }

        public void UpdatePostPhysics()
        {
            
        }
    }
}
