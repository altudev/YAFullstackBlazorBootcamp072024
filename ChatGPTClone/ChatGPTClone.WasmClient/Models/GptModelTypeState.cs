using ChatGPTClone.Domain.Enums;

namespace ChatGPTClone.WasmClient.Models;

public class GptModelTypeState
{
    public GptModelType GptModelType { get; set; } = GptModelType.GPT4o;
    public Action StateChanged { get; set; }
}