using AiAgentManager.Core.Infrastructure;

namespace AiAgentManager.Core.Models
{
    public class SavedAgents
    {
        public Guid Id { get; }
        public string Name { get; } = string.Empty;
        public string PathExe { get; } = string.Empty;

        private SavedAgents(Guid id, string name, string pathExe)
        {
            Id = id;
            Name = name;
            PathExe = pathExe;
        }

        public static ResultModel<SavedAgents> Create(Guid id, string name, string pathExe)
        {
            if (id == Guid.Empty)
            {
                return ResultModel<SavedAgents>.Failure("id is null");
            }
            if (string.IsNullOrEmpty(name))
            {
                return ResultModel<SavedAgents>.Failure("name is null");
            }
            if (string.IsNullOrEmpty(pathExe))
            {
                return ResultModel<SavedAgents>.Failure("path exe is null");
            }
            return ResultModel<SavedAgents>.Success(new SavedAgents(id, name, pathExe));
        }
    }
}
