using Dapper;
using System.Data;
using System.Runtime.CompilerServices;

namespace EasyTransaction.Demo.Service
{
    public class UserService : IUserService
    {

        private readonly IDbConnection dbConnection;

        public UserService(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }
        public async Task<string> GetUserName()
        {

            var result = await dbConnection.ExecuteAsync("INSERT INTO `student` (`name`, `age`, `describe`) VALUES ('测试事务GetUserName', 222, '3312321');");

            //测试事务回滚
            throw new Exception("测试事务回滚");
            return "OK";
        }
    }
}
