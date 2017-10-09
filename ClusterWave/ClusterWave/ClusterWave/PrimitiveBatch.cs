using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ClusterWave
{
    /// <summary>
    /// Used to batch primitives for streaming them to the GPU, with each flush resetting the primitive batch to 0 items.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class PrimitiveBatch<T> : IDisposable where T : struct, IVertexType
    {
        protected int lineVertexCount, triangleVertexCount;
        protected T[] lines;
        protected T[] triangles;

        DynamicVertexBuffer lineBuffer, triangleBuffer;

        public PrimitiveBatch()
        {
            lines = new T[128];
            triangles = new T[128];

            lineBuffer = new DynamicVertexBuffer(Game1.game.GraphicsDevice, typeof(T), lines.Length, BufferUsage.WriteOnly);
            triangleBuffer = new DynamicVertexBuffer(Game1.game.GraphicsDevice, typeof(T), triangles.Length, BufferUsage.WriteOnly);
        }

        public PrimitiveBatch(int lineCount, int triangleCount)
        {
            lines = new T[lineCount];
            triangles = new T[triangleCount];

            lineBuffer = new DynamicVertexBuffer(Game1.game.GraphicsDevice, typeof(T), lines.Length, BufferUsage.WriteOnly);
            triangleBuffer = new DynamicVertexBuffer(Game1.game.GraphicsDevice, typeof(T), triangles.Length, BufferUsage.WriteOnly);
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
            EnsureTriangleCapacity(triangleVertexCount + (arr.Length-2) * 3);

            int m = arr.Length - 1;
            for (int i = 1; i < m;)
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

        public void FlushAllLineFirst(GraphicsDevice device)
        {
            FlushLines(device);
            FlushTriangles(device);
        }

        public void FlushAllTriangleFirst(GraphicsDevice device)
        {
            FlushTriangles(device);
            FlushLines(device);
        }

        public void FlushLines(GraphicsDevice device)
        {
            if (lineVertexCount != 0)
            {
                device.SetVertexBuffer(null);
                if (lineBuffer.VertexCount < lineVertexCount)
                {
                    lineBuffer.Dispose();
                    lineBuffer = new DynamicVertexBuffer(Game1.game.GraphicsDevice, typeof(T), lines.Length, BufferUsage.WriteOnly);
                }
                //Game1.game.Window.Title = lineVertexCount.ToString() + "/" + lines.Length;
                lineBuffer.SetData(lines, 0, lineVertexCount);
                device.SetVertexBuffer(lineBuffer);
                device.DrawPrimitives(PrimitiveType.LineList, 0, lineVertexCount / 2);
                lineVertexCount = 0;
            }
        }

        public void FlushTriangles(GraphicsDevice device)
        {
            if (triangleVertexCount != 0)
            {
                if (triangleBuffer.VertexCount < triangleVertexCount)
                {
                    triangleBuffer.Dispose();
                    triangleBuffer = new DynamicVertexBuffer(Game1.game.GraphicsDevice, typeof(T), triangles.Length, BufferUsage.WriteOnly);
                }
                triangleBuffer.SetData(triangles, 0, triangleVertexCount);
                device.SetVertexBuffer(triangleBuffer);
                device.DrawPrimitives(PrimitiveType.TriangleList, 0, triangleVertexCount / 3);
                triangleVertexCount = 0;
            }
        }

        public void Dispose()
        {
            lineBuffer.Dispose();
            triangleBuffer.Dispose();
        }
    }
}
