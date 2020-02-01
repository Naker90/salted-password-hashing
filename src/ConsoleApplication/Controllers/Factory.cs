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
                    userRepository: CsvUserRepository(),
                    hashingService: new Sha512HasingService(),
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
                    userRepository: CsvUserRepository(),
                    hashingService: new Sha512HasingService());
            }
        }

        private static CsvUserRepository CsvUserRepository()
        {
            return new CsvUserRepository(absoluteFilePath: "/home/naker90/Desktop/Projects/salted-password-hashing/users.csv");
        }
    }
}