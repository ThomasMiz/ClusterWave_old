using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ClusterWaveServer.Scenarios.Dynamic
{
    class ShieldList
    {
        List<Shield> list;

        public ShieldList()
        {
            list = new List<Shield>(5);
        }

        public void Add(Shield shield)
        {
            list.Add(shield);
            shield.list = this;
        }

        public void Remove(Shield shield)
        {
            list.Remove(shield);
        }
    }
}
