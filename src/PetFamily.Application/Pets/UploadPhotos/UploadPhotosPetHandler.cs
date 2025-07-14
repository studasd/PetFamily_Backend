using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Application.Volonteers;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Application.Pets.UploadPhotos;

public class UploadPhotosPetHandler // CreatePetService
{
	private readonly IFileProvider minioProvider;
	private readonly IVolunteerRepository volunteerRepository;
	private readonly ILogger<UploadPhotosPetHandler> logger;
	private const string BUCKET_NAME = "photos";

	public UploadPhotosPetHandler(
		IFileProvider minioProvider,
		IVolunteerRepository volunteerRepository,
		ILogger<UploadPhotosPetHandler> logger)
	{
		this.minioProvider = minioProvider;
		this.volunteerRepository = volunteerRepository;
		this.logger = logger;
	}

	public async Task<Result<Guid, Error>> HandleAsync(UploadPhotosPetCommand command, CancellationToken token = default)
	{
		var volunteer = await volunteerRepository.GetByIdAsync(command.VolunteerId, token);
		if (volunteer.IsFailure)
			return volunteer.Error;

		var pet = volunteer.Value.Pets.FirstOrDefault(p => p.Id.Value == command.PetId);
		if (pet == null)
			return Errors.General.NotFound(command.PetId);


		List<FileStorage> filePaths = [];
		List<FileContent> fileContents = [];
		foreach(var file in command.UploadFiles)
		{
			var extension = Path.GetExtension(file.FileName);

			var filePath = FileStorage.Create(Guid.NewGuid(), extension);
			if (filePath.IsFailure)
				return filePath.Error;

			var fileContent = new FileContent(file.Stream, filePath.Value.PathToStorage);
			fileContents.Add(fileContent);
			filePaths.Add(filePath.Value);
		}

		var fileData = new FileUploadData(fileContents, BUCKET_NAME);

		var uploadResult = await minioProvider.UploadFilesAsync(fileData, token);
		if (uploadResult.IsFailure)
			return uploadResult.Error;


		pet.AddPhotos(filePaths);
		await volunteerRepository.SaveAsync(token);


		//logger.LogInformation("Created pet {petName} with id {petId}", pet.Name, pet.Id);

		return Guid.NewGuid();
	}
}
