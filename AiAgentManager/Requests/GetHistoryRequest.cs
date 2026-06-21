namespace AiAgentManager.Requests
{
    public class GetHistoryRequest
    {
        public string ChatName { get; set; } = string.Empty;
        public int Offset { get; set; }
    }
}
