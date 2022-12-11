using AutoMapper;
using CaseWork.Models;
using CaseWork.Models.Dto;

namespace CaseWork.Data;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {			
        CreateMap<UserSignup, User>();
        CreateMap<UserUpdate, User>();
    }
}