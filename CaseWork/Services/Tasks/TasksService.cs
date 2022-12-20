using System.Net;
using System.Text.Json;
using AutoMapper;
using CaseWork.Data;
using CaseWork.Exceptions;
using CaseWork.Models;
using CaseWork.Models.Dto;
using CaseWork.Services.Invites;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace CaseWork.Services.Tasks;

public class TasksService : ITasksService
{
    private readonly CaseWorkContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IInvitesService _invitesService;

    public TasksService(CaseWorkContext dbContext, IInvitesService invitesService, IMapper mapper)
    {
        _dbContext = dbContext;
        _invitesService = invitesService;
        _mapper = mapper;
    }

    public async Task<Models.Task?> GetById(int id)
    {
        return await _dbContext.Tasks.FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<Models.Task> GetByIdWithVerifies(int id, string accessEmail)
    {
        var res = await _dbContext.Tasks
            .Include(v => v.Employer)
            .Include(v => v.Executor)
            .FirstOrDefaultAsync(v => v.Id == id);
        if (res == null) throw new ErrorResponse("Task not found!", HttpStatusCode.NotFound);
        if (res.Employer.Email != accessEmail && res.Executor.Email != accessEmail)
            throw new ErrorResponse("Access denied!", HttpStatusCode.Unauthorized);
        return res;
    }

    public async Task<Models.Task> ToComplete(string userEmail, int taskId)
    {
        var task = await _dbContext.Tasks
            .Where(v => v.Executor.Email == userEmail)
            .FirstOrDefaultAsync(v => v.Id == taskId);
        if (task == null) throw new ErrorResponse("Task not found!", HttpStatusCode.NotFound);
        if (task.IsComplete == true) throw new ErrorResponse("Task is already complete!");
        task.IsComplete = true;
        task.CompletedTime = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
        await _dbContext.SaveChangesAsync();
        return task;
    }

    public async Task<Models.Task> Create(TaskCreate taskCreate, User userCreator, User invitedUser)
    {
        Models.Task task = _mapper.Map<Models.Task>(taskCreate);
        task.Employer = userCreator;
        task.Executor = invitedUser;
        _dbContext.Tasks.Add(task);
        await _dbContext.SaveChangesAsync();

        // creating invite
        var invite = await _invitesService.Create(userCreator.Email, invitedUser.Email, new InviteCreate()
        {
            InviteType = InviteType.ToTask,
            InviteEntityId = task.Id
        });
        task.Invite = invite;
        await _dbContext.SaveChangesAsync();
        return task;
    }

    public async Task<List<Models.Task>> GetByFilter(TasksTypeFilter filterType, TasksAccessFilter filterAccess, string accessEmail,
        int skip = 0, int take = 10)
    {
        var tasks = await _dbContext.Tasks
            .Include(v => v.Employer)
            .Include(v => v.Executor)
            .ToListAsync();

        switch (filterType)
        {
            case TasksTypeFilter.InCompleted:
            {
                tasks = tasks.Where(v => v.IsComplete == false)
                    .ToList();
                break;
            }
            case TasksTypeFilter.Completed:
            {
                tasks = tasks.Where(v => v.IsComplete == true)
                    .ToList();
                break;
            }
            case TasksTypeFilter.DeadLine:
            {
                tasks = tasks.OrderBy(v => v.DeadLine)
                    .ToList();
                break;
            }
            case TasksTypeFilter.Urgency:
            {
                tasks = tasks
                    .OrderByDescending(v => v.Urgency)
                    .ThenByDescending(v => v.DeadLine)
                    .ToList();
                break;
            }
        }

        switch (filterAccess)
        {
            case TasksAccessFilter.Employer:
            {
                tasks = tasks.Where(v => v.Employer.Email == accessEmail)
                    .ToList();
                break;
            }
            case TasksAccessFilter.Executor:
            {
                tasks = tasks.Where(v => v.Executor.Email == accessEmail)
                    .ToList();
                break;
            }
        }


        return tasks.Where(v => v.AcceptedTime != -1)
            .Skip(skip).Take(take)
            .ToList();
    }

    public async Task<IEnumerable<Models.Task>> GetNonAcceptedTasks(string accessEmail)
    {
        return await _dbContext.Tasks
            .Include(v => v.Invite)
            .Include(v => v.Executor)
            .Where(v => v.AcceptedTime == -1)
            .Where(v => v.Executor.Email == accessEmail)
            .ToListAsync();
    }
}