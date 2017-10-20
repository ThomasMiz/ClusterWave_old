using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClusterWave.Scenario.PlayerAnimation
{
    abstract class Anim
    {
        public float startTime;
        protected PlayerAnimator animator;

        public virtual bool IsMovingAnim { get { return false; } }
        public virtual bool IsShootingAnim { get { return false; } }
        public virtual bool IsIdleAnim { get { return false; } }

        public Anim(PlayerAnimator anim, float startTime)
        {
            this.animator = anim;
            this.startTime = startTime;
        }

        public abstract void Update();

        public virtual void OnApplied() { }
    }
}
