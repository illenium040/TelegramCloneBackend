namespace DatabaseLayer.Models.DTO
{
    public class MessageDTO
    {
        public string? Id { get; set; }
        public string UserIdTo { get; set; }
        public string ChatId { get; set; }
        public string UserIdFrom { get; set; }
        public DateTime? Created { get; set; }
        public string Content { get; set; }
        public MessageContentType ContentType { get; set; }
        public MessageState? State { get; set; }
    }
}
