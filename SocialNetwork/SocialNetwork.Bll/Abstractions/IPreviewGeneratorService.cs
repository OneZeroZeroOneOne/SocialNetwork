using System.Threading.Tasks;

namespace SocialNetwork.Bll.Abstractions
{
    public interface IPreviewGeneratorService
    {
        Task GeneratePreviewVideo(string videoDirectory, string videoName, string extension);
        void GeneratePreviewPreload(string previewDirectory, string imageName, string extension);
    }
}
