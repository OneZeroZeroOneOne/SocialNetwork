using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using SocialNetwork.Dal.Context;
using SocialNetwork.Dal.Models;

namespace SocialNetwork.Fake
{
    class Program
    {
        static void Main(string[] args)
        {
            var userGenerator = new FakeUser();
            var listUsers = userGenerator.Generate(3);

            var context = new PublicContext();
            context.AddRange(listUsers);
            context.SaveChanges();
        }

    }

    public class FakeUser
    {
        private Faker<User> _fakeUser;
        public User Generate()
        {
            var userId = Guid.NewGuid();
            var fakeComment = new Faker<OutCommentViewModel>()
                .RuleFor(x => x.Text, x => x.Lorem.Text())
                .RuleFor(x => x.Date, x => x.Date.Recent(12))
                .RuleFor(x => x.UserId, x => userId);


            var fakePost = new Faker<Post>()
                .RuleFor(x => x.Text, x => x.Lorem.Text())
                .RuleFor(x => x.Date, x => x.Date.Recent(12))
                .RuleFor(x => x.Comments, x => fakeComment.Generate(x.Random.Number(10)).ToList());

            /*var fakeUserSecurity = new Faker<UserSecurity>()
                .RuleFor(x => x.Password, x => x.Lorem.Word())
                .RuleFor(x => x.Email, x => x.Lorem.Word()+"@gmail.com")
                .RuleFor(x => x.UserId, x => userId);*/

            _fakeUser = new Faker<User>()
                .RuleFor(x => x.Name, x => x.Name.FullName())
                .RuleFor(x => x.Posts, x => fakePost.Generate(x.Random.Number(10)).ToList())
                .RuleFor(x => x.Id, x => userId);

            return _fakeUser.Generate();
        }

        public List<User> Generate(int count)
        {
            var list = new List<User>();
            for (int i = 0; i < count; i++)
            {
                list.Add(Generate());
            }

            return list;
        }
    }
}
