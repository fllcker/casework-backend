using AutoMapper;
using CaseWork.Data;
using CaseWork.Models;
using CaseWork.Models.Dto;
using CaseWork.Services.Tasks;
using CaseWork.Services.Users;
using Microsoft.EntityFrameworkCore;
using Task = CaseWork.Models.Task;

namespace CaseWork.Services.Invites;

public class InvitesService : IInvitesService
{
    private readonly CaseWorkContext _dbContext;
    private readonly IUsersService _usersService;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public InvitesService(CaseWorkContext dbContext, IUsersService usersService, 
        IMapper mapper, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _usersService = usersService;
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

        switch (invite.InviteType)
        {
            case InviteType.ToTask: 
                await AcceptToTask(invite, userEmail); 
                break;
            case InviteType.ToCompany:
                await AcceptToCompany(invite, userEmail);
                break;
        }
        
        return invite;
    }

    public async Task<IEnumerable<Invite>> GetUserInvites(string accessEmail)
    {
        return await _dbContext.Invites
            .Include(v => v.Target)
            .Where(v => v.IsAccepted == false)
            .Where(v => v.IsDenied == false)
            .Where(v => v.Target.Email == accessEmail)
            .ToListAsync();
    }

    private async Task<Models.Task> AcceptToTask(Invite invite, string userEmail)
    {
        var task = await _dbContext.Tasks.FirstOrDefaultAsync(v => v.Id == invite.InviteEntityId);
        if (task == null) throw new Exception("Task not found!");
        if (task.Executor.Email != userEmail) throw new Exception("Invite error!");
        task.AcceptedTime = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
        await _dbContext.SaveChangesAsync();
        return task;
    }

    private async Task<Company> AcceptToCompany(Invite invite, string userEmail)
    {
        var company = await _dbContext.Companies
            .Include(v => v.Users)
            .FirstOrDefaultAsync(v => v.Id == invite.InviteEntityId);
        if (company == null) throw new Exception("Company not found!");
        if (company.Users.Count(v => v.Email == userEmail) != 0)
            throw new Exception("You are already a member of this company");
        
        company.Users.Add(await _dbContext.Users.FirstOrDefaultAsync(v => v.Email == userEmail)
        ?? throw new Exception("User not found!"));
        await _dbContext.SaveChangesAsync();
        return company;
    }

}