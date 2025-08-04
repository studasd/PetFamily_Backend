using PetFamily.Core.Abstractions;

namespace PetFamily.Specieses.Application.Queries.CheckSpeciesBreedExist;

public record CheckSpeciesBreedExistQuery(Guid SpeciesId, Guid BreedId) : IQuery;
