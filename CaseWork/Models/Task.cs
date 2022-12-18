using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaseWork.Models;

public class Task
{
    public int Id { get; set; }
    [MaxLength(64)]
    [MinLength(1)]
    public string Title { get; set; }
    public string Assignment { get; set; }
    public int Urgency { get; set; } = 0; // procents
    public long DeadLine { get; set; }
    public bool? IsComplete { get; set; } = false;
    public long AcceptedTime { get; set; } = -1;
    public long CompletedTime { get; set; } = -1;
    
    public User Employer { get; set; }
    public User Executor { get; set; }
    public Task? SubTask { get; set; }
    
    public long CreatedAt { get; set; } = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
}