using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Entities;

public record Address(string Country, string City, string Street, int HouseNumber, string? HouseLiter, int Apartment)
{

	public static Result<Address> Create(string country, string city, string street, int houseNumber, int apartment, string? houseLiter = null)
	{
		if (string.IsNullOrWhiteSpace(country))
			return Result.Failure<Address>("Country cannot be empty");

		if (string.IsNullOrWhiteSpace(city))
			return Result.Failure<Address>("City cannot be empty");

		if (string.IsNullOrWhiteSpace(street))
			return Result.Failure<Address>("Street cannot be empty");

		if (houseNumber <= 0)
			return Result.Failure<Address>("House number error");

		if (apartment <= 0)
			return Result.Failure<Address>("Apartment number error");

		return new Address(country, city, street, houseNumber, houseLiter, apartment);
	}
}
