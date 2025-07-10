namespace PetFamily.Contracts.Pets.Create;

public record CreatePetRequestAddressDTO(string Country, string City, string Street, int HouseNumber, int Apartment, string? HouseLiter = null);