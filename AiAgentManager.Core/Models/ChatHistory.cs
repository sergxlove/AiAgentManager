using AiAgentManager.Core.Infrastructure;

namespace AiAgentManager.Core.Models
{
    public class ChatHistory
    {
        public Guid Id { get; }
        public Guid ChatId { get; }
        public string SenderMessage { get; } = string.Empty;
        public string Message { get; } = string.Empty;
        public DateTime DateSend { get; }

        private ChatHistory(Guid id, Guid chatId, string senderMessage, string message, DateTime dateSend)
        {
            Id = id;
            ChatId = chatId;
            SenderMessage = senderMessage;
            Message = message;
            DateSend = dateSend;
        }

        public static ResultModel<ChatHistory> Create(Guid id, Guid chatId, string senderMessage, 
            string message, DateTime dateSend)
        {
            if (id == Guid.Empty)
            {
                return ResultModel<ChatHistory>.Failure("id is null");
            }
            if(chatId == Guid.Empty)
            {
                return ResultModel<ChatHistory>.Failure("id chat is null");
            }
            if (string.IsNullOrEmpty(senderMessage))
            {
                return ResultModel<ChatHistory>.Failure("fromMessage is null");
            }
            if (string.IsNullOrEmpty(message))
            {
                return ResultModel<ChatHistory>.Failure("message is null");
            }
            if(senderMessage != "user" && senderMessage != "agent")
            {
                return ResultModel<ChatHistory>.Failure("fromMessage is not equals user or agent");
            }
            if (dateSend > DateTime.UtcNow.AddMinutes(5))
            {
                return ResultModel<ChatHistory>.Failure("dateSend cannot be in the future");
            }
            return ResultModel<ChatHistory>.Success(new ChatHistory(id, chatId, senderMessage, message, dateSend));
        }
    }
}
