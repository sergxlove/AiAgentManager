namespace AiAgentManager.Requests
{
    public class CreateHistoryRequest
    {
        public Guid ChatId { get; set; }
        public string SenderMessage { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
