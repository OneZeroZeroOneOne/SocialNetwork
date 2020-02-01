using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;

namespace SocialNetwork.Dal.ValueGenerators
{
    public class CurrentDateTimeGenerator : Microsoft.EntityFrameworkCore.ValueGeneration.ValueGenerator
    {
        public override bool GeneratesTemporaryValues => false;
        protected override object NextValue(EntityEntry entry)
        {
            return DateTime.UtcNow;
        }
    }
}
