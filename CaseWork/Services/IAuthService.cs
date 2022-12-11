using CaseWork.Models;
using CaseWork.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CaseWork.Services;

public interface IAuthService
{
    public Task<bool> EmailExists(string email);
    public Task<string> Create(User user);
    public Task<string> Login(UserLogin userLogin);
}