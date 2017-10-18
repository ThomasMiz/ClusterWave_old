using System;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Common;
using System.Collections.Generic;
using FarseerPhysics.Common.Decomposition;

namespace ClusterWaveServer.Scenario
{
    /// <summary>Encapsulates a Scenario with physics.</summary>
    class Scenario
    {
        float width, height;
        World world;
        Body staticBody;
        byte bgType;
        Vector2 powerupPos;
        Vector2[] players;
        String name;

        /// <summary>Gets the Physics World for the scenario</summary>
        public World PhysicsWorld { get { return world; } }
        /// <summary>Gets the Width (in meters)</summary>
        public float Width { get { return width; } }
        /// <summary>Gets the Height (in meters)</summary>
        public float Height { get { return height; } }
        /// <summary>Gets the number corresponding to which background the game should use</summary>
        public byte BackgroundType { get { return bgType; } }
        /// <summary>Gets the maximum amount of players the scenario supports</summary>
        public int PlayerCount { get { return players.Length; } }
        /// <summary>Gets the player's positions</summary>
        public Vector2[] PlayersPos { get { return players; } }
        /// <summary>Gets the scenario's name</summary>
        public String Name { get { return name; } }

        /// <summary>Creates a scenario with the given data</summary>
        public Scenario(float width, float height, byte bgType, Vector2 powerup, Vector2[] players, String name)
        {
            this.width = width;
            this.height = height;
            this.bgType = bgType;
            this.powerupPos = powerup;
            this.players = players;
            this.name = name;

            world = new World(Vector2.Zero);
            staticBody = new Body(world, Vector2.Zero, 0f, null);
            staticBody.BodyType = BodyType.Static;
            staticBody.CollisionCategories = Constants.WallsCategory;
            staticBody.Friction = Constants.WallsFriction;
            staticBody.Restitution = Constants.WallsRestitution;
            staticBody.CollidesWith = Constants.WallsCollideWith;
            AddRectangle(Vector2.Zero, new Vector2(width, height));
        }

        public void AddLinegroup(Vector2[] vertices)
        {
            Fixture f;
            if (vertices.Length == 2)
                f = staticBody.CreateFixture(new EdgeShape(vertices[0], vertices[1]));
            else
            {
                Vertices v = new Vertices(vertices);
                bool loop = (vertices[0] == vertices[vertices.Length - 1]);
                if (loop) v.RemoveAt(v.Count - 1);
                f = staticBody.CreateFixture(new ChainShape(v, loop));
            }
            f.CollisionCategories = Constants.WallsCategory;
            f.CollidesWith = Constants.WallsCollideWith;
            f.Friction = Constants.WallsFriction;
            f.Restitution = Constants.WallsRestitution;
        }
        public void AddPolygon(Vector2[] vertices)
        {
            List<Vertices> vert = Triangulate.ConvexPartition(new Vertices(vertices), TriangulationAlgorithm.Bayazit);
            for (int i = 0; i < vert.Count; i++)
            {
                Fixture f = staticBody.CreateFixture(new PolygonShape(vert[i], 1f));
                f.Friction = Constants.WallsFriction;
                f.Restitution = Constants.WallsRestitution;
                f.CollisionCategories = Constants.WallsCategory;
                f.CollidesWith = Constants.WallsCollideWith;
            }
        }
        public void AddRectangle(Vector2 topLeft, Vector2 size)
        {
            Vector2 bottomRight = topLeft + size;

            Vertices vert = new Vertices(5);
            vert.Add(new Vector2(topLeft.X, topLeft.Y));
            vert.Add(new Vector2(bottomRight.X, topLeft.Y));
            vert.Add(new Vector2(bottomRight.X, bottomRight.Y));
            vert.Add(new Vector2(topLeft.X, bottomRight.Y));
            Fixture f = staticBody.CreateFixture(new ChainShape(vert, true));
            f.Friction = Constants.WallsFriction;
            f.Restitution = Constants.WallsRestitution;
            f.CollisionCategories = Constants.WallsCategory;
            f.CollidesWith = Constants.WallsCollideWith;
        }

        /// <summary>Calles world.Step and advances the physics by Game1.DeltaTime</summary>
        public void PhysicsStep(float deltaTime)
        {
            world.Step(deltaTime);
        }
    }
}
