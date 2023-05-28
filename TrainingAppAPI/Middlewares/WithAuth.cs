using TrainingAppAPI.Models;
using TrainingAppAPI.Services;

namespace TrainingAppAPI.Middlewares;

public class WithAuth {
    private readonly RequestDelegate _next;
    private readonly SessionService _sessionService;
 
    public WithAuth(RequestDelegate next, SessionService sessionService)
    {
        _next = next;
        _sessionService = sessionService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Check if the request contains an authorization header
        if (!context.Request.Headers.ContainsKey("Authorization"))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        // Extract the token from the authorization header
        var authHeader = context.Request.Headers["Authorization"].ToString();
        if (!authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        var token = authHeader.Substring("Bearer ".Length).Trim();

        if (token == null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        // TO-DO: Actually authenticate. 
        var session = await _sessionService.GetSession(token);

        if (session == null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        context.Items["UserId"] = session.UserId;


        // User is authenticated, proceed to the next middleware
        await _next.Invoke(context);
    }
}