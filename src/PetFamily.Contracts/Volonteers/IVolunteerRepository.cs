using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerEntities;

namespace PetFamily.Contracts.Volonteers;

public interface IVolunteerRepository
{
	Task<Guid> AddAsync(Volunteer volunteer, CancellationToken token = default);
	Task<Result<Volunteer, Error>> GetByIdAsync(VolunteerId volunteerId, CancellationToken token = default);
	Task<Result<Volunteer, Error>> GetByNameAsync(VolunteerName volunteerName, CancellationToken token = default);
}