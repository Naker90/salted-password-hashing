using SaltedPasswordHashing.Src.Domain.User;
using SaltedPasswordHashing.Src.Domain.Types;
using System;
using System.IO;
using System.Text;
using CsvHelper;
using System.Collections.Generic;
using System.Linq;

namespace SaltedPasswordHashing.Src.Repositories
{
    public class CsvUserRepository : UserRepository
    {
        private const string DELIMITER = ";";
        private const string ABSOLUTE_FILE_PATH = "/home/naker90/Desktop/Projects/salted-password-hashing/users.csv";

        public void Create(User user)
        {
            using (TextWriter writer = File.AppendText(ABSOLUTE_FILE_PATH))
            using (var csvWriter = new CsvWriter(writer))
            {
                csvWriter.Configuration.Delimiter = DELIMITER;
                csvWriter.Configuration.HasHeaderRecord = true;
                csvWriter.Configuration.AutoMap<SaltedPasswordHashing.Src.Repositories.Entities.User>();

                var userEntity = BuildUserEntityFrom(state: user.State);
                csvWriter.WriteRecords(new [] { userEntity });

                writer.Flush();
            }
        }

        public bool Exist(Email email) {
            if(!File.Exists(ABSOLUTE_FILE_PATH))
            {
                return false;
            }
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
            return result.Any(x => x.Contains(email.Value));
        }

        public User FindBy(Email email)
        {
            if(!File.Exists(ABSOLUTE_FILE_PATH))
            {
                return null;
            }
            using (TextReader fileReader = File.OpenText(ABSOLUTE_FILE_PATH)) {
                var csv = new CsvReader(fileReader);
                csv.Configuration.Delimiter = DELIMITER;
                csv.Configuration.HasHeaderRecord = false;
                csv.Configuration.AutoMap<SaltedPasswordHashing.Src.Repositories.Entities.User>();
                var records = csv.GetRecords<SaltedPasswordHashing.Src.Repositories.Entities.User>();
                var entity = records.FirstOrDefault(x => x.Email == email.Value);
                return BuildUserFrom(entity);
            }   
        }

        private SaltedPasswordHashing.Src.Repositories.Entities.User BuildUserEntityFrom(User.PersistanceState state)
        {
            return new SaltedPasswordHashing.Src.Repositories.Entities.User
            {
                Id = state.Id.Value,
                Email = state.Email,
                Password = state.Password,
                Salt = state.Salt
            };
        }

        private User BuildUserFrom(SaltedPasswordHashing.Src.Repositories.Entities.User entity)
        {
            var state = new User.PersistanceState(
                id: new User.Id.PersistanceState(value: entity.Id),
                email: entity.Email,
                password: entity.Password,
                salt: entity.Salt);
            return new User(state);
        }
    }
}