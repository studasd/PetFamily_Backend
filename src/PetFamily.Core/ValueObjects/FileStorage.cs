using CSharpFunctionalExtensions;
using PetFamily.Core.Errores;

namespace PetFamily.Core.ValueObjects;

public record FileStorage
{
	private FileStorage(string pathToStorage, bool isPrime = false)
	{
		PathToStorage = pathToStorage;
		IsPrime = isPrime;
	}

	public string PathToStorage { get; }
	public bool IsPrime { get; }

	public static Result<FileStorage, Error> Create(Guid path, string extension)
	{
		// валядация расширений файла

		var fullPath = path + extension;

		return new FileStorage(fullPath);
	}

	public static Result<FileStorage, Error> Create(string fullPath, bool isPrime = false)
	{
		return new FileStorage(fullPath, isPrime);
	}
}
