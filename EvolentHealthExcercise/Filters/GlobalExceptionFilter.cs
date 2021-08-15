using EvolentHealthExercise.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace EvolentHealthExercise.WebApi.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger _logger;

        public GlobalExceptionFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GlobalExceptionFilter>();
        }

        public void OnException(ExceptionContext context)
        {
            try
            {
                if (context.HttpContext.Response?.StatusCode == StatusCodes.Status200OK)
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                }

                context.Result = new BadRequestObjectResult(
                    new[] { new ValidationError(context.Exception.Message) }
                );

                context.ExceptionHandled = true;

                _logger.LogError(context.Exception.Message);
            }
            catch (Exception exception)
            {
                context.Result = new BadRequestObjectResult(new[] { new ValidationError(exception.Message) });
                _logger.LogError(context.Exception.Message);
            }
        }
    }
}