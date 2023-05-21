using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.ErrorHandling;

public static class ErrorHandlingExtensions
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
    {
        app.Run(async context => {
            var logger = context.RequestServices.GetRequiredService<ILoggerFactory>()
                                                .CreateLogger("Error Handling");

            var exceptionDetails = context.Features.Get<IExceptionHandlerFeature>();
            var exception = exceptionDetails?.Error;
            logger.LogError(exception, "Could not process request on server {Server}. TraceId {TraceId}",
                                Environment.MachineName,Activity.Current?.TraceId);

            var problem = new ProblemDetails
            {
                Title = "We made a mistake but we are working on it",
                Status = StatusCodes.Status500InternalServerError,
                Extensions = 
                {
                    {"TraceId",Activity.Current?.TraceId.ToString()}
                }
                
            };
            var environment = context.RequestServices.GetRequiredService<IHostEnvironment>();
            if(environment.IsDevelopment())
            {
                problem.Detail = exception ?.ToString();
            }
            await Results.Problem(problem).ExecuteAsync(context);
        });
    }
}