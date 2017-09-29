using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ClusterWave.Particles
{
    abstract class Particle : IDisposable
    {
        private LinkedListNode<Particle> node;

        public abstract void Update();
        public abstract void Draw(SpriteBatch batch, GraphicsDevice device);

        public void SetNode(LinkedListNode<Particle> node)
        {
            this.node = node;
        }

        protected void GetRekkt()
        {
            node.List.Remove(node);
            Dispose();
        }

        public virtual void Dispose()
        {

        }
    }
}
