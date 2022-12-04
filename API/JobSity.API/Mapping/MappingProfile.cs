using AutoMapper;
using JobSity.API.ViewModels;
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
        }
    }
}
