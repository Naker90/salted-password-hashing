using SaltedPasswordHashing.Src.Domain.User.SignUp;
using SaltedPasswordHashing.Src.Domain.User.Login;
using SaltedPasswordHashing.Src.Repositories;
using SaltedPasswordHashing.Src.Security;
using ConsoleApplication.Services;

namespace ConsoleApplication.Controllers
{
    public static class Factory
    {
        public static SignUpController CreateSignUpController()
        {
            return new SignUpController(
                consoleLogger: new ConsoleLoggerService(),
                command: CreateUserSignUpCommand());

            UserSignUpCommand CreateUserSignUpCommand()
            {
                return new UserSignUpCommand(
                    userRepository: new CsvUserRepository(),
                    passwordEncryptionService: new BCryptPasswordEncryptionService(),
                    securePseudoRandomGenerator: new RNGSecurePseudoRandomGenerator());
            }
        }
    }
}