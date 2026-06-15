namespace AiAgentManager.Core.Infrastructure
{
    public class ResultModel<T>
    {
        public T Value { get; }
        public string Error { get; }
        public bool IsSuccess
        {
            get
            {
                if (Error == string.Empty) return true;
                return false;
            }
        }

        private ResultModel(T value, string error)
        {
            Value = value;
            Error = error;
        }

        public static ResultModel<T> Success(T value) => new ResultModel<T>(value, string.Empty);

        public static ResultModel<T> Failure(string error) => new ResultModel<T>(default!, error);
    }
}
