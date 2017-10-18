using System;
using System.Runtime.InteropServices;

using OpenTK;
using OpenTK.Graphics;

namespace MizTK
{
    [StructLayout(LayoutKind.Sequential)]
    struct VertexColor : IVertex
    {
        public static VertexDefinition Definition { get { return new VertexDefinition(AttribPointerDescription.Vector3("vPosition"), AttribPointerDescription.Vector4("vColor")); } }

        public VertexDefinition VertexDefinition { get { return new VertexDefinition(AttribPointerDescription.Vector3("vPosition"), AttribPointerDescription.Vector4("vColor")); } }

        public Vector3 Position;
        public Color4 Color;

        public VertexColor(Vector3 Position, Color4 Color)
        {
            this.Position = Position;
            this.Color = Color;
        }
    }
}
