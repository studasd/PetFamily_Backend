using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Contracts.Volonteers.Updates.SocialNetworks;

public class UpdateSocialNetworksHandler
{
	private readonly IVolunteerRepository volunteerRepository;
	private readonly ILogger<UpdateSocialNetworksHandler> logger;

	public UpdateSocialNetworksHandler(IVolunteerRepository volunteerRepository, ILogger<UpdateSocialNetworksHandler> logger)
	{
		this.volunteerRepository = volunteerRepository;
		this.logger = logger;
	}

	public async Task<Result<Guid, Error>> HandleAsync(UpdateSocialNetworksRequest request, CancellationToken token = default)
	{
		var volunteerResult = await volunteerRepository.GetByIdAsync(request.VolunteerId, token);

		if (volunteerResult.IsFailure)
			return volunteerResult.Error;

		var socNetworksResult = request.SocialNetworksDTO.SocialNetworks.Select(s => SocialNetwork.Create(s.Name, s.Link).Value);

		volunteerResult.Value.UpdateSocialNetworks(socNetworksResult);

		await volunteerRepository.SaveAsync();

		logger.LogInformation("Updated volunteer social networks {socials} with id {volunteerId}", socNetworksResult, volunteerResult.Value.Id);

		return volunteerResult.Value.Id.Value;
	}
}