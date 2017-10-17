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

    /// <summary>
    /// Encapsulates a Chat, containing all necessary to add text, it's formatting, wrapping, text typing for the user, etc.
    /// </summary>
    class Chat
    {
        public const int CharsPerLine = 70, MaxAllowedChars = 256, TYPING_DRAW_SEPARATION = 16;
        public const float MaxCharsWidth = 4872;
        private static Vector2 measure;
        public static SpriteFont Normal, Bold, Italic, Webdings;
        private static Color BackgroundColor = new Color(0, 0, 0, 200);
        private static Color BackgroundColorClosed = new Color(BackgroundColor.R, BackgroundColor.G, BackgroundColor.B, BackgroundColor.A / 2);
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
        /// <summary>Gets whether the chat is open and the user typing</summary>
        public bool IsOpen { get { return isOpen; } }

        StringBuilder typingText;
        Vector2 typingTextSize;
        int typingIndex = 0;
        float lastCharTime;
        float typingPosY;
        float textScale;
        Rectangle bounds, openRectangle, closedRectangle;

        int lineDrawCount, lineDrawIndex;

        List<ChatLine> lines;
        SpecialChatLine showTyping;

        List<NewChatLine> newLines;

        public Rectangle Bounds { get { return bounds; } set { SetBounds(value); } }

        public event LineEntered OnLineEntered;

        public Chat()
        {
            newLines = new List<NewChatLine>(8);
            lines = new List<ChatLine>(256);
            showTyping = new SpecialChatLine();
            typingText = new StringBuilder(MaxAllowedChars);
            typingTextSize = new Vector2(0, Normal.MeasureString("M").Y);

            StringBuilder builder = new StringBuilder();
            builder.Append("Hey there! wass uuuuup!");
            builder.Append("\nRemember u can du &red;all &blue;sorts &w;efejfeomf o&r; &green;of &w;efheinfhe12432 &r; &macri;colors &cyan;of &lime;all &niceblue;sort &brown;of &violet;shades!");
            builder.Append("\n\nSuch as:");
            builder.Append("\n&red; - red!");
            builder.Append("\n&blue; - blue!");
            builder.Append("\n&macri; - macri!");
            Add(builder.ToString());

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
            if (isOpen)
                showTyping.PreDraw(device, batch, isOpen && (Game1.Time - lastCharTime) % 1 < 0.5f);
        }

        public void Draw(SpriteBatch batch)
        {
            if (isOpen)
            {
                #region DrawOpen

                batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, transform);
                Rectangle rect = openRectangle;
                if (lines.Count < linesToShow)
                {
                    int liesToChildren = (int)(lines.Count * measure.Y + measure.Y + 16 + TYPING_DRAW_SEPARATION);
                    //children are told so many lies these days... i dont think I can take it
                    rect.Y = rect.Height - liesToChildren;
                    rect.Height = liesToChildren;
                }
                batch.Draw(Game1.whiteSquare, rect, BackgroundColor);

                showTyping.DrawAt(batch, typingPosY);
                int index = lineDrawIndex;
                float y = typingPosY - measure.Y - TYPING_DRAW_SEPARATION - TYPING_DRAW_SEPARATION;
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
                batch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, transform);

                Rectangle bgrect = closedRectangle;
                int xtray = (int)(newLines.Count * measure.Y);
                if (newLines.Count != 0)
                    xtray += TYPING_DRAW_SEPARATION*2;
                bgrect.Height += xtray;
                bgrect.Y -= xtray;
                batch.Draw(Game1.whiteSquare, bgrect, BackgroundColorClosed);

                batch.DrawString(Normal, "Press ENTER to open the chat", new Vector2(closedRectangle.X, closedRectangle.Y), new Color(200, 200, 200, 100));

                float y = typingPosY - measure.Y - TYPING_DRAW_SEPARATION*2;
                for (int i = newLines.Count-1; i != -1; i--)
                {
                    newLines[i].Draw(batch, y);
                    y -= measure.Y;
                }

                batch.End();
                #endregion
            }
        }

        /// <summary>Adds a text to the chat, processing text formatting and wrapping the text to it doesn't overflow</summary>
        /// <param name="text">The raw text to add.</param>
        public void Add(String fullText)
        {
            ChatLine last = null;
            SpriteFont font = Normal;

            String[] fullSplit = fullText.Split('\n');
            for (int i = 0; i < fullSplit.Length; i++)
            {
                String text = fullSplit[i];
                float width = 0;
                int index = 0, lastSpace = -1, lastCutEnd = 0;

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
                            end = index - 1;
                        else
                            end = lastSpace;

                        last = new ChatLine(text.Substring(begin, end - begin), last);
                        addLine(last);

                        if (lastSpace == -1)
                        {
                            lastCutEnd = end;
                            width = font.MeasureString(text[lastCutEnd].ToString()).X;
                        }
                        else
                        {
                            lastCutEnd = end + 1;
                            lastSpace = -1;
                            width -= last.MeasureX();

                        }
                    }

                    while (index < text.Length && text[index] == '&' && ChatLine.AdvanceIfNecesary(ref text, ref index, ref font)) ;
                }

                ChatLine tmp = new ChatLine(text.Substring(lastCutEnd), last);
                if (tmp.MeasureX() > 0)
                    addLine(tmp);
            }

            lineDrawIndex = lines.Count;
            lineDrawCount = Math.Min(lineDrawIndex, linesToShow);
        }

        private void addLine(ChatLine chatLine)
        {
            lines.Add(chatLine);
            newLines.Add(new NewChatLine(newLines, chatLine));
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
                        if (showTyping.ChatLineLength() != 0)
                        {
                            String s = typingText.ToString();
                            if (OnLineEntered != null)
                                OnLineEntered(ref s);
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

        /// <summary>Sets the boudns on screen on which the chat will draw</summary>
        /// <param name="rectangle">The rectangle on the screen (in pixels)</param>
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

            openRectangle = new Rectangle(0, 0, (int)MaxCharsWidth, (int)(linesToShow * measure.Y + measure.Y + 16 + TYPING_DRAW_SEPARATION));
            closedRectangle = new Rectangle(0, (int)(linesToShow * measure.Y + TYPING_DRAW_SEPARATION * 2), (int)MaxCharsWidth, (int)measure.Y);
        }
    }

    /// <summary>
    /// This struct is for drawing those new ChatLines when the chat is closed so the user can see them
    /// </summary>
    struct NewChatLine
    {
        public const float TotalTimeAlive = 5;

        ChatLine chatLine;
        float timeEnd;
        List<NewChatLine> list;

        public NewChatLine(List<NewChatLine> list, ChatLine line)
        {
            this.list = list;
            this.chatLine = line;
            timeEnd = Game1.Time + TotalTimeAlive;
        }

        public void Draw(SpriteBatch batch, float y)
        {
            float alpha = Math.Min(1, timeEnd - Game1.Time);
            if (alpha < 0)
                list.Remove(this);
            else
                chatLine.DrawAt(batch, y, alpha * 0.5f);
        }
    }
}
