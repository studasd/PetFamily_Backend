using PetFamily.Core.Abstractions;

namespace PetFamily.Specieses.Application.Queries.GetSpeciesPagination;

public record GetFilteredSpeciesWithPaginationQuery(int Page, int PageSize) : IQuery;