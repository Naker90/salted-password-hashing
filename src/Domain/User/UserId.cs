using System;

namespace SaltedPasswordHashing.Src.Domain.User
{
    public sealed class UserId 
    {
        public Guid Value { get; }

        public UserId(Guid value)
        {
            this.Value = value;
        }
    }
}