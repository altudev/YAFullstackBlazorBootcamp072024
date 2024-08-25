using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatGPTClone.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
        protected readonly ISender Mediatr;

        public ApiControllerBase(ISender mediatr)
        {
            Mediatr = mediatr;
        }

    }
}
