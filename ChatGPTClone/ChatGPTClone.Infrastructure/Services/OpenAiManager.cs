using System;
using ChatGPTClone.Application.Common.Interfaces;
using ChatGPTClone.Application.Common.Models.OpenAI;
using ChatGPTClone.Domain.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OpenAI.Interfaces;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;

namespace ChatGPTClone.Infrastructure.Services;

public class OpenAiManager : IOpenAiService
{
    private readonly IOpenAIService _openAIService;
    private readonly ICurrentUserService _currentUserService;

    public OpenAiManager(IOpenAIService openAIService, ICurrentUserService currentUserService)
    {
        _openAIService = openAIService;

        _currentUserService = currentUserService;
    }

    public async Task<OpenAIChatResponse> ChatAsync(OpenAIChatRequest request, CancellationToken cancellationToken)
    {

        var allMessages = GetChatMessages(request.Messages);

        var newMessage = ChatMessage.FromUser(request.Message);

        allMessages.Add(newMessage);

        var completionResult = await _openAIService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
        {
            Messages = allMessages,

            Model = GetChatGPTModel(request.Model),

            MaxTokens = 4096,

            User = _currentUserService.UserId.ToString()
        });

        if (!completionResult.Successful)
            throw new Exception(completionResult.Error.Message);

        return new OpenAIChatResponse(completionResult.Choices.First().Message.Content);
    }

    private string GetChatGPTModel(GptModelType model)
    {
        return model switch
        {
            GptModelType.GPT4o => Models.Gpt_4o,
            GptModelType.GPT4oMini => Models.Gpt_4o_mini,
            GptModelType.GPT4 => Models.Gpt_4,
            _ => Models.Gpt_4o,
        };
    }

    private List<ChatMessage> GetChatMessages(List<ChatGPTClone.Domain.ValueObjects.ChatMessage> messages)
    {
        return messages.Select(message => message.Type switch
        {
            ChatMessageType.System => ChatMessage.FromSystem(message.Content),
            ChatMessageType.Assistant => ChatMessage.FromAssistant(message.Content),
            _ => ChatMessage.FromUser(message.Content)
        }).ToList();
    }
}
