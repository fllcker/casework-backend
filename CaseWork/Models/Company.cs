using System.ComponentModel.DataAnnotations;

namespace CaseWork.Models;

public class Company
{
    public int Id { get; set; }
    [MaxLength(32)]
    public string Name { get; set; }
    public User? UserCreator { get; set; }

    public List<User> Users { get; set; } = new List<User>();
}