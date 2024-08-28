using ChatGPTClone.Application.Common.Models.Errors;
using ChatGPTClone.Application.Common.Models.General;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ChatGPTClone.WebApi.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, context.Exception.Message);

        context.ExceptionHandled = true;

        // Eğer hata bir doğrulama hatası ise
        if (context.Exception is ValidationException validationException)
        {
            var responseMessage = "One or more validation errors occurred.";

            var errors = validationException.Errors
                .GroupBy(e => e.PropertyName)
                .Select(g => new ErrorDto(g.Key, g.Select(e => e.ErrorMessage).ToList()))
                .ToList();

            // 400 - Bad Request
            context.Result = new BadRequestObjectResult(new ResponseDto<string>(responseMessage, errors))
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
        }
        else
        {
            // Diğer tüm hatalar için 500 - Internal Server Error
            context.Result = new ObjectResult(new ResponseDto<string>("Internal server error", false))
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }
}
