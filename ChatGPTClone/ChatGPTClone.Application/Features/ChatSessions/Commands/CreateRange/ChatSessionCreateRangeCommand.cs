using System;
using ChatGPTClone.Application.Common.Models.General;
using ChatGPTClone.Domain.Entities;
using ChatGPTClone.Domain.Enums;
using ChatGPTClone.Domain.ValueObjects;
using MediatR;

namespace ChatGPTClone.Application.Features.ChatSessions.Commands.CreateRange;

public class ChatSessionCreateRangeCommand : IRequest<ResponseDto<int>>
{
    public string Content { get; set; }
    public GptModelType Model { get; set; }
    public int Count { get; set; }


    public IEnumerable<ChatSession> MapToChatSessions(Guid appUserId)
    {
        return Enumerable.Range(0, Count)
        .Select(i =>
        {
            var chatSession = new ChatSession
            {
                Id = Guid.NewGuid(),
                CreatedByUserId = appUserId.ToString(),
                Model = Model,
                AppUserId = appUserId,
                CreatedOn = DateTime.UtcNow,
                Title = Content.Length >= 50 ? Content.Substring(0, 50) : Content,
                Threads = new List<ChatThread>(){
                new ChatThread
                {
                    Id = Ulid.NewUlid().ToString(),
                    CreatedOn = DateTime.UtcNow,
                    Messages = new List<ChatMessage>(){
                        new ChatMessage
                        {
                            Id = Ulid.NewUlid().ToString(),
                            Content = Content,
                            CreatedOn = DateTime.UtcNow,
                            Model = Model,
                            Type = ChatMessageType.User,
                        }
                    }
                }
            }
            };

            return chatSession;
        });
    }
}
