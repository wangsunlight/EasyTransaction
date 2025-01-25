using EsayTransaction.Aspnet;

namespace EasyTransaction.Demo.Service
{
    public interface IUserService
    {
        [EasyTransaction]
        public Task<string> GetUserName();
    }
}
