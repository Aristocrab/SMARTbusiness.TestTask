namespace SMARTbusiness.TestTask.WebApi.Middleware;

public class ApiKeySecurityMiddleware : IMiddleware
{
    private const string ApiKeyHeaderName = "X-Api-Key";
    private readonly string _apiKey;

    public ApiKeySecurityMiddleware(IConfiguration configuration)
    {
        _apiKey = configuration.GetValue<string>("ApiKey")!; 
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("API Key missing");
            return;
        }

        if (!string.Equals(extractedApiKey, _apiKey))
        {
            context.Response.StatusCode = 403;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("Unauthorized client");
            return;
        }

        await next(context);
    }
}