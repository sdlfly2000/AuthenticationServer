﻿using Domain.User.ValueObjects;
using Infra.Core.DomainBasics;

namespace Domain.User.Entities
{
    public class User : DomainEntity
    {
        public string UserName { get; set; }

        public string? PasswordHash { get; set; }

        public string? DisplayName { get;set;}

        public IList<Claim> Claims { get; private set; }

        public User() : base(new UserReference(Guid.NewGuid().ToString()))
        { 
            Claims = new List<Claim>();
        }

        public static User Create(string userName)
        {
            return new User { UserName = userName };
        }

    }
}