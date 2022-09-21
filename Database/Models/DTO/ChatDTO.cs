namespace DatabaseLayer.Models.DTO
{
    public class ChatDTO
    {
        public string Id { get; set; }
        public List<MessageDTO> Messages { get; set; }
    }
}
