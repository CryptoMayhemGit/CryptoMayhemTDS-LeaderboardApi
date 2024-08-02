using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Mayhem.Util.Exceptions;
using Mayhem.Util.Classes;

namespace Mayhem.LeaderBoardApi.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            this.logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            Exception ex = context.Exception;
            string errorMessage = ex.Message;

            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
                errorMessage = errorMessage + ". " + ex.Message;
            }

            if (context.Exception is NotFoundException nEx)
            {
                context.Result = new NotFoundObjectResult(nEx.ValidationMessage);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                logger.LogWarning(errorMessage);
            }
            else if (context.Exception is ValidationException vEx)
            {
                context.Result = new BadRequestObjectResult(vEx.ValidationMessage);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                logger.LogWarning(errorMessage);
            }
            else if (context.Exception is InternalException iEx)
            {
                context.Result = new ObjectResult(iEx.ValidationMessage);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                logger.LogError(errorMessage);
            }
            else
            {
                context.Result = new ObjectResult(new ErrorResponse(new ErrorModel("InternalServerError", "Internal server error occured")));
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                logger.LogError(errorMessage);
            }

            context.ExceptionHandled = true;
        }
    }
}
