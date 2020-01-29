using System;
using Bogus;
using SocialNetwork.Dal.Models;


namespace SocialNetwork.Fake.Fake
{
    class FakePost
    {
        public readonly Faker<Post> _fakePost;
        public FakePost()
        {

        }
        public User Generate()
        {
            return _fakePost.Generate();
        }

    }

    
}
