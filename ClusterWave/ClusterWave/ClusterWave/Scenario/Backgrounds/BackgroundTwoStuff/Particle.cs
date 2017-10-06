using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ClusterWave.Scenario.Backgrounds.BackgroundTwoStuff
{
    class Particle
    {
        public LinkedListNode<Particle> node;

        Vector2 pos, dir;

        public Particle(Vector2 pos)
        {
            this.pos = pos;
            this.dir = new Vector2(Game1.Random(-500, 500), Game1.Random(-500, 500));
        }

        public void Draw(VertexColorBatch batch)
        {
            pos += dir * Game1.DeltaTime;
            //if (pos.X < 0 || pos.Y < 0 || pos.X > Game1.ScreenWidth || pos.Y > Game1.ScreenHeight)
            //    node.List.Remove(node);
            batch.AddLineStrip(BackgroundTwo.circle, 5f, pos);
        }
    }
}
