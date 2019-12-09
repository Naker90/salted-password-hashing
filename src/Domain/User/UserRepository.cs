using SaltedPasswordHashing.Src.Domain.Types;

namespace SaltedPasswordHashing.Src.Domain.User
{
    public interface UserRepository
    {
        void Create(User user);
        bool Exist(Email email);
        User FindBy(Email email);
    }
}