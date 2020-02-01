using AutoMapper;
using SocialNetwork.Dal.Models;
using SocialNetwork.Dal.ViewModels;

namespace SocialNetwork.Dal
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<PostViewModel, Post>();
            CreateMap<Post, OutPostViewModel>();

            CreateMap<User, OutUserRegisterViewModel>();
        }
    }
}
