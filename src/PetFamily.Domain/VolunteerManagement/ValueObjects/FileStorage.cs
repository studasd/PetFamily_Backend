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

	public static Result<FileStorage, Error> Create(Guid path, string extension)
	{
		// валядация расширений файла

		var fullPath = path + extension;

		return new FileStorage(fullPath);
	}

	public static Result<FileStorage, Error> Create(string fullPath)
	{
		return new FileStorage(fullPath);
	}
}
