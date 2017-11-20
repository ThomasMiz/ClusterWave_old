using FarseerPhysics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using MizTK;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;

namespace ClusterWaveServer.DebugRenderer
{
    class DebugViewTK : IDisposable
    {
        const int CircleVertexCount = 64;

        public Color4 DefaultShapeColor = new Color4(0.9f, 0.7f, 0.7f, 1f);
        public Color4 InactiveShapeColor = new Color4(0.5f, 0.5f, 0.3f, 1f);
        public Color4 KinematicShapeColor = new Color4(0.5f, 0.5f, 0.9f, 1f);
        public Color4 SleepingShapeColor = new Color4(0.6f, 0.6f, 0.6f, 1f);
        public Color4 StaticShapeColor = new Color4(0.5f, 0.9f, 0.5f, 1f);

        private World world;
        private Matrix4 view, proj;
        public Matrix4 View
        {
            get { return view; }
            set
            {
                view = value;
                viewUniform.Set(value);
            }
        }
        public Matrix4 Projection
        {
            get { return proj; }
            set
            {
                proj = value;
                projUniform.Set(value);
            }
        }

        private PrimitiveBatch<VertexColor> batch;
        ShaderProgram program;
        ShaderUniform viewUniform, projUniform;

        private Vector2[] circleVertex = new Vector2[CircleVertexCount];

        public DebugViewTK(FarseerPhysics.Dynamics.World w)
        {
            world = w;
            batch = new PrimitiveBatch<VertexColor>();
            for (int i = 0; i < CircleVertexCount; i++)
            {
                float r = MathHelper.TwoPi * i / CircleVertexCount;
                circleVertex[i] = new Vector2((float)Math.Cos(r), (float)Math.Sin(r));
            }

            program = new ShaderProgram(
                "in vec3 vPosition;in vec4 vColor; uniform mat4 proj, view;varying vec4 fColor; void main(void){fColor=vColor; gl_Position=(proj * (view * vec4(vPosition, 1.0)));}",
                "varying vec4 fColor; void main(void){gl_FragColor=fColor;}",
                VertexColor.Definition
            );
            viewUniform = program.Uniforms["view"];
            projUniform = program.Uniforms["proj"];

            View = Matrix4.Identity;
            Projection = Matrix4.Identity;
        }

        private void DrawHollowCircle(Vector2 center, float radius, Color4 color)
        {
            VertexColor first = new VertexColor(new Vector3(circleVertex[0].X * radius + center.X, circleVertex[0].Y * radius + center.Y, 0), color);
            VertexColor last = first;
            for (int i = 1; i < CircleVertexCount; i++)
            {
                VertexColor c = new VertexColor(new Vector3(circleVertex[i].X * radius + center.X, circleVertex[i].Y * radius + center.Y, 0), color);
                batch.AddLine(last, c);
                last = c;
            }
            batch.AddLine(last, first);
        }

        private void DrawSolidCircle(Vector2 center, float radius, Color4 color)
        {
            VertexColor lel = new VertexColor(new Vector3(radius + center.X, center.Y, 0), color);
            for (int i = 1; i < CircleVertexCount; i++)
            {
                int im = i - 1;
                batch.AddTriangle(
                    lel,
                    new VertexColor(new Vector3(center.X + circleVertex[i].X * radius, center.Y + circleVertex[i].Y * radius, 0), color),
                    new VertexColor(new Vector3(center.X + circleVertex[im].X * radius, center.Y + circleVertex[im].Y * radius, 0), color)
                );
            }

            /*int dat = CircleVertexCount - 1;
            batch.AddTriangle(new VertexColor(
                new Vector3(center, 0), color),
                new VertexColor(new Vector3(center.X + circleVertex[0].X * radius, center.Y + circleVertex[0].Y * radius, 0), color),
                new VertexColor(new Vector3(center.X + circleVertex[dat].X * radius, center.Y + circleVertex[dat].Y * radius, 0), color)
            );*/
        }

