using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace ToDo.WebApi.Middlewares
{
    public class ErrorMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var messageError = string.Empty;
            if (exception is ValidationException){
                context.Response.StatusCode = HttpStatusCode.BadRequest.GetHashCode();
                messageError = exception.Message;
            }
            else
            {
                context.Response.StatusCode = HttpStatusCode.InternalServerError.GetHashCode();
                //messageError = "Ocorreu um erro interno no servidor.";
                messageError = exception.Message;
            }

            var response = new
            {
                message = messageError,
                statusCode = context.Response.StatusCode
            };

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}