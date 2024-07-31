using Microsoft.AspNetCore.Mvc;
using OpenAI.Interfaces;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using Xabe.FFmpeg;
using YA.VideoTranscriberApp.BlazorUI.Client.Models;

namespace YA.VideoTranscriberApp.BlazorUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IOpenAIService _openAiService;

        public VideosController(IWebHostEnvironment environment, IOpenAIService openAiService)
        {
            _environment = environment;
            _openAiService = openAiService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadAsync(VideoUploadRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var uploadsPath = EnsureUploadsDirectory();

                var videoFilePath = await SaveVideoFileAsync(request.Video, uploadsPath, cancellationToken);

                var audioFilePath = await ExtractAudioFromVideoAsync(videoFilePath, cancellationToken);

                var transcriptionResponse = await TranscribeAudioAsync(audioFilePath, cancellationToken);

                var transcriptions = await TranslateTranscriptionsAsync(transcriptionResponse, request.Languages, cancellationToken);

                return Ok(transcriptions);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        private async Task<List<TranscriptionModel>> TranslateTranscriptionsAsync(TranscribeAudioResponse transcription, string[] languages, CancellationToken cancellationToken)
        {
            var tasks = languages.Select(language => TranslateTranscriptionAsync(transcription, language, cancellationToken));

            var results = await Task.WhenAll(tasks);

            /*
             *  Hakan Hocam -> 1.3 sec.
                Fatih Bey -> 1.6 sec.
                Birgül Hanım -> 1.2 sec.

                Total: 4.1 sec.

                Parallel Process Time: 1.6 sec.
             */

            return results.ToList();
        }

        private async Task<TranscriptionModel> TranslateTranscriptionAsync(TranscribeAudioResponse transcription, string language, CancellationToken cancellationToken)
        {
            var completionResult = await _openAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
            {
                Messages = new List<ChatMessage>
                {
                    ChatMessage.FromSystem($"You are a helpful translator from {transcription.Language} to {language}. You've worked as a translator your whole life and are very good at it. Don't include any extra explanations in your responses!"),
                    ChatMessage.FromUser($"Could you please translate the text given below to {language}. The text is a \".srt\" file. Therefore, do not change the time values! If I like your suggestions, I'll tip you $5000 for your help.\n{transcription.Text}"),
                },
                Model = OpenAI.ObjectModels.Models.Gpt_4o,
            }, cancellationToken:cancellationToken);

            var translatedTranscription = completionResult.Choices.First().Message.Content;

            return new TranscriptionModel
            {
                Language = language,
                Text = translatedTranscription
            };
        }

        private async Task<TranscribeAudioResponse> TranscribeAudioAsync(string audioFilePath, CancellationToken cancellationToken)
        {
            var fileName = Path.GetFileName(audioFilePath);

            // Read the audio file as byteArray with FileStream
            var bufferSize = 1024 * 1024;

            await using var fileStream = new FileStream(audioFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, FileOptions.Asynchronous);

            await using var memoryStream = new MemoryStream();

            await fileStream.CopyToAsync(memoryStream, bufferSize, cancellationToken);

            var audioResult = await _openAiService.Audio.CreateTranscription(new AudioCreateTranscriptionRequest
            {
                FileName = fileName,
                File = memoryStream.ToArray(),
                Model = OpenAI.ObjectModels.Models.WhisperV1,
                ResponseFormat = StaticValues.AudioStatics.ResponseFormat.Srt,
            }, cancellationToken);

            return new TranscribeAudioResponse(audioResult.Text,audioResult.Language);
        }

        private string EnsureUploadsDirectory()
        {
            var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);
            return uploadsPath;
        }

        private async Task<string> SaveVideoFileAsync(IFormFile videoFile, string uploadsPath, CancellationToken cancellationToken)
        {
            var videoFileName = $"{Guid.NewGuid()}{Path.GetExtension(videoFile.FileName)}";
            var videoFilePath = Path.Combine(uploadsPath, videoFileName);

            await using var stream = new FileStream(videoFilePath, FileMode.Create);
            await videoFile.CopyToAsync(stream, cancellationToken);

            return videoFilePath;
        }

        private async Task<string> ExtractAudioFromVideoAsync(string videoFilePath, CancellationToken cancellationToken)
        {
            string audioFilePath = Path.ChangeExtension(videoFilePath, "mp3");

            FFmpeg.SetExecutablesPath("X:\\Downloads\\Compressed\\ffmpeg-master-latest-win64-gpl\\ffmpeg-master-latest-win64-gpl\\bin");

            var conversion = await FFmpeg.Conversions.FromSnippet.ExtractAudio(videoFilePath, audioFilePath);
            await conversion.Start(cancellationToken);

            Console.WriteLine("Conversion completed.");
            Console.WriteLine(audioFilePath);

            return audioFilePath;
        }
    }

    public class VideoUploadRequest
    {
        public IFormFile Video { get; set; }
        public string[] Languages { get; set; }
    }
    
    public class TranscribeAudioResponse
    {
        public string Text { get; set; }
        public string Language { get; set; }

        public TranscribeAudioResponse (string text, string language)
        {
            
            Text = text;

            Language = language;
        }
    }
}
