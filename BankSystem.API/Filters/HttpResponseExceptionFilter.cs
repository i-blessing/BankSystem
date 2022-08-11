using BankSystem.Business.Exceptions;
using BankSystem.Business.Extensions;
using BankSystem.Business.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace BankSystem.API.Filters
{
    public class HttpResponseExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<HttpResponseExceptionFilter> _logger;

        public HttpResponseExceptionFilter(ILogger<HttpResponseExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (context?.Exception == null) return;

            if(context?.Exception is BankSystemException gEx)
            {
                var errMsg = new ErrorMessage
                {
                    ErrorCode = (int)gEx.StatusCode,
                    Message = gEx.Message,
                    TraceId = Activity.Current?.Id,
                    RequestId = context?.HttpContext.TraceIdentifier
                };

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                context.Result = new ObjectResult(errMsg) { StatusCode = gEx.StatusCode.GetHttpStatusCode() };
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }            
            else
            {
                var errMsg = new ErrorMessage
                {
                    ErrorCode = (int) BankSystemExceptionStatusCodes.GenericFailure,
                    Message = $"Unhandled exception: {context?.Exception.Message}",
                    TraceId = Activity.Current?.Id,
                    RequestId = context?.HttpContext.TraceIdentifier
                };

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                context.Result = new ObjectResult(errMsg) { StatusCode = (int)StatusCodes.Status500InternalServerError };
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }

            context.ExceptionHandled = true;
        }

   
    }
}
