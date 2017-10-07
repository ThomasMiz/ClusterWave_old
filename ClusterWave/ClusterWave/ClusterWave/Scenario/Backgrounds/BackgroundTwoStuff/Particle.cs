using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ClusterWave.Scenario.Backgrounds.BackgroundTwoStuff
{
    class Particle
    {
        const float Speed = 300, elapsed = 0.025f;
        const int TrailLength = 8;
        public LinkedListNode<Particle> node;

        Vector2 pos, dir;
        Color[] colors;
        Vector2[] oldPos;
        int index;
        float hue, next;

        public Particle(Vector2 pos)
        {
            this.pos = pos;
            float r = Game1.Random(MathHelper.TwoPi);
            this.dir = new Vector2((float)Math.Cos(r) * Speed, (float)Math.Sin(r) * Speed);
            hue = Game1.Random();
            colors = new Color[TrailLength];
            oldPos = new Vector2[TrailLength];
            for (int i = 0; i < TrailLength; i++)
                oldPos[i] = new Vector2(-1024);
            index = 0;
            colors[0] = Stuff.ColorFromHue(hue);
            next = Game1.Time;
        }

        public void Draw(VertexColorBatch batch)
        {
            if (Game1.Time > next)
            {
                hue += elapsed;
                index = (index + 1) % TrailLength;
                colors[index] = Stuff.ColorFromHue(hue);

                pos += dir * elapsed;
                oldPos[index] = pos;
                next += elapsed;

                if (pos.X < -250 || pos.Y < -250 || pos.X > Game1.ScreenWidthXtra || pos.Y > Game1.ScreenHeightXtra)
                {
                    node.List.Remove(node);
                    return;
                }
            }

            int i = index;
            for (int c = 0; c < TrailLength; c++)
            {
                batch.AddLineStrip(BackgroundTwo.circle, 5f, oldPos[i], colors[i]);
                i = (i + 1) % TrailLength;
            }
        }
    }
}
