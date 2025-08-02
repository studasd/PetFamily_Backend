using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.SpeciesManagemets.Queries.GetSpeciesPagination;

public record GetFilteredSpeciesWithPaginationQuery(int Page, int PageSize) : IQuery;