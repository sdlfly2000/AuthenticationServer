﻿namespace AuthService
{
    public class JWTOptions
    {
        public string Issuer { get; set; }
        public string SigningKey { get; set; }
        public int ExpireSeconds { get; set; }
    }
}
