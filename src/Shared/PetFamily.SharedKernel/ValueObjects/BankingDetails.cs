using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.SharedKernel.ValueObjects;

public record BankingDetails(string? Name, string? Description)
{
	public static Result<BankingDetails, Error> Create(string name, string description)
	{
		if (string.IsNullOrWhiteSpace(name))
			return Errors.General.ValueIsRequired("Name");

		if (string.IsNullOrWhiteSpace(description))
			return Errors.General.ValueIsRequired("Description");

		return new BankingDetails(name, description);
	}
}
