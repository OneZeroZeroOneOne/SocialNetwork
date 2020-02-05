using Microsoft.EntityFrameworkCore;
using SocialNetwork.Bll.Abstractions;
using SocialNetwork.Dal.Context;
using SocialNetwork.Dal.Exceptions;
using SocialNetwork.Dal.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using SocialNetwork.Dal.Extensions;
namespace SocialNetwork.Bll.Services
{
    class ReactionService
    {

        private readonly PublicContext _context;

        public ReactionService(PublicContext publicContext)
        {
            _context = publicContext;
        }
}
