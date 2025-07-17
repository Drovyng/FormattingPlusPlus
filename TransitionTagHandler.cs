using Microsoft.Xna.Framework;
using Terraria.UI.Chat;

namespace FormattingPlusPlus
{
    public class TransitionTagHandler : ITagHandler
    {
        TextSnippet ITagHandler.Parse(string text, Color _, string options)
        {
            var o = options.Split('/');
            if (o.Length < 4 || !int.TryParse(o[0], out int speed) || !int.TryParse(o[1], out int size)) return new TextSnippet(text);
            var colors = new Color[o.Length - 2];
            for (int i = 2; i < o.Length; i++)
            {
                if (!int.TryParse(o[i], System.Globalization.NumberStyles.AllowHexSpecifier, null, out int result)) return new TextSnippet(text);
                colors[i - 2] = new Color((result >> 16) & 0xFF, (result >> 8) & 0xFF, result & 0xFF);
            }
            var s = new MultipleSnippets(new());
            for (int i = 0; i < text.Length; i++) s.snippets.Add(new GradientSnippet(text[i] + "", colors, size == 0 ? 0 : (float)i / size, speed == 0 ? 0 : 10f / speed));
            return s;
        }
    }

}
