using System.ComponentModel.DataAnnotations;

namespace CaseWork.Models.Dto;

public class UserSignup
{
    [EmailAddress]
    [MaxLength(64)]
    public string Email { get; set; }
    [MinLength(8)]
    public string Password { get; set; }
    
    [MaxLength(32)]
    public string? FirstName { get; set; }
    [MaxLength(32)]
    public string? LastName { get; set; }
    [MaxLength(32)]
    public string? Country { get; set; }
    [MaxLength(32)]
    public string? City { get; set; }
    public bool? Horse { get; set; } = false;
}