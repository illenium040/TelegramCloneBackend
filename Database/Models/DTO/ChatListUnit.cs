namespace Database.Models.DTO
{
    public class ChatListUnit
    {
        public UserDTO User { get; set; }
        public string ChatId { get; set; }
        public int? UnreadMessagesCount { get; set; }
        public MessageDTO? LastMessage { get; set; }
    }
}
