using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ClusterWave.Scenario.Dynamic
{
    class BulletList
    {
        LinkedList<Bullet> list;

        public BulletList()
        {
            list = new LinkedList<Bullet>();
        }

        public void UpdateBullets()
        {
            LinkedListNode<Bullet> b = list.First;
            while (b != null)
            {
                b.Value.Update();
                b = b.Next;
            }
        }

        public void DrawBullets(SpriteBatch batch)
        {
            LinkedListNode<Bullet> b = list.First;
            while (b != null)
            {
                b.Value.Draw(batch);
                b = b.Next;
            }
        }
        
        /// <summary>Calls PacketArrive(msg) on the bullet with the specified id. Returns whether such a bullet was found.</summary>
        public bool PassPacketTo(int id, Lidgren.Network.NetIncomingMessage msg)
        {
            LinkedListNode<Bullet> b = list.First;
            while (b != null)
            {
                if (b.Value.id == id)
                {
                    b.Value.PacketArrive(msg);
                    return true;
                }
                b = b.Next;
            }
            return false;
        }

        public void Add(Bullet bullet)
        {
            list.AddLast(bullet);
        }

        public void Remove(LinkedListNode<Bullet> bullet)
        {
            list.Remove(bullet);
        }

        public void Remove(Bullet bullet)
        {
            list.Remove(bullet);
        }
    }
}
