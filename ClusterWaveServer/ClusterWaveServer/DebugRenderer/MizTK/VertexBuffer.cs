using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace MizTK
{
    class VertexBuffer<T> : IDisposable where T : struct, IVertex
    {
        private int vbo, vao;
        private int count, vertexByteSize;
        BufferUsageHint hint;

        public int VertexCount { get { return count; } }

        public VertexBuffer(BufferUsageHint hint = BufferUsageHint.StaticDraw)
        {
            this.hint = hint;
            vbo = GL.GenBuffer();
            count = 0;
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);
            VertexDefinition def = new T().VertexDefinition;
            def.VertexAttribPointer();
            vertexByteSize = def.SizeInBytes;
        }

        public VertexBuffer(T[] data, BufferUsageHint hint = BufferUsageHint.StaticDraw)
        {
            this.hint = hint;
            vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            VertexDefinition definition = data[0].VertexDefinition;

            GL.BufferData<T>(BufferTarget.ArrayBuffer, data.Length * definition.SizeInBytes, data, hint);

            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);

            definition.VertexAttribPointer();
            count = data.Length;
            vertexByteSize = definition.SizeInBytes;
        }

        public void ChangeData(T[] data, int offset, int vertexCount)
        {
            GL.BufferSubData<T>(BufferTarget.ArrayBuffer, (IntPtr)offset, vertexCount * vertexByteSize, data);
        }

        public void SetData(T[] data)
        {
            count = data.Length;
            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * vertexByteSize, data, hint);
        }

        public void DrawArrays(PrimitiveType type)
        {
            GL.BindVertexArray(vao);
            GL.DrawArrays(type, 0, count);
        }

        public void DrawArrays(PrimitiveType type, int primitiveCount)
        {
            GL.BindVertexArray(vao);
            GL.DrawArrays(type, 0, primitiveCount);
        }

        public void DrawArrays(PrimitiveType type, int first, int count)
        {
            GL.BindVertexArray(vao);
            GL.DrawArrays(type, first, count);
        }

        public void Dispose()
        {
            GL.DeleteBuffer(vbo);
            GL.DeleteVertexArray(vao);
        }
    }
}
