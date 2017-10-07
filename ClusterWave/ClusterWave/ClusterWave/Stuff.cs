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

        /// <summary>THIS DOESNT WORK PROPERLY ON S AND V ARE NOT 1</summary>
        /// <param name="h">The Hue value</param>
        /// <param name="s">"This must be 1.0f" #FuckYou</param>
        /// <param name="v">"This must be 1.0f" #FuckYou</param>
        /// <returns></returns>
        public static Color ColorFromHSV(float h, float s, float v)
        {
            h %= 1f;
            Color hue = ColorFromHue(h);
            SaturateColor(ref hue, s);
            MultiplyColor(ref hue, v);
            return hue;
        }

        /// <summary>
        /// Cheap parabola-based wave that oscilates with amplitud of 1 and wavelength of 2.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float CheapSineWave(float x)
        {
            x = x % 2;
            return x < 1 ? (-4 * x * (x - 1)) : (4 * (x - 1) * (x - 2));
        }

        /// <summary>
        /// Cheap parabola-based wave that oscilates in between [0, 1] and has a wavelength of 2.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float CheapWave(float x)
        {
            x = x % 2;
            return x < 1 ? (-2 * x * (x - 1) + 0.5f) : (2 * (x - 1) * (x - 2) + 0.5f);
        }
    }
}
