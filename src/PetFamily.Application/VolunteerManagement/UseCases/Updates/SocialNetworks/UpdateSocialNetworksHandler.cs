using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Errores;
using PetFamily.Core.ValueObjects;

namespace PetFamily.Application.VolunteerManagement.UseCases.Updates.SocialNetworks;

public class UpdateSocialNetworksHandler : ICommandHandler<Guid, UpdateSocialNetworksCommand>
{
	private readonly IVolunteerRepository volunteerRepository;
	private readonly IValidator<UpdateSocialNetworksCommand> validator;
	private readonly ILogger<UpdateSocialNetworksHandler> logger;

	public UpdateSocialNetworksHandler(
		IVolunteerRepository volunteerRepository, 
		IValidator<UpdateSocialNetworksCommand> validator,
		ILogger<UpdateSocialNetworksHandler> logger)
	{
		this.volunteerRepository = volunteerRepository;
		this.validator = validator;
		this.logger = logger;
	}

	public async Task<Result<Guid, ErrorList>> HandleAsync(UpdateSocialNetworksCommand command, CancellationToken token)
	{
		var validateResult = await validator.ValidateAsync(command, token);
		if (validateResult.IsValid == false)
			return validateResult.ToErrorList();

		var volunteerResult = await volunteerRepository.GetByIdAsync(command.VolunteerId, token);

		if (volunteerResult.IsFailure)
			return volunteerResult.Error.ToErrorList();

		var socNetworksResult = command.SocialNetworks.Select(s => SocialNetwork.Create(s.Name, s.Link).Value);

		volunteerResult.Value.UpdateSocialNetworks(socNetworksResult);

		await volunteerRepository.SaveAsync(token);

		logger.LogInformation("Updated volunteer social networks {socials} with id {volunteerId}", socNetworksResult, volunteerResult.Value.Id);

		return volunteerResult.Value.Id.Value;
	}
}