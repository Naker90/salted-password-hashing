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
            var stdin = AskForUserAndPassword();
            var request = new SignUpController.SignUpRequestDto(
                email: stdin.Email,
                password: stdin.Password);
            controller.Execute(request);
        }

        static void Login()
        {
            var controller = Factory.CreateLoginController();
            var stdin = AskForUserAndPassword();
            var request = new LoginController.LoginRequestDto(
                email: stdin.Email,
                password: stdin.Password
            );
            controller.Execute(request);
        }

        static StdInDto AskForUserAndPassword()
        {
            Console.WriteLine("Email: ");
            var email = Console.ReadLine();
            Console.WriteLine("Password: ");
            var password = Console.ReadLine();
            return new StdInDto(email: email, password: password);
        }

        private class StdInDto
        {
            public string Email { get; }
            public string Password { get; }

            public StdInDto(string email, string password) 
            {
                Email = email;
                Password = password;
            }
        }
    }
}
