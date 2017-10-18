using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace MizTK
{
    class ShaderUniformList
    {
        private ShaderUniform[] data;

        public ShaderUniformList(ShaderUniform[] data)
        {
            this.data = data;
        }

        public ShaderUniform this[int index] { get { return data[index]; } }

        public ShaderUniform this[String name]
        {
            get
            {
                for (int i = 0; i < data.Length; i++)
                    if (data[i].IsName(name))
                        return data[i];
                return null;
            }
        }

        public void InformUse()
        {
            for (int i = 0; i < data.Length; i++)
                data[i].OnProgramUse();
        }
    }
}
