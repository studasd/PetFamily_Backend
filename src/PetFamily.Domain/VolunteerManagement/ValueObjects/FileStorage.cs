using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.VolunteerManagement.ValueObjects;

public record FileStorage
{
	private FileStorage(string pathToStorage)
	{
		PathToStorage = pathToStorage;
	}

	public string PathToStorage { get; }

	public static Result<FileStorage, Error> Create(string pathToStorage)
	{
		if (String.IsNullOrEmpty(pathToStorage))
			return Errors.General.ValueIsInvalid("PathToStorage");

		return new FileStorage(pathToStorage);
	}
}
