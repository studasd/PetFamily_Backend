using PetFamily.Core.Abstractions;

namespace PetFamily.Specieses.Application.Queries.GetBreedsPagination;

public record GetFilteredBreedsWithPaginationQuery(Guid SpeciesId, int Page, int PageSize) : IQuery;