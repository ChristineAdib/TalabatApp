﻿using System.Net;
using System.Text.Json;
using TalabatAPIs.Errors;

namespace TalabatAPIs.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate Next , ILogger<ExceptionMiddleware> logger,IHostEnvironment env)
        {
            _next = Next;
            this._logger= logger;
            this._env= env;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;


                var Response = _env.IsDevelopment() ? new ApiExceptionResponse(500, ex.Message, ex.StackTrace.ToString()) :
                    new ApiExceptionResponse(500, ex.Message, ex.StackTrace.ToString());
                var Options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var JsonResponse=JsonSerializer.Serialize(Response,Options);
                context.Response.WriteAsync(JsonResponse);
            }
        }
    }
}
