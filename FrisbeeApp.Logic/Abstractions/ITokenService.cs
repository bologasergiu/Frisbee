using FrisbeeApp.Logic.DtoModels;

namespace FrisbeeApp.Logic.Abstractions
{
    public interface ITokenService
    {
        string BuildToken(string key, string issuer, LoginDTO user, string role);
        bool ValidateToken(string key, string issuer, string token);
    }
}
