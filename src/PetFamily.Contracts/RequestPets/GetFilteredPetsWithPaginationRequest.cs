namespace PetFamily.Contracts.RequestPets;

public record GetFilteredPetsWithPaginationRequest(int Page, int PageSize, string? Name, int? PositionFrom, int? PositionTo);
