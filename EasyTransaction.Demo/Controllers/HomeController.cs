using Dapper;
using EasyTransaction.Core;
using EasyTransaction.Demo.Service;
using EsayTransaction.Aspnet;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.Common;
using System.Transactions;

namespace EasyTransaction.Demo.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class HomeController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly IDbConnection dbConnection;
        public HomeController(IUserService userService, IDbConnection dbConnection)
        {
            _userService = userService;
            this.dbConnection = dbConnection;
        }

        [HttpGet]
        //[EasyTransaction]
        public async Task<IActionResult> Index()
        {
            string name = await _userService.GetUserName();
            Console.WriteLine(name);
            return Ok("Hello World");
        }

        [HttpGet]
        [EasyTransaction]
        public async Task<IActionResult> Index1()
        {
            //using (TransactionScope scope = new())
            //{
            var result = await dbConnection.ExecuteAsync("INSERT INTO `student` (`name`, `age`, `describe`) VALUES ('测试事务Index1', 222, '3312321');");
            //scope.Complete();
            //}

            //string name = await _userService.GetUserName();
            //Console.WriteLine(name);

            throw new Exception("测试事务回滚");
            return Ok("Hello World");
        }

        [HttpGet]
        [EasyTransaction]
        public async Task<IActionResult> Index2()
        {
            var result = await dbConnection.ExecuteAsync("INSERT INTO `student` (`name`, `age`, `describe`) VALUES ('测试事务Index2', 222, '3312321');");

            string name = await _userService.GetUserName();
            Console.WriteLine(name);
            return Ok("Hello World");
        }

        [HttpGet]
        public async Task<IActionResult> Index3()
        {
            await EasyTransactionScopeHelper.ExecuteAsync(async () =>
            {
                var result = await dbConnection.ExecuteAsync("INSERT INTO `student` (`name`, `age`, `describe`) VALUES ('测试事务Index3', 222, '3312321');");
                throw new Exception("测试事务回滚");
            });
           
            return Ok("Hello World");
        }
    }
}
