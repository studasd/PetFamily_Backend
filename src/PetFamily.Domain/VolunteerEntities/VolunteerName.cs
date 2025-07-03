using CSharpFunctionalExtensions;

namespace PetFamily.Domain.VolunteerEntities;

public record VolunteerName (string Firstname, string Lastname, string Surname)
{
	public static Result<VolunteerName> Create(string firstname, string lastname, string surname)
	{
		if (string.IsNullOrWhiteSpace(firstname))
			return Result.Failure<VolunteerName>("Firstname cannot be empty");

		if (string.IsNullOrWhiteSpace(lastname))
			return Result.Failure<VolunteerName>("Lastname cannot be empty");

		if (string.IsNullOrWhiteSpace(surname))
			return Result.Failure<VolunteerName>("Surname cannot be empty");

		return new VolunteerName(firstname, lastname, surname);
	}
}
