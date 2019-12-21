using SaltedPasswordHashing.Src.Domain.Types;
using System;

namespace SaltedPasswordHashing.Src.Domain.User
{
    public sealed class User 
    {
        public Id IdProp { get; set; }
        public Email Email { get; }
        public Password Password { get; }
        public PersistanceState State => new PersistanceState(
            id: IdProp.State,
            email: Email.Value,
            password: Password.Value,
            salt: Password.SaltProp.Value);

        private User(Id id, Email email, Password password)
        {
            this.IdProp = id;
            this.Email = email;
            this.Password = password;
        }

        public User(PersistanceState state)
        {
            this.IdProp = new Id(state: state.Id);
            this.Email = new Email(value: state.Email);
            this.Password = new Password(value: state.Password, salt: state.Salt);
        }

        public static User Create(Email email, Password password)
        {
            return new User(
                id: Id.Create(),
                email: email,
                password: password
            );
        }

        public sealed class PersistanceState
        {
            public Id.PersistanceState Id { get; }
            public string Email { get; }
            public string Password { get; }
            public string Salt { get; }

            public PersistanceState(
                Id.PersistanceState id, 
                string email, 
                string password, 
                string salt)
            {
                this.Id = id;
                this.Email = email;
                this.Password = password;
                this.Salt = salt; 
            }
        }

        public sealed class Id 
        {
            public Guid Value { get; }
            public PersistanceState State => new PersistanceState(value: Value.ToString());

            private Id(Guid value)
            {
                this.Value = value;
            }

            public Id(PersistanceState state)
            {
                this.Value = new Guid(state.Value);
            }

            public static Id Create()
            {
                return new Id(value: Guid.NewGuid());
            }

            public sealed class PersistanceState
            {
                public string Value { get; }

                public PersistanceState(string value){
                    Value = value;
                }
            }
        }
    }
}