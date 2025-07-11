namespace PetFamily.Contracts.Pets;

public record AddPetRequestAddressDTO(string Country, string City, string Street, int HouseNumber, int Apartment, string? HouseLiter = null);