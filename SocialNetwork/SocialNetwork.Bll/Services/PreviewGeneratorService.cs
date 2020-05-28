using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SocialNetwork.Bll.Abstractions;
using System;
using System.IO;
using System.Threading.Tasks;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Model;

namespace SocialNetwork.Bll.Services
{
    public class PreviewGeneratorService : IPreviewGeneratorService
    {
        public async Task GeneratePreviewVideo(string videoDirectory, string videoName, string extension)
        {
            IConversionResult result = await Conversion.Snapshot(Path.Combine(videoDirectory, videoName + extension), Path.Combine(videoDirectory, videoName + "_preview.png"), TimeSpan.FromSeconds(0))
                .Start();
        }

        public void GeneratePreviewPreload(string previewDirectory, string imageName, string extension)
        {
            using (var image = Image.Load(Path.Combine(previewDirectory, imageName + extension)))
            {
                image.Mutate(x => x
                    .Resize(image.Width / 15, image.Height / 15));

                image.Save(Path.Combine(previewDirectory, imageName + "_preload.png")); // Automatic encoder selected based on extension.
            }
        }

        public async Task GetDimensions()
        {
        }
    }
}
