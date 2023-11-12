﻿namespace AuthService.Models
{
    public class AuthenticateResponse
    {
        public string JwtToken { get; set; }
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
