using System.Web.Http.Filters;
using qwe.Utilities;

namespace qwe.Utilities
{
    /// <summary>
    /// Global exception filter for Web API
    /// Catches and logs all unhandled exceptions in API controllers
    /// </summary>
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            // Log the exception
            Logger.Error(
                $"API Error in {context.ActionContext.ControllerContext.ControllerDescriptor.ControllerName}.{context.ActionContext.ActionDescriptor.ActionName}",
                context.Exception,
                "API"
            );

            // Return appropriate error response
            var errorMessage = "An error occurred processing your request.";
            
            // Show detailed errors only in development
            if (Configuration.AppSettings.ShowDetailedErrors)
            {
                errorMessage = context.Exception.Message;
            }

            context.Response = context.Request.CreateErrorResponse(
                System.Net.HttpStatusCode.InternalServerError,
                errorMessage
            );

            base.OnException(context);
        }
    }
}
