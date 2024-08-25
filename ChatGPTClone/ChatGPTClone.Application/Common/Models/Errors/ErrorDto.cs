namespace ChatGPTClone.Application.Common.Models.Errors
{
    public class ErrorDto
    {
        // Email
        public string PropertyName { get; set; }

        // "Email is required."
        // "Email is not a valid email address."
        // "Email is too long."
        public IReadOnlyList<string> Messages { get; set; }

        public ErrorDto(string propertyName, List<string> messages)
        {
            PropertyName = propertyName;

            Messages = messages;
        }
    }
}
