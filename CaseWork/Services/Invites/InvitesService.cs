﻿using AutoMapper;
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

        if (invite.InviteType == InviteType.ToTask)
        {
            var task = await _dbContext.Tasks.FirstOrDefaultAsync(v => v.Id == invite.InviteEntityId);
            if (task == null) throw new Exception("Task not found!");
            if (task.Executor.Email != userEmail) throw new Exception("Invite error!");
            task.AcceptedTime = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
            await _dbContext.SaveChangesAsync();
        }
        
        return invite;
    }
}