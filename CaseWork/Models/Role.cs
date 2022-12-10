using System.ComponentModel.DataAnnotations;

namespace CaseWork.Models;

public class Role
{
    public int Id { get; set; }
    [MaxLength(32)]
    public string Title { get; set; }
    public int Priority { get; set; } = 0;
}