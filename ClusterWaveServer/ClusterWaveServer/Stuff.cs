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
    }
}
