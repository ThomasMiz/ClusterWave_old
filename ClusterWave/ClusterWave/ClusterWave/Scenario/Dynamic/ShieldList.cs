using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ClusterWave.Scenario.Dynamic
{
    class ShieldList
    {
        List<Shield> list;

        public ShieldList()
        {
            list = new List<Shield>(5);
        }

        public void DrawShields(GraphicsDevice device)
        {
            device.SamplerStates[0] = SamplerState.PointWrap;
            for (int i = 0; i < list.Count; i++)
                list[i].Draw(device);
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

        public bool PassPacketTo(int id, Lidgren.Network.NetIncomingMessage msg)
        {
            for (int i = 0; i < list.Count; i++)
                if (list[i].id == id)
                {
                    list[i].OnPacketArrive(msg);
                    return true;
                }
            return false;
        }
    }
}
