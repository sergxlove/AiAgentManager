namespace AiAgentManager.Requests
{
    public class GetHistoryRequest
    {
        public Guid ChatId { get; set; }
        public int Offset { get; set; }
    }
}
