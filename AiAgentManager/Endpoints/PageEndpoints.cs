namespace AiAgentManager.Endpoints
{
    public static class PageEndpoints
    {
        public static IEndpointRouteBuilder MapPageEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/", async (HttpContext context) =>
            {
                try
                {
                    context.Response.ContentType = "text/html; charset=utf-8";
                    await context.Response.SendFileAsync("wwwroot/index.html");
                }
                catch { }
            });
            return app;
        }
    }
}
