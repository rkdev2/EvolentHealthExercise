using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EvolentHealthExercise.WebApi.Middlewares
{
    public sealed class RequestHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestHandlingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<RequestHandlingMiddleware>();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);

                var descriptor = GetDescriptor(context);

                var logEntry = new
                {
                    HttpMethod = context.Request.Method,
                    Uri = context.Request.GetDisplayUrl(),
                    Action = descriptor?.ActionName,
                    Service = descriptor?.ControllerName,
                    StatusCodes = context.Response.StatusCode.ToString()
                };

                _logger.LogInformation(JsonConvert.SerializeObject(logEntry));
            }
            catch (Exception exception)
            {
                _logger.LogInformation(exception.Message);
                throw;
            }
        }

        ControllerActionDescriptor GetDescriptor(HttpContext c) =>
            c.GetEndpoint()?.Metadata.GetMetadata<ControllerActionDescriptor>();
    }
}
