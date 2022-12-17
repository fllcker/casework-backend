namespace CaseWork.Models.Dto;

public enum TasksTypeFilter
{
    InCompleted = 0,
    Completed,
    All,
    DeadLine,
    Urgency
}

public enum TasksAccessFilter
{
    Executor = 0,
    Employer,
    All
}