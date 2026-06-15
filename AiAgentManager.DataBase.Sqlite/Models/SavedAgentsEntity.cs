namespace AiAgentManager.DataBase.Sqlite.Models
{
    public class SavedAgentsEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PathExe { get; set; } = string.Empty;
    }
}
