using DB.Models;
using WebApi.App.DTOs;

namespace WebApi.App.Services
{
    public interface IChatService
    {
        Task<IEnumerable<MessageDTO>> GetAll();
        Task<Message> Post(MessageDTO model);
    }
}
