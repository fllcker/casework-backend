namespace CaseWork.Models;

public class RoleRelation
{
    public int Id { get; set; }
    public Role Role { get; set; }
    public User User { get; set; }
}