using SaltedPasswordHashing.Src.Domain.User.Login;
using SaltedPasswordHashing.Src.Repositories;
using SaltedPasswordHashing.Src.Security;
using ConsoleApplication.Controllers;
using System;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("[Salted password hashing console app]");
            while (true)
            {
                Console.WriteLine($"[Menu] {System.Environment.NewLine}" +  
                    $"- press 1 to signup {System.Environment.NewLine}" +
                    $"- press 2 to login {System.Environment.NewLine}" +
                    "- press any other key to exit");
                string line = Console.ReadLine();
                if(line == "1")
                {
                    SignUp();
                }
                else if(line == "2")
                {
                    Login();
                }
                else
                {
                    break;
                }
            }
        }

        static void SignUp()
        {
            var controller = Factory.CreateSignUpController();

            Console.WriteLine("Email: ");
            var email = Console.ReadLine();
            Console.WriteLine("Password: ");
            var password = Console.ReadLine();

            var request = new SignUpController.SignUpRequestDto(
                email: email,
                password: password);
            controller.Execute(request);
        }

        static void Login()
        {
            Console.WriteLine("Email: ");
            var email = Console.ReadLine();
            Console.WriteLine("Password: ");
            var password = Console.ReadLine();

            ExecuteCommand();

            void ExecuteCommand()
            {
                var request =  UserLoginRequest.Create(
                    email: email,
                    password: password
                );
                var command = new UserLoginCommand(
                    userRepository: new CsvUserRepository(),
                    passwordEncryptionService: new BCryptPasswordEncryptionService());
                var commandResult = command.Execute(request);
                if(!commandResult.IsValid)
                {
                    Console.WriteLine(commandResult.Error.ToString());
                }else{
                    Console.WriteLine("User logged successfuly!");
                }
            }
        }
    }
}
