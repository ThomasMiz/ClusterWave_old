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
        Vector2 spd = Vector2.Zero;

        public void UpdatePrePhysics(Vector2 mousePos)
        {
            
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

            if (Game1.ks.IsKeyDown(Keys.D1) && Game1.oldks.IsKeyUp(Keys.D1))
            {
                SetAnimationGunType(Constants.ShotgunId);
            }
            else if (Game1.ks.IsKeyDown(Keys.D2) && Game1.oldks.IsKeyUp(Keys.D2))
            {}
                SetAnimationGunType(Constants.SniperId);
            }
            else if (Game1.ks.IsKeyDown(Keys.D3) && Game1.oldks.IsKeyUp(Keys.D3))
            {
                SetAnimationGunType(Constants.MachinegunId);
            }
            else if (Game1.ks.IsKeyDown(Keys.D4) && Game1.oldks.IsKeyUp(Keys.D4))
            {
                //add shield
            }

            if (Game1.ms.LeftButton == ButtonState.Pressed)
            {
                StartShootingAnimation(Game1.Time);
                scene.Client.Shoot();
            }

            body.LinearVelocity = spd;

            if (spd.X == 0 && spd.Y == 0)
                ResetAnimation();
            else
                StartMovingAnimation(Game1.Time);

            rotation = (float)Math.Atan2(mousePos.Y - body.Position.Y, mousePos.X - body.Position.X);
            scene.Client.SendRotation(rotation);
        }

        public void UpdatePostPhysics()
        {
            spd = Vector2.Zero;
        }
    }
}
