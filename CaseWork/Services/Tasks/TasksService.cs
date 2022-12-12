using CaseWork.Data;
using CaseWork.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace CaseWork.Services.Tasks;

public class TasksService : ITasksService
{
    private readonly CaseWorkContext _dbContext;

    public TasksService(CaseWorkContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<Models.Task>> GetInCompletedTasks(string userEmail)
    {
        return await _dbContext.Tasks
            .Include(v => v.Employer)
            .Include(v => v.Executor)
            .Where(v => v.Executor.Email == userEmail)
            .Where(v => v.IsComplete == false)
            .OrderByDescending(v => v.DeadLine)
            .ThenByDescending(v => v.AcceptedTime)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Models.Task>> GetInCompletedTasksByEmployer(string userEmail)
    {
        return await _dbContext.Tasks
            .Include(v => v.Employer)
            .Include(v => v.Executor)
            .Where(v => v.Employer.Email == userEmail)
            .Where(v => v.IsComplete == false)
            .OrderByDescending(v => v.DeadLine)
            .ThenByDescending(v => v.AcceptedTime)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Models.Task>> GetAllTasks(string userEmail, GetBy by = GetBy.Executor)
    {
        var tasks = await _dbContext.Tasks
            .Include(v => v.Employer)
            .Include(v => v.Executor)
            .Where(v => v.IsComplete == false)
            .OrderByDescending(v => v.DeadLine)
            .ThenByDescending(v => v.AcceptedTime)
            .ToListAsync();

        if (by == GetBy.Executor)
            return tasks.Where(v => v.Executor.Email == userEmail).ToList();
        return tasks.Where(v => v.Employer.Email == userEmail).ToList();
    }

    public async Task<Models.Task> ToComplete(string userEmail, int taskId)
    {
        var task = await _dbContext.Tasks
            .Where(v => v.Executor.Email == userEmail)
            .FirstOrDefaultAsync(v => v.Id == taskId);
        if (task == null) throw new Exception("Task not found!");
        if (task.IsComplete == true) throw new Exception("Task is already complete!");
        task.IsComplete = true;
        await _dbContext.SaveChangesAsync();
        return task;
    }

    public async Task<Models.Task> Create(Models.Task task)
    {
        _dbContext.Tasks.Add(task);
        await _dbContext.SaveChangesAsync();
        return task;
    }
}