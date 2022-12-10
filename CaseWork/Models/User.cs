using System.ComponentModel.DataAnnotations;

namespace CaseWork.Models;

public class User
{
    public int Id { get; set; }
    [EmailAddress]
    [MaxLength(64)]
    public string Email { get; set; }
    public string Password { get; set; }
    
    [MaxLength(32)]
    public string? FirstName { get; set; }
    [MaxLength(32)]
    public string? LastName { get; set; }
    public Company UserCompany { get; set; }
    [MaxLength(32)]
    public string? Country { get; set; }
    [MaxLength(32)]
    public string? City { get; set; }
    public bool? Horse { get; set; } = false;
    public List<RoleRelation> Roles { get; set; }
}