using Markdig.Helpers;
using Markdig.Parsers;

namespace SocialNetwork.Markdown.MarkdownExtensions.LinkTo
{
    public class LinkToParser : InlineParser
    {
        private static readonly char[] _openingCharacters =
        {
            '>'
        };

        public LinkToParser()
        {
            OpeningCharacters = _openingCharacters;
        }

        public override bool Match(InlineProcessor processor, ref StringSlice slice)
        {
            var previous = slice.PeekCharExtra(-1);

            if (!previous.IsWhiteSpaceOrZero()) return false;

            slice.NextChar();

            if (slice.CurrentChar != '>') return false;

            var current = slice.NextChar();
            var start = slice.Start;
            var end = start;

            while (current.IsDigit())
            {
                end = slice.Start;
                current = slice.NextChar();
            }

            if (!current.IsWhiteSpaceOrZero() && current.IsDigit()) return false;
            var inlineStart = processor.GetSourcePosition(slice.Start, out var line, out var column);

            if (!int.TryParse(new StringSlice(slice.Text, start, end).ToString(), out var parsedId))
                return false;

            processor.Inline = new LinkToParsedModel
            {
                Span =
                {
                    Start = inlineStart,
                    End = inlineStart + (end - start) + 1
                },
                Line = line,
                Column = column,
                Id = parsedId,
            };

            return true;
        }
    }
}
