using AutoMapper;
using CaseWork.Data;
using CaseWork.Models;
using CaseWork.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace CaseWork.Services.Users;

public class UsersService : IUsersService
{
    private readonly CaseWorkContext _dbContext;
    private readonly IMapper _mapper;
    
    public UsersService(CaseWorkContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<User> UpdateInfo(UserUpdate userUpdate)
    {
        var candidate = await _dbContext.Users.FirstOrDefaultAsync(v => v.Email == userUpdate.Email);
        if (candidate == null) throw new Exception("User not found!");

        _dbContext.Users.Update(_mapper.Map<User>(userUpdate));
        await _dbContext.SaveChangesAsync();
        return await _dbContext.Users.FirstAsync(v => v.Email == userUpdate.Email);
    }
}