using System;
using OpenTK.Graphics;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace MizTK
{
    class ShaderUniform
    {
        protected readonly string name;
        protected readonly int location;
        protected readonly ShaderProgram program;

        public String UniformName { get { return name; } }
        public int UniformLocation { get { return location; } }

        public ShaderUniform(String name, int location, ShaderProgram program)
        {
            this.program = program;
            this.name = name;
            this.location = location;
        }

        public bool IsName(String compare)
        {
            return name.Equals(compare);
        }

        #region Set

        public void Set(float value)
        {
            program.Use();
            GL.Uniform1(location, value);
        }
        public void Set(double value)
        {
            program.Use();
            GL.Uniform1(location, value);
        }
        public void Set(int value)
        {
            program.Use();
            GL.Uniform1(location, value);
        }

        public void Set(Vector2 value)
        {
            program.Use();
            GL.Uniform2(location, ref value);
        }
        public void Set(ref Vector2 value)
        {
            program.Use();
            GL.Uniform2(location, ref value);
        }
        public void Set(Vector2d value)
        {
            program.Use();
            GL.Uniform2(location, value.X, value.Y);
        }
        public void Set(ref Vector2d value)
        {
            program.Use();
            GL.Uniform2(location, value.X, value.Y);
        }
        public void Set(int x, int y)
        {
            program.Use();
            GL.Uniform2(location, x, y);
        }

        public void Set(Vector3 value)
        {
            program.Use();
            GL.Uniform3(location, ref value);
        }
        public void Set(ref Vector3 value)
        {
            program.Use();
            GL.Uniform3(location, ref value);
        }
        public void Set(int x, int y, int z)
        {
            program.Use();
            GL.Uniform3(location, x, y, z);
        }

        public void Set(Vector4 value)
        {
            program.Use();
            GL.Uniform4(location, ref value);
        }
        public void Set(ref Vector4 value)
        {
            program.Use();
            GL.Uniform4(location, ref value);
        }
        public void Set(Vector4d value)
        {
            program.Use();
            GL.Uniform4(location, value.X, value.Y, value.Z, value.W);
        }
        public void Set(ref Vector4d value)
        {
            program.Use();
            GL.Uniform4(location, value.X, value.Y, value.Z, value.W);
        }

        public void Set(Color4 value)
        {
            program.Use();
            GL.Uniform4(location, value.R, value.G, value.B, value.A);
        }
        public void Set(ref Color4 value)
        {
            program.Use();
            GL.Uniform4(location, value.R, value.G, value.B, value.A);
        }
        public void Set(int x, int y, int z, int w)
        {
            program.Use();
            GL.Uniform4(location, x, y, z, w);
        }

        public void Set(Matrix4 value)
        {
            program.Use();
            GL.UniformMatrix4(location, false, ref value);
        }
        public void Set(ref Matrix4 value)
        {
            program.Use();
            GL.UniformMatrix4(location, false, ref value);
        }
        public void Set(Matrix4 value, bool transpose)
        {
            program.Use();
            GL.UniformMatrix4(location, transpose, ref value);
        }
        public void Set(ref Matrix4 value, bool transpose)
        {
            program.Use();
            GL.UniformMatrix4(location, transpose, ref value);
        }

        #endregion

        public virtual void OnProgramUse() { }
    }
}
