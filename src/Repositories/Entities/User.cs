namespace SaltedPasswordHashing.Src.Repositories.Entities
{
    public sealed class User
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
    }
}