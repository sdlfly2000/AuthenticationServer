﻿using Infra.Core.DomainBasics;
using System.Security.Claims;

namespace Domain.User.ValueObjects
{
    public class Claim : DomainValueObject
    {
        public Claim(string name, string value, string valueType = ClaimValueTypes.String)
        {
            Name = name;
            Value = value;
            ValueType = valueType;

            _id = Guid.NewGuid().ToString();
            _userId = string.Empty;
        }

        public string Name { get; private set; }
        public string Value { get; private set; }
        public string ValueType { get; private set; }

        #region Database Usage

        private string _id { get; set; }
        private string _userId { get; set; }

        #endregion

        public void AssignUser(string userId)
        {
            _userId = userId;
        }

        public void SetValue(string value)
        {
            Value = value;
        }
    }
}
