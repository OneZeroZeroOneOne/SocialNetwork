using Markdig.Helpers;
using Markdig.Parsers;

namespace SocialNetwork.Markdown.MarkdownExtensions.GreenText
{
    public class GreenTextParser : InlineParser
    {
        private static readonly char[] _openingCharacters =
        {
            '>'
        };

        public GreenTextParser()
        {
            OpeningCharacters = _openingCharacters;
        }

        public override bool Match(InlineProcessor processor, ref StringSlice slice)
        {
            var previous = slice.PeekCharExtra(-1);

            if (!previous.IsWhiteSpaceOrZero()) return false;

            slice.NextChar();

            if (slice.CurrentChar == '>') return false;

            var current = slice.NextChar();
            var start = slice.Start;
            var end = start;

            while (!current.IsNewLine() && !current.IsZero())
            {
                end = slice.Start;
                current = slice.NextChar();
            }

            //if (!current.IsWhiteSpaceOrZero()) return false;
            var inlineStart = processor.GetSourcePosition(slice.Start, out var line, out var column);

            /*if (!int.TryParse(new StringSlice(slice.Text, start, end).ToString(), out var parsedId))
                return false;*/

            processor.Inline = new GreenTextParsedModel
            {
                Span =
                {
                    Start = inlineStart,
                    End = inlineStart + (end - start) + 1
                },
                Line = line,
                Column = column,
                Text = new StringSlice(slice.Text, start - 1, end).ToString(),
            };

            return true;
        }
    }
}