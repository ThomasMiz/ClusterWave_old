using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClusterWave.Scenario.PlayerAnimation
{
    class MoveAnim : Anim
    {
        public override bool IsMovingAnim { get { return true; } }

        public MoveAnim(PlayerAnimator anim, float startTime)
            : base(anim, startTime)
        {

        }

        public override void Update()
        {
            int frame = (int)((Game1.Time - startTime) * 10);
            if (frame >= PlayerAnimator.FrameCount)
            {
                startTime += PlayerAnimator.FrameCount / 10f;
                animator.AnimDone();
            }

            animator.playerSource.X = (frame % PlayerAnimator.FrameCount) * PlayerAnimator.FrameWidth;

        }
    }
}
