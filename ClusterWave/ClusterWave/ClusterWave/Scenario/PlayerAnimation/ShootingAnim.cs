using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClusterWave.Scenario.PlayerAnimation
{
    class ShootingAnim : Anim
    {
        public override bool IsShootingAnim { get { return true; } }

        public ShootingAnim(PlayerAnimator anim, float startTime)
            : base(anim, startTime)
        {
            
        }

        public override void Update()
        {
            int frame = (int)((Game1.Time - startTime) * 10);

            if (frame >= PlayerAnimator.FrameCount)
            {
                animator.playerSource.X = 0;
                animator.gunSource.Y = 0;
                animator.AnimDone();
            }
            else
            {
                animator.playerSource.X = frame * PlayerAnimator.FrameWidth;
            }
        }

        public override void OnApplied()
        {
            animator.gunSource.Y = PlayerAnimator.FrameHeight;
        }
    }
}
