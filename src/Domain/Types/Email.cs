namespace SaltedPasswordHashing.Src.Domain.Types
{
    public sealed class Email
    {
        public string Value { get; }
        public PersistanceState State => new PersistanceState(value: Value);

        private Email(string value)
        {
            this.Value = value;
        }

        public Email(PersistanceState state)
        {
            this.Value = state.Value;
        }
        
        public static Email CreateWithoutValidate(string value)
        {
            return new Email(value: value);
        }

        public static CreationResult<Email, Error> CreateAndValidate(string value)
        {
            if(string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            {
                return CreationResult<Email, Error>.CreateInvalidResult(error: Error.Required);
            }
            if(!IsValidEmail(email: value))
            {
                return CreationResult<Email, Error>.CreateInvalidResult(error: Error.InvalidFormat);
            }
            Email email = new Email(value: value);
            return CreationResult<Email, Error>.CreateValidResult(result: email);
        }

        private static bool IsValidEmail(string email)
        {
            try {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch {
                return false;
            }
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