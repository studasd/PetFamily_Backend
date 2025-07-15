using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Errores;
using PetFamily.Domain.VolunteerManagement.Entities;
using PetFamily.Domain.VolunteerManagement.IDs;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Application.Volonteers;

public interface IVolunteerRepository
{
	Task<Guid> AddAsync(Volunteer volunteer, CancellationToken token);
	Task<Guid> DeleteAsync(Volunteer volunteer, CancellationToken token);
	Task<Result<Volunteer, Error>> GetByIdAsync(VolunteerId volunteerId, CancellationToken token);
	Task<Result<Volunteer, Error>> GetByNameAsync(VolunteerName volunteerName, CancellationToken token);
	Task SaveAsync(CancellationToken token);
}