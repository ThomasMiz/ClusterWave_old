using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ClusterWaveServer.Scenario.Dynamic
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

        public void Add(Bullet bullet)
        {
            bullet.SetList(list, list.AddLast(bullet));
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
