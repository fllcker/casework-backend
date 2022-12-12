using System.ComponentModel.DataAnnotations;

namespace CaseWork.Models.Dto;

public class TaskCreate
{
    [MaxLength(64)]
    [MinLength(1)]
    public string Title { get; set; }
    public string Assignment { get; set; }
    public int Urgency { get; set; } = 0;
    public DateTimeKind DeadLine { get; set; }
}