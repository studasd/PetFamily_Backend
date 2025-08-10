using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Application.Abstractions;
public interface IAccountsManager
{
	Task CreateAdminAccountAsync(AdminAccount adminAccount, CancellationToken token);
	Task CreateParticipantAccountAsync(ParticipantAccount participantAccount, CancellationToken token);
	Task CreateVolunteerAccount(VolunteerAccount volunteerAccount, CancellationToken token);
}