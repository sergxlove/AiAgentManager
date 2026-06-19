namespace AiAgentManager.DataBase.Sqlite.Models
{
    public class ChatHistoryEntity
    {
        public Guid Id { get; set; }
        public Guid ChatId { get; set; }
        public string SenderMessage { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime DateSend { get; set; }

        public virtual SavedAgentsEntity? SavedAgents { get; set; } 
    }
}