        private void DrawTransform(Transform t)
        {
            DrawHollowCircle(t.p.ToTK(), 0.05f, Color4.Red);
            batch.AddLine(new VertexColor(new Vector3(t.p.ToTK(), 0), Color4.Blue), new VertexColor(new Vector3(t.p.X + t.q.c, t.p.Y + t.q.s, 0), Color4.Blue));
        }

        public void RenderDebug()
        {
            //foreach (Body b in world.BodyList)
            for (int xdddd = 0; xdddd < world.BodyList.Count; xdddd++)
            {
                Body b = world.BodyList[xdddd];
                Transform transform;
                b.GetTransform(out transform);
                DrawTransform(transform);

                Color4 col = GetColorFor(b);
                Color4 light = new Color4(col.R, col.G, col.B, col.A * 0.5f);
                //Matrix4 mat = Matrix4.CreateRotationZ(b.Rotation) * Matrix4.CreateTranslation(b.Position.X, b.Position.Y, 0);

                //foreach (Fixture f in b.FixtureList)
                if (b != null && b.FixtureList != null)
                    for (int nose = 0; nose < b.FixtureList.Count; nose++)
                    {
                        Fixture f = b.FixtureList[nose];
                        Vertices v;
                        switch (f.Shape.ShapeType)
                        {
                            case ShapeType.Polygon:
                                #region DrawPolygon
                                PolygonShape poly = (PolygonShape)f.Shape;
                                v = poly.Vertices;
                                VertexColor[] arr = new VertexColor[v.Count];
                                for (int i = 0; i < arr.Length; i++)
                                {
                                    Microsoft.Xna.Framework.Vector2 eso = v[i];
                                    arr[i] = new VertexColor(new Vector3(MathUtils.Mul(ref transform, ref eso).ToTK()), light);
                                }
                                batch.AddTriangleFan(arr);
                                for (int i = 0; i < arr.Length; i++)
                                    arr[i].Color.A = col.A;
                                batch.AddLineStrip(arr);
                                batch.AddLine(arr[arr.Length - 1], arr[0]);

                                #endregion
                                break;
                            case ShapeType.Edge:
                                #region DrawEdge
                                EdgeShape edge = (EdgeShape)f.Shape;
                                batch.AddLine(new VertexColor(new Vector3(edge.Vertex1.ToTK(), 0), col), new VertexColor(new Vector3(edge.Vertex2.ToTK(), 0), col));
                                #endregion
                                break;
                            case ShapeType.Chain:
                                #region DrawChain
                                ChainShape chain = (ChainShape)f.Shape;
                                v = chain.Vertices;
                                VertexColor last = new VertexColor(new Vector3(v[0].ToTK(), 0), col);
                                for (int i = 1; i < v.Count; i++)
                                {
                                    VertexColor c = new VertexColor(new Vector3(v[i].ToTK(), 0), col);
                                    batch.AddLine(last, c);
                                    last = c;
                                }
                                #endregion
                                break;
                            case ShapeType.Circle:
                                #region DrawCircle
                                CircleShape circ = (CircleShape)f.Shape;
                                DrawSolidCircle(b.Position.ToTK(), circ.Radius, light);
                                DrawHollowCircle(b.Position.ToTK(), circ.Radius, col);
                                #endregion
                                break;
                        }
                    }
            }

            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            program.Use();
            batch.FlushAllTriangleFirst();
        }

        private Color4 GetColorFor(Body b)
        {
            if (b.IsStatic)
                return StaticShapeColor;
            if (b.IsKinematic)
                return KinematicShapeColor;
            if (!b.Awake)
                return SleepingShapeColor;
            if (!b.Enabled)
                return InactiveShapeColor;
            return DefaultShapeColor;
        }

        public void Dispose()
        {
            batch.Dispose();
            program.Dispose();
        }
    }
}
