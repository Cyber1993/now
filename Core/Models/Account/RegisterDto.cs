﻿using Microsoft.AspNetCore.Http;

namespace Core.Models.Account
{
    public class RegisterDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
        public IFormFile? Image { get; set; }
    }
}