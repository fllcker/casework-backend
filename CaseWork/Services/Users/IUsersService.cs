using CaseWork.Models;
using CaseWork.Models.Dto;

namespace CaseWork.Services.Users;

public interface IUsersService
{
    public Task<User> UpdateInfo(UserUpdate userUpdate, string accessEmail);
    public Task<User?> GetByEmail(string email);
}