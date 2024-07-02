using DB.Models;
using WebApi.App.DTOs;

namespace WebApi.App.Services
{
    public interface IAccessService
    {
        Task<User> Register(UserDTO model);
        Task<string> Login(UserDTO model);
    }
}
