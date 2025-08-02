using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.SpeciesManagemets.Queries.GetBreedsPagination;

public record GetFilteredBreedsWithPaginationQuery(Guid SpeciesId, int Page, int PageSize) : IQuery;