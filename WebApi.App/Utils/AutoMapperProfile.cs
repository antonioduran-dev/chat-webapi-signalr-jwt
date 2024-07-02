using AutoMapper;
using DB.Models;
using WebApi.App.DTOs;

namespace WebApi.App.Utils
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Message, MessageDTO>(); // convert to DTO
            CreateMap<MessageDTO, Message>(); // convert to DB model

            CreateMap<User, UserDTO>(); // convert to DTO
            CreateMap<UserDTO, User>(); // convert to DB model
        }
    }
}
