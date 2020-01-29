using System;
using System.Linq;
using Bogus;
using SocialNetwork.Dal.Models;

namespace SocialNetwork.Fake
{
    class Program
    {
        static void Main(string[] args)
        {
            
        }

    }

    public class FakeUser
    {
        public Faker<User> _fakeUser;
        public FakeUser()
        {
            var fakePost = new Faker<Post>()
                .RuleFor(x => x.Text, x => x.Lorem.Text())
                .RuleFor(x => x.ImgUrl, x => x.Internet.Avatar())
                .RuleFor(x => x.Date, x => x.Date.Recent(12));
                //.RuleFor(x => x.PostComment, x => x.)

            var fakeUserPost = new Faker<UserPost>()
                .RuleFor(x => x.Post, x => fakePost.Generate(1).First());

            _fakeUser = new Faker<User>()
                .RuleFor(x => x.Name, x=> x.Name.FullName())
                .RuleFor(x => x.UserPost, x => fakeUserPost.Generate(10).ToList())
                .RuleFor()
        }
    }
}
