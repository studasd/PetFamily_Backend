using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Errores;

namespace PetFamily.Domain.Shared.ValueObjects;

public record Address(string Country, string City, string Street, int HouseNumber, string? HouseLiter, int Apartment)
{

	public static Result<Address, Error> Create(string country, string city, string street, int houseNumber, int apartment, string? houseLiter = null)
	{
		if (string.IsNullOrWhiteSpace(country))
			return Errors.General.ValueIsRequired("Country");

		if (string.IsNullOrWhiteSpace(city))
			return Errors.General.ValueIsRequired("City");

		if (string.IsNullOrWhiteSpace(street))
			return Errors.General.ValueIsRequired("Street");

		if (houseNumber <= 0)
			return Errors.General.ValueIsInvalid("House");

		if (apartment <= 0)
			return Errors.General.ValueIsInvalid("Apartment");

		return new Address(country, city, street, houseNumber, houseLiter, apartment);
	}
}
