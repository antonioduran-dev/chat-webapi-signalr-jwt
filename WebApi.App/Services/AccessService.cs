using DB.Data;
using DB.Models;
using Microsoft.EntityFrameworkCore;
using WebApi.App.DTOs;
using WebApi.App.Utils;

namespace WebApi.App.Services
{
    public class AccessService : IAccessService
    {
        private readonly ChatDbContext _chatDbContext;
        private readonly Utilities _utils;

        public AccessService(ChatDbContext chatDbContext, Utilities utils)
        {
            _chatDbContext = chatDbContext;
            _utils = utils;
        }

        public async Task<User> Register(UserDTO model)
        {
            try
            {
                var userExists = await _chatDbContext.Users.AnyAsync(u => u.UserName == model.UserName);

                if (!userExists)
                {
                    var userModel = new User
                    {
                        UserName = model.UserName,
                        Password = _utils.Encrypt(model.Password)
                    };

                    await _chatDbContext.Users.AddAsync(userModel);
                    await _chatDbContext.SaveChangesAsync();

                    if (userModel.UserId != 0)
                        return userModel;
                    else
                        return null;
                }
                else
                {
                    throw new Exception("Usuario ya existe");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> Login(UserDTO model)
        {
            try
            {
                var userDB = await _chatDbContext.Users
                    .Where(u => u.UserName == model.UserName &&
                    u.Password == _utils.Encrypt(model.Password)).FirstOrDefaultAsync();

                if (userDB != null)
                    return _utils.GenerateJWT(userDB);
                else
                    return null;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
