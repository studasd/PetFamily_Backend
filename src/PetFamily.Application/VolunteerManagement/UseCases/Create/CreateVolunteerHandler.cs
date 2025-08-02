
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Errores;
using PetFamily.Core.ValueObjects;
using PetFamily.Domain.VolunteerManagement.Entities;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Application.VolunteerManagement.UseCases.Create;

public class CreateVolunteerHandler : ICommandHandler<Guid, CreateVolunteerCommand> // CreateVolunteerService
{
	private readonly IVolunteerRepository volunteerRepository;
	private readonly IValidator<CreateVolunteerCommand> validator;
	private readonly ILogger<CreateVolunteerHandler> logger;

	public CreateVolunteerHandler(
		IVolunteerRepository volunteerRepository, 
		IValidator<CreateVolunteerCommand> validator,
		ILogger<CreateVolunteerHandler> logger)
	{
		this.volunteerRepository = volunteerRepository;
		this.validator = validator;
		this.logger = logger;
	}

	public async Task<Result<Guid, ErrorList>> HandleAsync(CreateVolunteerCommand command, CancellationToken token)
	{
		var validateResult = await validator.ValidateAsync(command, token);
		if (validateResult.IsValid == false)
			return validateResult.ToErrorList();

		var volunteerName = VolunteerName.Create(command.Name.Firstname, command.Name.Lastname, command.Name.Surname).Value;
		
		var volunteerNameExist = await volunteerRepository.GetByNameAsync(volunteerName, token);
		
		if (volunteerNameExist.IsSuccess)
			return Errors.General.AlreadyExist($"Volunteer with name '{volunteerName.Firstname} {volunteerName.Lastname} {volunteerName.Surname}'").ToErrorList();

		var phone = Phone.Create(command.Phone).Value;

		var volunteer = Volunteer.Create(volunteerName, command.Email, command.Description, command.ExperienceYears, phone).Value;

		if (command.SocialNetworks.Count() > 0)
		{
			var socNetworksResult = command.SocialNetworks.Select(s => SocialNetwork.Create(s.Name, s.Link).Value);
			volunteer.AddSocialNetworks(socNetworksResult);
		}

		if (command.BankingDetails.Count() > 0)
		{
			var bankDetailsResult = command.BankingDetails.Select(s => BankingDetails.Create(s.Name, s.Description).Value);
			volunteer.AddBankingDetails(bankDetailsResult);
		}

		await volunteerRepository.AddAsync(volunteer, token);

		logger.LogInformation("Created volunteer {volunteerName} with id {volunteerId}", volunteerName, volunteer.Id);

		return volunteer.Id.Value;
	}
}
