using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;

namespace ClusterWave.Scenario
{
    class RectangleShape : Shape
    {
        public const byte Type = 2;

        public RectangleShape(Vector2 topLeft, Vector2 size, Body physicsBody)
        {
            linePrimitiveCount = 4;
            lightPrimitiveCount = 8;
            Vector2 bottomRight = topLeft + size;

            lineBuffer = new VertexBuffer(Game1.game.GraphicsDevice, typeof(VertexPositionTexture), 5, BufferUsage.WriteOnly);
            VertexPositionTexture[] data = new VertexPositionTexture[10];
            data[0] = CreateVertex(new Vector2(topLeft.X, topLeft.Y));
            data[1] = CreateVertex(new Vector2(bottomRight.X, topLeft.Y));
            data[2] = CreateVertex(new Vector2(bottomRight.X, bottomRight.Y));
            data[3] = CreateVertex(new Vector2(topLeft.X, bottomRight.Y));
            data[4] = data[0];
            lineBuffer.SetData(data, 0, 5);

            lightBuffer = new VertexBuffer(Game1.game.GraphicsDevice, typeof(VertexPositionTexture), 10, BufferUsage.WriteOnly);
            data[0] = CreateVertex(new Vector3(topLeft.X, topLeft.Y, 0));
            data[1] = CreateVertex(new Vector3(topLeft.X, topLeft.Y, LightWallZ));
            data[2] = CreateVertex(new Vector3(bottomRight.X, topLeft.Y, 0));
            data[3] = CreateVertex(new Vector3(bottomRight.X, topLeft.Y, LightWallZ));
            data[4] = CreateVertex(new Vector3(bottomRight.X, bottomRight.Y, 0));
            data[5] = CreateVertex(new Vector3(bottomRight.X, bottomRight.Y, LightWallZ));
            data[6] = CreateVertex(new Vector3(topLeft.X, bottomRight.Y, 0));
            data[7] = CreateVertex(new Vector3(topLeft.X, bottomRight.Y, LightWallZ));
            data[8] = data[0];
            data[9] = data[1];
            lightBuffer.SetData(data);

            Vertices vert = new Vertices(5);
            vert.Add(new Vector2(topLeft.X, topLeft.Y));
            vert.Add(new Vector2(bottomRight.X, topLeft.Y));
            vert.Add(new Vector2(bottomRight.X, bottomRight.Y));
            vert.Add(new Vector2(topLeft.X, bottomRight.Y));
            Fixture f = physicsBody.CreateFixture(new ChainShape(vert, true));
            f.CollisionCategories = Constants.WallsCategory;
            f.CollidesWith = Constants.WallsCollideWith;
            f.Friction = Constants.WallsFriction;
            f.Restitution = Constants.WallsRestitution;
        }
    }
}
