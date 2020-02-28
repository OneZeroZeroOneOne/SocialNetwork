using System.Collections.Generic;
using System.Linq;
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
            CreateMap<PagedResult<Comment>, PagedResult<OutCommentViewModel>>();
            CreateMap<PagedResult<Post>, PagedResult<OutPostViewModel>>();

            CreateMap<List<AttachmentComment>, List<OutAttachmentViewModel>>();
            CreateMap<List<AttachmentPost>, List<OutAttachmentViewModel>>();

            CreateMap<AttachmentComment, OutAttachmentViewModel>()
                .ForMember(x => x.Id, y => y.MapFrom(x => x.Attachment.Id))
                .ForMember(x => x.Path, y => y.MapFrom(x => x.Attachment.Path))
                .ForMember(x => x.Preview, y => y.MapFrom(x => x.Attachment.Preview));

            CreateMap<AttachmentPost, OutAttachmentViewModel>()
                .ForMember(x => x.Id, y => y.MapFrom(x => x.Attachment.Id))
                .ForMember(x => x.Path, y => y.MapFrom(x => x.Attachment.Path))
                .ForMember(x => x.Preview, y => y.MapFrom(x => x.Attachment.Preview));

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
