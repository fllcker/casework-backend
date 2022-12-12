using AutoMapper;
using CaseWork.Data;
using CaseWork.Models;
using CaseWork.Models.Dto;
using CaseWork.Services.Tasks;
using CaseWork.Services.Users;
using Microsoft.EntityFrameworkCore;

namespace CaseWork.Services.Invites;

public class InvitesService : IInvitesService
{
    private readonly CaseWorkContext _dbContext;
    private readonly IUsersService _usersService;
    private readonly ITasksService _tasksService;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public InvitesService(CaseWorkContext dbContext, IUsersService usersService, 
        IMapper mapper, IConfiguration configuration, ITasksService tasksService)
    {
        _dbContext = dbContext;
        _usersService = usersService;
        _tasksService = tasksService;
        _configuration = configuration;
        _mapper = mapper;
    }

    public async Task<Invite> Create(string initEmail, string targetEmail, InviteCreate inviteCreate)
    {
        var user = await _usersService.GetByEmail(initEmail);
        var target = await _usersService.GetByEmail(targetEmail);
        if (user == null || target == null) throw new Exception("Error");
        Invite invite = _mapper.Map<Invite>(inviteCreate);
        invite.Initiator = user;
        invite.Target = target;
        invite.LinkHash = BCrypt.Net.BCrypt.HashPassword(
                invite.InviteEntityId + _configuration.GetSection("Config:Secret").Value!)
            .ToCharArray()
            .Take(10)
            .ToString();
        _dbContext.Invites.Add(invite);
        await _dbContext.SaveChangesAsync();
        return invite;
    }

    public async Task<Invite> DenyInvite(int inviteId, string userEmail)
    {
        var invite = await _dbContext.Invites
            .Include(v => v.Target)
            .FirstOrDefaultAsync(v => v.Id == inviteId);
        if (invite == null) throw new Exception("Invite not found!");
        if (invite.IsDenied) throw new Exception("Invite already denied");
        if (invite.Target.Email != userEmail) throw new Exception("This invite is not for you");
        invite.IsDenied = true;
        await _dbContext.SaveChangesAsync();
        return invite;
    }

    public async Task<Invite> AcceptInvite(int inviteId, string userEmail)
    {
        var invite = await _dbContext.Invites
            .Include(v => v.Target)
            .FirstOrDefaultAsync(v => v.Id == inviteId);
        if (invite == null) throw new Exception("Invite not found!");
        if (invite.IsAccepted) throw new Exception("Invite already accepted");
        if (invite.Target.Email != userEmail) throw new Exception("This invite is not for you");
        invite.IsAccepted = true;
        await _dbContext.SaveChangesAsync();

        if (invite.InviteType == InviteType.ToTask)
        {
            var task = new Models.Task()
            {
                
            };
            var returnedTask = await _tasksService.Create(task); // не доделано
        }
        
        return invite; 
    }
}