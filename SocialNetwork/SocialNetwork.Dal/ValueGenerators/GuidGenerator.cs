using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;

namespace SocialNetwork.Dal.ValueGenerators
{
    public class GuidGenerator : Microsoft.EntityFrameworkCore.ValueGeneration.ValueGenerator
    {
        public override bool GeneratesTemporaryValues => false;
        protected override object NextValue(EntityEntry entry)
        {
            return Guid.NewGuid();
        }
    }
}
