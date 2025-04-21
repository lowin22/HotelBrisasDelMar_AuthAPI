using Core.Entities;


namespace Core.Interfaces
{
    public interface IAdminRepository
    {
        Task<string> AuthWithCredentials(Admin admin, string password);
        Task<bool> VerifyToken(Token token);
        Task<bool> RegisterNewAdmin(Admin admin, string password);
    }
}
