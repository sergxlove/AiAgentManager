using AiAgentManager.Core.Models;
using AiAgentManager.DataBase.Sqlite.Interfaces;
using AiAgentManager.DataBase.Sqlite.Models;
using Microsoft.EntityFrameworkCore;

namespace AiAgentManager.DataBase.Sqlite.Repositories
{
    public class SavedAgentsRepository : ISavedAgentsRepository
    {
        private readonly AiAgentManagerDbContext _context;
        public SavedAgentsRepository(AiAgentManagerDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> AddAsync(SavedAgents agent, CancellationToken token)
        {
            try
            {
                SavedAgentsEntity agentEntity = new()
                {
                    Id = agent.Id,
                    Name = agent.Name,
                    PathExe = agent.PathExe,
                };
                await _context.SavedAgents.AddAsync(agentEntity, token);
                await _context.SaveChangesAsync();
                return agentEntity.Id;
            }
            catch
            {
                return Guid.Empty;
            }
        }

        public async Task<List<SavedAgents>> GetAllAsync(CancellationToken token)
        {
            try
            {
                List<SavedAgentsEntity> agentsEntity = await _context.SavedAgents
                    .AsNoTracking()
                    .ToListAsync(token);
                List<SavedAgents> result = new List<SavedAgents>();
                foreach (SavedAgentsEntity a in agentsEntity)
                {
                    result.Add(SavedAgents.Create(a.Id, a.Name, a.PathExe).Value);
                }
                return result;
            }
            catch
            {
                return [];
            }
        }

        public async Task<bool> CheckAsync(string name, CancellationToken token)
        {
            try
            {
                SavedAgentsEntity? result = await _context.SavedAgents
                    .AsNoTracking()
                    .FirstOrDefaultAsync(a => a.Name == name, token);
                if (result is null) return false;
                return true;
            }
            catch
            {
                return true;
            }
        }

        public async Task<SavedAgents?> GetByNameAsync(string name, CancellationToken token)
        {
            try
            {
                SavedAgentsEntity? resultEntity = await _context.SavedAgents
                    .AsNoTracking()
                    .FirstOrDefaultAsync(a => a.Name == name, token);
                if (resultEntity is null) return null;
                return SavedAgents.Create(resultEntity.Id, resultEntity.Name, resultEntity.PathExe).Value;
            }
            catch
            {
                return null;
            }
        }

        public async Task<int> RenameUpdateAsync(string oldName, string newName, CancellationToken token)
        {
            try
            {
                return await _context.SavedAgents
                    .Where(a => a.Name == oldName)
                    .ExecuteUpdateAsync(a => a
                    .SetProperty(a => a.Name, newName), token);
            }
            catch
            {
                return 0;
            }
        }

        public async Task<int> DeleteAsync(string name, CancellationToken token)
        {
            try
            {
                return await _context.SavedAgents
                    .Where(a => a.Name == name)
                    .ExecuteDeleteAsync(token);
            }
            catch
            {
                return 0;
            }
        }
    }
}
