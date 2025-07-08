using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerManagement.Entities;
using PetFamily.Domain.VolunteerManagement.IDs;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Contracts.Volonteers;

public interface IVolunteerRepository
{
	Task<Guid> AddAsync(Volunteer volunteer, CancellationToken token = default);
	Task<Result<Volunteer, Error>> GetByIdAsync(VolunteerId volunteerId, CancellationToken token = default);
	Task<Result<Volunteer, Error>> GetByNameAsync(VolunteerName volunteerName, CancellationToken token = default);
	Task SaveAsync(CancellationToken token = default);
}