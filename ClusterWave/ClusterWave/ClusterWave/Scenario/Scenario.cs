using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using Lidgren.Network;
using ClusterWave.Scenario.Backgrounds;

namespace ClusterWave.Scenario
{
    /// <summary>Encapsulates a Scenario, with physics, bounds, shapes, etc. This class takes a
    /// ByteStream and turns it into a scenario, saving all the data in the stream.</summary>
    class Scenario : IDisposable
    {
        byte V1, V2;
        float _width, _height, screenScale, _halfw, _halfh;
        List<ClusterWave.Scenario.Shape> shapes;
        World world;
        Body staticBody;
        byte backgroundType;
        Vector2 powerupPos;
        Vector2[] players;
        String name;
        bool done;

        Background background;

        /// <summary>Gets the Physics World for the scenario</summary>
        public World PhysicsWorld { get { return world; } }
        /// <summary>Gets the Width (in meters)</summary>
        public float Width { get { return _width; } }
        /// <summary>Gets the Width (in meters) devided by two</summary>
        public float HalfWidth { get { return _halfw; } }
        /// <summary>Gets the Height (in meters)</summary>
        public float Height { get { return _height; } }
        /// <summary>Gets the Height (in meters) devided by two</summary>
        public float HalfHeight { get { return _halfh; } }
        /// <summary>Gets the number corresponding to which background the game should use</summary>
        public byte BackgroundType { get { return backgroundType; } }
        /// <summary>Gets the maximum amount of players the scenario supports</summary>
        public int PlayerCount { get { return players.Length; } }
        /// <summary>Gets the player's positions</summary>
        /// <remarks>This will only work on local-loaded scenarios, NO NETWORK!!!</remarks>
        public Vector2[] PlayersPos { get { return players; } }
        /// <summary>Gets the scale ratio the projection should use, also used for calculating the mouse's position.</summary>
        public float ScreenToSizeRatio { get { return screenScale; } }
        /// <summary>Gets the scenario's name</summary>
        public String Name { get { return name; } }
        /// <summary>Returns the Scenario.Backgrounds.Background object</summary>
        public Background BackgroundObject { get { return background; } }

        public bool DoneLoading { get { return done; } }

        /// <summary>Creates a scenario from a specified byte stream but doesn't create the scenario itself, just loads the basic data.</summary>
        /// <param name="stream">The stream to load data from</param>
        public Scenario(ByteStream stream)
        {
            done = true;
            V1 = stream.ReadByte();
            V2 = stream.ReadByte();

            name = stream.ReadString();

            backgroundType = (byte)(stream.ReadByte() % 3);

            _width = stream.ReadFloat();
            _height = stream.ReadFloat();
            _halfw = _width / 2f;
            _halfh = _height / 2f;

            powerupPos = stream.ReadVector2();

            players = new Vector2[stream.ReadByte()];
            for (int i = 0; i < players.Length; i++)
                players[i] = stream.ReadVector2();

            _Init(100);
            while (stream.HasNext())
            {
                switch (stream.ReadByte())
                {
                    case LineGroup.Type:
                        AddLinegroup(stream.ReadVector2Array());
                        break;
                    case Polygon.Type:
                        AddPolygon(stream.ReadVector2Array());
                        break;
                    case Rectangle.Type:
                        AddRectangle(stream.ReadVector2(), stream.ReadVector2());
                        break;
                }
            }
        }

        /// <summary>Creates a scenario for loading from network. Call CreatePacketArrive for... packet arrivals</summary>
        public Scenario()
        {
            done = false;
            _width = -1;
            _height = -1;
        }

        private void _Init(int shapeCount)
        {
            switch (backgroundType)
            {
                default:
                case 1:
                    background = new BackgroundOne();
                    break;
                case 2:
                    background = new BackgroundOne();
                    break;
                case 3:
                    background = new BackgroundOne();
                    break;
            }
            world = new World(Vector2.Zero);
            shapes = new List<Shape>(shapeCount);
            staticBody = new Body(world, Vector2.Zero, 0f, null);
            staticBody.BodyType = BodyType.Static;
            staticBody.CollisionCategories = Constants.WallsCategory;
            staticBody.CollidesWith = Category.All;
            staticBody.Friction = Constants.WallsFriction;
            staticBody.Restitution = Constants.WallsRestitution;
            AddRectangle(Vector2.Zero, new Vector2(_width, _height));
        }

