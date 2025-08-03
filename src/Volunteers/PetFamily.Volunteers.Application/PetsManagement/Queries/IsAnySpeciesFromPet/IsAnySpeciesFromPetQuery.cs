using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.PetsManagement.Queries.IsAnySpeciesFromPet;

public record IsAnySpeciesFromPetQuery(Guid SpeciesId) : IQuery;
