namespace ChatGPTClone.Domain.ValueObjects
{
    public sealed class ChatThread
    {
        public string Id { get; set; }

        public List<ChatMessage> Messages { get; set; } = [];

        public DateTimeOffset CreatedOn { get; set; }
    }
}
