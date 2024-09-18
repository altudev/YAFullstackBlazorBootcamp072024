using ChatGPTClone.Domain.Enums;
using ChatGPTClone.Domain.ValueObjects;

namespace ChatGPTClone.Application.Common.Models.OpenAI;

public class OpenAIChatRequest
{
    public GptModelType Model { get; set; }
    public string Message { get; set; }
    public List<ChatMessage> Messages { get; set; }

    public OpenAIChatRequest(GptModelType model, string message)
    {
        Model = model;
        Message = message;
        Messages = [];
    }

    public OpenAIChatRequest(GptModelType model, string message, List<ChatMessage> messages) : this(model, message)
    {
        Messages = messages;
    }
}
