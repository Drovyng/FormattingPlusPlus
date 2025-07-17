using System.Collections.Generic;
using Terraria.UI.Chat;

namespace FormattingPlusPlus
{
    public class MultipleSnippets : TextSnippet
    {
        public List<TextSnippet> snippets;
        public MultipleSnippets(List<TextSnippet> _snippets) { snippets = _snippets; }
    }
}
