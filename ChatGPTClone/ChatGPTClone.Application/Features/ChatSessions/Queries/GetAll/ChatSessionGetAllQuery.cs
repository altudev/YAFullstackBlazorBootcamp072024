using MediatR;

namespace ChatGPTClone.Application.Features.ChatSessions.Queries.GetAll
{
    public class ChatSessionGetAllQuery: IRequest<List<ChatSessionGetAllDto>>
    {
        public ChatSessionGetAllQuery()
        {
            
        }
    }
}
