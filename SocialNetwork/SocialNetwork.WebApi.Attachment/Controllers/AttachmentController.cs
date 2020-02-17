using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Bll.Abstractions;
using SocialNetwork.Dal.ViewModels.In;
using SocialNetwork.Dal.ViewModels.Out;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace SocialNetwork.WebApi.Attachment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AttachmentController
    {
        private readonly IAttachmentService _attachmentService;
        private readonly IMapper _mapper;

        public AttachmentController(IAttachmentService attachmentService, IMapper mapper)
        {
            _attachmentService = attachmentService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<OutAttachmentViewModel> CreateAttachment(IFormFile postAttachment)
        {
            return _mapper.Map<Dal.Models.Attachment, OutAttachmentViewModel>(await _attachmentService.SaveAttachment(new AttachmentViewModel() {UploadFile = postAttachment}));
        }

        [HttpPut, Route("Post")]
        public async Task<bool> CreatePostAttachment(PostAttachmentViewModel postAttachment)
        {
            return await _attachmentService.AttachmentToPost(postAttachment);
        }

        [HttpPut, Route("Comment")]
        public async Task<bool> CreateCommentAttachment(CommentAttachmentViewModel commentAttachment)
        {
            return await _attachmentService.AttachmentToComment(commentAttachment);
        }
    }
}
