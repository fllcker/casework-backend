using CaseWork.Models.Dto;

namespace CaseWork.Services.Tasks;

public interface ITasksService
{
    public Task<IEnumerable<Models.Task>> GetInCompletedTasks(string userEmail);
    public Task<IEnumerable<Models.Task>> GetInCompletedTasksByEmployer(string userEmail);
    public Task<IEnumerable<Models.Task>> GetAllTasks(string userEmail, GetBy by = GetBy.Executor);
    public Task<Models.Task> ToComplete(string userEmail, int taskId);
    public Task<Models.Task> Create(Models.Task task);
}