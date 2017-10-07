using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ClusterWave.Scenario.Backgrounds.BackgroundTwoStuff
{
    class VertexColorBatch : PrimitiveBatch<VertexPositionColor>
    {
        public VertexColorBatch()
            : base()
        {

        }

        public VertexColorBatch(int lineCount, int triangleCount)
            : base(lineCount, triangleCount)
        {

        }

        public void AddLine(VertexPositionColor a, VertexPositionColor b, float scale, Vector2 translation)
        {
            EnsureLineCapacity(lineVertexCount + 2);
            lines[lineVertexCount++] = a;
            lines[lineVertexCount++] = b;
        }

        public void AddLineStrip(VertexPositionColor[] arr, float scale, Vector2 translation)
        {
            int less = arr.Length - 1;
            EnsureLineCapacity(lineVertexCount + less * 2);
            VertexPositionColor prev = new VertexPositionColor(new Vector3(arr[0].Position.X * scale + translation.X, arr[0].Position.Y * scale + translation.Y, arr[0].Position.Z), arr[0].Color);
            for (int i = 0; i < less; )
            {
                lines[lineVertexCount++] = prev;
                i++;
                prev = new VertexPositionColor(new Vector3(arr[i].Position.X * scale + translation.X, arr[i].Position.Y * scale + translation.Y, arr[i].Position.Z), arr[i].Color);
                lines[lineVertexCount++] = prev;
            }
        }

        public void AddLineStrip(VertexPositionColor[] arr, float scale, Vector2 translation, Color color)
        {
            int less = arr.Length - 1;
            EnsureLineCapacity(lineVertexCount + less * 2);
            VertexPositionColor prev = new VertexPositionColor(new Vector3(arr[0].Position.X * scale + translation.X, arr[0].Position.Y * scale + translation.Y, arr[0].Position.Z), color);
            for (int i = 0; i < less; )
            {
                lines[lineVertexCount++] = prev;
                i++;
                prev = new VertexPositionColor(new Vector3(arr[i].Position.X * scale + translation.X, arr[i].Position.Y * scale + translation.Y, arr[i].Position.Z), color);
                lines[lineVertexCount++] = prev;
            }
        }
    }
}
