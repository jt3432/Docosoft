namespace Docosoft.API.Models.Request
{
    public class NewDocosoftUserRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public required string Email { get; set; }
    }
}
