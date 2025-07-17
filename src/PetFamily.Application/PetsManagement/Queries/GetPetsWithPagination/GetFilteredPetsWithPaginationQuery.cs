using PetFamily.Application.Abstractions;

namespace PetFamily.Application.PetsManagement.Queries.GetPetsWithPagination;

public record GetFilteredPetsWithPaginationQuery(string? Name, int Page, int PageSize) : IQuery;
