using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ClusterWave
{
    class ChatLine
    {
        public const char markStart = '&', markEnd = ';', markBold = 'b', markItalics = 'i', markReset = 'r';

        #region StaticShit

        //static bool DidLastFindEndGetToTheEnd = false;

        public static Color[] AllColors = new Color[] { 
            new Color(0, 0, 0), //black
            new Color(128, 128, 128), //gray
            new Color(255, 255, 255), //white

            new Color(0, 0, 255), //red
            new Color(255, 106, 0), //orange
            new Color(255, 216, 0), //yellow 5
            new Color(182, 255, 0),
            new Color(76, 255, 0), //green 7
            new Color(0, 255, 33), //lime
            new Color(0, 255, 144), //9
            new Color(0, 255, 255), //cyan
            new Color(0, 148, 255), //lightblue 11
            new Color(0, 38, 255), //blue 12
            new Color(72, 0, 255), //13
            new Color(178, 0, 255), //14 violet
            new Color(255, 0, 220), //15
            new Color(255, 0, 110),
        };

        public static NamedColor[] colorWords = new NamedColor[]{
            new NamedColor("black", new Color(0, 0, 0)),
            new NamedColor("gray", new Color(128, 128, 128)),
            new NamedColor("white", new Color(255, 255, 255)),
            new NamedColor("red", new Color(255, 0, 0)),
            new NamedColor("orange", new Color(255, 106, 0)),
            new NamedColor("yellow", new Color(255, 216, 0)),
            new NamedColor("green", new Color(76, 255, 0)),
            new NamedColor("lime", new Color(0, 255, 33)),
            new NamedColor("cyan", new Color(0, 255, 255)),
            new NamedColor("lightblue", new Color(0, 148, 255)),
            new NamedColor("blue", new Color(0, 38, 255)),
            new NamedColor("violet", new Color(178, 0, 255)),
            new NamedColor("magenta", new Color(255, 0, 220)),
            new NamedColor("pink", new Color(255, 0, 110)),
            new NamedColor("brown", Color.Brown),
            new NamedColor("niceblue", Color.CornflowerBlue),
            new NamedColor("shit", new Color(99, 39, 0)),
            new NamedColor("macri", Color.Yellow),
        };

        public static String GetRandomColorName()
        {
            return colorWords[Game1.r.Next(colorWords.Length)].name;
        }

        public static readonly Color DefaultColor = new Color(200, 200, 200);

        #endregion

        String[] text;
        Color[] colors, backColors;
        SpriteFont[] fonts;
        float[] positions;

        public ChatLine(String textLine)
        {
            Init(DefaultColor, Chat.Normal, textLine);
        }

        public ChatLine(String textLine, ChatLine previousFormat)
        {
            if (previousFormat == null)
                Init(DefaultColor, Chat.Normal, textLine);
            else
            {
                int i = previousFormat.text.Length - 1;
                Init(previousFormat.colors[i], previousFormat.fonts[i], textLine);
            }
        }

        private void Init(Color lastColor, SpriteFont lastFont, String textLine)
        {
            List<String> textList = new List<String>(16);
            List<Color> colorList = new List<Color>(16);
            List<SpriteFont> fontList = new List<SpriteFont>(16);
            StringBuilder currentText = new StringBuilder(textLine.Length);

            int index = 0;
            while (index < textLine.Length)
            {
                if (textLine[index] == markStart)
                {
                    int ind = index;
                    String beforeLower = FindEnd(ref textLine, ref index), thing = beforeLower.ToLower();
                    if (thing.Length != 0)
                    {
                        #region FindFormat
                        int res;
                        if (Int32.TryParse(thing, out res) && res < AllColors.Length)
                        {
                            textList.Add(currentText.ToString());
                            currentText.Clear();
                            colorList.Add(lastColor);
                            fontList.Add(lastFont);

                            lastColor = AllColors[res];
                        }
                        else if (thing.Equals("n") || thing.Equals("normal"))
                        {
                            textList.Add(currentText.ToString());
                            currentText.Clear();
                            colorList.Add(lastColor);
                            fontList.Add(lastFont);

                            lastFont = Chat.Normal;
                        }
                        else if (thing.Equals("b") || thing.Equals("bold"))
                        {
                            textList.Add(currentText.ToString());
                            currentText.Clear();
                            colorList.Add(lastColor);
                            fontList.Add(lastFont);

                            lastFont = Chat.Bold;
                        }
                        else if (thing.Equals("i") || thing.Equals("italics"))
                        {
                            textList.Add(currentText.ToString());
                            currentText.Clear();
                            colorList.Add(lastColor);
                            fontList.Add(lastFont);

                            lastFont = Chat.Italic;
                        }
                        else if (thing.Equals("w") || thing.Equals("webdings"))
                        {
                            textList.Add(currentText.ToString());
                            currentText.Clear();
                            colorList.Add(lastColor);
                            fontList.Add(lastFont);

                            lastFont = Chat.Webdings;
                        }
                        else if (thing.Equals("r") || thing.Equals("reset"))
                        {
                            textList.Add(currentText.ToString());
                            currentText.Clear();
                            colorList.Add(lastColor);
                            fontList.Add(lastFont);

                            lastColor = DefaultColor;
                            lastFont = Chat.Normal;
                        }
                        else
                        {
                            Color c;
                            if (TextToColor(thing, out c))
                            {
                                textList.Add(currentText.ToString());
                                currentText.Clear();
                                colorList.Add(lastColor);
                                fontList.Add(lastFont);

                                lastColor = c;
                            }
                            else
                            {
                                currentText.Append('&');
                                currentText.Append(beforeLower);
                                currentText.Append(';');
                                //index++;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        currentText.Append(markStart);
                        index = ind + 1;
                    }
                }
                else
                {
                    currentText.Append(textLine[index]);
                    index++;
                }
            }
            textList.Add(currentText.ToString());
            colorList.Add(lastColor);
            fontList.Add(lastFont);

            text = textList.ToArray();
            colors = colorList.ToArray();
            backColors = new Color[colors.Length];
            for (int i = 0; i < backColors.Length; i++)
                backColors[i] = CalculateBackColor(colors[i]);
            fonts = fontList.ToArray();

            positions = new float[text.Length];
            float x = 0;
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = x;
                x += fonts[i].MeasureString(text[i]).X;
            }
        }

        public void DrawAt(SpriteBatch batch, float y)
        {
            for (int i = 0; i < text.Length; i++)
            {
                batch.DrawString(fonts[i], text[i], new Vector2(positions[i]+15, y+15), backColors[i]);
                batch.DrawString(fonts[i], text[i], new Vector2(positions[i], y), colors[i]);
            }
        }

        public float MeasureX()
        {
            int ind = text.Length - 1;
            return positions[ind] + fonts[ind].MeasureString(text[ind]).X;
        }

        private static String FindEnd(ref String line, ref int index)
        {
            //DidLastFindEndGetToTheEnd = false;
            int start = index;
            while (++index < line.Length)
            {
                if (line[index] == markEnd)
                {
                    index++;
                    return line.Substring(start + 1, index - start - 2);
                }

                if (line[index] == markStart || !((line[index] >= '0' && line[index] <= '9') || (line[index] >= 'a' && line[index] <= 'z') || (line[index] >= 'A' && line[index] <= 'Z')))
                {
                    index = start;
                    return new String(new char[] { }); //si encuentra un & o el caracter no pertenece a (0-9 o a-z)
                }
            }
            //DidLastFindEndGetToTheEnd = true;
            index = start;
            return new String(new char[] { });
        }

        public  static bool TextToColor(String text, out Color color)
        {
            for (int i = 0; i < colorWords.Length; i++)
                if (colorWords[i].name.Equals(text))
                {
                    color = colorWords[i].color;
                    return true;
                }
            color = DefaultColor;
            return false;
        }

        public static bool AdvanceIfNecesary(ref String text, ref int index, ref SpriteFont font)
        {
            int start = index;
            String thing = FindEnd(ref text, ref index);

            if (thing.Length == 0)
            {
                return false;
            }

            int res;
            if ((Int32.TryParse(thing, out res) && res < AllColors.Length))
            {

            }
            else if (thing.Equals("r") || thing.Equals("reset"))
            {
                font = Chat.Normal;
            }
            else if (thing.Equals("i") || thing.Equals("italics"))
            {
                font = Chat.Italic;
            }
            else if (thing.Equals("b") || thing.Equals("bold"))
            {
                font = Chat.Bold;
            }
            else if (thing.Equals("w") || thing.Equals("webdings"))
            {
                font = Chat.Webdings;
            }
            else if (thing.Equals("n") || thing.Equals("normal"))
            {
                font = Chat.Normal;
            }
            else
            {
                Color c;
                if (!TextToColor(thing, out c))
                { //nothing? oh well, return index to its original value and fuck off
                    index = start;
                    return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            StringBuilder b = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
                b.Append(text[i]);
            return b.ToString();
        }

        public static float MeasureX(String s, ChatLine prev)
        {
            SpriteFont font = prev.fonts[prev.fonts.Length - 1];
            float wid = 0;
            int index = 0;
            while (index < s.Length)
            {
                while (index < s.Length && s[index] == markStart && AdvanceIfNecesary(ref s, ref index, ref font)) ;
                if (index < s.Length)
                    wid += font.MeasureString(s[index++].ToString()).X;

            }
            return wid;
        }

        public static Color CalculateBackColor(Color v)
        {
            return Color.Multiply(v, 0.5f);
        }
    }

    struct NamedColor
    {
        public string name;
        public Color color;

        public NamedColor(string name, Color color)
        {
            this.name = name;
            this.color = color;
        }
    }

    /// <summary>Dont even ask why on the fuck's name this is named like this. I just couldn't think of an appropiate name -_("/)_-</summary>
    class AutisticChatLine
    {
        /// <summary>This name has nothing to do with the fact that this class is named with the word "Autistic".
        /// I realised it might sound offensive after writting it and realised "special" could be mis-interpreted so</summary>
        static Color SpecialColor = new Color(196, 196, 196, 128);

        List<String> textList;
        List<Color> colorList, backColors;
        List<SpriteFont> fontList;

        RenderTarget2D target;
        float scale, width;
        Matrix mat;

        float[] positions;

        public AutisticChatLine()
        {
            textList = new List<string>(16);
            colorList = new List<Color>(16);
            backColors = new List<Color>(16);
            fontList = new List<SpriteFont>(16);
        }
        ~AutisticChatLine()
        {
            target.Dispose();
        }

        public void Set(StringBuilder textLine)
        {
            Clear();

            SpriteFont lastFont = Chat.Normal;
            Color lastColor = ChatLine.DefaultColor;
            StringBuilder currentText = new StringBuilder(textLine.Length);

            int index = 0;
            while (index < textLine.Length)
            {
                if (textLine[index] == ChatLine.markStart)
                {
                    int ind = index;
                    String beforeLower = FindEnd(textLine, ref index), thing = beforeLower.ToLower();
                    if (thing.Length != 0)
                    {
                        #region FindFormat
                        int res;
                        if (Int32.TryParse(thing, out res) && res < ChatLine.AllColors.Length)
                        {
                            textList.Add(currentText.ToString());
                            currentText.Clear();
                            colorList.Add(lastColor);
                            fontList.Add(lastFont);

                            lastColor = ChatLine.AllColors[res];

                            colorList.Add(Color.Lerp(SpecialColor, lastColor, 0.5f));
                            fontList.Add(Chat.Italic);
                            textList.Add(String.Concat(ChatLine.markStart, beforeLower, ChatLine.markEnd));
                        }
                        else if (thing.Equals("n") || thing.Equals("normal"))
                        {
                            textList.Add(currentText.ToString());
                            currentText.Clear();
                            colorList.Add(lastColor);
                            fontList.Add(lastFont);

                            lastFont = Chat.Normal;

                            colorList.Add(Color.Lerp(SpecialColor, lastColor, 0.5f));
                            fontList.Add(Chat.Italic);
                            textList.Add(String.Concat(ChatLine.markStart, beforeLower, ChatLine.markEnd));
                        }
                        else if (thing.Equals("b") || thing.Equals("bold"))
                        {
                            textList.Add(currentText.ToString());
                            currentText.Clear();
                            colorList.Add(lastColor);
                            fontList.Add(lastFont);

                            lastFont = Chat.Bold;

                            colorList.Add(Color.Lerp(SpecialColor, lastColor, 0.5f));
                            fontList.Add(Chat.Italic);
                            textList.Add(String.Concat(ChatLine.markStart, beforeLower, ChatLine.markEnd));
                        }
                        else if (thing.Equals("i") || thing.Equals("italics"))
                        {
                            textList.Add(currentText.ToString());
                            currentText.Clear();
                            colorList.Add(lastColor);
                            fontList.Add(lastFont);

                            lastFont = Chat.Italic;

                            colorList.Add(Color.Lerp(SpecialColor, lastColor, 0.5f));
                            fontList.Add(Chat.Italic);
                            textList.Add(String.Concat(ChatLine.markStart, beforeLower, ChatLine.markEnd));
                        }
                        else if (thing.Equals("w") || thing.Equals("webdings"))
                        {
                            textList.Add(currentText.ToString());
                            currentText.Clear();
                            colorList.Add(lastColor);
                            fontList.Add(lastFont);

                            lastFont = Chat.Webdings;

                            colorList.Add(Color.Lerp(SpecialColor, lastColor, 0.5f));
                            fontList.Add(Chat.Italic);
                            textList.Add(String.Concat(ChatLine.markStart, beforeLower, ChatLine.markEnd));
                        }
                        else if (thing.Equals("r") || thing.Equals("reset"))
                        {
                            textList.Add(currentText.ToString());
                            currentText.Clear();
                            colorList.Add(lastColor);
                            fontList.Add(lastFont);

                            lastColor = ChatLine.DefaultColor;
                            lastFont = Chat.Normal;

                            colorList.Add(Color.Lerp(SpecialColor, lastColor, 0.5f));
                            fontList.Add(Chat.Italic);
                            textList.Add(String.Concat(ChatLine.markStart, beforeLower, ChatLine.markEnd));
                        }
                        else
                        {
                            Color c;
                            if (ChatLine.TextToColor(thing, out c))
                            {
                                textList.Add(currentText.ToString());
                                currentText.Clear();
                                colorList.Add(lastColor);
                                fontList.Add(lastFont);

                                lastColor = c;

                                colorList.Add(Color.Lerp(SpecialColor, lastColor, 0.5f));
                                fontList.Add(Chat.Italic);
                                textList.Add(String.Concat(ChatLine.markStart, beforeLower, ChatLine.markEnd));
                            }
                            else
                            {
                                currentText.Append(ChatLine.markStart);
                                currentText.Append(beforeLower);
                                currentText.Append(ChatLine.markEnd);
                                //index++;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        currentText.Append(ChatLine.markStart);
                        index = ind + 1;
                    }
                }
                else
                {
                    currentText.Append(textLine[index]);
                    index++;
                }
            }
            textList.Add(currentText.ToString());
            colorList.Add(lastColor);
            fontList.Add(lastFont);

            backColors.Clear();
            for (int i = 0; i < colorList.Count; i++)
                backColors.Add(ChatLine.CalculateBackColor(colorList[i]));

            positions = new float[textList.Count];
            float x = 0;
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = x;
                x += fontList[i].MeasureString(textList[i]).X;
            }
            width = MeasureX();
        }

        public void Clear()
        {
            textList.Clear();
            colorList.Clear();
            fontList.Clear();
            width = 0;
        }

        private static String FindEnd(StringBuilder line, ref int index)
        {
            int start = index;
            while (++index < line.Length)
            {
                if (line[index] == ChatLine.markEnd)
                {
                    index++;
                    char[] arr = new char[index - start - 2];
                    for (int i = 0; i < arr.Length; i++)
                        arr[i] = line[start + 1 + i];
                    return new String(arr);
                }

                if (line[index] == ChatLine.markStart || !((line[index] >= '0' && line[index] <= '9') || (line[index] >= 'a' && line[index] <= 'z') || (line[index] >= 'A' && line[index] <= 'Z')))
                {
                    index = start;
                    return new String(new char[] { }); //si encuentra un & o el caracter no pertenece a (0-9 o a-z)
                }
            }
            index = start;
            return new String(new char[] { });
        }

        public void PreDraw(GraphicsDevice device, SpriteBatch batch, bool drawThingy)
        {
            device.SetRenderTarget(target);
            device.Clear(Color.Transparent);
            float offset = Math.Min(0, Chat.MaxCharsWidth - width - 70);
            batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, mat * Matrix.CreateTranslation(offset*scale, 0, 0));
            for (int i = 0; i < textList.Count; i++)
            {
                batch.DrawString(fontList[i], textList[i], new Vector2(positions[i]+15, 15), backColors[i]);
                batch.DrawString(fontList[i], textList[i], new Vector2(positions[i], 0), colorList[i]);
            }
            if (drawThingy)
            {
                batch.DrawString(Chat.Normal, "_", new Vector2(width + 15, 15), Color.Black);
                batch.DrawString(Chat.Normal, "_", new Vector2(width, 0), Color.Gray);
            }
            batch.End();
        }

        public void DrawAt(SpriteBatch batch, float y)
        {
            batch.Draw(target, new Vector2(0, y), null, Color.White, 0f, Vector2.Zero, 1f/scale, SpriteEffects.None, 0f);
        }

        public float MeasureX()
        {
            float val = 0;
            for (int i = 0; i < textList.Count; i++)
                val += fontList[i].MeasureString(textList[i]).X;
            return val;
        }

        public void OnResize(int width, float scale)
        {
            this.scale = scale;
            if (target != null) target.Dispose();
            target = new RenderTarget2D(Game1.game.GraphicsDevice, width, (int)(Chat.Webdings.MeasureString("YEAH BUOY 420 M8").Y * scale+0.5f));
            mat = Matrix.CreateScale(scale);
        }
    }
}
