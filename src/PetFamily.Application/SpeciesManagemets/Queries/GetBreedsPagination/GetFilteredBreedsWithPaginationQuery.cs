using PetFamily.Application.Abstractions;

namespace PetFamily.Application.SpeciesManagemets.Queries.GetBreedsPagination;

public record GetFilteredBreedsWithPaginationQuery(Guid SpeciesId, int Page, int PageSize) : IQuery;