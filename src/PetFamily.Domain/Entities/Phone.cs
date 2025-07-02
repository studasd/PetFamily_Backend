using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Entities;

public record Phone(string phone)
{
	public static Result<Phone> Create(string phone)
	{
		if (string.IsNullOrWhiteSpace(phone))
			return Result.Failure<Phone>("Phone cannot be empty");

		return new Phone(phone);
	}
}
