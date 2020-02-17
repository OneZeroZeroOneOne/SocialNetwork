using System;
using System.IO;
using SocialNetwork.Bll.Abstractions;

namespace SocialNetwork.Bll.Services
{
    public class AttachmentPathProvider : IAttachmentPathProvider
    {
        public string Path;
        public static bool IsLinux
        {
            get
            {
                var p = (int)Environment.OSVersion.Platform;
                return (p == 4) || (p == 6) || (p == 128);
            }
        }

        public string GetPath()
        {
            return Path;
        }

        public void ConfigurePath()
        { 
            Path = IsLinux ? "/root/Files" : Directory.GetCurrentDirectory() + @"\wwwroot\Files";
        }
    }
}
