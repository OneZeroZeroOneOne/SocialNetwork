namespace SocialNetwork.Dal.Models
{
    public partial class CommentComment
    {
        public int CommentId { get; set; }
        public int ReplyToCommentId { get; set; }

        public virtual Comment Comment { get; set; }
        public virtual Comment ReplyToComment { get; set; }
    }
}
