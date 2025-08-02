using CSharpFunctionalExtensions;
using PetFamily.Core.Errores;

namespace PetFamily.Domain.VolunteerManagement.ValueObjects;

public record VolunteerName (string Firstname, string Lastname, string Surname)
{
	public static Result<VolunteerName, Error> Create(string firstname, string lastname, string surname)
	{
		if (string.IsNullOrWhiteSpace(firstname))
			return Errors.General.ValueIsRequired("Firstname");

		if (string.IsNullOrWhiteSpace(lastname))
			return Errors.General.ValueIsRequired("Lastname");

		if (string.IsNullOrWhiteSpace(surname))
			return Errors.General.ValueIsRequired("Surname");

		return new VolunteerName(firstname, lastname, surname);
	}
}
