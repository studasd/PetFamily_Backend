using PetFamily.Application.Abstractions;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteerManagement.Enums;

namespace PetFamily.Application.PetsManagement.Queries.GetPetsWithPagination;

public record GetFilteredPetsWithPaginationQuery(
	int Page, 
	int PageSize,
	Guid[]? VolunteerIds,
	string? Name, 
	int? Age,
	Guid? SpeciesId, 
	Guid? BreedId, 
	string? Color,
	decimal? Weight,
	decimal? Height,
	string? Country,
	string? City,
	PetHelpStatuses? HelpStatus,
	int? PositionFrom, 
	int? PositionTo,
	string? SortBy,
	string? SortDirection // desc, asc
) : IQuery;
