using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.SharedKernel.ValueObjects;

public record FullName
{
	private FullName(string firstName, string lastName)
	{
		FirstName = firstName;
		LastName = lastName;
	}

	public string FirstName { get; }

	public string LastName { get; }

	public static Result<FullName, Error> Create(string firstName, string lastName)
	{
		if (string.IsNullOrWhiteSpace(firstName))
			return Errors.General.ValueIsInvalid("FirstName");

		if (string.IsNullOrWhiteSpace(lastName))
			return Errors.General.ValueIsInvalid("LastName");

		return new FullName(firstName, lastName);
	}	
}
