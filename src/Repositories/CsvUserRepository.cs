using SaltedPasswordHashing.Src.Domain.User;
using SaltedPasswordHashing.Src.Domain.Types;
using System;
using System.IO;
using System.Text;
using CsvHelper;

namespace SaltedPasswordHashing.Src.Repositories
{
    public class CsvUserRepository : UserRepository
    {

        private const string DELIMITER = ";";

        public User Create(User user)
        {
            using (var mem = new MemoryStream())
            using (var writer = new StreamWriter(mem))
            using (var csvWriter = new CsvWriter(writer))
            {
                csvWriter.Configuration.Delimiter = DELIMITER;
                csvWriter.Configuration.HasHeaderRecord = true;
                csvWriter.Configuration.AutoMap<User.PersistanceState>();

                csvWriter.WriteHeader<User.PersistanceState>();
                csvWriter.WriteRecords(new [] { user.State });

                writer.Flush();
                return user;
            }
        }    
    }
}