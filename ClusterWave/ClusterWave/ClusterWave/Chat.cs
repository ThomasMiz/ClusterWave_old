using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ClusterWave
{
    delegate void LineEntered(ref String line);

    class Chat
    {
        public const int CharsPerLine = 70, MaxAllowedChars = 256, TYPING_DRAW_SEPARATION = 16;
        public const float MaxCharsWidth = 4872;
        static Vector2 measure;
        public static SpriteFont Normal, Bold, Italic, Webdings;
        public static void LoadFonts(ContentManager Content)
        {
            Normal = Content.Load<SpriteFont>("Chat/normal");
            Bold = Content.Load<SpriteFont>("Chat/bold");
            Italic = Content.Load<SpriteFont>("Chat/italic");
            Webdings = Content.Load<SpriteFont>("Chat/webdings");
            measure = Normal.MeasureString("M");
        }

        /// <summary>Whether an enter key event will make the chat open and start typing</summary>
        public bool AllowOpen = true;

        Matrix transform;
        int linesToShow;
        bool isOpen = false;
        public bool IsOpen { get { return isOpen; } }

        StringBuilder typingText;
        Vector2 typingTextSize;
        int typingIndex = 0;
        float lastCharTime;
        float typingPosY;
        float textScale;
        Rectangle bounds;

        int lineDrawCount, lineDrawIndex;

        List<ChatLine> lines;

        AutisticChatLine showTyping;

        public Rectangle Bounds { get { return bounds; } set { SetBounds(value); } }

        public event LineEntered OnLineEntered;

        public Chat()
        {
            lines = new List<ChatLine>(256);
            showTyping = new AutisticChatLine();
            typingText = new StringBuilder(MaxAllowedChars);
            typingTextSize = new Vector2(0, Normal.MeasureString("M").Y);

            Add("h &blue;a tdos&red; los pts &italics;co &ekid;e mo &bold;&cyan;gra&reset;afeeeeeeeeeeeeeeee eeeeeeeeeeeeeeeabc &magenta;abcdefghijk&violet;lmnopqrstuvwxyzekidekidekidekidekidekidekidekidekidekidekidekidekidekidekidekidekidekidekide&pink;kidekidekidekidekidekidekidekidekidekidekide");
            //Add("hol&blue;a a tod&w;o&r;s!");
            Add("ola&w;1234567890123456789012345&r;E&w;678901234567890123456789012345678901234567890123456789&r;EKIDE LOLAZO PAPA&w;012345678901234567890EKIDEKIDE KIDEKIDEKIDE&r;12345");

            Add("Se que nadie quiere&red; ver si esta mie&blue;rda anda &w;pero yo tendr&macri;ia que arre&pink;glarla &r;y es mi prob&macri;lema xdddd &w;me voy a s&red;uicidar &r;lol voy a ex&bold;tender un poco cua&italics;ndo dura este texto agregando cu&pink;alquier pelotude&niceblue;z que se me venga a la ca&cyan;beza que sea un toque razonab&w;le o watever ni t&b;engo idea p&green;or que hago estooooeoeoeoeoeoeoeoe&w;oeoeoeoeoeoeoeoeo&red;eoeoeoeoeo&n;eoeoeoeoeoeoe&7;oeoeoeoeoeoeoeoeoe&2;oeoeoeoeoeoe&4;oeoeoeoeoeoeoeoeoeoeoe");

            Add("eeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee&w;eeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee&r;eeeeeeeeeeeee&w;eeeeeeeeeeeeee&r;eeeeeeeeeeeeeee&w;eeeeeeeeeeeeeeee&r;eeeeeeeeeeeeeeeee&w;eeeeeeeeeeeeeeeeeee&r;eeeeeeeeeeeeeeeeee&w;eeeeeeeeeeee");

            StringBuilder build = new StringBuilder(1000);
            for (int i = 0; i < 100; i++)
                build.Append("&n;eeeeeeee&w;eeeeeeee");
            for (int i = 0; i < 10; i++)
                build.Insert(Game1.r.Next(build.Length), String.Concat("&", ChatLine.GetRandomColorName(), ';'));
            Add(build.ToString());

            EventInput.CharEntered += OnCharEnter;
        }
        ~Chat()
        {
            EventInput.CharEntered -= OnCharEnter;
        }

        private int FindSpace(ref String s, int index, int bottom)
        {
            for (int i = index; i > bottom; i--)
                if (s[i] == ' ')
                    return i;
            return index;
        }

        private int FindSpace420_69_360NoScope()
        {
            for (int i = typingIndex; i > 0; )
            {
                i--;
                if (typingText[i] == ' ')
                    return i;
            }
            return 0;
        }

        public void Update()
        {
            if (Game1.ms.ScrollWheelValue != Game1.oldms.ScrollWheelValue)
            {
                int diff = Game1.ms.ScrollWheelValue - Game1.oldms.ScrollWheelValue;
                lineDrawIndex = Math.Max(lineDrawIndex + diff / 120, lineDrawCount);
                if (lineDrawIndex > lines.Count)
                    lineDrawIndex = lines.Count;
            }
        }

        public void PreDraw(GraphicsDevice device, SpriteBatch batch)
        {
            showTyping.PreDraw(device, batch, isOpen && (Game1.Time - lastCharTime) % 1 < 0.5f);
        }

        public void Draw(SpriteBatch batch)
        {
            if (isOpen)
            {
                #region DrawOpen
                batch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null);
                batch.Draw(Game1.whiteSquare, bounds, new Color(0, 0, 0, 200));
                batch.End();

                batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, transform);

                showTyping.DrawAt(batch, typingPosY);
                if (isOpen && (Game1.Time - lastCharTime) % 1 < 0.5f)
                {
                    //batch.DrawString(Normal, "_", new Vector2(typingTextSize.X, typingPosY), Color.Gray);
                }
                int index = lineDrawIndex;
                float y = typingPosY - measure.Y - TYPING_DRAW_SEPARATION;
                for (int i = 0; i < lineDrawCount; i++)
                {
                    lines[--index].DrawAt(batch, y);
                    y -= measure.Y;
                }

                batch.End();
                #endregion
            }
            else
            {
                #region DrawClosed
                #endregion
            }
        }

        public void Add(String text)
        {
            ChatLine last = null;
            int index = 0, lastSpace = -1, lastCutEnd = 0;
            SpriteFont font = Normal;
            float width = 0;

            while (index < text.Length && text[index] == '&' && ChatLine.AdvanceIfNecesary(ref text, ref index, ref font)) ;

            while (index < text.Length)
            {
                if (text[index] == ' ')
                    lastSpace = index;

                width += font.MeasureString(text[index++].ToString()).X;
                if (width >= MaxCharsWidth)
                {
                    int begin = lastCutEnd;
                    int end;
                    if (lastSpace == -1)
                        end = index-1;
                    else
                        end = lastSpace;

                    last = new ChatLine(text.Substring(begin, end-begin), last);
                    lines.Add(last);

                    if (lastSpace == -1)
                    {
                        lastCutEnd = end;
                        width = font.MeasureString(text[lastCutEnd].ToString()).X;
                    }
                    else
                    {
                        lastCutEnd = end+1;
                        lastSpace = -1;
                        width -= last.MeasureX();

                    }
                }

                while (index < text.Length && text[index] == '&' && ChatLine.AdvanceIfNecesary(ref text, ref index, ref font)) ;
            }

            lines.Add(new ChatLine(text.Substring(lastCutEnd), last));

            lineDrawIndex = lines.Count;
            lineDrawCount = Math.Min(lineDrawIndex, linesToShow);
        }

        public void StartTyping()
        {
            if (!isOpen)
            {
                isOpen = true;
            }
        }

        void OnCharEnter(object sender, CharacterEventArgs e)
        {
            if (isOpen)
            {
                lastCharTime = Game1.Time;
                char c = e.Character;
                //Game1.game.Window.Title = "char=" + c.ToString() + " ascii=" + ((int)(c)) + " at " + Game1.Time;
                switch ((int)c)
                {
                    case 13: //enter character
                        #region Enter
                        if (typingText.Length != 0)
                        {
                            String s = typingText.ToString();
                            if (OnLineEntered != null)
                                OnLineEntered(ref s);
                            Game1.game.Window.Title = typingText.ToString();
                            typingText.Clear();
                            typingIndex = 0;
                            showTyping.Clear();
                            typingTextSize.X = 0;
                        }
                        break;
                        #endregion

                    case 8: //delete 1 char
                        #region Delete
                        if (typingIndex != 0)
                        {
                            if (Game1.ks.IsKeyDown(Keys.LeftShift))
                            {
                                int s = FindSpace420_69_360NoScope();
                                //we have to delete chars from S to typingIndex
                                int len = typingIndex - s;
                                typingText.Remove(s, len);
                                typingIndex -= len;
                            }
                            else
                            {
                                typingIndex--;
                                typingText.Remove(typingIndex, 1);
                            }
                            showTyping.Set(typingText);
                            typingTextSize.X = showTyping.MeasureX();
                        }
                        break;
                        #endregion

                    case 127: //delete until space
                        #region MoreDelete
                        if (typingIndex != 0)
                        {
                            int lel = FindSpace420_69_360NoScope();
                            //we have to delete chars from S to typingIndex
                            int lolazo = typingIndex - lel;
                            typingText.Remove(lel, lolazo);
                            typingIndex -= lolazo;
                            showTyping.Set(typingText);
                            typingTextSize.X = showTyping.MeasureX();
                        }
                        #endregion
                        break;

                    case 27: //ESC
                        PauseTyping();
                        break;

                    default:
                        #region Char
                        if (c > 31 && c < 255 && typingIndex < MaxAllowedChars)
                        {
                            typingText.Insert(typingIndex++, c);
                            showTyping.Set(typingText);
                            typingTextSize.X = showTyping.MeasureX();
                        }
                        #endregion
                        break;
                }
            }
            else
            {
                if(AllowOpen && e.Character == 13)
                    StartTyping();
            }
        }

        public void PauseTyping()
        {
            if (isOpen)
            {
                isOpen = false;
            }
        }

        public void SetBounds(Rectangle rectangle)
        {
            this.bounds = rectangle;
            textScale = rectangle.Width / MaxCharsWidth;
            transform = Matrix.CreateScale(textScale) * Matrix.CreateTranslation(rectangle.X, rectangle.Y, 0);

            linesToShow = (int)(bounds.Height / (measure.Y * textScale));
            bounds.Height = (int)((linesToShow + 1) * measure.Y * textScale) + TYPING_DRAW_SEPARATION / 2;
            typingPosY = (bounds.Height / textScale - measure.Y);
            lineDrawCount = Math.Min(linesToShow, lines.Count);
            lineDrawIndex = lines.Count;

            showTyping.OnResize(rectangle.Width, textScale);
        }
    }
}
