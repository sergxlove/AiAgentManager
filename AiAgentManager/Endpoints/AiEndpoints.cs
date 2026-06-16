using AiAgentManager.Core.Models;
using AiAgentManager.DataBase.Sqlite.Interfaces;
using AiAgentManager.Requests;
using Microsoft.AspNetCore.Mvc;

namespace AiAgentManager.Endpoints
{
    public static class AiEndpoints
    {
        public static IEndpointRouteBuilder MapAiEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/agents", async ([FromServices] ISavedAgentsRepository repo,
                CancellationToken token) =>
            {
                try
                {
                    var agents = await repo.GetAllAsync(token);
                    return Results.Ok(agents);
                }
                catch
                {
                    return Results.InternalServerError();
                }
            });

            app.MapGet("/api/agents/{name}", async ([FromBody]string name, 
                [FromServices] ISavedAgentsRepository repo, 
                CancellationToken token) =>
            {
                try
                {
                    var agents = await repo.GetByNameAsync(name, token);
                    if (agents == null)
                        return Results.NotFound($"Агент с именем {name} не найден");
                    return Results.Ok(agents);
                }
                catch
                {
                    return Results.InternalServerError();
                }
            });

            app.MapPost("/api/agents", async ([FromBody] CreateAgentRequest request,
                [FromServices] ISavedAgentsRepository repo, 
                CancellationToken token) =>
            {
                try
                {
                    var exists = await repo.CheckAsync(request.Name, token);
                    if (exists)
                        return Results.BadRequest($"Агент с именем '{request.Name}' уже существует");
                    var agent = SavedAgents.Create(Guid.NewGuid(), request.Name, request.PathExe);
                    if (!string.IsNullOrEmpty(agent.Error))
                        return Results.BadRequest(agent.Error);
                    var id = await repo.AddAsync(agent.Value, token);
                    if (id == Guid.Empty)
                        return Results.InternalServerError();
                    return Results.Created($"/api/agents/{id}", agent.Value);
                }
                catch
                {
                    return Results.InternalServerError();
                }
            });

            app.MapDelete("/api/agents/{name}", async ([FromBody] string name,
                [FromServices] ISavedAgentsRepository repo, 
                CancellationToken token) =>
            {
                try
                {
                    var result = await repo.DeleteAsync(name, token);
                    if (result == 0)
                        return Results.InternalServerError();
                    return Results.Ok();
                }
                catch
                {
                    return Results.InternalServerError();
                }
            });

            return app;
        }
    }
}
