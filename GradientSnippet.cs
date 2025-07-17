using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.UI.Chat;

namespace FormattingPlusPlus
{
    public class GradientSnippet : TextSnippet
    {
        public Color[] colors;
        public float offset;
        public float speed;
        public GradientSnippet(string text, Color[] colors, float offset, float speed)
        {
            this.colors = colors;
            this.offset = offset;
            this.speed = speed;
            Color = Color.White;
            Text = TextOriginal = text;
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
    }
}
