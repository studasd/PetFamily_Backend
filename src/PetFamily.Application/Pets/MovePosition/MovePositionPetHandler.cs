using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Pets.DeletePhotos;
using PetFamily.Application.Volonteers;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Application.Pets.MovePosition;

public class MovePositionPetHandler
{
	private readonly IVolunteerRepository volunteerRepository;
	private readonly IUnitOfWork unitOfWork;
	private readonly ILogger<DeletePhotosPetHandler> logger;
	private const string BUCKET_NAME = "photos";

	public MovePositionPetHandler(
		IVolunteerRepository volunteerRepository,
		IUnitOfWork unitOfWork,
		ILogger<DeletePhotosPetHandler> logger)
	{
		this.volunteerRepository = volunteerRepository;
		this.unitOfWork = unitOfWork;
		this.logger = logger;
	}

	public async Task<Result<Guid, Error>> HandleAsync(MovePositionPetCommand command, CancellationToken token)
	{
		using var transaction = await unitOfWork.BeginTransactionAsync(token);

		try
		{
			var volunteerResult = await volunteerRepository.GetByIdAsync(command.VolunteerId, token);
			if (volunteerResult.IsFailure)
				return volunteerResult.Error;

			var petResult = volunteerResult.Value.Pets.FirstOrDefault(p => p.Id.Value == command.PetId);
			if (petResult == null)
				return Errors.General.NotFound(command.PetId);

			var newPositionResult = Position.Create(command.NewPosition);
			if (newPositionResult.IsFailure)
				return newPositionResult.Error;

			volunteerResult.Value.MovePet(petResult, newPositionResult.Value);

			await unitOfWork.SaveChangesAsync(token);

			transaction.Commit();

			logger.LogInformation("Move pet {id} position.", command.PetId);

			return petResult.Id.Value;
		}
		catch (Exception ex)
		{
			transaction.Rollback();

			logger.LogError(ex, "Error move pet {id} position", command.PetId);

			return Error.Failure("failure_move_pet", "Error move pet position");
		}
	}
}
