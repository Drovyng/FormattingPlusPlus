using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria.GameContent.UI.Chat;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace FormattingPlusPlus
{
    public class Singleton<T> where T : ITagHandler
    {
        public static T Instance;
        public static TextSnippet Parse(string text, Color color, string options) => Instance.Parse(text, color, options);
    }
	public class FormattingPlusPlus : Mod
	{
        public const string SpecialCharacters = "\\[]:";
        public override void Load()
        {
            On_ChatManager.ParseMessage += CustomParser;
            Singleton<ColorTagHandler>.Instance = new();
            Singleton<ItemTagHandler>.Instance = new();
            Singleton<NameTagHandler>.Instance = new();
            Singleton<AchievementTagHandler>.Instance = new();
            Singleton<GlyphTagHandler>.Instance = new();
            Singleton<LocalizationTagHandler>.Instance = new();
            Singleton<TransitionTagHandler>.Instance = new();
            Singleton<URLTagHandler>.Instance = new();
        }
        private static List<TextSnippet> CustomParser(On_ChatManager.orig_ParseMessage orig, string text, Color baseColor)
        {
            List<TextSnippet> snippets = new List<TextSnippet>();
            Parse(ref text, -1, ref snippets, baseColor);
            return snippets;
        }
        private static int? Parse(ref string text, int i, ref List<TextSnippet> snippets, Color color)
        {
            char handler = ' ';
            string options = "";
            string buffer = "";
            bool hide = false;
            int state = i == -1 ? -1 : 1;
            if (state == -1) i++;
            while (i < text.Length)
            {
                var c = text[i];
                switch (state)
                {
                    case 1:
                        handler = char.ToLower(c); state++;
                        break;
                    case 2:
                        if (hide)
                        {
                            hide = false;
                            if (SpecialCharacters.Contains(c)) { options += c; break; }
                            else { options += "\\"; break; }
                        }
                        else if (c == '\\') { hide = true; break; }
                        if (c == ':') { 
                            state++;
                            hide = false;
                            if (options.StartsWith("/")) options = options.Substring(1); 
                            break;
                        }
                        if (c == ']')
                        {
                            state++;
                            i--;
                            break;
                        }
                        options += c;
                        break;
                    default:
                        if (hide)
                        {
                            hide = false;
                            if (SpecialCharacters.Contains(c)) { buffer += c; break; }
                            else buffer += "\\";
                        }
                        else if (c == '\\') { hide = true; break; }
                        if (c == '[')
                        {
                            hide = false;
                            if (state == -1) snippets.Add(new TextSnippet(buffer, color));
                            //else if (handler == 'c') snippets.Add(Singleton<ColorTagHandler>.Parse(buffer, color, options));
                            //else if (handler != 't') snippets.Add(new TextSnippet("[" + handler + options + ":" + buffer, color));
                            color = snippets[snippets.Count - 1].Color;
                            buffer = "";
                            var iNew = Parse(ref text, i+1, ref snippets, color);
                            if (iNew.HasValue) { 
                                i = iNew.Value;
                                var l = snippets[snippets.Count - 1];
                                if (handler == 't' || handler == 'u' || l is LocalizationTagHandler.LocalizationSnippet)
                                {
                                    if (handler == 'u')
                                    {
                                        if (l is GradientSnippet)
                                        {
                                            int j = 1;
                                            while (l is GradientSnippet g)
                                            {
                                                g.SetURL(options);
                                                j++;
                                                l = snippets[snippets.Count - j];
                                            }
                                        }
                                        else
                                        {
                                            snippets[snippets.Count - 1] = new URLTagHandler.URLSnippet(l.Text, l.Color, options);
                                        }
                                        break;
                                    }
                                    if (l is GradientSnippet)
                                    {
                                        string revb = "";
                                        while (l is GradientSnippet g)
                                        {
                                            l = snippets[snippets.Count - 1];
                                            revb = l.Text + revb;
                                            snippets.RemoveAt(snippets.Count - 1);
                                        }
                                        buffer += revb;
                                    }
                                    else
                                    {
                                        buffer += l.Text;
                                        snippets.RemoveAt(snippets.Count - 1);
                                    }
                                }
                                break; 
                            }
                        }
                        if (c == ']' && state != -1)
                        {
                            hide = false;
                            if (state == -1) snippets.Add(new TextSnippet(buffer, color));
                            else
                            {
                                var s = ParseInternal(handler, buffer, color, options);
                                if (s == null) return null;
                                if (s is MultipleSnippets ms) snippets.AddRange(ms.snippets);
                                else snippets.Add(s);
                            }
                            return i;
                        }
                        buffer += c;
                        break;
                }
                i++;
            }
            if (hide) buffer += "\\";
            if (buffer.Length != 0 && state == -1) snippets.Add(new TextSnippet(buffer, color));
            return null;
        }
        private static TextSnippet ParseInternal(char handler, string text, Color color, string options)
        {
            return handler switch
            {
                'c' => Singleton<ColorTagHandler>.Parse(text, color, options),
                'i' => Singleton<ItemTagHandler>.Parse(text, color, options),
                'n' => Singleton<NameTagHandler>.Parse(text, color, options),
                'a' => Singleton<AchievementTagHandler>.Parse(text, color, options),
                'g' => Singleton<GlyphTagHandler>.Parse(text, color, options),
                'l' => Singleton<LocalizationTagHandler>.Parse(text, color, options),
                't' => Singleton<TransitionTagHandler>.Parse(text, color, options),
                'u' => Singleton<URLTagHandler>.Parse(text, color, options),
                _ => null
            };
        }
    }
}
