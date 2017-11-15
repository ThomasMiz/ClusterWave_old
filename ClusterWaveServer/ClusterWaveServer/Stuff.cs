using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClusterWaveServer
{
    class SosUnPelotudoException : Exception
    {
        public SosUnPelotudoException() : base("sos un pelotudo") { }
        public SosUnPelotudoException(String msg) : base(msg) { }
    }

    static class Stuff
    {
        public static OpenTK.Vector2 ToTK(this Microsoft.Xna.Framework.Vector2 xna)
        {
            return new OpenTK.Vector2(xna.X, xna.Y);
        }

        public static Microsoft.Xna.Framework.Vector2 ToXNA(this OpenTK.Vector2 tk){
            return new Microsoft.Xna.Framework.Vector2(tk.X, tk.Y);
        }
        /// <summary>Rotates a vector around (0, 0)</summary>
        /// <param name="vec">A reference to the vector to rotate</param>
        /// <param name="rot">The rotation in radians</param>
        public static void RotateVector(this Microsoft.Xna.Framework.Vector2 vec, float rot)
        {
            float cos = (float)Math.Cos(rot);
            float sin = (float)Math.Sin(rot);
            float x = vec.X, y = vec.Y;
            vec.X = cos * x - sin * y;
            vec.Y = sin * x + cos * y;
        }
    }
}
