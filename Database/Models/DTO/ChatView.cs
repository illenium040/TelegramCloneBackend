namespace DatabaseLayer.Models.DTO
{
    public class ChatView
    {
        public UserDTO User { get; set; }
        public string ChatId { get; set; }
        public int? UnreadMessagesCount { get; set; }
        public MessageDTO? LastMessage { get; set; }
        public ChatToUserDTO ChatToUser { get; set; }
    }
}
