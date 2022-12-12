using AutoMapper;
using CaseWork.Models;
using CaseWork.Models.Dto;
using Task = CaseWork.Models.Task;

namespace CaseWork.Data;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {			
        CreateMap<UserSignup, User>();
        CreateMap<UserUpdate, User>();
        CreateMap<User, UserProfileData>();
        CreateMap<InviteCreate, Invite>();
        CreateMap<TaskCreate, Task>();
    }
}