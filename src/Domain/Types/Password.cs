using SaltedPasswordHashing.Src.Domain.Security;
using System.Linq;
using System;

namespace SaltedPasswordHashing.Src.Domain.Types
{
    public sealed class Password
    {
        public string Value { get; private set; }
        public Salt SaltProp { get; private set; }
        public PersistanceState State => new PersistanceState(value: Value, salt: SaltProp.State);

        private Password(string value)
        {
            this.Value = value;
            this.SaltProp = null;
        }

        public Password(PersistanceState state)
        {
            this.Value = state.Value;
            this.SaltProp = new Salt(state.Salt);
        }

        public static Password CreateWithoutValidate(string value)
        {
            return new Password(value: value);
        }

        public static CreationResult<Password, Error> CreateAndValidate(string value)
        {
            if(string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            {
                return CreationResult<Password, Error>.CreateInvalidResult(error: Error.Required);
            }
            if(!IsValidPassword(password: value)){
                return CreationResult<Password, Error>.CreateInvalidResult(error: Error.InvalidFormat);
            }
            Password password = new Password(value: value);
            return CreationResult<Password, Error>.CreateValidResult(result: password);
        }

        public void Encrypt(
            EncryptionService encryptionService,
            SecurePseudoRandomGenerator securePseudoRandomGenerator)
        {
            Salt salt = securePseudoRandomGenerator.Generate();
            var saltedPassword = this.Value + salt.Value;
            var encryptedPassword = encryptionService.Encrypt(saltedPassword);
            this.Value = encryptedPassword;
            this.SaltProp = salt;
        }

        private static bool IsValidPassword(string password)
        {
            const int MIN_ALLOWED_PASSWORD_LENGHT = 8;
            return password.Length >= MIN_ALLOWED_PASSWORD_LENGHT 
                    && IsAlphanumeric()
                    && ContainsAtLeastOfOneUpperCaseLetter()
                    && ContainsAtLeastOfOneSymbol();

            bool IsAlphanumeric()
            {
                return password.Any(char.IsNumber) && password.Any(char.IsLetter); 
            }

            bool ContainsAtLeastOfOneUpperCaseLetter(){
                return password.Any(char.IsUpper);
            }

            bool ContainsAtLeastOfOneSymbol(){
                return password.Any(char.IsSymbol);
            }
        }

        public sealed class PersistanceState
        {
            public string Value { get; }
            public Salt.PersistanceState Salt { get; }

            public PersistanceState(string value, Salt.PersistanceState salt)
            {
                this.Value = value;
                this.Salt = salt;
            }
        }

        public sealed class Salt
        {
            public string Value { get; }
            public PersistanceState State => new PersistanceState(value: Value);

            public Salt(string value)
            {
                this.Value = value;
            }

            public Salt(PersistanceState state)
            {
                this.Value = state.Value;
            }

            public sealed class PersistanceState
            {
                public string Value { get; }

                public PersistanceState(string value)
                {
                    this.Value = value;
                }
            }
        }
    }
}