using System.Net;
using AvtMedia.CleanArchitecture.DomainLayer.Extensions.Shared;

namespace GrpcApp.Middlewares;

public class GlobalErrorHandlerMiddleware : IMiddleware
{
	private readonly ILogger<GlobalErrorHandlerMiddleware> _logger;
	public GlobalErrorHandlerMiddleware(ILogger<GlobalErrorHandlerMiddleware> logger) => _logger = logger;

	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		try
		{
			await next(context);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "GlobalErrorHandlerMiddleware failed {ex}", ex.Message);

			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
			await context.Response.WriteAsync(new Error(AvtMedia.Errors.Common.General.Unknown, ex.Message));
		}
	}
}

