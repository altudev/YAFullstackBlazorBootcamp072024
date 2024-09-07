using ChatGPTClone.Application.Common.Interfaces;

namespace ChatGPTClone.WebApi.Services;

public class EnvironmentManager : IEnvironmentService
{
    public string WebRootPath { get; }

    public EnvironmentManager(string webRootPath)
    {
        WebRootPath = webRootPath;
    }
}