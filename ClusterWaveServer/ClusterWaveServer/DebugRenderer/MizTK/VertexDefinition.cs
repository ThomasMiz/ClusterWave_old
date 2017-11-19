using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace MizTK
{
    /// <summary>
    /// Can only be used for single vertex buffer stuff, positions, colors, textures and all in one buffer.
    /// </summary>
    class VertexDefinition
    {
        AttribPointer[] data;
        int stride;

        public int SizeInBytes { get { return stride; } }

        public VertexDefinition(params AttribPointerDescription[] description)
        {
            data = new AttribPointer[description.Length];
            stride = 0;
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = new AttribPointer(description[i].size, description[i].type, description[i].normalize, stride, description[i].name);
                stride += description[i].size * 4; //this should actually check the type and decide how much to multiply by
            }
        }

        public void VertexAttribPointer()
        {
            for (int i = 0; i < data.Length; i++)
            {
                GL.VertexAttribPointer(i, data[i].size, data[i].type, data[i].normalize, stride, data[i].offset);
                GL.EnableVertexAttribArray(i);
            }
        }

        public void BindAttribLocation(int program)
        {
            for (int i = 0; i < data.Length; i++)
                GL.BindAttribLocation(program, i, data[i].name);
        }
    }

    struct AttribPointer
    {
        public int size, offset;
        public VertexAttribPointerType type;
        public bool normalize;
        public String name;

        public AttribPointer(int size, VertexAttribPointerType type, bool normalize, int offset, String name)
        {
            this.name = name;
            this.size = size;
            this.type = type;
            this.normalize = normalize;
            this.offset = offset;
        }
    }

    struct AttribPointerDescription
    {
        public static AttribPointerDescription Vector4(String name)
        {
            return new AttribPointerDescription(4, VertexAttribPointerType.Float, name);
        }
        public static AttribPointerDescription Vector3(String name)
        {
            return new AttribPointerDescription(3, VertexAttribPointerType.Float, name);
        }
        public static AttribPointerDescription Vector2(String name)
        {
            return new AttribPointerDescription(2, VertexAttribPointerType.Float, name);
        }
        public static AttribPointerDescription Float(String name)
        {
            return new AttribPointerDescription(1, VertexAttribPointerType.Float, name);
        }

        public int size;
        public VertexAttribPointerType type;
        public bool normalize;
        public String name;

        public AttribPointerDescription(int size, VertexAttribPointerType type, bool normalize, String name)
        {
            this.name = name;
            this.size = size;
            this.type = type;
            this.normalize = normalize;
        }

        public AttribPointerDescription(int size, VertexAttribPointerType type, String name)
        {
            this.name = name;
            this.size = size;
            this.type = type;
            this.normalize = false;
        }
    }
}
