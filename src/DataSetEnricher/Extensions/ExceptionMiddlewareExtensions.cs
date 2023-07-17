using System.Net;
using System.Text;
using CardanoAssignment.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CardanoAssignment.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.ContentType = "application/problem+json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                var problemDetails = new ProblemDetails();
             
                if (contextFeature != null)
                {
                    switch (contextFeature.Error)
                    {
                        case LeiDataEnrichmentException leiDataEnrichmentException:
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            problemDetails.Status = (int)HttpStatusCode.InternalServerError;
                            problemDetails.Title = leiDataEnrichmentException.Message;
                            break;
                        case CsvConversionException csvConversionException:
                            context.Response.StatusCode = (int)csvConversionException.StatusCode;
                            problemDetails.Status = (int)csvConversionException.StatusCode;
                            problemDetails.Title = csvConversionException.Message;
                            break;
                        default:
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            problemDetails.Status = (int)HttpStatusCode.InternalServerError;
                            problemDetails.Title = "Internal Server Error";
                            break;
                    }
                }
                var problemDetailsJson = JsonConvert.SerializeObject(problemDetails, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
                await context.Response.WriteAsync(problemDetailsJson, Encoding.UTF8);
            });
        });
    }
}