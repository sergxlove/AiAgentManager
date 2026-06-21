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

            app.MapGet("/api/agents/{name}", async (string name,
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

            app.MapDelete("/api/agents/{name}", async (string name,
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

            app.MapPost("/api/agents/{name}/start", async (string name,
                [FromServices] ISavedAgentsRepository repo,
                [FromServices] IChatHistoryRepository chatRepo,
                CancellationToken token) =>
            {
                try
                {
                    var result = await repo.GetByNameAsync(name, token);
                    if (result is null)
                        return Results.BadRequest("no found object");
                    string response = $"Привет, {result.Name} готов к работе. Напишите ваш запрос :)";
                    var historyAgent = ChatHistory.Create(Guid.NewGuid(), result.Id, "agent", response,
                        DateTime.UtcNow);
                    return Results.Ok(response);
                }
                catch
                {
                    return Results.InternalServerError();
                }
            });

            app.MapPost("/api/agents/{name}/stop", async (string name,
                [FromServices] ISavedAgentsRepository repo,
                CancellationToken token) =>
            {
                try
                {
                    var result = await repo.GetByNameAsync(name, token);
                    if (result is null)
                        return Results.BadRequest("no found object");
                    return Results.Ok(new { message = $"Агент '{result.Name}' остановлен" });
                }
                catch
                {
                    return Results.InternalServerError();
                }
            });

            app.MapPost("/api/agents/{name}/command", async (string name,
                [FromBody] CommandRequest request,
                [FromServices] ISavedAgentsRepository repo,
                [FromServices] IChatHistoryRepository chatRepo,
                CancellationToken token) =>
            {
                try
                {
                    var result = await repo.GetByNameAsync(name, token);
                    if (result is null)
                        return Results.BadRequest("no found object");
                    var historyUser = ChatHistory.Create(Guid.NewGuid(), result.Id, "user", request.Command,
                        DateTime.UtcNow);
                    if (!string.IsNullOrEmpty(historyUser.Error))
                        return Results.BadRequest(historyUser.Error);
                    await chatRepo.AddAsync(historyUser.Value, token);
                    string response = $"Агент '{result.Name}' принял команду";
                    var historyAgent = ChatHistory.Create(Guid.NewGuid(), result.Id, "agent", response,
                        DateTime.UtcNow);
                    if (!string.IsNullOrEmpty(historyAgent.Error))
                        return Results.BadRequest(historyAgent.Error);
                    await chatRepo.AddAsync(historyAgent.Value, token);
                    return Results.Ok(response);
                }
                catch
                {
                    return Results.InternalServerError();
                }
            });

            app.MapPost("/api/agents/history", async ([FromBody] GetHistoryRequest request,
                [FromServices] IChatHistoryRepository repo,
                [FromServices] ISavedAgentsRepository agentsRepo,
                CancellationToken token) =>
            {
                try
                {
                    var agents = await agentsRepo.GetByNameAsync(request.ChatName, token);
                    if (agents is null)
                        return Results.BadRequest("no found object");
                    var result = await repo.GetByPaginationAsync(agents.Id, 50, request.Offset, token);
                    if (result is null)
                        return Results.BadRequest("no found object");
                    return Results.Ok(result);
                }
                catch
                {
                    return Results.InternalServerError();
                }
            });

            app.MapDelete("/api/agents/history/{chatId:guid}", async (Guid chatId,
                [FromServices] IChatHistoryRepository repo,
                CancellationToken token) =>
            {
                try
                {
                    int resultDelete = await repo.DeleteAsync(chatId, token);
                    if (resultDelete == 0)
                        return Results.BadRequest("object no has been deleted");
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
