using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ClusterWave.Scenario.PlayerAnimation
{
    class PlayerAnimator
    {
        protected const float PlayerDrawSize = Constants.PlayerColliderSize / 0.33f;
        public const int FrameWidth = 24, FrameHeight = 21, FrameCount = 7;
        public static Texture2D[] PlayerTextures, GunTextures;
        static Vector2 playerOrigin, gunOrigin;
        static float playerScale, gunScale;
        public static void Load(ContentManager Content)
        {
            PlayerTextures = new Texture2D[1];

            for (int i = 0; i < PlayerTextures.Length; )
                PlayerTextures[i] = Content.Load<Texture2D>("Player/walk" + ++i);
            GunTextures = new Texture2D[3];

            GunTextures[Constants.ShotgunId] = Content.Load<Texture2D>("Player/shotgun");
            GunTextures[Constants.SniperId] = Content.Load<Texture2D>("Player/sniper");
            GunTextures[Constants.MachinegunId] = Content.Load<Texture2D>("Player/machinegun");

            playerOrigin = new Vector2(12, 14);
            playerScale = PlayerDrawSize / FrameWidth;
            //PlayerTextures[0] = Content.Load<Texture2D>("Player/dusto");
            gunOrigin = playerOrigin;
            gunScale = playerScale;
        }

        public Texture2D playerTexture;
        public Rectangle playerSource;

        public Texture2D gunTexture;
        public Rectangle gunSource;

        private Anim currentAnim, nextAnim;
        private bool hasNext;

        public PlayerAnimator(PlayerController controller)
        {
            SetPlayerColorIndex(0);
            playerSource = new Rectangle(0, 0, FrameWidth, FrameHeight);

            gunTexture = GunTextures[0];
            gunSource = new Rectangle(0, 0, FrameWidth, FrameHeight);

            hasNext = false;
            currentAnim = new MoveAnim(this, Game1.Time);
        }

        public void Draw(SpriteBatch batch, Vector2 pos, float rotation)
        {
            currentAnim.Update();

            rotation += MathHelper.PiOver2;
            gunSource.X = playerSource.X;
            batch.Draw(playerTexture, pos, playerSource, Color.White, rotation, playerOrigin, playerScale, SpriteEffects.None, 0f);
            batch.Draw(gunTexture, pos, gunSource, Color.White, rotation, gunOrigin, gunScale, SpriteEffects.None, 0f);
        }

        public void SetGunType(int type)
        {
            gunTexture = GunTextures[type];
        }

        public void SetPlayerColorIndex(int index)
        {
            playerTexture = PlayerTextures[index];
        }

        public void ResetAnimation()
        {
            if (currentAnim.IsIdleAnim)
                return;

            if (hasNext)
            {
                if (!nextAnim.IsIdleAnim)
                    nextAnim = new IdleAnim(this, Game1.Time);
            }
            else
            {
                hasNext = true;
                nextAnim = new IdleAnim(this, Game1.Time);
            }
        }

        public void StartMovingAnimation(float time)
        {
            if (currentAnim.IsMovingAnim)
                return;

            if (currentAnim.IsIdleAnim)
            {
                currentAnim = new MoveAnim(this, time);
                currentAnim.OnApplied();
                return;
            }

            if(currentAnim.IsShootingAnim)
            {
                if (hasNext)
                {
                    if (!nextAnim.IsMovingAnim)
                        nextAnim = new MoveAnim(this, time);
                }
                else
                {
                    hasNext = true;
                    nextAnim = new MoveAnim(this, time);
                }
            }
        }

        public void StartShootingAnimation(float time)
        {
            if (!currentAnim.IsShootingAnim)
            {
                currentAnim = new ShootingAnim(this, time);
                currentAnim.OnApplied();
            }
            return;
        }

        public void AnimDone()
        {
            if (hasNext)
            {
                currentAnim = nextAnim;
                currentAnim.OnApplied();
                nextAnim = null;
                hasNext = false;
            }
            /*else if (currentAnim.IsMovingAnim)
            {
                
            }*/
        }
    }
}
