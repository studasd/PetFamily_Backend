using PetFamily.Core.Abstractions;

namespace PetFamily.Application.PetsManagement.Queries.GetPetById;

public record GetPetByIdQuery(Guid Id) : IQuery;