using System;
using System.Runtime.InteropServices;

namespace MizTK
{
    [StructLayout(LayoutKind.Sequential)]
    struct Color4
    {
        public float R, G, B, A;

        public Color4(float R, float G, float B, float A)
        {
            this.R = R;
            this.G = G;
            this.B = B;
            this.A = A;
        }
    }
}