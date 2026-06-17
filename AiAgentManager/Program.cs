using AiAgentManager.DataBase.Sqlite;
using AiAgentManager.DataBase.Sqlite.Interfaces;
using AiAgentManager.DataBase.Sqlite.Repositories;
using AiAgentManager.Endpoints;
using Microsoft.EntityFrameworkCore;

namespace AiAgentManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            IConfigurationSection? sqliteSetting = builder.Configuration.GetSection("SqliteSetting");
            IConfigurationSection? aspnetSetting = builder.Configuration.GetSection("AspnetSetting");
            builder.Services.AddDbContext<AiAgentManagerDbContext>(options =>
                options.UseSqlite(sqliteSetting["ConnectionString"]));
            builder.Services.AddScoped<ISavedAgentsRepository, SavedAgentsRepository>();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.SetIsOriginAllowed(origin => true)
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            });
            builder.WebHost.UseUrls($"http://localhost:{aspnetSetting["Port"]}");
            var app = builder.Build();
            app.UseStaticFiles();
            app.UseCors("AllowAll");
            app.MapAiEndpoints();
            app.Run();
        }
    }
}
