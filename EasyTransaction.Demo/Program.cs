using EasyTransaction.Demo.Service;
using EsayTransaction.Aspnet;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using System.Data;

namespace EasyTransaction.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseEsayTransactionDependencyInjection();

            // Add services to the container.

            builder.Services.AddScoped<IUserService, UserService>();

            var db = new MySqlConnection("Server=192.168.31.122;Database=test;User Id=xxxx;Password=xxxx;Port=3306;");
            builder.Services.AddSingleton<IDbConnection>(db);

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
