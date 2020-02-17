using SocialNetwork.Dal.Models;
using SocialNetwork.Dal.ViewModels.In;
using System.Threading.Tasks;

namespace SocialNetwork.Bll.Abstractions
{
    public interface IAttachmentService
    {
        Task<Attachment> SaveAttachment(AttachmentViewModel attachment);
        Task<bool> AttachmentToPost(PostAttachmentViewModel postAttachment);

        Task<bool> AttachmentToComment(CommentAttachmentViewModel commentAttachment);
    }
}
