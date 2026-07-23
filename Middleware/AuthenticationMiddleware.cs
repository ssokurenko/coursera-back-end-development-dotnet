namespace UserManagementAPI.Middleware;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHostEnvironment _env;

    public AuthenticationMiddleware(RequestDelegate next, IHostEnvironment env)
    {
        _next = next;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Skip authentication for Swagger endpoints
        if (context.Request.Path.StartsWithSegments("/swagger"))
        {
            await _next(context);
            return;
        }

        var token = context.Request.Headers["Authorization"].FirstOrDefault();

        if (string.IsNullOrEmpty(token) || !IsValidToken(token))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("{\"error\": \"Unauthorized. Valid token required.\"}");
            return;
        }

        await _next(context);
    }

    private const string MockToken = "valid-token-123";

    private bool IsValidToken(string token)
    {
        // Only accept the mock token while running in Development, so local
        // testing (e.g. via Swagger) doesn't need a real identity provider.
        // In production, replace this with real token validation (e.g. JWT).
        if (!_env.IsDevelopment())
            return false;

        // Accept the token with or without a "Bearer " prefix, since not every
        // client (e.g. Swagger UI's http/bearer scheme) adds it automatically.
        const string prefix = "Bearer ";
        var value = token.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
            ? token[prefix.Length..]
            : token;

        return value == MockToken;
    }
}