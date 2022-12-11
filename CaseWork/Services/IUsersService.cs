using CaseWork.Models;
using CaseWork.Models.Dto;

namespace CaseWork.Services;

public interface IUsersService
{
    public Task<User> UpdateInfo(UserUpdate userUpdate);
}