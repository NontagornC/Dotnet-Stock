using dotnet_stock.Entities;

namespace dotnet_stock.Interfaces
{
    public interface IAccountService
    {
        Task Register(Account account);
        Task<Account> Login(string username, string password);
        string GenerateToken(Account account);
        Account GetInfo(string accessToken);

    }
}