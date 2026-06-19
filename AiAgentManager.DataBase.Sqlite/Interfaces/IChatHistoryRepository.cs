using AiAgentManager.Core.Models;

namespace AiAgentManager.DataBase.Sqlite.Interfaces
{
    public interface IChatHistoryRepository
    {
        Task<Guid> AddAsync(ChatHistory chatHistory, CancellationToken token);
        Task<int> DeleteAsync(Guid chatId, CancellationToken token);
        Task<List<ChatHistory>> GetByPaginationAsync(Guid chatId, int limit, int offset, CancellationToken token);
    }
}