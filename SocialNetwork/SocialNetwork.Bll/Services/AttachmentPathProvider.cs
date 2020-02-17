using SocialNetwork.Bll.Abstractions;
using System.IO;
using System.Runtime.InteropServices;

namespace SocialNetwork.Bll.Services
{
    public class AttachmentPathProvider : IAttachmentPathProvider
    {
        public string Path;
        public static bool IsLinux => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        public string GetPath()
        {
            return Path;
        }

        public void ConfigurePath()
        { 
            Path = IsLinux ? "/root" : Directory.GetCurrentDirectory() + @"\wwwroot";
        }
    }
}
