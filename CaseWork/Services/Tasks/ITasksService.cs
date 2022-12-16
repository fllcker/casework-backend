using CaseWork.Models;
using CaseWork.Models.Dto;

namespace CaseWork.Services.Tasks;

public interface ITasksService
{
    public Task<Models.Task?> GetById(int id);
    public Task<Models.Task> GetByIdWithVerifies(int id, string accessEmail);
    // public Task<IEnumerable<Models.Task>> GetInCompletedTasks(string userEmail);
    // public Task<IEnumerable<Models.Task>> GetInCompletedTasksByEmployer(string userEmail);
    // public Task<IEnumerable<Models.Task>> GetAllTasks(string userEmail, GetBy by = GetBy.Executor);
    public Task<Models.Task> ToComplete(string userEmail, int taskId);
    public Task<Models.Task> Create(TaskCreate taskCreate, User userCreator, User invitedUser);

    public Task<List<Models.Task>> GetByFilter(TasksTypeFilter filterType, TasksAccessFilter filterAccess,
        string accessEmail);
}