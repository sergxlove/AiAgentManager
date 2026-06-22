using System.Diagnostics;

namespace AiAgentManager.Models
{
    public class AgentProcess
    {
        public Process Process { get; set; } = null!;
        public DateTime StartTime { get; set; }
    }
}
