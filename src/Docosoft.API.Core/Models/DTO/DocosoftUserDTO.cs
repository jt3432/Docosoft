namespace Docosoft.API.Core.Models.DTO
{
    public class DocosoftUserDTO
    {
        public DocosoftUser? User { get; set; }

        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = String.Empty;  
    }
}
