using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.PetsManagement.Queries.GetPetById;

public record GetPetByIdQuery(Guid Id) : IQuery;