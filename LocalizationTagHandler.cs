using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.Localization;
using Terraria.UI.Chat;

namespace FormattingPlusPlus
{
    public class LocalizationTagHandler : ITagHandler
    {
        public class LocalizationSnippet : TextSnippet { public LocalizationSnippet(string text, Color color) : base(text, color) { } }
        TextSnippet ITagHandler.Parse(string text, Color baseColor, string options)
        {
            if (options.Length == 0) return new TextSnippet(text, baseColor);
            string t = "";
            try
            {
                t = Language.GetTextValue(options, Split(text));
            }
            catch
            {
                try
                {
                    t = Language.GetTextValue(options);
                }
                catch
                {
                    t = "<" + options + ">";    // I can do in in initialization, but i do it here ;)
                }
            }
            return new LocalizationSnippet(t, baseColor);
        }
        public string[] Split(string text)
        {
            if (text.Length == 0) return [];
            if (!text.Contains("/")) return [text];
            var result = new List<string>(2) { "" };
            var hide = false;
            foreach (var item in text)
            {
                if (hide)
                {
                    hide = false;
                    if (item != '/') result[0] += "\\";
                }
                else if (item == '/') { if (result[0].Length != 0) result.Insert(0, ""); continue; }
                else if (item == '\\') { hide = true; continue; }
                result[0] += item;
            }
            if (hide) result[0] += "\\";
            if (result[0].Length == 0) result.RemoveAt(0);
            result.Reverse();
            return result.ToArray();    // I do not want to rewrite code, so i just reverse list XD
        }
    }

}
