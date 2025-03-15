namespace Docosoft.API.Core.Models
{    public class DocosoftUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;
        public string Title { get; set; } = String.Empty;
        public required string Email { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
