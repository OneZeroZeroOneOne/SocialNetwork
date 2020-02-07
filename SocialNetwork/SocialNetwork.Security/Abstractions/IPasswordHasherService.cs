namespace SocialNetwork.Security.Abstractions
{
    public interface IPasswordHasherService
    {
        string Hash(string password);
        (bool Verified, bool NeedsUpgrade) Check(string hash, string password);
    }
}
