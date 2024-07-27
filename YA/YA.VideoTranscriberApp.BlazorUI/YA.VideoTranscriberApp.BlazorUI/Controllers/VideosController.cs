using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xabe.FFmpeg;

namespace YA.VideoTranscriberApp.BlazorUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        public VideosController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpPost]
        public async Task<IActionResult> UploadAsync(VideoUploadRequest request, CancellationToken cancellationToken)
        {
            // wwwroot/uploads/
            var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads");

            // Create uploads directory if it does not exist
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            // cdc86842-2d6e-4358-834b-a3fb8c435d9f.mp4
            var videoFileName =$"{Guid.NewGuid()}{Path.GetExtension(request.Video.FileName)}";

            // wwwroot/uploads/cdc86842-2d6e-4358-834b-a3fb8c435d9f.mp4
            var videoFilePath = Path.Combine(uploadsPath, videoFileName);

            await using var stream = new FileStream(videoFilePath, FileMode.Create);

            await request.Video.CopyToAsync(stream, cancellationToken);

            // wwwroot/uploads/cdc86842-2d6e-4358-834b-a3fb8c435d9f.mp3
            string audioFilePath = Path.ChangeExtension(videoFilePath, "mp3");

            FFmpeg.SetExecutablesPath("X:\\Downloads\\Compressed\\ffmpeg-master-latest-win64-gpl\\ffmpeg-master-latest-win64-gpl\\bin");

            var conversion = await FFmpeg.Conversions.FromSnippet.ExtractAudio(videoFilePath, audioFilePath);

            await conversion.Start(cancellationToken);

            Console.WriteLine("Conversion completed.");

            Console.WriteLine(audioFilePath);

            return Ok();
        }
    }

    public class VideoUploadRequest
    {
        public IFormFile Video { get; set; }
        public string[] Languages { get; set; }
    }
}
