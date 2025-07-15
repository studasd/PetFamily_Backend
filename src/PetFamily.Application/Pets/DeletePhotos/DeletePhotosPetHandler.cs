using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Application.Volonteers;
using PetFamily.Domain.Shared.Errores;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Application.Pets.DeletePhotos;

public class DeletePhotosPetHandler
{
	private readonly IFileProvider fileProvider;
	private readonly IVolunteerRepository volunteerRepository;
	private readonly ILogger<DeletePhotosPetHandler> logger;
	private const string BUCKET_NAME = "photos";

	public DeletePhotosPetHandler(
		IFileProvider fileProvider,
		IVolunteerRepository volunteerRepository,
		ILogger<DeletePhotosPetHandler> logger)
	{
		this.fileProvider = fileProvider;
		this.volunteerRepository = volunteerRepository;
		this.logger = logger;
	}

	public async Task<Result<Guid, Error>> HandleAsync(DeletePhotosPetCommand command, CancellationToken token)
	{
		var volunteerResult = await volunteerRepository.GetByIdAsync(command.VolunteerId, token);
		if (volunteerResult.IsFailure)
			return volunteerResult.Error;

		var petResult = volunteerResult.Value.Pets.FirstOrDefault(p => p.Id.Value == command.PetId);
		if (petResult == null)
			return Errors.General.NotFound(command.PetId);

		foreach (var file in command.DeleteFiles)
		{
			var deleteResult = await fileProvider.DeleteFileAsync(new FileInform(file.ToString(), BUCKET_NAME), token);
			if (deleteResult.IsFailure)
				return deleteResult.Error;
		}

		var deleteFiles = command.DeleteFiles.Select(f => FileStorage.Create(f.ToString()).Value);

		petResult.DeletePhotos(deleteFiles);

		await volunteerRepository.SaveAsync(token);

		logger.LogInformation("Delete pet {id} photos.", command.PetId);

		return petResult.Id.Value;
	}
}
