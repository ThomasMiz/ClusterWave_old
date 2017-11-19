using System;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace MizTK
{
    class ShaderProgram : IDisposable
    {
        private static int boundProgram = Int32.MinValue;
        private readonly int vs, fs, program;
        public int Handle { get { return program; } }

        ShaderUniformList uniforms;
        public ShaderUniformList Uniforms { get { return uniforms; } }

        public ShaderProgram(String vertexShader, String fragmentShader, VertexDefinition definition)
        {
            vs = CreateShader(vertexShader, ShaderType.VertexShader);
            fs = CreateShader(fragmentShader, ShaderType.FragmentShader);

            program = CreateProgram(vs, fs, definition);

            InitUniforms();

        }

        private void InitUniforms()
        {
            int total = -1;
            GL.GetProgram(program, GetProgramParameterName.ActiveUniforms, out total);
            ShaderUniform[] unif = new ShaderUniform[total];
            for (int i = 0; i < total; i++)
            {
                StringBuilder name = new StringBuilder(64);
                String s;
                int len, size;
                ActiveUniformType type;
                GL.GetActiveUniform(program, i, 64, out len, out size, out type, name);
                s = name.ToString();
                int location = GL.GetUniformLocation(program, s);
                unif[i] = new ShaderUniform(s, location, this);
            }

            uniforms = new ShaderUniformList(unif);
        }
        
        public void Use()
        {
            if (boundProgram != program)
            {
                GL.UseProgram(program);
                boundProgram = program;
                uniforms.InformUse();
            }
        }

        public void Dispose()
        {
            GL.DeleteShader(vs);
            GL.DeleteShader(fs);
            GL.DeleteProgram(program);
        }

        private static int CreateShader(String code, ShaderType type)
        {
            int shader = GL.CreateShader(type);
            GL.ShaderSource(shader, code);
            GL.CompileShader(shader);

            int parm = -1;
            GL.GetShader(shader, ShaderParameter.CompileStatus, out parm);
            Console.WriteLine("-------------------");
            if (parm != 1)
                Console.WriteLine("Shader compiled unsuccessfully.");
            String text;
            GL.GetShaderInfoLog(shader, out text);
            Console.WriteLine(text);

            return shader;
        }

        private static int CreateProgram(int vs, int fs, VertexDefinition definition)
        {
            int program = GL.CreateProgram();
            GL.AttachShader(program, vs);
            GL.AttachShader(program, fs);

            definition.BindAttribLocation(program);

            GL.LinkProgram(program);

            GL.DetachShader(program, vs);
            GL.DetachShader(program, fs);

            /*int attribCount; //this seems to not be necessary after all
            GL.GetProgram(program, GetProgramParameterName.ActiveAttributes, out attribCount);
            for (int i = 0; i < attributes.Length; i++)
            {
                //GL.BindAttribLocation(program, i, attributes[i]);
                int length, size;
                ActiveAttribType type;
                StringBuilder name = new StringBuilder(64);
                GL.GetActiveAttrib(program, i, 64, out length, out size, out type, name);
            }*/ 

            return program;
        }
    }
}
