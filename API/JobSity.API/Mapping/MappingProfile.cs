using AutoMapper;
using JobSity.API.ViewModels;
using JobSity.Model.Helpers.InputModels;
using JobSity.Model.Models;
using Microsoft.AspNetCore.Identity;

namespace JobSity.API.Mapping
{
    public class MappingProfile : Profile
    {
        private readonly IMapper _mapper;


        public MappingProfile(IMapper mapper)
        {
            this._mapper = mapper;
        }

        public MappingProfile()
        {
            CreateMap<User, UserLoginViewModel>();
            CreateMap<RoomInputModel, Room>();
            CreateMap<Room, RoomViewModel>();

            CreateMap<Chat, ChatViewModel>()
               .ForMember(dest => dest.Username, opt =>
               {
                   opt.MapFrom(src => src.User.UserName);
               });

            CreateMap<ChatInputModel, Chat>();
        }
    }
}
