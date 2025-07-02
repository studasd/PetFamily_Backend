using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Entities;

public record BankingDetails(string Name, string Description)
{
	public static Result<BankingDetails> Create(string name, string description)
	{
		if (string.IsNullOrWhiteSpace(name))
			return Result.Failure<BankingDetails>("Country cannot be empty");

		if (string.IsNullOrWhiteSpace(description))
			return Result.Failure<BankingDetails>("City cannot be empty");

		return new BankingDetails(name, description);
	}
}
