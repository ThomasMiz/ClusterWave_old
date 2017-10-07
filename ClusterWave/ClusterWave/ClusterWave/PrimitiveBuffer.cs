using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ClusterWave
{
    /// <summary>
    /// Used to batch primitives for streaming them to the GPU, with each flush resetting the primitive batch to 0 items.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class PrimitiveBuffer<T> where T : struct, IVertexType
    {
        protected int lineVertexCount, triangleVertexCount;
        protected T[] lines;
        protected T[] triangles;

        public PrimitiveBuffer()
        {
            lines = new T[128];
            triangles = new T[128];
        }

        public PrimitiveBuffer(int lineCount, int triangleCount)
        {
            lines = new T[lineCount];
            triangles = new T[triangleCount];
        }

        private void ExpandLineList(int newCapacity)
        {
            T[] old = lines;
            lines = new T[newCapacity];
            for (int i = 0; i < old.Length; i++)
                lines[i] = old[i];
        }
        private void ExpandTriangleList(int newCapacity)
        {
            T[] old = triangles;
            triangles = new T[newCapacity];
            for (int i = 0; i < old.Length; i++)
                triangles[i] = old[i];
        }

        public void EnsureLineCapacity(int capacity)
        {
            if (lines.Length <= capacity)
                ExpandLineList(Math.Max(lines.Length + lines.Length, capacity + lines.Length));
        }
        public void EnsureTriangleCapacity(int capacity)
        {
            if (triangles.Length <= capacity)
                ExpandTriangleList(Math.Max(triangles.Length + triangles.Length, capacity + triangles.Length));
        }

        public void AddLine(T a, T b)
        {
            EnsureLineCapacity(lineVertexCount + 2);
            lines[lineVertexCount++] = a;
            lines[lineVertexCount++] = b;
        }

        public void AddLineList(T[] arr)
        {
            EnsureLineCapacity(lineVertexCount + arr.Length);
            for (int i = 0; i < arr.Length; i++)
                lines[lineVertexCount++] = arr[i];
        }

        public void AddLineList(T[] arr, int index, int count)
        {
            EnsureLineCapacity(lineVertexCount + count);
            int max = index + count;
            for (int i = index; i < max; i++)
                lines[lineVertexCount++] = arr[i];
        }

        public void AddLineStrip(T[] arr)
        {
            int less = arr.Length - 1;
            EnsureLineCapacity(lineVertexCount + less * 2);
            for (int i = 0; i < less; )
            {
                lines[lineVertexCount++] = arr[i];
                lines[lineVertexCount++] = arr[++i];
            }
        }

        public void AddLineStrip(T[] arr, int index, int count)
        {
            int dat = count - 1;
            EnsureLineCapacity(lineVertexCount + dat * 2);
            dat += index;
            for (int i = index; i < dat; )
            {
                lines[lineVertexCount++] = arr[i];
                lines[lineVertexCount++] = arr[++i];
            }
        }

        public void AddTriangleList(T[] arr)
        {
            EnsureTriangleCapacity(triangleVertexCount + arr.Length);
            for (int i = 0; i < arr.Length; i++)
                triangles[triangleVertexCount++] = arr[i];
        }

        public void AddTriangleList(T[] arr, int index, int count)
        {
            EnsureTriangleCapacity(triangleVertexCount + count);
            int max = index + count;
            for (int i = index; i < max; i++)
                triangles[triangleVertexCount++] = arr[i];
        }

        public void AddTriangleStrip(T[] arr)
        {
            int less = arr.Length - 2;
            int max = less * 3 + triangleVertexCount;
            EnsureTriangleCapacity(max);

            for (int i = 0; i < less; )
            {
                triangles[triangleVertexCount++] = arr[i];
                triangles[triangleVertexCount++] = arr[++i];
                triangles[triangleVertexCount++] = arr[i + 1];
            }
        }

        public void AddTriangleStrip(T[] arr, int index, int count)
        {
            int dat = arr.Length - 2;
            EnsureTriangleCapacity(dat * 3 + triangleVertexCount);
            dat += index;
            for (int i = index; i < dat; )
            {
                triangles[triangleVertexCount++] = arr[i];
                triangles[triangleVertexCount++] = arr[++i];
                triangles[triangleVertexCount++] = arr[i + 1];
            }
        }

        public void AddTriangleFan(T[] arr)
        {
            EnsureTriangleCapacity(triangleVertexCount + (arr.Length - 2) * 3);

            int m = arr.Length - 1;
            for (int i = 1; i < m; )
            {
                triangles[triangleVertexCount++] = arr[0];
                triangles[triangleVertexCount++] = arr[i];
                triangles[triangleVertexCount++] = arr[++i];
            }
        }

        public void AddTriangleVertex(T vertex)
        {
            EnsureTriangleCapacity(triangleVertexCount + 1);
            triangles[triangleVertexCount++] = vertex;
        }

        public void AddLineVertex(T vertex)
        {
            EnsureLineCapacity(lineVertexCount + 1);
            lines[lineVertexCount++] = vertex;
        }

        public void AddTriangle(T a, T b, T c)
        {
            EnsureTriangleCapacity(triangleVertexCount + 3);
            triangles[triangleVertexCount++] = a;
            triangles[triangleVertexCount++] = b;
            triangles[triangleVertexCount++] = c;
        }

        public void ClearLines()
        {
            lineVertexCount = 0;
        }

        public void ClearTriangles()
        {
            triangleVertexCount = 0;
        }

        public VertexBuffer CreateTriangleBuffer()
        {
            if (triangleVertexCount == 0)
                return null;
            VertexBuffer buffer = new VertexBuffer(Game1.game.GraphicsDevice, typeof(T), triangleVertexCount, BufferUsage.WriteOnly);
            buffer.SetData(triangles, 0, triangleVertexCount);
            return buffer;
        }

        public VertexBuffer CreateLineBuffer()
        {
            if (lineVertexCount == 0)
                return null;
            VertexBuffer buffer = new VertexBuffer(Game1.game.GraphicsDevice, typeof(T), lineVertexCount, BufferUsage.WriteOnly);
            buffer.SetData(lines, 0, lineVertexCount);
            return buffer;
        }
    }
}
