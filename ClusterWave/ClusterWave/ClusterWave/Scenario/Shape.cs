using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;

namespace ClusterWave.Scenario
{
    abstract class Shape : IDisposable
    {
        protected const float LightWallZ = 1;

        protected VertexBuffer lineBuffer, lightBuffer;
        protected int linePrimitiveCount, lightPrimitiveCount;
        public virtual void DrawLines(GraphicsDevice device)
        {
            device.SetVertexBuffer(lineBuffer);
            device.DrawPrimitives(PrimitiveType.LineStrip, 0, linePrimitiveCount);
        }
        public virtual void DrawLight(GraphicsDevice device)
        {
            device.SetVertexBuffer(lightBuffer);
            device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, lightPrimitiveCount);
        }

        public virtual void DrawFill(GraphicsDevice device) { }

        public virtual void Dispose()
        {
            lineBuffer.Dispose();
            lightBuffer.Dispose();
        }

        protected static VertexPositionTexture CreateVertex(Vector2 pos)
        {
            return new VertexPositionTexture(new Vector3(pos.X, pos.Y, 1), pos);
        }
        protected static VertexPositionTexture CreateVertex(Vector3 pos)
        {
            return new VertexPositionTexture(pos, new Vector2(pos.X, pos.Y));
        }
    }
}
