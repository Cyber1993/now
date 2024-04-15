using Microsoft.AspNetCore.Http;

namespace Core.Models.User
{
    public class EditUserDto
    {
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Sex { get; set; }
        public string Position { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public IFormFile? Image { get; set; }
        public IEnumerable<string>? Roles { get; set; }
    }
}