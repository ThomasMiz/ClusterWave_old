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
            int x = (animator.playerSource.X + PlayerAnimator.FrameWidth);
            if (x > PlayerAnimator.FrameWidth * 7)
            {
                x = 0;
                animator.AnimDone();
            }
            animator.playerSource.X = x;

            Game1.game.Window.Title = "move";
        }
    }
}
