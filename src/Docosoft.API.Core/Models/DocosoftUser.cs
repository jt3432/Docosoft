﻿namespace Docosoft.API.Core.Models
{    public class DocosoftUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public required string Email { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
