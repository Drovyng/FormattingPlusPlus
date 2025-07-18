using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader.UI;
using Terraria.UI.Chat;

namespace FormattingPlusPlus
{
    public class URLTagHandler : ITagHandler
    {
        TextSnippet ITagHandler.Parse(string text, Color color, string options)
        {
            return new URLSnippet(text, color, options);
        }
        public class URLSnippet : TextSnippet
        {
            public string url;
            public URLSnippet(string text, Color color, string _url)
            {
                CheckForHover = true;
                Text = TextOriginal = text;
                Color = color;
                if (!_url.Contains("://")) url = "https://" + _url;
                else url = _url;
            }
            public override void OnClick()
            {
                Utils.OpenToURL(url);
            }
            public override void OnHover()
            {
                UICommon.TooltipMouseText(url);
            }
        }
    }
}
