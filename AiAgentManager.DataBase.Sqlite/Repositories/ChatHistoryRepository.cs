using AiAgentManager.Core.Models;
using AiAgentManager.DataBase.Sqlite.Interfaces;
using AiAgentManager.DataBase.Sqlite.Models;
using Microsoft.EntityFrameworkCore;

namespace AiAgentManager.DataBase.Sqlite.Repositories
{
    public class ChatHistoryRepository : IChatHistoryRepository
    {
        private readonly AiAgentManagerDbContext _context;
        public ChatHistoryRepository(AiAgentManagerDbContext context)
        {
            _context = context;
        }

        public async Task<List<ChatHistory>> GetByPaginationAsync(Guid chatId, int limit, int offset,
            CancellationToken token)
        {
            var messages = await _context.ChatHistories
                .Where(m => m.ChatId == chatId)
                .OrderByDescending(m => m.DateSend)
                .Skip(offset)
                .Take(limit)
                .OrderBy(m => m.DateSend)
                .ToListAsync(token);
            List<ChatHistory> result = new List<ChatHistory>();
            foreach (var m in messages)
            {
                result.Add(ChatHistory.Create(m.Id, m.ChatId, m.SenderMessage, m.Message, m.DateSend).Value);
            }
            return result;
        }

        public async Task<Guid> AddAsync(ChatHistory chatHistory, CancellationToken token)
        {
            try
            {
                ChatHistoryEntity chatHistoryEntity = new()
                {
                    Id = chatHistory.Id,
                    ChatId = chatHistory.ChatId,
                    SenderMessage = chatHistory.SenderMessage,
                    Message = chatHistory.Message,
                    DateSend = chatHistory.DateSend
                };
                await _context.ChatHistories.AddAsync(chatHistoryEntity, token);
                await _context.SaveChangesAsync(token);
                return chatHistoryEntity.Id;
            }
            catch
            {
                return Guid.Empty;
            }
        }

        public async Task<int> DeleteAsync(Guid chatId, CancellationToken token)
        {
            return await _context.ChatHistories
                .Where(a => a.ChatId == chatId)
                .ExecuteDeleteAsync(token);
        }
    }
}
