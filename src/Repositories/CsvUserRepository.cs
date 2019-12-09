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
        private const string ABSOLUTE_FILE_PATH = "";

        public User Create(User user)
        {
            using (TextWriter writer = File.AppendText(ABSOLUTE_FILE_PATH))
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

        public bool Exist(string email) {
            List<string> result = new List<string>();
            string value;
            using (TextReader fileReader = File.OpenText(ABSOLUTE_FILE_PATH)) {
                var csv = new CsvReader(fileReader);
                csv.Configuration.HasHeaderRecord = false;
                while (csv.Read()) {
                    for(int i=0; csv.TryGetField<string>(i, out value); i++) {
                            result.Add(value);
                        }
                    }
                }
            return result.Exist(x => x.Contains(email));
        }
    }
}