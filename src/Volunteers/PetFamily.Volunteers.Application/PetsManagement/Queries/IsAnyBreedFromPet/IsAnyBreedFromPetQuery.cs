using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.PetsManagement.Queries.IsAnyBreedFromPet;

public record IsAnyBreedFromPetQuery(Guid BreedId) : IQuery;