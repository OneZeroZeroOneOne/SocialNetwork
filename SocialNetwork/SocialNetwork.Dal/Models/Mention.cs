namespace SocialNetwork.Dal.Models
{
    public class Mention
    {
        public int MentionerId { get; set; }
        public int MentionId { get; set; }
        public bool IsComment { get; set; }


        public virtual Comment Comment { get; set; }
        public virtual Post Post { get; set; }
    }
}
