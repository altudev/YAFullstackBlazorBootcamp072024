using ChatGPTClone.Application.Common.Models.OpenAI;

namespace ChatGPTClone.Application.Common.Interfaces;

public interface IOpenAiService
{
    Task<OpenAIChatResponse> ChatAsync(OpenAIChatRequest request, CancellationToken cancellationToken);
}
