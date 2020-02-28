using System.Threading.Tasks;

namespace SocialNetwork.Bll.Abstractions
{
    public interface IPreviewGeneratorService
    {
        Task GeneratePreview(string videoDirectory, string videoName, string extension);
    }
}
