using System.Net;
using AutoMapper;
using CaseWork.Data;
using CaseWork.Exceptions;
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
        if (user == null || target == null) 
            throw new ErrorResponse("Not found user or company", HttpStatusCode.NotFound);
        Invite invite = _mapper.Map<Invite>(inviteCreate);
        invite.Initiator = user;
        invite.Target = target;
        _dbContext.Invites.Add(invite);
        await _dbContext.SaveChangesAsync();

        if (invite.Initiator.Id == invite.Target.Id)
            await AcceptInvite(invite.Id, invite.Initiator.Email);
        
        return invite;
    }

    public async Task<Invite> DenyInvite(int inviteId, string userEmail)
    {
        var invite = await _dbContext.Invites
            .Include(v => v.Target)
            .FirstOrDefaultAsync(v => v.Id == inviteId);
        
        if (invite == null) 
            throw new ErrorResponse("Invite not found!", HttpStatusCode.NotFound);
        if (invite.IsDenied) 
            throw new ErrorResponse("Invite already denied", HttpStatusCode.BadRequest);
        if (invite.Target.Email != userEmail) 
            throw new ErrorResponse("This invite is not for you", HttpStatusCode.Unauthorized);
        
        invite.IsDenied = true;
        await _dbContext.SaveChangesAsync();
        return invite;
    }

    public async Task<Invite> AcceptInvite(int inviteId, string userEmail)
    {
        var invite = await _dbContext.Invites
            .Include(v => v.Target)
            .FirstOrDefaultAsync(v => v.Id == inviteId);
        
        if (invite == null) 
            throw new ErrorResponse("Invite not found!", HttpStatusCode.NotFound);
        if (invite.IsAccepted) 
            throw new ErrorResponse("Invite already accepted", HttpStatusCode.BadRequest);
        if (invite.Target.Email != userEmail) 
            throw new ErrorResponse("This invite is not for you", HttpStatusCode.Unauthorized);
        
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

    public async Task<Invite> GetInviteByTask(int taskId)
    {
        var task = await _dbContext.Tasks
            .Include(v => v.Executor)
            .FirstOrDefaultAsync(v => v.Id == taskId);
        if (task == null) throw new ErrorResponse("Task not found!", HttpStatusCode.NotFound);
        
        return await _dbContext.Invites
            .Include(v => v.Target)
            .Where(v => v.InviteEntityId == task.Id)
            .Where(v => v.Target.Id == task.Executor.Id)
            .FirstOrDefaultAsync() 
               ?? throw new ErrorResponse("Invite not found!", HttpStatusCode.NotFound);
    }
    
    private async Task<Models.Task> AcceptToTask(Invite invite, string userEmail)
    {
        var task = await _dbContext.Tasks.FirstOrDefaultAsync(v => v.Id == invite.InviteEntityId);
        if (task == null) throw new ErrorResponse("Task not found!", HttpStatusCode.NotFound);
        if (task.Executor.Email != userEmail) throw new ErrorResponse("Invite error!");
        task.AcceptedTime = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
        await _dbContext.SaveChangesAsync();
        return task;
    }

    private async Task<Company> AcceptToCompany(Invite invite, string userEmail)
    {
        var company = await _dbContext.Companies
            .Include(v => v.Users)
            .FirstOrDefaultAsync(v => v.Id == invite.InviteEntityId);
        if (company == null) throw new ErrorResponse("Company not found!", HttpStatusCode.NotFound);
        if (company.Users.Count(v => v.Email == userEmail) != 0)
            throw new Exception("You are already a member of this company");

        company.Users.Add(await _dbContext.Users.FirstOrDefaultAsync(v => v.Email == userEmail)
                          ?? throw new ErrorResponse("User not found!", HttpStatusCode.NotFound));
        await _dbContext.SaveChangesAsync();
        return company;
    }
}