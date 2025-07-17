namespace PetFamily.Contracts.RequestPets;

public record GetFilteredPetsWithPaginationRequest(string? Name, int Page, int PageSize);
