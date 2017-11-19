using System;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace MizTK
{
    [StructLayout(LayoutKind.Sequential)]
    struct VertexColorTexture : IVertex
    {
        public static VertexDefinition Definition { get { return new VertexDefinition(AttribPointerDescription.Vector3("vPosition"), AttribPointerDescription.Vector4("vColor"), AttribPointerDescription.Vector2("vTexCoords")); } }

        public VertexDefinition VertexDefinition { get { return new VertexDefinition(AttribPointerDescription.Vector3("vPosition"), AttribPointerDescription.Vector4("vColor"), AttribPointerDescription.Vector2("vTexCoords")); } }

        public Vector3 Position;
        public Color4 Color;
        public Vector2 TexCoords;

        public VertexColorTexture(Vector3 Position, Color4 Color, Vector2 TexCoords)
        {
            this.Position = Position;
            this.Color = Color;
            this.TexCoords = TexCoords;
        }
    }
}
