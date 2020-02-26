namespace SocialNetwork.Dal.Models
{
    public partial class CommentPost
    {
        public int CommentId { get; set; }
        public int ReplyToPostId { get; set; }

        public virtual Comment Comment { get; set; }
        public virtual Post ReplyToPost { get; set; }
    }
}
