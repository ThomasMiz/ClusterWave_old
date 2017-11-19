using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;

namespace MizTK
{
    [StructLayout(LayoutKind.Sequential)]
    struct VertexPosition : IVertex
    {
        public static VertexDefinition Definition { get { return new VertexDefinition(AttribPointerDescription.Vector3("vPosition")); } }

        public VertexDefinition VertexDefinition { get { return new VertexDefinition(AttribPointerDescription.Vector3("vPosition")); } }

        public Vector3 Position;

        public VertexPosition(Vector3 Position)
        {
            this.Position = Position;
        }

        public VertexPosition(float x, float y, float z)
        {
            Position = new Vector3(x, y, z);
        }
    }
}
