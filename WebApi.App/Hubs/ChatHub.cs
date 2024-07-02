using DB.Data;
using DB.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebApi.App.DTOs;

namespace WebApi.App.Hubs
{
    // inherit of Hub
    public class ChatHub : Hub
    {
        private readonly ChatDbContext _context;

        // inject context in constructor
        public ChatHub(ChatDbContext context)
        {
            _context = context;
        }

        public async Task SendMessage(string user, string message)
        {
            try
            {
                var userDb = await _context.Users.Where(u => u.UserName == user).FirstOrDefaultAsync();

                if (userDb != null)
                {
                    // create a new message
                    var chatMessage = new Message
                    {
                        Text = message,
                        TimeStamp = DateTime.Now,
                        UserId = userDb.UserId
                    };

                    // save the message in DB.
                    _context.Messages.Add(chatMessage);
                    await _context.SaveChangesAsync();

                    // send the respons to all clients.
                    await Clients.All.SendAsync("ReceiveMessage", user, message, chatMessage.TimeStamp);
                }
                else
                {
                    throw new Exception("Usuario incorrecto");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
        }
    }
}
