using PetFamily.Core.DTOs;

namespace PetFamily.Volunteers.Application;

public interface IReadDbContext
{
	IQueryable<VolunteerDto> Volunteers { get; }
	IQueryable<PetDto> Pets { get; }
}
