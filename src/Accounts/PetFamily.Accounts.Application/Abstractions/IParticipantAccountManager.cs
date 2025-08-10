using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Application.Abstractions;

public interface IParticipantAccountManager
{
	Task CreateParticipantAccountAsync(ParticipantAccount participantAccount, CancellationToken token);
}
