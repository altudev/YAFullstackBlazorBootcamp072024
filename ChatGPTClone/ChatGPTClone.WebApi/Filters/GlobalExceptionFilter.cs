using ChatGPTClone.Application.Common.Localization;
using ChatGPTClone.Application.Common.Models.Errors;
using ChatGPTClone.Application.Common.Models.General;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;

namespace ChatGPTClone.WebApi.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;
    private readonly IStringLocalizer<CommonLocalization> _localizer;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger, IStringLocalizer<CommonLocalization> localizer)
    {
        _logger = logger;
        _localizer = localizer;
    }

    public void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, context.Exception.Message);

        context.ExceptionHandled = true;

        // Eğer hata bir doğrulama hatası ise
        if (context.Exception is ValidationException validationException)
        {

            var responseMessage = _localizer[CommonLocalizationKeys.GeneralValidationException];

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
            context.Result = new ObjectResult(new ResponseDto<string>(_localizer[CommonLocalizationKeys.GeneralInternalServerException], false))
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }
}
