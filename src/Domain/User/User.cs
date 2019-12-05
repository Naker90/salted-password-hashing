using SaltedPasswordHashing.Src.Domain.Types;

namespace SaltedPasswordHashing.Src.Domain.User
{
    public sealed class User 
    {
        public UserId Id { get; set; }
        public Email Email { get; }
        public Password Password { get; }

        public User(UserId id, Email email, Password password)
        {
            this.Id = id;
            this.Email = email;
            this.Password = password;
        }
    }
}