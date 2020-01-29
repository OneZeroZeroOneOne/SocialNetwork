using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
