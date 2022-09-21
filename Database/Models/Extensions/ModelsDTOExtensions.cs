using DatabaseLayer.Models.DTO;

namespace DatabaseLayer.Models.Extensions
{
    public static class ModelsDTOExtensions
    {
        public static UserDTO ToDTO(this User user) => new UserDTO
        {
            Id = user.Id,
            Name = user.DisplayName,
            LoginName = user.UserName,
            Avatar = user.Avatar,
            Email = user.Email,
        };

        public static MessageDTO ToDTO(this Message message) => new MessageDTO
        {
            ChatId = message.ChatId,
            Content = message.Content,
            ContentType = message.ContentType,
            Created = message.Created,
            Id = message.Id,
            State = message.MessageState,
            UserIdFrom = message.FromUserId,
            UserIdTo = message.ToUserId
        };
    }
}
