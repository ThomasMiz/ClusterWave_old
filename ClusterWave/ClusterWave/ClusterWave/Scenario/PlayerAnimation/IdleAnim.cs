using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClusterWave.Scenario.PlayerAnimation
{
    class IdleAnim : Anim
    {
        public override bool IsIdleAnim { get { return true; } }

        public IdleAnim(PlayerAnimator anim, float startTime)
            : base(anim, startTime)
        {

        }

        public override void Update()
        {
            Game1.game.Window.Title = "idle";
        }

        public override void OnApplied()
        {
            animator.gunSource.X = 0;
            animator.gunSource.Y = 0;
            animator.playerSource.X = 0;
            animator.playerSource.Y = 0;
        }
    }
}
