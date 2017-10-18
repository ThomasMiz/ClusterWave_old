using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ClusterWave
{
    static class Stuff
    {
        public static Color ColorFromHue(float hue)
        {
            hue %= 1;
            hue *= 6;
            return new Color(Math.Abs(hue - 3) - 1, 2 - Math.Abs(hue - 2), 2 - Math.Abs(hue - 4));
        }

        public static void SaturateColor(ref Color color, float s)
        {
            color.R = (byte)(255 - color.R * s);
            color.G = (byte)(255 - color.G * s);
            color.B = (byte)(255 - color.B * s);
        }

        public static void MultiplyColor(ref Color color, float v)
        {
            color.R = (byte)(color.R * v);
            color.G = (byte)(color.G * v);
            color.B = (byte)(color.B * v);
        }

        /// <summary>Cheap parabola-based wave that oscilates with amplitud of 1 and wavelength of 2</summary>
        /// <param name="x">... dont make me fucking explain this param</param>
        public static float CheapSineWave(float x)
        {
            x = x % 2;
            return x < 1 ? (-4 * x * (x - 1)) : (4 * (x - 1) * (x - 2));
        }

        /// <summary>
        /// Cheap parabola-based wave that oscilates in between [0, 1] and has a wavelength of 2.
        /// <para>After a bit of testing, this takes about 2/3s as much time as Math.Sin</para>
        /// </summary>
        /// <param name="x">fuck u dusto</param>
        public static float CheapWave(float x)
        {
            x = x % 2;
            return x < 1 ? (-2 * x * (x - 1) + 0.5f) : (2 * (x - 1) * (x - 2) + 0.5f);
        }

        /// <summary>Rotates a vector around (0, 0)</summary>
        /// <param name="vec">A reference to the vector to rotate</param>
        /// <param name="rot">The rotation in radians</param>
        public static void RotateVector(this Vector2 vec, float rot)
        {
            float cos = (float)Math.Cos(rot);
            float sin = (float)Math.Sin(rot);
            float x = vec.X, y = vec.Y;
            vec.X = cos * x - sin * y;
            vec.Y = sin * x + cos * y;
        }

        /// <summary>
        /// Tries to load a Scenario from a file. 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static Scenario.Scenario TryLoadScenario(String file)
        {
            if (System.IO.File.Exists(file))
            {
                ByteStream b = new ByteStream(System.IO.File.ReadAllBytes(file));
                if (!(b.HasNext(56) && b.ReadByte() == 222 && b.ReadByte() == 111 && b.ReadByte() == 41 && b.ReadByte() == 231 && b.ReadByte() == 60))
                    return null;
                return new Scenario.Scenario(b);
            }
            return null;
        }
    }

    class SosUnPelotudoException : Exception
    {
        public SosUnPelotudoException() : base("sos un pelotudo") { }
        public SosUnPelotudoException(String msg) : base(msg) { }
    }
}
