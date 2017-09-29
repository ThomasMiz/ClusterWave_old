using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ClusterWave.Particles
{
    class ParticleList : IDisposable
    {
        LinkedList<Particle> particles;

        public ParticleList()
        {
            particles = new LinkedList<Particle>();
        }

        public void Add(Particle p)
        {
            p.SetNode(particles.AddLast(p));
        }

        public void UpdateParticles()
        {
            LinkedListNode<Particle> p = particles.First;
            while (p != null)
            {
                p.Value.Update();
                p = p.Next;
            }
        }

        public void DrawParticles(SpriteBatch batch, GraphicsDevice device)
        {
            LinkedListNode<Particle> p = particles.First;
            while(p != null)
            {
                p.Value.Draw(batch, device);
                p = p.Next;
            }
        }

        public void Dispose()
        {
            LinkedListNode<Particle> p = particles.First;
            while (p != null)
            {
                p.Value.Dispose();
                p = p.Next;
            }
        }
    }
}