        public void CreatePacketArrive(byte[] data)
        {
            ByteStream msg = new ByteStream(data);
            switch (msg.ReadByte())
            {
                case 0:
                    #region Linegroup
                    Vector2[] data_l = new Vector2[msg.ReadInt32()];
                    for (int i = 0; i < data_l.Length; i++)
                        data_l[i] = new Vector2(msg.ReadFloat(), msg.ReadFloat());
                    AddLinegroup(data_l);
                    break;
                    #endregion

                case 1:
                    #region Polygon
                    Vector2[] data_p = new Vector2[msg.ReadInt32()];
                    for (int i = 0; i < data_p.Length; i++)
                        data_p[i] = new Vector2(msg.ReadFloat(), msg.ReadFloat());
                    AddPolygon(data_p);
                    break;
                    #endregion

                case 2:
                    #region Rectangle
                    AddRectangle(new Vector2(msg.ReadFloat(), msg.ReadFloat()), new Vector2(msg.ReadFloat(), msg.ReadFloat()));
                    break;
                    #endregion

                case 3:
                    #region Data
                    backgroundType = msg.ReadByte();
                    _width = msg.ReadFloat();
                    _height = msg.ReadFloat();
                    _halfh = _height / 2f;
                    _halfw = _width / 2f;
                    _Init(msg.ReadInt32());
                    #endregion
                    break;
                case 4:
                    #region Name
                    name = msg.ReadString();
                    #endregion
                    break;
                case 254:
                    #region End
                    done = true;
                    #endregion
                    break;
            }

            /* if the first byte is 0, the rest of the packet is a Vector2[] array for a LineGroup
             * if the first byte is 1, the rest of the packet is a Vector2[] array for a Polygon.
             * if the first byte is 2, read two Vector2-s and create a rectangle with those.
             * 
             * if the first byte is 3, read another byte and it's the background. The next two floats describe width and height
             *     and one extra int tells how many shapes are coming.
             *     
             * if the first byte is 4, read a string. That's the name. */
        }

        private void AddPolygon(Vector2[] vertices)
        {
            shapes.Add(new Polygon(vertices, staticBody, background.ShapeTexture));
        }
        private void AddLinegroup(Vector2[] vertices) { shapes.Add(new LineGroup(vertices, staticBody)); }
        private void AddRectangle(Vector2 pos, Vector2 size) { shapes.Add(new Rectangle(pos, size, staticBody)); }

        /// <summary>Calles world.Step and advances the physics by Game1.DeltaTime</summary>
        public void PhysicsStep()
        {
            world.Step(Game1.DeltaTime);
        }

        /// <summary>Draws the shapes whatever no need to explain, m8</summary>
        public void DrawShapeLines(GraphicsDevice device)
        {
            Effect fx = background.ShapeLineFx;
            fx.Parameters["time"].SetValue(Game1.Time);
            fx.CurrentTechnique.Passes[0].Apply();
            for (int i = 0; i < shapes.Count; i++)
                shapes[i].DrawLines(device);
        }

        public void DrawShapeFill(GraphicsDevice device)
        {
            Effect fx = background.ShapeFillFx;
            fx.Parameters["time"].SetValue(Game1.Time);
            fx.CurrentTechnique.Passes[0].Apply();
            device.SamplerStates[0] = SamplerState.PointWrap;
            for (int i = 0; i < shapes.Count; i++)
                shapes[i].DrawFill(device, fx);
        }

        /// <summary>Draws all the light walls. Make sure to use the correct Effect, as this function doesn't control any of that stuff</summary>
        public void DrawLightWalls(GraphicsDevice device)
        {
            Effect fx = background.RayLightFx;
            fx.Parameters["time"].SetValue(Game1.Time);
            fx.CurrentTechnique.Passes[0].Apply();
            for (int i = 0; i < shapes.Count; i++)
                shapes[i].DrawLight(device);
        }

        /// <summary>Creates a projection matrix centering the scenario in the screen</summary>
        public Matrix CreateProjectionMatrix()
        {
            Vector2 size;
            if ((float)Game1.ScreenWidth / (float)Game1.ScreenHeight < (_width / _height))
            { //should try to adjust based on width
                size = new Vector2(_width, Game1.ScreenHeight * _width / Game1.ScreenWidth);
                screenScale = _width / Game1.ScreenWidth;
            }
            else
            { //should try to adjust based on height
                size = new Vector2(Game1.ScreenWidth * _height / Game1.ScreenHeight, _height);
                screenScale = _height / Game1.ScreenHeight;
            }

            size *= 1.05f;
            screenScale *= 1.05f;

            return Matrix.CreateOrthographic(size.X, -size.Y, 0f, 100f);
        }

        /// <summary>Disposes all the VertexBuffers from the shapes. If they are not disposed they will remain in memory.</summary>
        public void Dispose()
        {
            for (int i = 0; i < shapes.Count; i++)
                shapes[i].Dispose();
            background.Dispose();
        }
    }
}
