using System;
using System.IO;
using System.Threading.Tasks;
using SocialNetwork.Bll.Abstractions;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Enums;
using Xabe.FFmpeg.Model;

namespace SocialNetwork.Bll.Services
{
    public class PreviewGeneratorService : IPreviewGeneratorService
    {
        public async Task GeneratePreview(string videoDirectory, string videoName, string extension)
        {
            IConversionResult result = await Conversion.Snapshot(Path.Combine(videoDirectory, videoName + extension), Path.Combine(videoDirectory, videoName + "_preview.png"), TimeSpan.FromSeconds(0))
                .Start();
        }
    }
}
