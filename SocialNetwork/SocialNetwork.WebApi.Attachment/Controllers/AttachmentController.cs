using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Bll.Abstractions;
using SocialNetwork.Dal.ViewModels.In;
using SocialNetwork.Dal.ViewModels.Out;
using System.Threading.Tasks;


namespace SocialNetwork.WebApi.Attachment.Controllers
{
    public class AttachmentController
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IAttachmentService _attachmentService;

        public AttachmentController(IWebHostEnvironment appEnvironment, IAttachmentService attachmentService)
        {
            _appEnvironment = appEnvironment;
            _attachmentService = attachmentService;
        }

        [HttpPost, Route("Post")]
        public async Task<OutAttachmentViewModel> CreatePostAttachment(PostAttachmentViewModel postAttachment)
        {
            return await _attachmentService.SaveAttachmentPost(postAttachment, _appEnvironment.ContentRootPath);
        }

        [HttpPost, Route("Comment")]
        public async Task<OutAttachmentViewModel> CreateCommentAttachment(CommentAttachmentViewModel commentAttachment)
        {
            return await _attachmentService.SaveAttachmentComment(commentAttachment, _appEnvironment.ContentRootPath);
        }
    }
}
