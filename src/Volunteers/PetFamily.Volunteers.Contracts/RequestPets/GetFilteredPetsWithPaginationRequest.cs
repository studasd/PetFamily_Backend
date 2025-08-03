using PetFamily.Volunteers.Contracts.Enums;

namespace PetFamily.Volunteers.Contracts.RequestPets;

public record GetFilteredPetsWithPaginationRequest(
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
);
