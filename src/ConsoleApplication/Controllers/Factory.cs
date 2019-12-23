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
                consoleLogger: new ConsoleLogger(),
                command: CreateUserSignUpCommand());

            UserSignUpCommand CreateUserSignUpCommand()
            {
                return new UserSignUpCommand(
                    userRepository: new CsvUserRepository(),
                    encryptionService: new BCryptEncryptionService(),
                    securePseudoRandomGenerator: new RNGSecurePseudoRandomGenerator());
            }
        }

        public static LoginController CreateLoginController()
        {
            return new LoginController(
                consoleLogger: new ConsoleLogger(),
                command: CreateUserLoginCommand());

            UserLoginCommand CreateUserLoginCommand()
            {
                return new UserLoginCommand(
                    userRepository: new CsvUserRepository(),
                    encryptionService: new BCryptEncryptionService());
            }
        }
    }
}