namespace SocialNetwork.Markdown.Jsonize.Common
{
    public class Counter
    {
        private int Count { get; set; }
        public Counter()
        {
            Count = 0;
        }

        public int Next()
        {
            return Count++;
        }
    }
}
