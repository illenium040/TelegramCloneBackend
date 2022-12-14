using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseLayer.Models
{
    [Table("Messages")]
    public class Message
    {
        public string ChatId { get; set; }
        public Chat Chat { get; set; }
        public string Id { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Content { get; set; }
        public MessageContentType ContentType { get; set; }
        public MessageState MessageState { get; set; }
        public DateTime Created { get; set; }
    }

    public enum MessageState
    {
        LOADING,
        SENDED_TO_USER,
        READ,
        ERROR
    }


    public enum MessageContentType
    {
        Text = 1,
        Video = 2,
        Audio = 4,
        Image = 8,
        Link = 16,
    }
}
