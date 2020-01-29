using System;
using Bogus;
using SocialNetwork.Dal.Models;

namespace SocialNetwork.Fake.Fake
{
    class FakeUser
    {
        private readonly Faker<User> _fakeUser;
        public FakeUser()
        {
            _fakeUser = new Faker<User>()
                .RuleFor(x => x.Name, x => x.Name.LastName())
        }
        public User Generate()
        {
            return _fakeUser.Generate();
        }
    }
}
