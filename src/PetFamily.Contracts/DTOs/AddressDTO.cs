namespace PetFamily.Contracts.DTOs;

public record AddressDTO(string Country, string City, string Street, int HouseNumber, int Apartment, string? HouseLiter = null);