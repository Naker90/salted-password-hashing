using SaltedPasswordHashing.Src.Domain.Types;
using SaltedPasswordHashing.Src.Domain.User;

namespace SaltedPasswordHashing.Test.Domain.Unit.Builders
{
    public static class UserBuilder
    {
        public static SaltedPasswordHashing.Src.Domain.User.User Build(
            string email = "user@email.com",
            string password = "Pass0word$")
        {
            return new SaltedPasswordHashing.Src.Domain.User.User(
                state: new SaltedPasswordHashing.Src.Domain.User.User.PersistanceState(
                    id: new SaltedPasswordHashing.Src.Domain.User.User.Id.PersistanceState(value: "ad0313cd-ab24-48d1-8405-5a9d12110cb6"),
                    email: new Email.PersistanceState(value: email),
                    password: new Password.PersistanceState(
                        value: password,
                        salt: new Password.Salt.PersistanceState(value: "salt")))
                );
        }
    }    
}