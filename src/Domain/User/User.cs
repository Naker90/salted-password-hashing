using SaltedPasswordHashing.Src.Domain.Types;
using System;

namespace SaltedPasswordHashing.Src.Domain.User
{
    public sealed class User 
    {
        public Id IdProp { get; set; }
        public Email Email { get; }
        public Password Password { get; }

        private User(Id id, Email email, Password password)
        {
            this.IdProp = id;
            this.Email = email;
            this.Password = password;
        }

        public static User Create(Email email, Password password)
        {
            return new User(
                id: Id.Create(),
                email: email,
                password: password
            );
        }

        public sealed class Id {
            public Guid Value { get; }

            private Id(Guid value)
            {
                this.Value = value;
            }

            public static Id Create()
            {
                return new Id(value: Guid.NewGuid());
            }
        }
    }
}