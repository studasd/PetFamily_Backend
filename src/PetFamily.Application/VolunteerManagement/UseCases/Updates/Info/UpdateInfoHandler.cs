using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Contracts.RequestVolonteers;
using PetFamily.Domain.Shared.Errores;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Application.VolunteerManagement.UseCases.Updates.Info;

public class UpdateInfoHandler : ICommandHandler<Guid, UpdateInfoCommand>
{
	private readonly IVolunteerRepository volunteerRepository;
	private readonly IValidator<UpdateInfoCommand> validator;
	private readonly ILogger<UpdateInfoHandler> logger;

	public UpdateInfoHandler(
		IVolunteerRepository volunteerRepository,
		IValidator<UpdateInfoCommand> validator,
		ILogger<UpdateInfoHandler> logger)
	{
		this.volunteerRepository = volunteerRepository;
		this.validator = validator;
		this.logger = logger;
	}

	public async Task<Result<Guid, ErrorList>> HandleAsync(UpdateInfoCommand command, CancellationToken token)
	{
		var validateResult = await validator.ValidateAsync(command, token);
		if(validateResult.IsValid == false)
			return validateResult.ToErrorList();


		var volunteerResult = await volunteerRepository.GetByIdAsync(command.VolunteerId, token);

		if(volunteerResult.IsFailure)
			return volunteerResult.Error.ToErrorList();

		var nameReq = command.Name;
		var name = VolunteerName.Create(nameReq.Firstname, nameReq.Lastname, nameReq.Surname).Value;

		volunteerResult.Value.UpdateInfo(name, command.Email, command.Description);

		await volunteerRepository.SaveAsync(token);

		logger.LogInformation("Updated volunteer {name}, {email}, {description} with id {volunteerId}", name, command.Email, command.Description, volunteerResult.Value.Id);

		return volunteerResult.Value.Id.Value;
	}
}
