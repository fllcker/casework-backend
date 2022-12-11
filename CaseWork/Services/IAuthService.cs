using CaseWork.Models;

namespace CaseWork.Services;

public interface IAuthService
{
    public Task<bool> EmailExists(string email);
    public Task<string> Create(User user);
}