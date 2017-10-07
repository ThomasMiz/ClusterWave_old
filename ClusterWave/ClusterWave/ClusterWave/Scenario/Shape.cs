using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;

namespace ClusterWave.Scenario
{
    abstract class Shape
    {
        protected const float LightWallZ = 1;

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
