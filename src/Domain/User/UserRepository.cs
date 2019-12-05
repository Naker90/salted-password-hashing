namespace SaltedPasswordHashing.Src.Domain.User
{
    public interface UserRepository
    {
        User Create(User user);
    }
}