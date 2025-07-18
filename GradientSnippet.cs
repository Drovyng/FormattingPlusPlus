using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader.UI;
using Terraria.UI.Chat;

namespace FormattingPlusPlus
{
    public class GradientSnippet : TextSnippet
    {
        public Color[] colors;
        public float offset;
        public float speed;
        public string url;
        public GradientSnippet(string text, Color[] colors, float offset, float speed)
        {
            this.colors = colors;
            this.offset = offset;
            this.speed = speed;
            Color = Color.White;
            Text = TextOriginal = text;
        }
        public void SetURL(string _url)
        {
            if (!_url.Contains("://")) url = "https://" + _url;
            else url = _url;
            CheckForHover = true;
        }
        public override void Update()
        {
            var l = (float)Main._drawInterfaceGameTime.TotalGameTime.TotalSeconds * speed + offset;
            Color = Color.Lerp(
                colors[(int)l % colors.Length],
                colors[(int)(l + 1) % colors.Length],
                l % 1f
            );
        }
        public override void OnClick()
        {
            if (url != null) Utils.OpenToURL(url);
        }
        public override void OnHover()
        {
            if (url != null) UICommon.TooltipMouseText(url);
        }
    }
}
