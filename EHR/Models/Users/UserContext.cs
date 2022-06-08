using DataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EHR.Models.Users
{
    internal class UserContext
    {
        private readonly SQLDataAccess _db;

        public UserContext(SQLDataAccess db)
        {
            _db = db;
        }

        public Task<List<User>> VerifyLogin(User user)
        {
            string sql = @"SELECT [Id]
                              ,[Username]
                              ,[Password]
                              ,[FirstName]
                              ,[LastName]
                          FROM[dbo].[Users]
                          WHERE[Username] = @Username
                          AND[Password] = @Password";

            return _db.LoadData<User, dynamic>(sql, user);
        }
    }
}
