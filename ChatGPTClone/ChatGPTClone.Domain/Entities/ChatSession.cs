using ChatGPTClone.Domain.Common;
using ChatGPTClone.Domain.Enums;
using ChatGPTClone.Domain.Identity;
using ChatGPTClone.Domain.ValueObjects;

namespace ChatGPTClone.Domain.Entities;

public sealed class ChatSession:EntityBase
{
    public string Title { get; set; }
    public GptModelType Model { get; set; }

    // If the type is List in the entity then it is a ValueObject
    public List<ChatThread> Threads { get; set; } = [];

    public Guid AppUserId { get; set; }
    public AppUser AppUser { get; set; }

    // If the type of the List is ICollection then it is an Entity
    //public ICollection<ChatMessage> ChatMessages { get; set; }
}