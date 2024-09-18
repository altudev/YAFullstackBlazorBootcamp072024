namespace ChatGPTClone.Application.Common.Models.OpenAI;

public class OpenAIChatResponse
{
    public string Response { get; set; }

    public OpenAIChatResponse(string response)
    {
        Response = response;
    }
}
