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
            Game1.game.Window.Title = "shoot";
        }
    }
}
