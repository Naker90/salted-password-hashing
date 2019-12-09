using SaltedPasswordHashing.Src.Domain.User.SignUp;
using SaltedPasswordHashing.Src.Repositories;
using SaltedPasswordHashing.Src.Security;
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
                    "- press 3 to exit");
                string line = Console.ReadLine();
                if(line == "1")
                {
                    SignUp();
                }
                else if(line == "2")
                {
                    Console.Write("Login");
                }
                else
                {
                    break;
                }
            }
        }

        static void SignUp()
        {
            Console.WriteLine("Email: ");
            //var email = Console.ReadLine();
            var email = "antonio@gmail.com";
            Console.WriteLine("Password: ");
            //var password = Console.ReadLine();
            var password = "Passw0rd$";
            var requestResult = UserSignUpRequest.Create(
                email: email,
                password: password
            );
            if(!requestResult.IsValid)
            {
                Console.WriteLine(requestResult.Errors);
            }else{
                var request = requestResult.Result;
                var command = new UserSignUpCommand(
                    userRepository: new CsvUserRepository(),
                    passwordEncryptionService: new BCryptPasswordEncryptionService(),
                    securePseudoRandomGenerator: new RNGSecurePseudoRandomGenerator()
                );
                var commandResult = command.Execute(request);
                if(!commandResult.IsValid)
                {
                    Console.WriteLine(requestResult.Errors);
                }else{
                    Console.WriteLine("User registered successfuly!");
                }
            }
        }
    }
}
