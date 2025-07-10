
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Entities;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerEntities;

namespace PetFamily.Contracts.Volonteers.CreateVolonteer;

public class CreateVolunteerHandler // CreateVolunteerService
{
	private readonly IVolunteerRepository volunteerRepository;
	private readonly ILogger<CreateVolunteerHandler> logger;

	public CreateVolunteerHandler(IVolunteerRepository volunteerRepository, ILogger<CreateVolunteerHandler> logger)
	{
		this.volunteerRepository = volunteerRepository;
		this.logger = logger;
	}

	public async Task<Result<Guid, Error>> HandleAsync(CreateVolunteerRequest request, CancellationToken token = default)
	{
		var volunteerName = VolunteerName.Create(request.Firstname, request.Lastname, request.Surname).Value;
		
		var volunteerNameExist = await volunteerRepository.GetByNameAsync(volunteerName, token);
		
		if (volunteerNameExist.IsSuccess)
			return Errors.General.AlreadyExist("Volunteer");

		var phone = Phone.Create(request.Phone).Value;

		var volunteer = Volunteer.Create(volunteerName, request.Email, request.Description, request.ExperienceYears, phone).Value;

		if (request.SocialNetworks.Count() > 0)
		{
			foreach (var network in request.SocialNetworks)
				volunteer.AddSocialNetwork(network.Name, network.Link);
		}

		if (request.BankingDetails is not null)
			volunteer.AddBankingDetails(request.BankingDetails.Name, request.BankingDetails.Description);

		await volunteerRepository.AddAsync(volunteer, token);

		logger.LogInformation("Created volunteer {volunteerName} with id {volunteerId}", volunteerName, volunteer.Id);

		return volunteer.Id.Value;
	}
}
