using SaltedPasswordHashing.Src.Domain.User.SignUp;
using SaltedPasswordHashing.Src.Repositories;
using SaltedPasswordHashing.Src.Security;
using ConsoleApplication.Services;

namespace ConsoleApplication.Controllers
{
    public sealed class SignUpController
    {
        private readonly Logger consoleLogger;
        private readonly UserSignUpCommand command;

        public SignUpController(
            Logger consoleLogger,
            UserSignUpCommand command)
        {
            this.consoleLogger = consoleLogger;
            this.command = command;
        }

        public void Execute(SignUpRequestDto request)
        {
            var userSignUpRequestCreationResult = UserSignUpRequest.Create(
                email: request.Email,
                password: request.Password
            );
            if(!userSignUpRequestCreationResult.IsValid)
            {
                PrintErrors();
            }
            else
            {
                ExecuteCommand();
            }

            void PrintErrors()
            {
                foreach (var error in userSignUpRequestCreationResult.Errors)
                {
                    consoleLogger.LogInfo(error.FieldId);
                    consoleLogger.LogInfo(error.Error.ToString());   
                }
            }

            void ExecuteCommand()
            {
                var request = userSignUpRequestCreationResult.Result;
                var commandResult = command.Execute(request);
                if(!commandResult.IsValid)
                {
                    consoleLogger.LogInfo(commandResult.Error.ToString());
                }else{
                    consoleLogger.LogInfo("User registered successfuly!");
                }
            }
        }

        public sealed class SignUpRequestDto
        {
            public string Email { get; }
            public string Password { get; }

            public SignUpRequestDto(string email, string password)
            {
                Email = email;
                Password = password;
            }
        }

    }
}