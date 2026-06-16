using AiAgentManager.Core.Models;

namespace AiAgentManager.DataBase.Sqlite.Interfaces
{
    public interface ISavedAgentsRepository
    {
        Task<Guid> AddAsync(SavedAgents agent, CancellationToken token);
        Task<bool> CheckAsync(string name, CancellationToken token);
        Task<int> DeleteAsync(string name, CancellationToken token);
        Task<List<SavedAgents>> GetAllAsync(CancellationToken token);
        Task<int> RenameUpdateAsync(string oldName, string newName, CancellationToken token);
        Task<SavedAgents?> GetByNameAsync(string name, CancellationToken token);
    }
}