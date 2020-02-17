using AutoMapper;
using SocialNetwork.Dal.Models;
using SocialNetwork.Dal.ViewModels;
using SocialNetwork.Dal.ViewModels.In;
using SocialNetwork.Dal.ViewModels.Out;

namespace SocialNetwork.Dal
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Attachment, OutAttachmentViewModel>();

            CreateMap<Board, OutBoardViewModel>();

            // Add as many of these lines as you need to map your objects
            CreateMap<PostViewModel, Post>();
            CreateMap<Post, OutPostViewModel>();
            CreateMap<EditPostViewModel, Post>();

            CreateMap<CommentViewModel, Comment>();
            CreateMap<Comment, OutCommentViewModel>();
            CreateMap<EditCommentViewModel, Comment>()
                .ForMember(destinationMember => destinationMember.Id, sourceMember => sourceMember.MapFrom(x => x.CommentId));

            CreateMap<User, OutUserRegisterViewModel>();

            CreateMap<ReactionPostViewModel, ReactionPost>()
                .ForMember(destinationMember => destinationMember.PostId, sourceMember => sourceMember.MapFrom(x => x.PostId));
            CreateMap<ReactionPost, OutReactionPostViewModel>()
                .ForMember(destinationMember => destinationMember.PostId, sourceMember => sourceMember.MapFrom(x => x.PostId));
            CreateMap<ReactionCommentViewModel, ReactionComment>()
                .ForMember(destinationMember => destinationMember.CommentId, sourceMember => sourceMember.MapFrom(x => x.CommentId));
            CreateMap<ReactionComment, OutReactionCommentViewModel>()
                .ForMember(destinationMember => destinationMember.CommentId, sourceMember => sourceMember.MapFrom(x => x.CommentId));
        }
    }
}
