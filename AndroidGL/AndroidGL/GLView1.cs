using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES30;
using OpenTK.Platform;
using OpenTK.Platform.Android;
using Android.Views;
using Android.Content;
using Android.Util;
using MizTK;

namespace AndroidGL
{
    class GLView1 : AndroidGameView
    {
        Random r = new Random();
        float time = 0;
        PrimitiveBatch<VertexColor> batch;
        ShaderProgram program;

        public GLView1(Context context) : base(context)
        {

        }

        float rand(float max)
        {
            return (float)r.NextDouble() * max;
        }

        float rand(float min, float max)
        {
            return (float)r.NextDouble() * (max - min) + min;
        }

        // This gets called when the drawing surface is ready
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            

            batch = new PrimitiveBatch<VertexColor>();
            program = new ShaderProgram(
                "in vec3 vPosition; in vec4 vColor; varying vec4 fColor; void main(void){ gl_Position = vPosition; fColor = vColor; }",
                "varying vec4 fColor; void main(void) { gl_FragColor = fColor; }",
                VertexColor.Definition
            );

            Run();
        }

        // This method is called everytime the context needs
        // to be recreated. Use it to set any egl-specific settings
        // prior to context creation
        //
        // In this particular case, we demonstrate how to set
        // the graphics mode and fallback in case the device doesn't
        // support the defaults
        protected override void CreateFrameBuffer()
        {
            // the default GraphicsMode that is set consists of (16, 16, 0, 0, 2, false)
            try
            {
                Log.Verbose("GLCube", "Loading with default settings");

                // if you don't call this, the context won't be created
                base.CreateFrameBuffer();
                return;
            }
            catch (Exception ex)
            {
                Log.Verbose("GLCube", "{0}", ex);
            }

            // this is a graphics setting that sets everything to the lowest mode possible so
            // the device returns a reliable graphics setting.
            try
            {
                Log.Verbose("GLCube", "Loading with custom Android settings (low mode)");
                GraphicsMode = new AndroidGraphicsMode(0, 0, 0, 0, 0, false);

                // if you don't call this, the context won't be created
                base.CreateFrameBuffer();
                return;
            }
            catch (Exception ex)
            {
                Log.Verbose("GLCube", "{0}", ex);
            }
            throw new Exception("Can't load egl, aborting");
        }

        // This gets called on each frame render
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            for (int i = 0; i < 50; i++)
            {
                batch.AddLine(
                    new VertexColor(new Vector3(rand(-1, 1),rand(-1, 1),rand(-1, 1)), new Color4(rand(1),rand(1),rand(1),1f)),
                    new VertexColor(new Vector3(rand(-1, 1),rand(-1, 1),rand(-1, 1)), new Color4(rand(1),rand(1),rand(1),1f))
                );
            }

            time += (float)e.Time;
            base.OnRenderFrame(e);

            GL.ClearColor(1f, 0f, 0f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            program.Use();

            batch.Flush();

            SwapBuffers();
        }

        protected override void OnClosed(EventArgs e)
        {
            batch.Dispose();
        }
    }
}
