namespace CaseWork.Models;

public class Invite
{
    public int Id { get; set; }
    public InviteType InviteType { get; set; }
    public int InviteEntityId { get; set; }

    public bool IsAccepted { get; set; } = false;
    public bool IsDenied { get; set; } = false;

    public long CreatedAt { get; set; } = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();

    public User Initiator { get; set; }
    public User Target { get; set; }
}

public enum InviteType
{
    ToTask,
    ToCompany
}