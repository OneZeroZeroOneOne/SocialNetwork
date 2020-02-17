using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.Dal.ViewModels.In;
using SocialNetwork.Dal.ViewModels.Out;

namespace SocialNetwork.Bll.Abstractions
{
    public interface IAttachmentService
    {
        Task<OutAttachmentViewModel> SaveAttachmentPost(PostAttachmentViewModel postAttachment, string rootPath);

        Task<OutAttachmentViewModel> SaveAttachmentComment(CommentAttachmentViewModel commentAttachment, string rootPath);
    }
}
