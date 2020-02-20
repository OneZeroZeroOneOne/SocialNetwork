using System;
using System.Collections.Generic;

namespace SocialNetwork.Dal.Models
{
    public class User
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            Posts = new HashSet<Post>();
            ReactionPosts = new HashSet<ReactionPost>();
            ReactionComments = new HashSet<ReactionComment>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public virtual UserSecurity UserSecurity { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<ReactionPost> ReactionPosts { get; set; }
        public virtual ICollection<ReactionComment> ReactionComments{ get; set; }
    }
}
