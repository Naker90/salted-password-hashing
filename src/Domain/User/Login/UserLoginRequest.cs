using SaltedPasswordHashing.Src.Domain.Types;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SaltedPasswordHashing.Src.Domain.User.Login
{
    public sealed class UserLoginRequest
    {
        public Email Email { get; }
        public Password Password { get; }

        private UserLoginRequest(Email email, Password password)
        {
            this.Email = email;
            this.Password = password;
        }

        public static UserLoginRequest Create(string email, string password)
        {
            return new UserLoginRequest(
                email: Email.CreateWithoutValidate(value: email),
                password: Password.CreateWithoutValidate(value: password));
        }
    }
}