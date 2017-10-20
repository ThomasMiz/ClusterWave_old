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

        }

        public override void OnApplied()
        {
            animator.gunSource.Y = 0;
            animator.playerSource.X = 0;
            animator.playerSource.Y = 0;
        }
    }
}
