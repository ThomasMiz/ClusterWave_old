using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ClusterWave.Scenario.Backgrounds.BackgroundTwoStuff
{
    class Line
    {
        Vector2 center;
        BackgroundTwo bg;
        float csOff, csMult;
        float t;

        public Line(Vector2 center, BackgroundTwo bg)
        {
            this.center = center;
            this.bg = bg;
            csOff = Game1.Random(2f, 6f);
            csMult = Game1.Random(0.5f) + 0.75f;
            t = Game1.Random(4f);
        }

        public void Draw(VertexColorBatch batch)
        {
            //float t = (Game1.Time + timeOff) % 4;
            t += Game1.DeltaTime;
            while(t > 4)
            {
                t -= 4;
                bg.AddParticle(new Particle(center));
            }
            float cs = Stuff.CheapSineWave(Game1.Time * csMult + csOff) * 10;
            if (t < 2)
            {
                float wid = (-t * (t - 2)) * 100;
                float min = center.X - wid, max = center.X + wid;
                batch.AddLine(
                    new VertexPositionColor(new Vector3(min, center.Y, 0), Color.DimGray),
                    new VertexPositionColor(new Vector3(max, center.Y, 0), Color.DimGray)
                );

                batch.AddLineStrip(BackgroundTwo.circle, cs, new Vector2(min, center.Y), Color.DimGray);
                batch.AddLineStrip(BackgroundTwo.circle, cs, new Vector2(max, center.Y), Color.DimGray);
            }
            else
            {
                batch.AddLineStrip(BackgroundTwo.circle, cs, center, Color.DimGray);
            }
        }
    }
}
