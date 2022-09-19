namespace MediatR.Handlers.Models
{
    public class UserModel
    {
        public string? Token { get; set; }
        public string DisplayName { get; set; }
        public string Login { get; set; }
        public string Id { get; set; }
        public string? Avatar { get; internal set; }
    }
}
