using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Errores;
using PetFamily.Core.ValueObjects;
using PetFamily.Volunteers.Application.PetsManagement.Commands.DeletePhotos;
using PetFamily.Volunteers.Application.VolunteerManagement;

namespace PetFamily.Volunteers.Application.PetsManagement.Commands.MovePosition;

public class MovePositionPetHandler : ICommandHandler<Guid, MovePositionPetCommand>
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

	public async Task<Result<Guid, ErrorList>> HandleAsync(MovePositionPetCommand command, CancellationToken token)
	{
		var volunteerResult = await volunteerRepository.GetByIdAsync(command.VolunteerId, token);
		if (volunteerResult.IsFailure)
			return volunteerResult.Error.ToErrorList();

		var petResult = volunteerResult.Value.GetPetById(command.PetId);
		if (petResult.IsFailure)
			return petResult.Error.ToErrorList();

		var newPositionResult = Position.Create(command.NewPosition);
		if (newPositionResult.IsFailure)
			return newPositionResult.Error.ToErrorList();


		using var transaction = await unitOfWork.BeginTransactionAsync(token);

		try
		{
			volunteerResult.Value.MovePet(petResult.Value, newPositionResult.Value);

			await unitOfWork.SaveChangesAsync(token);

			transaction.Commit();

			logger.LogInformation("Move pet {id} position.", command.PetId);

			return petResult.Value.Id.Value;
		}
		catch (Exception ex)
		{
			transaction.Rollback();

			logger.LogError(ex, "Error move pet {id} position", command.PetId);

			return Error.Failure("failure_move_pet", "Error move pet position").ToErrorList();
		}
	}
}
