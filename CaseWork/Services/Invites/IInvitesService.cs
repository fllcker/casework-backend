using CaseWork.Models;
using CaseWork.Models.Dto;

namespace CaseWork.Services.Invites;

public interface IInvitesService
{
    public Task<Invite> Create(string initEmail, string targetEmail, InviteCreate inviteCreate);
    public Task<Invite> DenyInvite(int inviteId, string userEmail);
    public Task<Invite> AcceptInvite(int inviteId, string userEmail);
    public Task<IEnumerable<Invite>> GetUserInvites(string accessEmail);
}