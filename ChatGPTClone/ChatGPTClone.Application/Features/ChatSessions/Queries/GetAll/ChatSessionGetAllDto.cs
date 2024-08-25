using ChatGPTClone.Domain.Entities;
using ChatGPTClone.Domain.Enums;

namespace ChatGPTClone.Application.Features.ChatSessions.Queries.GetAll;

public sealed class ChatSessionGetAllDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public GptModelType Model { get; set; }
    public Guid AppUserId { get; set; }
    public DateTimeOffset CreatedOn { get; set; }

    public static ChatSessionGetAllDto MapFromChatSession(ChatSession chatSession)
    {
        return new ChatSessionGetAllDto()
        {
            Id = chatSession.Id,
            Title = chatSession.Title,
            Model = chatSession.Model,
            AppUserId = chatSession.AppUserId,
            CreatedOn = chatSession.CreatedOn
        };
    }
}