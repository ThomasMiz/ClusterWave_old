using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;

namespace ClusterWave.Scenario
{
    class LineGroup : Shape
    {
        public const byte Type = 0;

        public LineGroup(Vector2[] vertices, Body physicsBody, PrimitiveBuffer<VertexPositionTexture> buffer)
        {
            VertexPositionTexture[] data = new VertexPositionTexture[vertices.Length + vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
                data[i] = CreateVertex(vertices[i]);
            buffer.AddLineStrip(data, 0, vertices.Length);

            int index = 0;
            for (int i = 0; i < vertices.Length; i++)
            {
                data[index++] = CreateVertex(new Vector3(vertices[i], 0));
                data[index++] = CreateVertex(new Vector3(vertices[i], LightWallZ));
            }
            buffer.AddTriangleStrip(data);

            Fixture f;
            if (vertices.Length == 2)
                f = physicsBody.CreateFixture(new EdgeShape(vertices[0], vertices[1]));
            else
            {
                Vertices v = new Vertices(vertices);
                bool chain = (vertices[0] == vertices[vertices.Length-1]);
                if (chain) v.RemoveAt(v.Count - 1);
                f = physicsBody.CreateFixture(new ChainShape(v, chain));
            }
            f.CollisionCategories = Constants.WallsCategory;
            f.CollidesWith = Constants.WallsCollideWith;
            f.Friction = Constants.WallsFriction;
            f.Restitution = Constants.WallsRestitution;
        }
    }
}
