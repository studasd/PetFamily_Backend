using PetFamily.Application.Abstractions;

namespace PetFamily.Application.PetsManagement.Queries.GetPetById;

public record GetPetByIdQuery(Guid Id) : IQuery;