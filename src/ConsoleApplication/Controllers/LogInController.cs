using SaltedPasswordHashing.Src.Domain.User.Login;
using ConsoleApplication.Services;

namespace ConsoleApplication.Controllers
{
    public sealed class LoginController
    {
        private readonly Logger consoleLogger;
        private readonly UserLoginCommand command;

        public LoginController(
            Logger consoleLogger,
            UserLoginCommand command)
        {
            this.consoleLogger = consoleLogger;
            this.command = command;
        }

        public void Execute(LoginRequestDto request)
        {
            var userLoginRequest =  UserLoginRequest.Create(
                email: request.Email,
                password: request.Password
            );
            var commandResult = command.Execute(userLoginRequest);
            if(!commandResult.IsValid)
            {
                consoleLogger.LogInfo(commandResult.Error.ToString());
            }else{
                consoleLogger.LogInfo("User logged successfuly!");
            }
        }

        public sealed class LoginRequestDto
        {
            public string Email { get; }
            public string Password { get; }

            public LoginRequestDto(string email, string password)
            {
                Email = email;
                Password = password;
            }
        }
    }
}