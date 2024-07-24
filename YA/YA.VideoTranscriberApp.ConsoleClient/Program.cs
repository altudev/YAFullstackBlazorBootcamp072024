using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels;
using System;
using System.Resources;
using Xabe.FFmpeg;
using OpenAI.Managers;
using OpenAI;

var videoFilePath = "C:\\Users\\alper\\Desktop\\Sam-Bankman-Fried.mp4";

// Get audio file path
// "C:\\Users\\alper\\Desktop\\Sam-Bankman-Fried.mp3";
string audioFilePath = Path.ChangeExtension(videoFilePath, "mp3");

CancellationTokenSource cts = new CancellationTokenSource();

// CancellationToken

var conversion = await FFmpeg.Conversions.FromSnippet.ExtractAudio(videoFilePath, audioFilePath);

await conversion.Start(cts.Token);

Console.WriteLine("Conversion completed.");

Console.WriteLine(audioFilePath);


const string fileName = "Sam-Bankman-Fried.mp3";

var sampleFile = await File.ReadAllBytesAsync(audioFilePath);

var apiKey = File.ReadAllText("C:\\Users\\alper\\Desktop\\OpenAIApiKey.txt");

var openAiService = new OpenAIService(new OpenAiOptions()
{
    ApiKey = apiKey

});

var audioResult = await openAiService.Audio.CreateTranscription(new AudioCreateTranscriptionRequest
{
    FileName = fileName,
    File = sampleFile,
    Model = Models.WhisperV1,
    ResponseFormat = StaticValues.AudioStatics.ResponseFormat.Srt
});

var transcription = audioResult.Text;

Console.WriteLine(transcription);

await File.WriteAllTextAsync("C:\\Users\\alper\\Desktop\\WhisperResponse.srt", transcription, cts.Token);

var language = "Turkish";

var completionResult = await openAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
{
    Messages = new List<ChatMessage>
    {
        ChatMessage.FromSystem($"You are a helpful translator from English to {language}. You've worked as a translator your whole life and are very good at it. Don't include any extra explanations in your responses!"),
        ChatMessage.FromUser($"Could you please translate the text given below to {language}. The text is a \".srt\" file. Therefore, do not change the time values! If I like your suggestions, I'll tip you $5000 for your help.\n{transcription}"),
    },
    Model = Models.Gpt_4o,
});
if (completionResult.Successful)
{
    var turkishTranscription = completionResult.Choices.First().Message.Content;

    Console.WriteLine(turkishTranscription);

    await File.WriteAllTextAsync("C:\\Users\\alper\\Desktop\\WhisperResponse.tr-TR.srt", turkishTranscription, cts.Token);
}