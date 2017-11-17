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
        float shootingCooldown = 0;
        string selectedWeapon = "Smg";

        public void UpdatePrePhysics(Vector2 mousePos)
        {
            Move();

            CheckWeaponInput();

            if (Game1.ms.LeftButton == ButtonState.Pressed && shootingCooldown <= 0)
            {
                Shoot();
            }

            body.LinearVelocity = spd;

            if (spd.X == 0 && spd.Y == 0)
                ResetAnimation();
            else
                StartMovingAnimation(Game1.Time);

            Rotate(mousePos);

            shootingCooldown -= Game1.DeltaTime;
        }

        public void UpdatePostPhysics()
        {
            spd = Vector2.Zero;
        }

        void Shoot()
        {
            StartShootingAnimation(Game1.Time);
            switch (selectedWeapon)
            {
                case "Smg":
                    scene.Client.ShootSmg();
                    shootingCooldown = Constants.MachinegunFireTimeSeconds;
                    break;
                case "Shoty":
                    scene.Client.ShootShotgun();
                    shootingCooldown = Constants.ShotgunFireTimeSeconds;
                    break;
                case "Sniper":
                    scene.Client.ShootSniper();
                    shootingCooldown = Constants.SniperFireTimeSeconds;
                    break;
            }
            
        }

        void CheckWeaponInput()
        {
            if (Game1.ks.IsKeyDown(Keys.D1) && Game1.oldks.IsKeyUp(Keys.D1))
            {
                SetAnimationGunType(Constants.ShotgunId);
                selectedWeapon = "Shoty";
            }
            else if (Game1.ks.IsKeyDown(Keys.D2) && Game1.oldks.IsKeyUp(Keys.D2))
            {
                SetAnimationGunType(Constants.SniperId);
                selectedWeapon = "Sniper";
            }
            else if (Game1.ks.IsKeyDown(Keys.D3) && Game1.oldks.IsKeyUp(Keys.D3))
            {
                SetAnimationGunType(Constants.MachinegunId);
                selectedWeapon = "Smg";
            }
            else if (Game1.ks.IsKeyDown(Keys.D4) && Game1.oldks.IsKeyUp(Keys.D4))
            {
                //add shield
            }
        }

        void Move()
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
        }

        void Rotate(Vector2 mousePos)
        {
            rotation = (float)Math.Atan2(mousePos.Y - body.Position.Y, mousePos.X - body.Position.X);
            scene.Client.SendRotation(rotation);
        }
    }
}
