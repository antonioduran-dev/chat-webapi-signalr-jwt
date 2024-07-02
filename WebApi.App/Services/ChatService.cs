using AutoMapper;
using DB.Data;
using DB.Models;
using Microsoft.EntityFrameworkCore;
using WebApi.App.DTOs;

namespace WebApi.App.Services
{
    public class ChatService : IChatService
    {
        private readonly ChatDbContext _chatDbContext;
        private readonly IMapper _mapper;

        public ChatService(ChatDbContext chatDbContext, IMapper mapper)
        {
            _chatDbContext = chatDbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MessageDTO>> GetAll()
        {
            try
            {
                // return the messages in DB ordered by date.
                return _mapper.Map<IEnumerable<MessageDTO>>(await _chatDbContext.Messages.OrderBy(m => m.TimeStamp).ToListAsync()); // convert FB model to DTO.
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Message> Post(MessageDTO model)
        {
            try
            {
                // set the datetime in the message TimeStamp.
                model.TimeStamp = DateTime.Now;
                var dbModel = _mapper.Map<Message>(model);
                // save the message in DB.
                _chatDbContext.Messages.Add(dbModel);
                await _chatDbContext.SaveChangesAsync();

                return await _chatDbContext.Messages.Include(m => m.User).FirstAsync(m => m.MessageId == dbModel.MessageId); ;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
