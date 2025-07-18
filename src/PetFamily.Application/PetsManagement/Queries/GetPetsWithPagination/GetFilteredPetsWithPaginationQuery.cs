using PetFamily.Application.Abstractions;

namespace PetFamily.Application.PetsManagement.Queries.GetPetsWithPagination;

public record GetFilteredPetsWithPaginationQuery(int Page, int PageSize, string? Name, int? PositionFrom, int? PositionTo) : IQuery;
