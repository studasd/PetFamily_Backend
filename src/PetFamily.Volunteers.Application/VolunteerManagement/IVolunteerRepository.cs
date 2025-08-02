using CSharpFunctionalExtensions;
using PetFamily.Core.Errores;
using PetFamily.Volunteers.Domain.Entities;
using PetFamily.Volunteers.Domain.IDs;
using PetFamily.Volunteers.Domain.ValueObjects;

namespace PetFamily.Volunteers.Application.VolunteerManagement;

public interface IVolunteerRepository
{
	Task<Guid> AddAsync(Volunteer volunteer, CancellationToken token);
	Task<Guid> DeleteAsync(Volunteer volunteer, CancellationToken token);
	Task<Result<Volunteer, Error>> GetByIdAsync(VolunteerId volunteerId, CancellationToken token);
	Task<Result<Volunteer, Error>> GetByNameAsync(VolunteerName volunteerName, CancellationToken token);
	Task SaveAsync(CancellationToken token);
}