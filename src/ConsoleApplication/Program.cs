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
            var email = Console.ReadLine();
            Console.WriteLine("Password: ");
            var password = Console.ReadLine();

            var userSignUpRequestCreationResult = UserSignUpRequest.Create(
                email: email,
                password: password
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
                    Console.WriteLine(error.FieldId);
                    Console.WriteLine(error.Error.ToString());   
                }
            }

            void ExecuteCommand()
            {
                var request = userSignUpRequestCreationResult.Result;
                var command = new UserSignUpCommand(
                    userRepository: new CsvUserRepository(),
                    passwordEncryptionService: new BCryptPasswordEncryptionService(),
                    securePseudoRandomGenerator: new RNGSecurePseudoRandomGenerator()
                );
                var commandResult = command.Execute(request);
                if(!commandResult.IsValid)
                {
                    Console.WriteLine(commandResult.Error.ToString());
                }else{
                    Console.WriteLine("User registered successfuly!");
                }
            }
        }
    }
}
