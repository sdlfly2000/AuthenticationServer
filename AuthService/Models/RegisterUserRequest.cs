﻿namespace AuthService.Models
{
    public class RegisterUserRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DisplayName {  get; set; }
    }
}
