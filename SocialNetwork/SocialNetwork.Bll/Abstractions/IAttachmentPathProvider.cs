namespace SocialNetwork.Bll.Abstractions
{
    public interface IAttachmentPathProvider
    {
        void ConfigurePath();

        string GetPath();
    }
}
