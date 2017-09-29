using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;

namespace ClusterWave
{
    static class Constants
    {
        public const Category PlayerCategory = Category.Cat1;
        public const Category WallsCategory = Category.Cat2;
        public const Category BulletsCategory = Category.Cat3;
        public const Category ParticleCategory = Category.Cat4;
        public const Category ShieldCategory = Category.Cat5;

        public const Category PlayersCollideWith = PlayerCategory | WallsCategory | BulletsCategory;
        public const Category WallsCollideWith = Category.All;
        public const Category BulletsCollideWith = BulletsCategory | PlayerCategory | WallsCategory;
        public const Category ParticleCollideWith = WallsCategory | ShieldCategory;
        public const Category ShieldCollideWith = PlayerCategory | BulletsCategory;
    }

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

        /* "HSV to RGB 'C' function"
void HSVtoRGB( float *r, float *g, float *b, float h, float s, float v )
{
	int i;
	float f, p, q, t;
	if( s == 0 ) {
		// achromatic (grey)
		*r = *g = *b = v;
		return;
	}
	h /= 60;			// sector 0 to 5
	i = floor( h );
	f = h - i;			// factorial part of h
	p = v * ( 1 - s );
	q = v * ( 1 - s * f );
	t = v * ( 1 - s * ( 1 - f ) );
	switch( i ) {
		case 0:
			*r = v;
			*g = t;
			*b = p;
			break;
		case 1:
			*r = q;
			*g = v;
			*b = p;
			break;
		case 2:
			*r = p;
			*g = v;
			*b = t;
			break;
		case 3:
			*r = p;
			*g = q;
			*b = v;
			break;
		case 4:
			*r = t;
			*g = p;
			*b = v;
			break;
		default:		// case 5:
			*r = v;
			*g = p;
			*b = q;
			break;
	}
}       */
    }
}
