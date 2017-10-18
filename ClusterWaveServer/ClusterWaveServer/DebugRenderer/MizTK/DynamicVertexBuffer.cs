using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace MizTK
{
    class DynamicVertexBuffer<T> : IDisposable where T : struct, IVertex
    {
        private const BufferUsageHint hint = BufferUsageHint.DynamicDraw;
        private int vbo, vao;
        private int capacity, count, vertexByteSize;

        public int VertexCapacity { get { return capacity; } }
        public int VertexCount
        {
            get { return count; }
            set { count = Math.Max(0, Math.Min(value, capacity)); }
        }

        public DynamicVertexBuffer()
        {
            vbo = GL.GenBuffer();
            vao = GL.GenVertexArray();
            capacity = 0;

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BindVertexArray(vao);

            VertexDefinition def = new T().VertexDefinition;
            vertexByteSize = def.SizeInBytes;
            def.VertexAttribPointer();
        }

        public void SetData(T[] data)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            if (data.Length > capacity)
            {
                capacity = data.Length;
                GL.BufferData<T>(BufferTarget.ArrayBuffer, capacity * vertexByteSize, data, hint);
            }
            else
            {
                GL.BufferSubData<T>(BufferTarget.ArrayBuffer, (IntPtr)0, capacity * vertexByteSize, data);
            }
            count = capacity;
        }

        public void SetData(T[] data, int dataOffset, int dataCount, int bufferOffset)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            if (dataCount > capacity)
            {
                capacity = dataCount;
                GL.BufferData<T>(BufferTarget.ArrayBuffer, dataCount * vertexByteSize, ref data[dataOffset], hint);
            }
            else
            {
                GL.BufferSubData<T>(BufferTarget.ArrayBuffer, (IntPtr)bufferOffset, dataCount * vertexByteSize, ref data[dataOffset]);
            }
        }

        public void DrawArrays(PrimitiveType primitive, int vertexCount)
        {
            GL.BindVertexArray(vao);
            GL.DrawArrays(primitive, 0, vertexCount);
        }

        public void Dispose()
        {
            GL.DeleteBuffer(vbo);
            GL.DeleteVertexArray(vao);
        }
    }
}
