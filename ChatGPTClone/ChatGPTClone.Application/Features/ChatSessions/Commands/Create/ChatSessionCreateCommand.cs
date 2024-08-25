using ChatGPTClone.Application.Common.Models.General;
using ChatGPTClone.Domain.Entities;
using ChatGPTClone.Domain.Enums;
using ChatGPTClone.Domain.ValueObjects;
using MediatR;

namespace ChatGPTClone.Application.Features.ChatSessions.Commands.Create
{
    public class ChatSessionCreateCommand:IRequest<ResponseDto<Guid>>
    {
        public GptModelType Model { get; set; }
        public string Content { get; set; }

        public ChatSession ToChatSession(Guid userId)
        {
            return new ChatSession()
            {
                Id = Ulid.NewUlid().ToGuid(), // DateTime.UtcNow DateTime.Now IIDService.NewId(), Guid.V7.NEWGuid
                Model = Model,
                AppUserId = userId,
                CreatedOn = DateTimeOffset.UtcNow,
                CreatedByUserId = userId.ToString(),
                Title = Content.Length >= 50 ? Content.Substring(0, 50) : Content,
                Threads =
                [
                    new ChatThread()
                    {
                        Id = Ulid.NewUlid().ToString(),
                        CreatedOn = DateTimeOffset.UtcNow,
                        Messages =
                        [
                            new ChatMessage()
                            {
                                Id = Ulid.NewUlid().ToString(),
                                Model = Model,
                                Type = ChatMessageType.System,
                                Content = "You're a very helpful and happy assistant which loves to help people.",
                                CreatedOn = DateTimeOffset.UtcNow
                            },
                            new ChatMessage()
                            {
                                Id = Ulid.NewUlid().ToString(),
                                Model = Model,
                                Type = ChatMessageType.User,
                                Content = Content,
                                CreatedOn = DateTimeOffset.UtcNow
                            }
                        ]
                    },
                ]
            };
        } 
    }
}
