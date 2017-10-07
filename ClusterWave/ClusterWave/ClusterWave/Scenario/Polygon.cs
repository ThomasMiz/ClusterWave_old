using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;

namespace ClusterWave.Scenario
{
    class Polygon : Shape
    {
        public const byte Type = 1;

        public Polygon(Vector2[] vertices, Body physicsBody, PrimitiveBuffer<VertexPositionTexture> buffer, PrimitiveBuffer<VertexPositionTexture> fillBuffer)
        {
            // god forbid the unreadability of this constructor
            // the readability has improved since that comment was written.

            #region CreateLineBuffer
            VertexPositionTexture[] data = new VertexPositionTexture[vertices.Length + vertices.Length + 2];
            int i = 0;
            for (; i < vertices.Length; i++)
                data[i] = CreateVertex(vertices[i]);
            data[i] = data[0];
            buffer.AddLineStrip(data, 0, vertices.Length+1);
            
            int index = 0;
            Vector2 min = vertices[0], max = min;
            for (i = 0; i < vertices.Length; i++)
            {
                data[index++] = CreateVertex(new Vector3(vertices[i], 0));
                data[index++] = CreateVertex(new Vector3(vertices[i], LightWallZ));

                if (vertices[i].X < min.X) min.X = vertices[i].X;
                else if (vertices[i].X > max.X) max.X = vertices[i].X;

                if (vertices[i].Y < min.Y) min.Y = vertices[i].Y;
                else if (vertices[i].Y > max.Y) max.Y = vertices[i].Y;
            }
            data[index++] = data[0];
            data[index++] = data[1];

            buffer.AddTriangleStrip(data);
            #endregion
            
            List<Vertices> vert = Triangulate.ConvexPartition(new Vertices(vertices), TriangulationAlgorithm.Bayazit);

            #region FillBuffer
            for (int c = 0; c < vert.Count; c++)
            {
                Vertices currentVertices = vert[c];
                #region Physics
                Fixture f = physicsBody.CreateFixture(new PolygonShape(currentVertices, 1f));
                f.CollisionCategories = Constants.WallsCategory;
                f.CollidesWith = Constants.WallsCollideWith;
                f.Friction = Constants.WallsFriction;
                f.Restitution = Constants.WallsRestitution;
                #endregion

                VertexPositionTexture z = new VertexPositionTexture(new Vector3(currentVertices[0], 0), Vector2.Zero);

                int lss = currentVertices.Count - 1;
                for (int a = 0; a < lss;)
                {
                    fillBuffer.AddTriangleVertex(z);
                    fillBuffer.AddTriangleVertex(new VertexPositionTexture(new Vector3(currentVertices[a].X, currentVertices[a].Y, 0), Vector2.Zero));
                    a++;
                    fillBuffer.AddTriangleVertex(new VertexPositionTexture(new Vector3(currentVertices[a].X, currentVertices[a].Y, 0), Vector2.Zero));
                }
                fillBuffer.AddTriangleVertex(z);
                fillBuffer.AddTriangleVertex(new VertexPositionTexture(new Vector3(currentVertices[lss].X, currentVertices[lss].Y, 0), Vector2.Zero));
                fillBuffer.AddTriangleVertex(new VertexPositionTexture(new Vector3(currentVertices[0].X, currentVertices[0].Y, 0), Vector2.Zero));
            }
            #endregion
        }
    }
}
